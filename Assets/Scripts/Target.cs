using UnityEngine;

public class Target : MonoBehaviour
{
    private float _lifeTime;
    private float _logTick;
    
    private void Start()
    {
        Debug.Log("Target created.");
        Destroy(gameObject, 3f);
    }

    private void Update()
    {
        _lifeTime += Time.deltaTime;
        _logTick += Time.deltaTime;
        if (_logTick >= 0.99f)
        {
            Debug.Log($"Target still alive {_lifeTime}s.");
            _logTick = 0;
        }
    }

    private void OnDestroy() => Debug.Log("Target destroyed.");
}
