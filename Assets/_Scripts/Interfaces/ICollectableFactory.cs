using _Scripts.Gameplay.World;
using UnityEngine;

namespace _Scripts.Interfaces
{
    public interface ICollectableFactory
    {
        Collectable Spawn(Transform spawnPoint);
    }
}