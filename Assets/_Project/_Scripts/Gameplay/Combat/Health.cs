using System;
using System.Collections;
using _Project._Scripts.Interfaces;
using _Project._Scripts.Interfaces.Gameplay.Combat;
using _Project._Scripts.Network.Player;
using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project._Scripts.Gameplay.Combat
{
    [RequireComponent(typeof(GamePlayer))]
    public class Health : NetworkBehaviour, IHealable, IDamageable
    {
        [Header("Health")]
        [SerializeField] private int _maxHp = 100;
        [SerializeField] private float _respawnDelay = 4f;

        [SyncVar(hook = nameof(OnHpChanged))]
        private int _currentHp;

        [SyncVar]
        private bool _isDead;

        private GamePlayer _player;

        public int CurrentHp => _currentHp;
        public int MaxHp => _maxHp;
        public bool IsDead => _isDead;
    
        public event Action<int, int> HpChanged; 

        private void Awake()
        {
            _player = GetComponent<GamePlayer>();
            
            if (!_player)
            {
                Debug.LogError("[Health] GamePlayer component not found.", this);
                enabled = false;
                return;
            }

            if (_maxHp <= 0)
            {
                Debug.LogError("[Health] MaxHp must be greater than 0.", this);
                enabled = false;
                return;
            }

            if (_respawnDelay < 0f)
            {
                Debug.LogError("[Health] RespawnDelay cannot be negative.", this);
                enabled = false;
                return;
            }
        }

        public override void OnStartServer()
        {
            _currentHp = _maxHp;
            _isDead = false;
        }
        
        [Server]
        public void TakeDamage(int damage, GamePlayer attacker)
        {
            if (_isDead)
                return;

            _currentHp = Mathf.Max(0, _currentHp - damage);

            if (_currentHp <= 0)
                Die(attacker);
        }

        [Server]
        public int Restore(int amount)
        {
            if (_isDead || amount <= 0)
                return 0;

            int oldHp = _currentHp;
            _currentHp = Mathf.Min(_maxHp, _currentHp + amount);
            return _currentHp - oldHp;
        }
        
        [Server]
        private void RestoreFull()
        {
            _currentHp = _maxHp;
            _isDead = false;
        }

        [Server]
        private void Die(GamePlayer attacker)
        {
            if (_isDead)
                return;

            _isDead = true;

            Debug.Log($"[Health] {gameObject.name} died. Killer: {(attacker ? attacker.name : "unknown")}");

            if (_player)
                _player.ServerSetAliveState(false);

            StartCoroutine(ServerRespawnRoutine());
        }

        [Server]
        private IEnumerator ServerRespawnRoutine()
        {
            yield return new WaitForSeconds(_respawnDelay);

            Transform spawn = GetRespawnPoint();
            if (spawn && _player)
            {
                _player.ServerRespawnAt(spawn.position, spawn.rotation);
            }
            else
            {
                Debug.LogWarning("[Health] Respawn point not found.");
            }

            RestoreFull();

            if (_player)
                _player.ServerSetAliveState(true);

            Debug.Log($"[Health] {gameObject.name} respawned with full HP.");
        }

        [Server]
        private Transform GetRespawnPoint()
        {
            if (!NetworkManager.singleton)
                return null;

            var points = NetworkManager.startPositions;
            if (points == null || points.Count == 0)
                return null;

            int index = Random.Range(0, points.Count);
            return points[index];
        }

        private void OnHpChanged(int oldHp, int newHp)
        {
            if (_player && _player.isLocalPlayer)
            {
                HpChanged?.Invoke(newHp, _maxHp);
            }
        }
    }
}