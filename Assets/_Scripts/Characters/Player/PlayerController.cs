using System;
using Characters.Enemy;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace Characters.Player
{
    [RequireComponent(typeof(NavMeshAgent))]
    public sealed class PlayerController : MonoBehaviour
    {
        [Header("Move")]
        [SerializeField] private LayerMask _groundMask = ~0;
        [SerializeField][Min(1f)] private float _clickRayRange = 500f;

        [Header("Camera")]
        [SerializeField] private UnityEngine.Camera _camera;
        [SerializeField] private Transform _cameraRigRoot;
        [SerializeField] private float _yawSpeed = 0.2f;

        [Header("RMB: click vs hold")]
        [SerializeField] private float _holdThreshold = 0.15f;
        [SerializeField] private float _dragThresholdPixels = 8f;

        [Header("Shooting")]
        [SerializeField] private Transform _shootingPoint;
        [SerializeField] private float _shootRange = 50f;
        [SerializeField] private LayerMask _shootMask = ~0;
        [SerializeField] private LayerMask _aimMask = ~0;
        [SerializeField] private ShootTracerView _tracerPrefab;

        private NavMeshAgent _agent;
        private PlayerMover _mover;
        
        private PlayerEntity _playerEntity;
        
        private bool _rmbPressed;
        private bool _rmbDragging;
        private float _rmbPressTime;
        private Vector2 _rmbPressPos;
        private Vector2 _prevMousePos;

        public void Init(PlayerEntity playerEntity)
        {
            _playerEntity = playerEntity;
            _playerEntity.Died += OnDied;
        }

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            
            if (NavMesh.SamplePosition(transform.position, out var hit, 5f, NavMesh.AllAreas))
                transform.position = hit.position;
            
            if (_camera == null) _camera = UnityEngine.Camera.main;
            if (_cameraRigRoot == null && _camera != null)
            {
                _cameraRigRoot = _camera.transform.parent != null ? _camera.transform.parent : _camera.transform;
            }
            
            _agent.updateRotation = false;
        }

        private void Start()
        {
            _mover = new PlayerMover(_agent);
            _mover.SetMoveSpeed(_playerEntity.MoveSpeed);
        }

        private void OnDestroy()
        {
            if (_playerEntity != null)
                _playerEntity.Died -= OnDied;
        }

        private void Update()
        {
            if (_playerEntity == null || !_playerEntity.IsAlive)
                return;

            HandleRightMouse();
            UpdateRotationToCursor();
            HandleShooting();
        }

        private void HandleRightMouse()
        {
            if (Mouse.current == null) return;

            Vector2 mousePos = Mouse.current.position.ReadValue();

            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                _rmbPressed = true;
                _rmbDragging = false;
                _rmbPressTime = Time.time;
                _rmbPressPos = mousePos;
                _prevMousePos = mousePos;
                return;
            }

            if (Mouse.current.rightButton.isPressed && _rmbPressed)
            {
                if (!_rmbDragging)
                {
                    float held = Time.time - _rmbPressTime;
                    float sqrDelta = (mousePos - _rmbPressPos).sqrMagnitude;

                    if (held > _holdThreshold || sqrDelta > _dragThresholdPixels * _dragThresholdPixels)
                        _rmbDragging = true;
                }

                if (_rmbDragging)
                {
                    Vector2 delta = mousePos - _prevMousePos;
                    RotateCameraYaw(delta.x);
                }

                _prevMousePos = mousePos;
                return;
            }

            if (Mouse.current.rightButton.wasReleasedThisFrame && _rmbPressed)
            {
                if (!_rmbDragging && TryGetGroundPoint(out Vector3 p))
                    _agent.SetDestination(p);

                _rmbPressed = false;
                _rmbDragging = false;
            }
        }

        private void RotateCameraYaw(float mouseDeltaX)
        {
            if (!_cameraRigRoot) return;
            _cameraRigRoot.Rotate(Vector3.up, mouseDeltaX * _yawSpeed, Space.World);
        }

        private bool TryGetGroundPoint(out Vector3 point)
        {
            point = default;
            if (!_camera || Mouse.current == null) return false;

            Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit, _clickRayRange, _groundMask))
            {
                point = hit.point;
                
                if (NavMesh.SamplePosition(point, out var navHit, 1.5f, NavMesh.AllAreas))
                    point = navHit.position;

                return true;
            }

            return false;
        }

        private void UpdateRotationToCursor()
        {
            if (!TryGetAimPoint(out Vector3 aimPoint))
                return;

            Vector3 look = aimPoint - transform.position;
            look.y = 0f;

            if (look.sqrMagnitude > 0.0001f)
                transform.forward = look.normalized;
        }

        private bool TryGetAimPoint(out Vector3 aimPoint)
        {
            aimPoint = default;

            if (!_camera || Mouse.current == null)
                return false;

            Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
            
            if (Physics.Raycast(ray, out RaycastHit hit, _shootRange, _aimMask))
            {
                aimPoint = hit.point;
                return true;
            }
            
            Plane ground = new Plane(Vector3.up, Vector3.zero);
            if (ground.Raycast(ray, out float t))
            {
                aimPoint = ray.GetPoint(t);
                return true;
            }

            return false;
        }

        private void HandleShooting()
        {
            if (Mouse.current == null || !Mouse.current.leftButton.wasPressedThisFrame)
                return;

            if (!_shootingPoint || !TryGetAimPoint(out Vector3 aimPoint))
                return;

            Vector3 from = _shootingPoint.position;
            Vector3 dir = (aimPoint - from);
            if (dir.sqrMagnitude < 0.0001f) return;
            dir.Normalize();

            Vector3 to = from + dir * _shootRange;

            if (Physics.Raycast(from, dir, out RaycastHit hit, _shootRange, _shootMask))
            {
                to = hit.point;
                hit.collider.GetComponent<EnemyController>()?.TakeDamage(_playerEntity.Damage);
            }
            
            if (_tracerPrefab)
            {
                var tracer = Instantiate(_tracerPrefab);
                tracer.Show(from, to);
            }
        }

        private void OnDied()
        {
            _mover.Stop();
            _agent.enabled = false;
        }
    }
}
