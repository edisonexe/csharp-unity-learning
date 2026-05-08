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
        [SerializeField] private Button _toggleBtn;
        
        private IStatsProvider _statsProvider;
        private ISimulationController _controller;
        private SimulationStats _latestStats;
        
        private static readonly string[] _modeDisplayStrings = { "Mode: Naive", "Mode: Pool" };
        
        private const string ACT_P_F = "Active Proj: {0}";
        private const string ACT_T_F = "Active Targets: {0}";
        private const string TOT_P_F = "Total Created Proj: {0}";
        private const string TOT_T_F = "Total Created Targets: {0}";
        
        private const string P_SIZE_F = "Proj Pool Size: {0}";
        private const string T_SIZE_F = "Target Pool Size: {0}";
        private const string P_AVAIL_F = "Available Proj: {0}";
        private const string T_AVAIL_F = "Available Targets: {0}";
        private const string P_REUSED_F = "Reused Proj: {0}";
        private const string T_REUSED_F = "Reused Targets: {0}";
        
        private const string NA_VALUE = "N/A";

        public void Init(IStatsProvider statsProvider, ISimulationController controller)
        {
            _statsProvider = statsProvider ?? throw new System.ArgumentNullException(nameof(statsProvider));
            _controller = controller ?? throw new System.ArgumentNullException(nameof(controller));
            
            _statsProvider.OnStatsChanged += OnStatsUpdated;
            _toggleBtn?.onClick.AddListener(_controller.ToggleMode);
            
            _statsProvider.UpdateStats();
        }

        private void OnDestroy()
        {
            if (_statsProvider != null)
                _statsProvider.OnStatsChanged -= OnStatsUpdated;
            
            if (_toggleBtn)
                _toggleBtn.onClick.RemoveAllListeners();
        }
        
        private void OnStatsUpdated(SimulationStats stats)
        {
            _latestStats = stats;
            RefreshVisuals();
        }

        private void RefreshVisuals()
        {
            _execModeText.text = _modeDisplayStrings[(int)_latestStats.Mode];
            
            _activeProjsText.SetText(ACT_P_F, _latestStats.ActiveProjs);
            _activeTargetsText.SetText(ACT_T_F, _latestStats.ActiveTargets);
            _totalCreatedProjsText.SetText(TOT_P_F, _latestStats.TotalProjs);
            _totalCreatedTargetsText.SetText(TOT_T_F, _latestStats.TotalTargets);

            bool isPool = _latestStats.Mode == ExecutionMode.Pool;
            
            UpdatePoolInfo(_projsPoolSizeText, P_SIZE_F, _latestStats.ProjPoolSize, isPool);
            UpdatePoolInfo(_targetsPoolSizeText, T_SIZE_F, _latestStats.TargetPoolSize, isPool);
            UpdatePoolInfo(_availableProjsText, P_AVAIL_F, _latestStats.ProjAvailable, isPool);
            UpdatePoolInfo(_availableTargetsText, T_AVAIL_F, _latestStats.TargetAvailable, isPool);
            UpdatePoolInfo(_reusedProjsText, P_REUSED_F, _latestStats.ProjReused, isPool);
            UpdatePoolInfo(_reusedTargetsText, T_REUSED_F, _latestStats.TargetReused, isPool);
        }

        private void UpdatePoolInfo(TMP_Text tmp, string format, int value, bool isPool)
        {
            if (isPool) 
                tmp.SetText(format, value);
            else 
                tmp.text = NA_VALUE;
        }
    }
}