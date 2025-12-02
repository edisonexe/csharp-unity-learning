using System;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private void OnEnable() => EventManager.OnGameEvent += OnGameEventReceived;

    private void OnDisable() => EventManager.OnGameEvent -= OnGameEventReceived;

    private void OnGameEventReceived(GameEvent e)
    {
        if (e.Type == GameEventType.EnemySpotted)
        {
            Debug.Log($"[EnemyManager] Появился враг");
        }
    }
}