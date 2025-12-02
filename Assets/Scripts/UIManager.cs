using UnityEngine;

public class UIManager : MonoBehaviour
{
    private void OnEnable() => EventManager.OnGameEvent += OnGameEventReceived;

    private void OnDisable() => EventManager.OnGameEvent -= OnGameEventReceived;

    private void OnGameEventReceived(GameEvent e)
    {
        if (e.Type == GameEventType.ItemPicked)
        {
            Debug.Log($"[UIManager] Предмет подобран (+1).");
        }
    }
}
