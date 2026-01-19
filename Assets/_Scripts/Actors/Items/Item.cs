using Events;
using UnityEngine;

namespace Actors.Items
{
    public sealed class Item : MonoBehaviour
    {
        [SerializeField] private int _scoreValue = 100;

        private void Start() => Destroy(gameObject, 5f);

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            EventBus.Raise(GameEventType.ItemPicked, _scoreValue);
            Destroy(gameObject);
        }
    }

}