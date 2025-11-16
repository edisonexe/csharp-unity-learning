using DefaultNamespace;
using Managers;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _sensitivity = 1.25f;
    
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private GunConfig _gunCfg;
    
    [SerializeField] private GameEvents _events;
    [SerializeField] private AudioManager _audioManager;
    
    private int _shotsFired;
    private int _targetHits;
    
    private Camera _camera;
    private float _lastShotTime;
    
    private float _rotationX;
    private float _rotationY;
    
    private void Start()
    {
        _camera = Camera.main;
        if (_gunCfg == null) 
            Debug.LogError("[ERROR] Gun Config is NULL!");
        if (_projectilePrefab == null)
            Debug.LogError("[ERROR] Projectile Prefab is NULL!");
    }

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
        
        _rotationX = Mathf.Clamp(_rotationX, -50f, 30f);
        
        transform.rotation = Quaternion.Euler(_rotationX, _rotationY, 0f);
    }
    
    private void HandleShooting()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time < _lastShotTime + _gunCfg.FireCooldown)
            {
                Debug.LogWarning("[WARNING] Attempted to shoot while reloading");
                return;
            }
            
            Shoot();
            _lastShotTime = Time.time;
        }
    }
    
    private void Shoot()
    {
        GameObject projectile = Instantiate(_projectilePrefab, _firePoint.position, Quaternion.identity);
        Vector3 direction = _camera.transform.forward;
        projectile.GetComponent<Projectile>().AddForce(direction, _gunCfg.ProjectileSpeed);
        
        _events.RaiseShotFired();
        Debug.Log("[SHOT] Shot fired");
    }
    
}
