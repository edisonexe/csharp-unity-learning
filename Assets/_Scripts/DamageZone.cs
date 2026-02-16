using _Scripts.Interfaces;
using UnityEngine;

public sealed class DamageZone : MonoBehaviour
{
    [SerializeField] private int _damage = 10;
    private IHealth _health;

    public void Init(IHealth health)
    {
        _health = health;
    }

    private void OnTriggerEnter(Collider other)
    {
        _health?.Damage(_damage);
    }
}