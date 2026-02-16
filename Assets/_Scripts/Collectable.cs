using _Scripts.Interfaces;
using UnityEngine;

public sealed class Collectable : MonoBehaviour
{
    private ILoggerService _logger;

    public void Init(ILoggerService logger)
    {
        _logger = logger;
    }

    private void OnTriggerEnter(Collider other)
    {
        _logger.Log("Item collected");
        Destroy(gameObject);
    }
}