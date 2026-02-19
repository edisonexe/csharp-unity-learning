using _Scripts.Gameplay.World;
using _Scripts.Interfaces;
using UnityEngine;

namespace _Scripts.Gameplay.Factories
{
    public class CollectableFactory : ICollectableFactory
    {
        private readonly Collectable _prefab;
        private readonly IScore _score;
        private readonly ILoggerService _logger;

        public CollectableFactory(Collectable prefab, IScore score, ILoggerService logger)
        {
            _prefab = prefab;
            _score = score;
            _logger = logger;
        }
        
        public Collectable Spawn(Transform spawnPoint)
        {
            if (!_prefab)
            {
                _logger?.Error("CollectableFactory: prefab is null");
                return null;
            }

            if (!spawnPoint)
            {
                _logger?.Error("CollectableFactory: spawnPoint is null");
                return null;
            }
            
            var item = Object.Instantiate(_prefab, spawnPoint.position, spawnPoint.rotation);
            item.Init(_score, _logger);
            return item;
        }
    }
}