using System;
using System.Collections.Generic;
using _Project._Scripts.Interfaces;
using _Project._Scripts.Network;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project._Scripts.UI
{
    public class LobbyView : MonoBehaviour, ILobbyView
    {
        [SerializeField] private TMP_InputField _nicknameField;

        [SerializeField] private Button _readyButton;
        [SerializeField] private TMP_Text _readyBtnText;
        [SerializeField] private Button _startGameButton;

        [SerializeField] private Transform _playersListRoot;
        [SerializeField] private PlayerListItemView _playerItemPrefab;

        [SerializeField] private TMP_Dropdown _colorDropdown;
        private readonly Color[] _colors = 
        {
            Color.white, 
            Color.black, 
            Color.yellow, 
            Color.green, 
            Color.red
        };

        public event Action ReadyClicked;
        public event Action StartGameClicked;
        public event Action<string> NicknameChanged;
        public event Action<Color> ColorChanged;

        private readonly List<PlayerListItemView> _playerItems = new();
        private bool _isInitialized;

        public bool Init()
        {
            if (_isInitialized)
            {
                Debug.LogWarning("[LobbyView] Already initialized");
                return true;
            }

            if (!_nicknameField)
            {
                Debug.LogError("[LobbyView] Nickname field is not assigned.");
                return false;
            }

            if (!_colorDropdown)
            {
                Debug.LogError("[LobbyView] Color dropdown is not assigned.");
                return false;
            }

            if (_colors.Length == 0)
            {
                Debug.LogError("[LobbyView] Colors array is empty.");
                return false;
            }

            if (!_playerItemPrefab)
            {
                Debug.LogError("[LobbyView] Player item prefab is not assigned.");
                return false;
            }

            if (!_startGameButton || !_readyButton || !_readyBtnText)
            {
                Debug.LogError("[LobbyView] Buttons are not assigned.");
                return false;
            }

            if (!_playersListRoot)
            {
                Debug.LogError("[LobbyView] Players list root is not assigned.");
                return false;
            }

            _readyButton.onClick.AddListener(() => ReadyClicked?.Invoke());
            _startGameButton.onClick.AddListener(() => StartGameClicked?.Invoke());
            _nicknameField.onEndEdit.AddListener(v => NicknameChanged?.Invoke(v));
            _colorDropdown.onValueChanged.AddListener(OnColorChanged);

            _isInitialized = true;
            return true;
        }

        public void SetPlayers(IEnumerable<RoomPlayer> players, Func<RoomPlayer, bool> isHost)
        {
            if (!_playerItemPrefab || !_playersListRoot)
            {
                Debug.LogError("[LobbyView] Player list is not configured.");
                return;
            }
            
            foreach (var item in _playerItems)
            {
                if (item)
                    Destroy(item.gameObject);
            }

            _playerItems.Clear();

            if (players == null)
                return;

            foreach (var player in players)
            {
                if (!player)
                    continue;

                var item = Instantiate(_playerItemPrefab, _playersListRoot);
                item.Bind(player, isHost);
                _playerItems.Add(item);
            }
        }

        public void SetStartGameAvailable(bool available)
        {
            if (!_startGameButton) return;
            _startGameButton.interactable = available;
        }

        public void SetStartGameVisible(bool value)
        {
            if (!_startGameButton) return;
            _startGameButton.gameObject.SetActive(value);
        }

        public void SetReadyButtonText(string text)
        {
            if (_readyBtnText)
                _readyBtnText.text = text;
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void OnColorChanged(int index)
        {
            if (_colors.Length == 0)
            {
                Debug.LogError("[LobbyView] Colors array is empty.");
                return;
            }

            if (index < 0 || index >= _colors.Length)
            {
                Debug.LogError($"[LobbyView] Color index {index} is out of range. Colors count: {_colors.Length}");
                return;
            }

            ColorChanged?.Invoke(_colors[index]);
        }
    }
}