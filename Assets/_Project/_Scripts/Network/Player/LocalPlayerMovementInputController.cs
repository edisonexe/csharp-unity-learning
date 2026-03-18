using System;
using _Project._Scripts.Interfaces.Gameplay.Camera;
using _Project._Scripts.Interfaces.Gameplay.Input;
using UnityEngine;

namespace _Project._Scripts.Network.Player
{
    public sealed class LocalPlayerMovementInputController
    {
        private readonly IPlayerInputReader _inputReader;
        private readonly ILookController _lookController;
        private readonly Action<PlayerNetworkInput> _sendInputToServer;

        public LocalPlayerMovementInputController(
            IPlayerInputReader inputReader,
            ILookController lookController,
            Action<PlayerNetworkInput> sendInputToServer)
        {
            _inputReader = inputReader ?? throw new ArgumentNullException(nameof(inputReader));
            _lookController = lookController ?? throw new ArgumentNullException(nameof(lookController));
            _sendInputToServer = sendInputToServer ?? throw new ArgumentNullException(nameof(sendInputToServer));
        }

        public void Tick()
        {
            UpdateLook();
            SendMovementInput();
        }

        private void UpdateLook()
        {
            Vector2 lookDelta = _inputReader.LookDelta;
            _lookController.Apply(lookDelta.x, lookDelta.y);
        }

        private void SendMovementInput()
        {
            Vector2 move = _inputReader.Move;

            if (move.sqrMagnitude > 1f)
                move.Normalize();

            PlayerNetworkInput input = new PlayerNetworkInput
            {
                Move = move,
                JumpPressed = _inputReader.JumpPressedThisFrame,
                Yaw = _lookController.Yaw,
                Pitch = _lookController.Pitch
            };

            _sendInputToServer(input);
        }
    }
}