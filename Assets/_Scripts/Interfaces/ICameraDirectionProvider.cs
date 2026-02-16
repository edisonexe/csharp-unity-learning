using UnityEngine;

namespace _Scripts.Interfaces
{
    public interface ICameraDirectionProvider
    {
        public Vector3 Forward { get; }
        public Vector3 Right { get; }
    }
}