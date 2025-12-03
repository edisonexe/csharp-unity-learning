using UnityEngine;
using System;
using System.Collections;

public class AutoEventGenerator : MonoBehaviour
{
    [SerializeField] private float _delay = 2f;
    [SerializeField] private Vector2 _enemySpawnRange = new Vector2(-10f, 10f);
    private GameEventType[] _eventTypes;

    private void Awake() => _eventTypes = (GameEventType[])Enum.GetValues(typeof(GameEventType));

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
        var randomType = _eventTypes[UnityEngine.Random.Range(0, _eventTypes.Length)];

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
        float x = UnityEngine.Random.Range(_enemySpawnRange.x, _enemySpawnRange.y);
        float z = UnityEngine.Random.Range(_enemySpawnRange.x, _enemySpawnRange.y);
        
        EventManager.TriggerEnemySpotted(x, z);
    }
}
