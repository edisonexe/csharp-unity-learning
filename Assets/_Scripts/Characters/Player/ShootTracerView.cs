using Pooling;
using UnityEngine;

namespace Characters.Player
{
    public sealed class ShootTracerView : MonoBehaviour, IPoolable
    {
        [SerializeField] private LineRenderer _line;
        [SerializeField] private float _lifeTime = 0.08f;
        private IPoolable _poolableImplementation;

        private ObjectPool<ShootTracerView> _pool;
        private float _dieTime;
        private bool _alive;
        
        private void Reset() => _line = GetComponent<LineRenderer>();

        public void Init(ObjectPool<ShootTracerView> pool, Vector3 from, Vector3 to)
        {
            _pool = pool;
            Show(from, to);
        }

        public void Show(Vector3 from, Vector3 to)
        {
            if (!_line) return;

            _line.positionCount = 2;
            _line.SetPosition(0, from);
            _line.SetPosition(1, to);

            _dieTime = Time.time + _lifeTime;
            _alive = true;

            enabled = true;
        }

        private void Update()
        {
            if (!_alive) return;

            if (Time.time >= _dieTime)
            {
                _alive = false;
                _pool?.Return(this);
            }
        }
        
        public void OnSpawned()
        {
            _alive = false;
            enabled = false;
        }

        public void OnDespawned()
        {
            _alive = false;
            enabled = false;
        }
    }

}