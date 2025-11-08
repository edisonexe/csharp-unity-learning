using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody _rb;

    private void Awake() => _rb = GetComponent<Rigidbody>();

    private void Start()
    {
        Destroy(gameObject, 5f);
    }

    public void AddForce(Vector3 direction, float force)
    {
        _rb.AddForce(direction * force, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Target"))
        {
            Debug.Log("Target hit!");
            Destroy(gameObject);
            Destroy(other.gameObject);
        }
    }
}
