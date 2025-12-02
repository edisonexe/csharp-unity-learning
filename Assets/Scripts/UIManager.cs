using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _logsPanel;
    [SerializeField] private TMP_Text _showLogsBtnText;
    
    [SerializeField] private TMP_Text _logText;
    [SerializeField] private int _lastCount = 10;
    
    private readonly StringBuilder _builder = new();
    
    private const string SHOW_LOGS = "Показать логи";
    private const string CLOSE_LOGS = "Закрыть логи";
    private bool _logsOpened;
    
    private enum LogViewMode
    {
        All,
        LastN,
        OnlyBattles,
        OnlyEnemySpots,
        Top3,
        PickedItems
    }

    private LogViewMode _mode = LogViewMode.All;
    
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

    public void OnShowLogsPanelClicked()
    {
        _logsOpened = !_logsOpened;
        _logsPanel.SetActive(_logsOpened);
        _showLogsBtnText.text = _logsOpened ? CLOSE_LOGS : SHOW_LOGS;
    }

    public void OnShowAllClicked()
    {
        _mode = LogViewMode.All;
        RefreshCurrentView();
    }

    public void OnShowLastNClicked()
    {
        _mode = LogViewMode.LastN;
        RefreshCurrentView();
    }

    public void OnShowBattleStartClicked()
    {
        _mode = LogViewMode.OnlyBattles;
        RefreshCurrentView();
    }

    public void OnShowEnemySpotsClicked()
    {
        _mode = LogViewMode.OnlyEnemySpots;
        RefreshCurrentView();
    }

    public void OnShowTop3Clicked()
    {
        _mode = LogViewMode.Top3;
        RefreshCurrentView();
    }

    public void OnPickedItemsClicked()
    {
        _mode = LogViewMode.PickedItems;
        RefreshCurrentView();
    }
    
    private void OnGameEventReceived(GameEvent e)
    {
        if (_mode != LogViewMode.LastN)
            RefreshCurrentView();
    }
    
    private void RefreshCurrentView()
    {
        switch (_mode)
        {
            case LogViewMode.All:
                RenderEvents(EventManager.EventHistory);
                break;

            case LogViewMode.LastN:
                RenderEvents(EventManager.GetLastEventsByCount(_lastCount));
                break;

            case LogViewMode.OnlyBattles:
                RenderEvents(EventManager.GetEventsByType(GameEventType.BattleStart));
                break;

            case LogViewMode.OnlyEnemySpots:
                RenderEvents(EventManager.GetEventsByType(GameEventType.EnemySpotted));
                break;

            case LogViewMode.Top3:
                RenderStats();
                break;
            
            case LogViewMode.PickedItems:
                RenderCoundPickedItems();
                break;
        }
    }

    private void RenderEvents(IEnumerable<GameEvent> eventsToShow)
    {
        _builder.Clear();

        foreach (var e in eventsToShow)
            _builder.AppendLine(e.ToLogString());

        _logText.text = _builder.ToString();
    }

    private void RenderStats()
    {
        _builder.Clear();
        _builder.AppendLine("<b>Топ3 типа событий:</b>");

        foreach (var (type, count) in EventManager.GetMostFrequentEventsByCount())
        {
            _builder.AppendLine($"{type}: {count}");
        }

        _logText.text = _builder.ToString();
    }

    private void RenderCoundPickedItems()
    {
        _builder.Clear();
        var count = EventManager.GetEventCountByType(GameEventType.ItemPicked);
        
        _builder.AppendLine($"<b>Кол-во поднятых предметов:</b> {count}");
        _logText.text = _builder.ToString();
    }
}
