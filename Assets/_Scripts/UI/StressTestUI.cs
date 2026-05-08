using System.Text;
using _Scripts.Domain;
using _Scripts.Interfaces;
using StressTest.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
    [AddComponentMenu("StressTest/UI/Stress Test UI")]
    public class StressTestUI : MonoBehaviour
    {
        [Header("General Stats")]
        [SerializeField] private TMP_Text _execModeText;
        [SerializeField] private TMP_Text _activeProjsText;
        [SerializeField] private TMP_Text _activeTargetsText;
        [SerializeField] private TMP_Text _totalCreatedProjsText;
        [SerializeField] private TMP_Text _totalCreatedTargetsText;

        [Header("Pool Stats")]
        [SerializeField] private TMP_Text _projsPoolSizeText;
        [SerializeField] private TMP_Text _targetsPoolSizeText;
        [SerializeField] private TMP_Text _availableProjsText;
        [SerializeField] private TMP_Text _availableTargetsText;
        [SerializeField] private TMP_Text _reusedProjsText;
        [SerializeField] private TMP_Text _reusedTargetsText;

        [Header("Settings")]
        [SerializeField] private float _updateInterval = 0.1f;

        [SerializeField] private Button _toggleBtn;
        
        private IStatsProvider _statsProvider;
        private ISimulationController _controller;
        private SimulationStats _latestStats;
        private float _timer;
        
        private readonly StringBuilder _sb = new(64);
        
        private const string MODE_L = "Mode: ";
        private const string ACT_P_L = "Active Proj: ";
        private const string ACT_T_L = "Active Targets: ";
        private const string TOT_P_L = "Total Created Proj: ";
        private const string TOT_T_L = "Total Created Targets: ";
        
        private const string P_SIZE_L = "Proj Pool Size: ";
        private const string T_SIZE_L = "Target Pool Size: ";
        private const string P_AVAIL_L = "Available Proj: ";
        private const string T_AVAIL_L = "Available Targets: ";
        private const string P_REUSED_L = "Reused Proj: ";
        private const string T_REUSED_L = "Reused Targets: ";
        
        private const string NA_VALUE = "N/A";

        public void Init(IStatsProvider statsProvider, ISimulationController controller)
        {
            _statsProvider = statsProvider;
            _controller = controller;
            _statsProvider.OnStatsChanged += (s) => _latestStats = s;
            _toggleBtn?.onClick.AddListener(_controller.ToggleMode);
            _statsProvider.UpdateStats();
        }

        private void OnDestroy()
        {
            if (_statsProvider != null)
                _statsProvider.OnStatsChanged -= OnStatsUpdated;
            _toggleBtn?.onClick.RemoveAllListeners();
        }
        
        private void OnStatsUpdated(SimulationStats stats) => _latestStats = stats;

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= _updateInterval)
            {
                _timer = 0;
                RefreshVisuals();
            }
        }

        private void RefreshVisuals()
        {
            SetTextWithLabel(_execModeText, MODE_L, _latestStats.Mode.ToString());
            
            SetTextWithInt(_activeProjsText, ACT_P_L, _latestStats.ActiveProjs);
            SetTextWithInt(_activeTargetsText, ACT_T_L, _latestStats.ActiveTargets);
            SetTextWithInt(_totalCreatedProjsText, TOT_P_L, _latestStats.TotalProjs);
            SetTextWithInt(_totalCreatedTargetsText, TOT_T_L, _latestStats.TotalTargets);

            bool isPool = _latestStats.Mode == ExecutionMode.Pool;
            
            SetPoolInfo(_projsPoolSizeText, P_SIZE_L, _latestStats.ProjPoolSize, isPool);
            SetPoolInfo(_targetsPoolSizeText, T_SIZE_L, _latestStats.TargetPoolSize, isPool);
            SetPoolInfo(_availableProjsText, P_AVAIL_L, _latestStats.ProjAvailable, isPool);
            SetPoolInfo(_availableTargetsText, T_AVAIL_L, _latestStats.TargetAvailable, isPool);
            SetPoolInfo(_reusedProjsText, P_REUSED_L, _latestStats.ProjReused, isPool);
            SetPoolInfo(_reusedTargetsText, T_REUSED_L, _latestStats.TargetReused, isPool);
        }

        private void SetPoolInfo(TMP_Text tmp, string label, int value, bool isPool)
        {
            if (isPool) SetTextWithInt(tmp, label, value);
            else SetTextWithLabel(tmp, label, NA_VALUE);
        }

        private void SetTextWithInt(TMP_Text tmp, string label, int value)
        {
            _sb.Clear().Append(label).Append(value);
            tmp.SetText(_sb);
        }

        private void SetTextWithLabel(TMP_Text tmp, string label, string value)
        {
            _sb.Clear().Append(label).Append(value);
            tmp.SetText(_sb);
        }
    }
}