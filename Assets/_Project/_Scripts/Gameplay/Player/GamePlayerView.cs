using _Project._Scripts.Interfaces;
using TMPro;
using UnityEngine;

namespace _Project._Scripts.Gameplay.Player
{
    public sealed class GamePlayerView : MonoBehaviour, IPlayerView
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private TMP_Text _nicknameText;
        [SerializeField] private UnityEngine.Camera _playerCamera;
        [SerializeField] private GameObject _localVisualRoot;
        [SerializeField] private GameObject _remoteVisualRoot;

        private MaterialPropertyBlock _propertyBlock;

        public void SetNickname(string nickname)
        {
            if (_nicknameText)
                _nicknameText.text = nickname;
        }

        public void SetColor(Color color)
        {
            if (!_renderer)
                return;

            _propertyBlock ??= new MaterialPropertyBlock();
            _renderer.GetPropertyBlock(_propertyBlock);
            _propertyBlock.SetColor("_BaseColor", color);
            _renderer.SetPropertyBlock(_propertyBlock);
        }

        public void SetLocalState(bool isLocal)
        {
            if (_playerCamera)
                _playerCamera.gameObject.SetActive(isLocal);

            if (_localVisualRoot)
                _localVisualRoot.SetActive(isLocal);

            if (_remoteVisualRoot)
                _remoteVisualRoot.SetActive(!isLocal);
        }
    }
}