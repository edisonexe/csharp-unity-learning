using System.Collections;
using UnityEngine;

namespace Characters.Player
{
    public sealed class ShootTracerView : MonoBehaviour
    {
        [SerializeField] private LineRenderer _line;
        [SerializeField] private float _lifeTime = 0.08f;

        private void Reset() => _line = GetComponent<LineRenderer>();

        public void Show(Vector3 from, Vector3 to)
        {
            if (_line == null) return;
            _line.positionCount = 2;
            _line.SetPosition(0, from);
            _line.SetPosition(1, to);

            StopAllCoroutines();
            StartCoroutine(Life());
        }

        private IEnumerator Life()
        {
            yield return new WaitForSeconds(_lifeTime);
            Destroy(gameObject);
        }
    }

}