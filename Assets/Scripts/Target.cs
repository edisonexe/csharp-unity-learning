using System.Collections;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private float _timeToDestroy = 3f;
    [SerializeField] private float _rotationSpeed = 20f;
    
    private Animator _animator;
    private const string IS_HIT = "IsHit";
    private bool _isHit;
    
    private float _lifeTime;
    private float _logTick;
    private Coroutine _lifeCoroutine;

    private void Awake() => _animator = GetComponent<Animator>();

    private void Start() => _lifeCoroutine = StartCoroutine(LifeTimer());

    private void Update()
    {
        transform.Rotate(0, _rotationSpeed * Time.deltaTime,0);
        _lifeTime += Time.deltaTime;
        _logTick += Time.deltaTime;
    }

    private void OnDestroy()
    {
        if (_lifeCoroutine != null)
            StopCoroutine(_lifeCoroutine);
    }

    public void Hit()
    {
        if (_isHit) return;

        _isHit = true;
        
        var collider = GetComponent<Collider>();
        if (collider != null)
            collider.enabled = false;
        
        if (_animator != null) 
            _animator.SetBool(IS_HIT, true);
    }
    
    public void DestroySelf() => Destroy(gameObject);

    private IEnumerator LifeTimer()
    {
        yield return new WaitForSeconds(_timeToDestroy);
        
        if (!_isHit) 
            Hit();
    }

}
