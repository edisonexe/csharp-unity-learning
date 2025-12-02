using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class WeatherManager : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    private Color[] _colors = 
    {
        new Color(0.29f, 0.35f, 0.35f),
        new Color(0.56f, 1f, 0.92f),
        new Color(0.90f, 0.90f, 0.90f)
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
