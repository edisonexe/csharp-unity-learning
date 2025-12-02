using System;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private string _name = "NPC";
    private Rigidbody _rb;
    private Vector3 _target;
    private bool _hasTarget;
    private float _moveSpeed = 5f;
    
    private void Awake() => _rb = GetComponent<Rigidbody>();

    private void OnEnable() => EventManager.OnEnemySpotted += OnEnemySpotted;

    private void FixedUpdate()
    {
        if (!_hasTarget) return;
        
        Vector3 newPosition = Vector3.MoveTowards(transform.position, _target, 
            _moveSpeed * Time.fixedDeltaTime);
        _rb.MovePosition(newPosition);
    }
    
    private void OnDisable() => EventManager.OnEnemySpotted -= OnEnemySpotted;
    
    private void SetTarget(float x, float z)
    {
        _target = new Vector3(x, transform.position.y, z);
        _hasTarget = true;
    }
    
    private void OnEnemySpotted(float x, float y)
    {
        SetTarget(x, y);
        Debug.Log($"{_name} побежал на битву с врагом на координаты ({x},{y})");
    }

    private void OnCollisionEnter(Collision other)
    {
        if (_hasTarget && other.gameObject.CompareTag("Enemy"))
        {
            _hasTarget = false;
            Destroy(other.gameObject);
        }
    }
}
