using System;
using _Project._Scripts.Interfaces;
using _Project._Scripts.Network;
using _Project._Scripts.Network.Player;
using UnityEngine;

namespace _Project._Scripts.Gameplay.Movement
{
    public sealed class NetworkPlayerMovementController
    {
        private readonly IPlayerInputReader _inputReader;
        private readonly ILookController _lookController;
        private readonly Action<PlayerNetworkInput> _sendInputToServer;

        public NetworkPlayerMovementController(
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
            Vector2 lookDelta = _inputReader.LookDelta;
            _lookController.Apply(lookDelta.x, lookDelta.y);

            Vector2 moveInput = _inputReader.Move;
            if (moveInput.sqrMagnitude > 1f)
                moveInput.Normalize();

            PlayerNetworkInput input = new PlayerNetworkInput
            {
                Move = moveInput,
                JumpPressed = _inputReader.JumpPressedThisFrame,
                Yaw = _lookController.Yaw,
                Pitch = _lookController.Pitch
            };

            _sendInputToServer(input);
        }
    }
}