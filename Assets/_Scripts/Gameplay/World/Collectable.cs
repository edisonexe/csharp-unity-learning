using _Scripts.Interfaces;
using UnityEngine;

namespace _Scripts.Gameplay.World
{
    public sealed class Collectable : MonoBehaviour
    {
        [SerializeField] private int _scoreAmount = 10;
        private ILoggerService _logger;
        private IScore _score;
    
        public void Init(IScore score, ILoggerService logger)
        {
            _score = score;
            _logger = logger;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            _logger.Log("Item collected");
            _score.Add(_scoreAmount);
            Destroy(gameObject);
        }
    }
}