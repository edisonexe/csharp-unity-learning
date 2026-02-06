using Input;
using UnityEngine;
using UnityEngine.AI;

namespace Characters.Player.Controllers
{
    public class PlayerMovementController
    {
        private readonly NavMeshAgent _agent;
        private readonly UnityEngine.Camera _camera;
        private readonly Transform _cameraRigRoot;
        private readonly LayerMask _groundMask;
        private readonly float _clickRayRange;
        private readonly float _yawSpeed;
        private readonly float _holdThreshold;
        private readonly float _dragThresholdPixels;

        private bool _rmbPressed;
        private bool _rmbDragging;
        private float _rmbPressTime;
        private Vector2 _rmbPressPos;
        private Vector2 _prevMousePos;

        public PlayerMovementController(NavMeshAgent agent, UnityEngine.Camera camera, Transform cameraRigRoot,
            LayerMask groundMask, float clickRayRange, float yawSpeed, float holdThreshold, float dragThresholdPixels)
        {
            _agent = agent;
            _camera = camera;
            _cameraRigRoot = cameraRigRoot;
            _groundMask = groundMask;
            _clickRayRange = clickRayRange;
            _yawSpeed = yawSpeed;
            _holdThreshold = holdThreshold;
            _dragThresholdPixels = dragThresholdPixels;

            _agent.updateRotation = false;
        }

        public void SetSpeed(float speed)
        {
            if (!_agent) return;
            _agent.speed = Mathf.Max(0f, speed);
        }

        public void Stop()
        {
            if (!_agent) return;
            _agent.isStopped = true;
            _agent.ResetPath();
        }

        public void Update(PlayerInputReader input)
        {
            if (!input || !_agent) return;

            Vector2 mousePos = input.MousePos;

            if (input.RmbPressedThisFrame)
            {
                _rmbPressed = true;
                _rmbDragging = false;
                _rmbPressTime = Time.time;
                _rmbPressPos = mousePos;
                _prevMousePos = mousePos;
                return;
            }

            if (input.RmbHeld && _rmbPressed)
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
                    RotateCameraYaw(input.MouseDelta.x);
                }

                _prevMousePos = mousePos;
                return;
            }
            
            if (input.RmbReleasedThisFrame && _rmbPressed)
            {
                if (!_rmbDragging && TryGetGroundPoint(mousePos, out Vector3 p))
                {
                    _agent.isStopped = false;
                    _agent.SetDestination(p);
                }

                _rmbPressed = false;
                _rmbDragging = false;
            }
        }
        
        private void RotateCameraYaw(float mouseDeltaX)
        {
            if (!_cameraRigRoot) return;
            _cameraRigRoot.Rotate(Vector3.up, mouseDeltaX * _yawSpeed, Space.World);
        }

        private bool TryGetGroundPoint(Vector2 mousePos, out Vector3 point)
        {
            point = default;
            if (!_camera) return false;

            Ray ray = _camera.ScreenPointToRay(mousePos);
            if (Physics.Raycast(ray, out RaycastHit hit, _clickRayRange, _groundMask))
            {
                point = hit.point;

                if (NavMesh.SamplePosition(point, out var navHit, 1.5f, NavMesh.AllAreas))
                    point = navHit.position;
                
                return true;
            }

            return false;
        }
    }
}