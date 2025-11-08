using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _sensitivity = 1.25f;
    
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _shootForce = 20f;
    [SerializeField] private float _fireCooldown = 0.5f;
    
    private Camera _camera;
    private float _lastShotTime;
    
    private float _rotationX;
    private float _rotationY;

    private void Start() => _camera = Camera.main;

    private void Update()
    {
        HandleShooting();
        HandleRotation();
    }
    
    private void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * _sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * _sensitivity;
        
        _rotationY += mouseX;
        _rotationX -= mouseY;
        
        _rotationX = Mathf.Clamp(_rotationX, -20f, 20f);
        _rotationY = Mathf.Clamp(_rotationY, -30f, 30f);
        
        transform.rotation = Quaternion.Euler(_rotationX, _rotationY, 0f);
    }
    
    private void HandleShooting()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= _lastShotTime + _fireCooldown)
        {
            Shoot();
            _lastShotTime = Time.time;
        }
    }
    
    private void Shoot()
    {
        GameObject projectile = Instantiate(_projectilePrefab, _firePoint.position, Quaternion.identity);
        Vector3 direction = _camera.transform.forward;
        projectile.GetComponent<Projectile>().AddForce(direction, _shootForce);
    }
}
