using System.Collections;
using Mirror;
using UnityEngine;

namespace _Project._Scripts.Gameplay.Items
{
    public sealed class ItemSpawner : NetworkBehaviour
    {
        [Header("Spawn Points")]
        [SerializeField] private Transform[] _medkitSpawnPoints;
        [SerializeField] private Transform[] _grenadeSpawnPoints;

        [Header("Pickup Prefabs")]
        [SerializeField] private PickupItem _medkitPickupPrefab;
        [SerializeField] private PickupItem _grenadePickupPrefab;

        [Header("Initial Counts")]
        [SerializeField] private int _initialMedkitsCount = 3;
        [SerializeField] private int _initialGrenadesCount = 3;

        [Header("Respawn")]
        [SerializeField] private float _respawnDelay = 15f;

        private void Awake()
        {
            if (_medkitSpawnPoints == null || _medkitSpawnPoints.Length == 0)
            {
                Debug.LogError("[ItemSpawner] Medkit spawn points are not assigned.", this);
                enabled = false;
                return;
            }

            if (_grenadeSpawnPoints == null || _grenadeSpawnPoints.Length == 0)
            {
                Debug.LogError("[ItemSpawner] Grenade spawn points are not assigned.", this);
                enabled = false;
                return;
            }

            if (!_medkitPickupPrefab)
            {
                Debug.LogError("[ItemSpawner] Medkit pickup prefab is not assigned.", this);
                enabled = false;
                return;
            }

            if (!_grenadePickupPrefab)
            {
                Debug.LogError("[ItemSpawner] Grenade pickup prefab is not assigned.", this);
                enabled = false;
            }
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            SpawnInitialItems();
        }

        [Server]
        private void SpawnInitialItems()
        {
            SpawnBatch(ItemType.Medkit, _initialMedkitsCount, _medkitSpawnPoints);
            SpawnBatch(ItemType.Grenade, _initialGrenadesCount, _grenadeSpawnPoints);
        }

        [Server]
        private void SpawnBatch(ItemType itemType, int count, Transform[] spawnPoints)
        {
            if (spawnPoints == null || spawnPoints.Length == 0)
                return;

            int spawnCount = Mathf.Min(count, spawnPoints.Length);

            for (int i = 0; i < spawnCount; i++)
            {
                SpawnItem(spawnPoints[i], itemType);
            }
        }

        [Server]
        private void SpawnItem(Transform spawnPoint, ItemType itemType)
        {
            PickupItem prefab = GetPickupPrefab(itemType);
            if (!prefab || !spawnPoint)
                return;

            PickupItem item = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
            item.Initialize(this, spawnPoint, itemType);

            NetworkServer.Spawn(item.gameObject);
        }

        [Server]
        public void NotifyItemPicked(Transform spawnPoint, ItemType itemType)
        {
            StartCoroutine(RespawnRoutine(spawnPoint, itemType));
        }

        [Server]
        private IEnumerator RespawnRoutine(Transform spawnPoint, ItemType itemType)
        {
            yield return new WaitForSeconds(_respawnDelay);
            SpawnItem(spawnPoint, itemType);
        }

        private PickupItem GetPickupPrefab(ItemType itemType)
        {
            return itemType switch
            {
                ItemType.Medkit => _medkitPickupPrefab,
                ItemType.Grenade => _grenadePickupPrefab,
                _ => null
            };
        }
    }
}