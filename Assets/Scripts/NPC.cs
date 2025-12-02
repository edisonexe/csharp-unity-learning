using System;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private string _name = "NPC";

    private void OnEnable() => EventMananger.OnGameEvent += OnGameEventReceived;

    private void OnDisable() => EventMananger.OnGameEvent -= OnGameEventReceived;

    private void OnGameEventReceived(GameEvent e)
    {
        if (e.Type == GameEventType.BattleStart)
        {
            Debug.Log($"[NPC] {_name} побежал на битву!");
        }
    }
}
