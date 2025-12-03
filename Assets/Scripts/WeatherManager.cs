using UnityEngine;
using Random = UnityEngine.Random;

public class WeatherManager : MonoBehaviour
{
    private Camera _camera;

    private readonly Color[] _colors = 
    {
        new (0.29f, 0.35f, 0.35f),
        new (0.56f, 1f, 0.92f),
        new (0.90f, 0.90f, 0.90f)
    };
    
    private void Awake() => _camera = Camera.main;

    private void Start() => _camera.backgroundColor = _colors[1];

    private void OnEnable() => EventManager.OnGameEvent += OnGameEventReceived;
    private void OnDisable() => EventManager.OnGameEvent -= OnGameEventReceived;

    private void OnGameEventReceived(GameEvent e)
    {
        if (e.Type == GameEventType.WeatherChanged)
        {
            int index = Random.Range(0, _colors.Length);
            _camera.backgroundColor = _colors[index];
        }
    }
}
