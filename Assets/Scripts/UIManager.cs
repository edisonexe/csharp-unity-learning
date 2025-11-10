using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class UIManager : MonoBehaviour
    {
        [Header("UI Texts")]
        [SerializeField] private Text _targetsSpawnedText;
        [SerializeField] private Text _shotsText;
        [SerializeField] private Text _targetHitsText;
        
        [Header("GameEvents")]
        [SerializeField] private GameEvents _events;

        private int _shots;
        private int _hits;
        
        private void OnEnable()
        {
            _events.OnShotFired += ShotsFired;
            _events.OnTargetHit += TargetHit;
        }

        private void OnDisable()
        {
            _events.OnShotFired -= ShotsFired;
            _events.OnTargetHit -= TargetHit;
        }
        
        private void TargetHit()
        {
            _hits ++;
            if (_targetHitsText)
                _targetHitsText.text = $"Hits: {_hits}";
        }

        private void ShotsFired()
        {
            _shots++;
            if (_shotsText)
                _shotsText.text = $"Shots {_shots}";
        }
    }
}