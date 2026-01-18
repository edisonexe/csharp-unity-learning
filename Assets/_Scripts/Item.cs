using Events;

namespace _Scripts
{
    using UnityEngine;

    public sealed class Item : MonoBehaviour
    {
        [SerializeField] private int scoreValue = 100;

        private void Start() => Destroy(gameObject, 5f);

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            EventBus.Raise(GameEventType.ItemPicked, scoreValue);
            Destroy(gameObject);
        }
    }

}