using System.Text;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _logsPanel;
    [SerializeField] private TMP_Text _showLogsBtnText;
    
    [SerializeField] private TMP_Text _logText;
    private readonly StringBuilder _builder = new();
    
    private const string SHOW_LOGS = "Показать логи";
    private const string CLOSE_LOGS = "Закрыть логи";
    private bool _logsOpened;
    
    private void Awake()
    {
        if (!_showLogsBtnText || !_logsPanel || !_logText) 
            Debug.LogError("[UIManager] Не заданы поля компонента");
    }
    
    private void OnEnable()
    {
        EventManager.OnGameEvent += OnGameEventReceived;
        
        _builder.Clear();
        foreach (var e in EventManager.EventHistory)
            _builder.AppendLine(e.ToLogString());

        _logText.text = _builder.ToString();
    }

    private void Start() => _logsPanel.SetActive(false);

    private void OnDisable() => EventManager.OnGameEvent -= OnGameEventReceived;

    public void ShowLogsPanel()
    {
        _logsOpened = !_logsOpened;
        _logsPanel.SetActive(_logsOpened);
        _showLogsBtnText.text = _logsOpened ? CLOSE_LOGS : SHOW_LOGS;
    }
    
    private void OnGameEventReceived(GameEvent e)
    {
        _builder.AppendLine(e.ToLogString());
        _logText.text = _builder.ToString();
    }
}
