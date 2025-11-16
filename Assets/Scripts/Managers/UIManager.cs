using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        [Header("UI Texts")]
        [SerializeField] private Text _targetsSpawnedText;
        [SerializeField] private Text _shotsText;
        [SerializeField] private Text _targetHitsText;
        
        [Header("GameEvents")]
        [SerializeField] private GameEvents _events;

        private int _targetsSpawned;
        private int _shots;
        private int _hits;
        private int _bestHits;
        
        private void OnEnable()
        {
            _events.OnTargetSpawned += TargetSpawned;
            _events.OnShotFired += ShotsFired;
            _events.OnTargetHit += TargetHit;
        }

        private void Start() => _bestHits = PlayerPrefs.GetInt(PrefsKeys.SCORE_HITS, 0);

        private void OnDisable()
        {
            _events.OnTargetSpawned -= TargetSpawned;
            _events.OnShotFired -= ShotsFired;
            _events.OnTargetHit -= TargetHit;
        }
        
        private void TargetSpawned()
        {
            _targetsSpawned ++;
            if (_targetsSpawnedText)
                _targetsSpawnedText.text = $"Targets spawned: {_targetsSpawned}";
        }
        
        private void TargetHit()
        {
            _hits ++;
            if (_targetHitsText)
                _targetHitsText.text = $"Hits: {_hits}";

            if (_hits <= _bestHits) return;
            _bestHits = _hits;
            PlayerPrefs.SetInt(PrefsKeys.SCORE_HITS, _bestHits);
            PlayerPrefs.SetInt(PrefsKeys.SCORE_SHOTS, _shots);
        }

        private void ShotsFired()
        {
            _shots++;
            if (_shotsText)
                _shotsText.text = $"Shots: {_shots}";
        }
    }
}