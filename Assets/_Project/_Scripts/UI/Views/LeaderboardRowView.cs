using _Project._Scripts.Gameplay.Match;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project._Scripts.UI.Views
{
    public sealed class LeaderboardRowView : MonoBehaviour
    {
        private const string WINNER_LABEL = "WINNER";

        [SerializeField] private Image _backgroundImage;
        [SerializeField] private TMP_Text _placeText;
        [SerializeField] private TMP_Text _nicknameText;
        [SerializeField] private TMP_Text _killsText;
        [SerializeField] private TMP_Text _deathsText;
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private TMP_Text _winnerText;
        [SerializeField] private Color _normalBackgroundColor = new(0f, 0f, 0f, 0.35f);
        [SerializeField] private Color _winnerBackgroundColor = new(0.85f, 0.7f, 0.15f, 0.65f);

        private void Awake()
        {
            if (!_backgroundImage || !_placeText || !_nicknameText || !_killsText || !_deathsText ||
                !_scoreText || !_winnerText)
            {
                Debug.LogError("[LeaderboardRowView] References are not assigned.", this);
                enabled = false;
            }
        }
        
        public void Bind(LeaderboardEntryData entryData)
        {
            if (_placeText)
                _placeText.text = entryData.Place.ToString();

            if (_nicknameText)
                _nicknameText.text = entryData.Nickname;

            if (_killsText)
                _killsText.text = entryData.Kills.ToString();

            if (_deathsText)
                _deathsText.text = entryData.Deaths.ToString();

            if (_scoreText)
                _scoreText.text = entryData.Score.ToString();

            if (_winnerText)
            {
                _winnerText.gameObject.SetActive(true);
                _winnerText.text = entryData.IsWinner ? WINNER_LABEL : string.Empty;
            }

            if (_backgroundImage)
                _backgroundImage.color = entryData.IsWinner ? _winnerBackgroundColor : _normalBackgroundColor;
        }
    }
}