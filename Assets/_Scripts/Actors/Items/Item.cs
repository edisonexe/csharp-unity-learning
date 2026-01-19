using Events;
using UnityEngine;

namespace Actors.Items
{
    public sealed class Item : MonoBehaviour
    {
        [SerializeField] private int _scoreValue = 100;
        [SerializeField] private float _lifeTime = 3f;
        private bool _picked;
        
        private void Start() => Destroy(gameObject, _lifeTime);

        private void OnTriggerEnter(Collider other)
        {
            if (_picked) return;
            if (!other.CompareTag("Player")) return;

            _picked = true;
            
            EventBus.Raise(GameEventType.ItemPicked, _scoreValue);
            Destroy(gameObject);
        }
    }

}