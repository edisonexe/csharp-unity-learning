using _Scripts.Interfaces;
using UnityEngine;

namespace _Scripts.Services.Input
{
    public sealed class FakeInputService : IInputService
    {
        private readonly Vector2 _fixed;

        public FakeInputService(Vector2 fixedDirection) => _fixed = fixedDirection;

        public Vector2 MoveAxis => _fixed;
    }
}