using System.Collections;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private float _timeToDestroy = 3f;
    [SerializeField] private float _rotationSpeed = 20f;
    private float _lifeTime;
    private float _logTick;
    private Coroutine _scaleCoroutine;
    
    [Header("Scaling")]
    [SerializeField] private float _minScale = 1f;
    [SerializeField] private float _maxScale = 2f;
    [SerializeField] private float _scaleSpeed = 1f;
    private bool _scalingUp = true;
    
    private void Start()
    {
        Debug.Log("Target created.");
        _scaleCoroutine = StartCoroutine(Scale());
        Destroy(gameObject, _timeToDestroy);
    }

    private void Update()
    {
        transform.Rotate(0, _rotationSpeed * Time.deltaTime,0);
        _lifeTime += Time.deltaTime;
        _logTick += Time.deltaTime;
        if (_logTick >= 0.99f)
        {
            Debug.Log($"Target still alive {_lifeTime}s.");
            _logTick = 0;
        }
    }

    private void OnDestroy()
    {
        Debug.Log("Target destroyed.");
        if (_scaleCoroutine != null)
            StopCoroutine(_scaleCoroutine);
    }

    private IEnumerator Scale()
    {
        while (true)
        {
            float targetScale = _scalingUp ? _maxScale : _minScale;
            Vector3 startScale = transform.localScale;
            Vector3 endScale = Vector3.one * targetScale;
            float scaleProgress = 0f;
            
            while (scaleProgress < 1f)
            {
                scaleProgress += Time.deltaTime * _scaleSpeed;
                transform.localScale = Vector3.Lerp(startScale, endScale, scaleProgress);
                yield return null;
            }
            _scalingUp = !_scalingUp;
        }
    }
}
