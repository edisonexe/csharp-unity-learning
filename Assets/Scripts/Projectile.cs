using DefaultNamespace;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private GameEvents  _events;
    private Rigidbody _rb;
    
    private void Awake() => _rb = GetComponent<Rigidbody>();

    private void Start() => Destroy(gameObject, 5f);

    public void AddForce(Vector3 direction, float force) => 
        _rb.AddForce(direction * force, ForceMode.Impulse);

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Target"))
        {
            _events.RaiseTargetHit();
            Destroy(gameObject);
            Destroy(other.gameObject);
        }
    }
}
