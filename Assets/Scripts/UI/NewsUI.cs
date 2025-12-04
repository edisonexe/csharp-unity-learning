using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Configs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class NewsUI : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private TextMeshProUGUI _statusText;
        [SerializeField] private GameObject _loadingSpinner;
        [SerializeField] private Button _refreshButton;
        [SerializeField] private TextMeshProUGUI _newsText;
    
        [Header("Display configs")]
        [SerializeField] private NewsDisplayConfig _newsDisplayCfg;
        [SerializeField] private StatusTextDisplayConfig _statusDisplayCfg;
        
        private NewsLoader _loader;
        private Coroutine _showCoroutine;
        private readonly List<NewsItem> _newsList = new();
        
        private bool _hadErrorDuringLoad;
    
        private void Start()
        {
            _loader = new NewsLoader(OnLoadError);
            _refreshButton.onClick.AddListener(OnRefreshClicked);

            LoadNews();
        }

        private void OnRefreshClicked() => LoadNews();

        private async void LoadNews()
        {
            _hadErrorDuringLoad = false;
        
            if (_showCoroutine != null)
            {
                StopCoroutine(_showCoroutine);
                _showCoroutine = null;
            }
        
            SetLoading(true);
            ClearNewsText();
        
            ShowStatus("Загрузка новостей...", _statusDisplayCfg.InfoColor);
        
            _newsList.Clear();
            _newsList.AddRange(await _loader.LoadNewsAsync());

            await Task.Delay(TimeSpan.FromSeconds(3)); 
        
            SetLoading(false);

            if (_hadErrorDuringLoad)
                return;
        
            if (_newsList.Count == 0)
            {
                ShowStatus("Нет новостей", _statusDisplayCfg.WarningColor);
                return;
            }

            ShowStatus($"Загружено {_newsList.Count} новостей", _statusDisplayCfg.SuccessColor);
        
            _showCoroutine = StartCoroutine(ShowNewsCoroutine());
        }

        private IEnumerator ShowNewsCoroutine()
        {
            ClearNewsText();

            foreach (var item in _newsList)
            {
                _newsText.text += 
                    $"<color=#{ColorUtility.ToHtmlStringRGBA(_newsDisplayCfg.TitleColor)}>{item.SafeTitle}</color>\n" +
                    $"<color=#{ColorUtility.ToHtmlStringRGBA(_newsDisplayCfg.ContentColor)}>{item.SafeContent}</color>\n" +
                    $"<color=#{ColorUtility.ToHtmlStringRGBA(_newsDisplayCfg.DateColor)}>{item.SafeDate}</color>\n\n";

                yield return new WaitForSeconds(2f);
            }
        }

        private void ClearNewsText() => _newsText.text = "";

        private void ShowStatus(string msg, Color color)
        {
            if (_statusText == null) return;

            _statusText.color = color;
            _statusText.text = msg;
        }

        private void OnLoadError(string msg)
        {
            _hadErrorDuringLoad = true;
            ShowStatus(msg, _statusDisplayCfg.ErrorColor);
        }
    
        private void SetLoading(bool value)
        {
            _loadingSpinner.SetActive(value);
            _refreshButton.interactable = !value;
        }
    }
}
