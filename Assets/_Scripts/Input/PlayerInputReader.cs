using UnityEngine;

namespace Input
{
    public sealed class PlayerInputReader : MonoBehaviour
    {
        public Vector2 MousePos { get; private set; }
        public Vector2 MouseDelta { get; private set; }

        public float ScrollY { get; private set; }

        public bool ShootHeld { get; private set; }
        public bool ShootPressedThisFrame { get; private set; }

        public bool RmbHeld { get; private set; }
        public bool RmbPressedThisFrame { get; private set; }
        public bool RmbReleasedThisFrame { get; private set; }

        public bool ReloadPressedThisFrame { get; private set; }

        private GameInputActions _actions;

        private bool _prevShootHeld;
        private bool _prevRmbHeld;

        private void Awake()
        {
            _actions = new GameInputActions();
            _actions.Enable();
            
            _actions.Player.Reload.performed += _ => ReloadPressedThisFrame = true;
        }

        private void Update()
        {
            MousePos = _actions.Player.MousePoint.ReadValue<Vector2>();
            MouseDelta = _actions.Player.MouseDelta.ReadValue<Vector2>();
            ScrollY = _actions.Player.Scroll.ReadValue<Vector2>().y;
            
            ShootHeld = _actions.Player.Shoot.IsPressed();
            ShootPressedThisFrame = !_prevShootHeld && ShootHeld;
            _prevShootHeld = ShootHeld;
            
            RmbHeld = _actions.Player.MoveRotate.IsPressed();
            RmbPressedThisFrame = !_prevRmbHeld && RmbHeld;
            RmbReleasedThisFrame = _prevRmbHeld && !RmbHeld;
            _prevRmbHeld = RmbHeld;
        }

        private void LateUpdate() => ReloadPressedThisFrame = false;

        private void OnDestroy() => _actions?.Disable();
    }
}