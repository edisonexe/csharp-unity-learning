using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class AudioManager : MonoBehaviour
    {
        [Header("Audio Settings")]
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _shootClip;
        [SerializeField] private AudioClip _targetHitClip;

        [Header("GameEvents")]
        [SerializeField] private GameEvents _events;

        private void OnEnable()
        {
            _events.OnShotFired += ShotFired;
            _events.OnTargetHit += TargetHit;
        }

        private void OnDisable()
        {
            _events.OnShotFired -= ShotFired;
            _events.OnTargetHit -= TargetHit;
        }
        
        private void TargetHit() => _audioSource.PlayOneShot(_targetHitClip);
        
        private void ShotFired() => _audioSource.PlayOneShot(_shootClip);
    }
}