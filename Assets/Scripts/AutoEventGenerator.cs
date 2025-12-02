using UnityEngine;
using System;
using System.Collections;

public class AutoEventGenerator : MonoBehaviour
{
    [SerializeField] private float _delay = 2f;
    private void Start() => StartCoroutine(EventRoutine());

    private IEnumerator EventRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_delay);
            
            TriggerRandomEvent();
        }
    }

    private void TriggerRandomEvent()
    {
        Array values = Enum.GetValues(typeof(GameEventType));
        GameEventType randomType = (GameEventType)values.GetValue(UnityEngine.Random.Range(0, values.Length));

        string description = randomType switch
        {
            GameEventType.BattleStart  => "Битва началась!",
            GameEventType.ItemPicked     => "Кто-то подобрал случайный предмет.",
            GameEventType.WeatherChanged => "Погода изменилась.",
            GameEventType.EnemySpotted   => "Враг создан.",
            _                            => "Произошло случайное событие."
        };

        var gameEvent = new GameEvent(
            randomType,
            DateTime.Now,
            description
        );

        EventManager.TriggerEvent(gameEvent);
        
        if (randomType == GameEventType.EnemySpotted)
        {
            TriggerRandomEnemySpotted();
        }
    }

    private void TriggerRandomEnemySpotted()
    {
        float x = UnityEngine.Random.Range(-10f, 10f);
        float z = UnityEngine.Random.Range(-10f, 10f);
        
        EventManager.TriggerEnemySpotted(x, z);

        Debug.Log($"[AutoEvent] Враг замечен на координатах ({x:F1}, {z:F1})");
    }
}
