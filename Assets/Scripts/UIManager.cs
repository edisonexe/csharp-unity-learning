using UnityEngine;

public class UIManager : MonoBehaviour
{
    private void OnEnable() => EventMananger.OnGameEvent += OnGameEventReceived;

    private void OnDisable() => EventMananger.OnGameEvent -= OnGameEventReceived;

    private void OnGameEventReceived(GameEvent e)
    {
        if (e.Type == GameEventType.ItemPicked)
        {
            Debug.Log($"[UIManager] Предмет подобран (+1).");
        }
    }
}
