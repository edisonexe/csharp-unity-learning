using System;
using _Project._Scripts.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project._Scripts.UI.Views
{
    public class ConnectionBarView : MonoBehaviour, IConnectionBarView
    {
        [SerializeField] private TMP_InputField _addressField;
        [SerializeField] private Button _hostButton;
        [SerializeField] private Button _clientButton;
        [SerializeField] private Button _stopButton;
        [SerializeField] private TMP_Text _statusText;
        [SerializeField] private TMP_Text _errorText;

        private bool _isInitialized;

        public string Address => _addressField ? _addressField.text : string.Empty;

        public event Action HostClicked;
        public event Action ClientClicked;
        public event Action StopClicked;

        public bool Init()
        {
            if (_isInitialized)
            {
                Debug.LogWarning("[ConnectionBarView] Already initialized");
                return true;
            }

            if (!_addressField)
            {
                Debug.LogError("[ConnectionBarView] Address field is not assigned.");
                return false;
            }
            
            if (!_hostButton || !_clientButton || !_stopButton)
            {
                Debug.LogError("[ConnectionBarView] Buttons are not assigned");
                return false;
            }

            if (!_statusText || !_errorText)
            {
                Debug.LogError("[ConnectionBarView] Status/Error text is not assigned");
                return false;
            }

            ClearLogs();

            _hostButton.onClick.AddListener(() => HostClicked?.Invoke());
            _clientButton.onClick.AddListener(() => ClientClicked?.Invoke());
            _stopButton.onClick.AddListener(() => StopClicked?.Invoke());

            _isInitialized = true;
            return true;
        }

        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);
        
        public void SetConnectButtonsInteractable(bool value)
        {
            if (_hostButton) _hostButton.interactable = value;
            if (_clientButton) _clientButton.interactable = value;
        }

        public void SetStopButtonInteractable(bool value)
        {
            if (_stopButton) _stopButton.interactable = value;
        }

        public void SetStopButtonVisible(bool value)
        {
            if (_stopButton) _stopButton.gameObject.SetActive(value);
        }

        public void SetAddressInteractable(bool value)
        {
            if (_addressField) _addressField.interactable = value;
        }
        
        public void ShowStatus(string message)
        {
            if (!_statusText) return;

            _statusText.text = message;
            _statusText.gameObject.SetActive(!string.IsNullOrWhiteSpace(message));
        }

        public void ShowError(string message)
        {
            if (!_errorText) return;

            _errorText.text = message;
            _errorText.gameObject.SetActive(!string.IsNullOrWhiteSpace(message));
        }

        public void ClearError()
        {
            if (!_errorText) return;

            _errorText.text = string.Empty;
            _errorText.gameObject.SetActive(false);
        }

        private void ClearStatus()
        {
            if (!_statusText) return;

            _statusText.text = string.Empty;
            _statusText.gameObject.SetActive(false);
        }
        
        private void ClearLogs()
        {
            ClearStatus();
            ClearError();
        }
    }
}