using _Project._Scripts.Interfaces;
using _Project._Scripts.Interfaces.Views;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project._Scripts.UI.Views
{
    public sealed class PlayerHudView : MonoBehaviour, IPlayerHudView
    {
        [SerializeField] private TMP_Text _hpText;
        [SerializeField] private Image _hpFill;
        [SerializeField] private TMP_Text _medkitsText;
        [SerializeField] private TMP_Text _grenadesText;

        private void Awake()
        {
            if (!_hpText)
            {
                Debug.LogError("[PlayerHudView] HpText is not assigned.", this);
                enabled = false;
                return;
            }

            if (!_hpFill)
            {
                Debug.LogError("[PlayerHudView] HpFill is not assigned.", this);
                enabled = false;
                return;
            }

            if (!_medkitsText)
            {
                Debug.LogError("[PlayerHudView] MedkitsText is not assigned.", this);
                enabled = false;
                return;
            }

            if (!_grenadesText)
            {
                Debug.LogError("[PlayerHudView] GrenadesText is not assigned.", this);
                enabled = false;
            }
        }

        public void SetHp(int cur, int max)
        {
            float ratio = max > 0 ? (float)cur / max : 0f;

            if (_hpFill)
                _hpFill.fillAmount = ratio;

            if (_hpText)
                _hpText.text = $"{cur} / {max}";
        }

        public void SetMedkitsCount(int count)
        {
            if (_medkitsText)
                _medkitsText.text = $"Medkits: {count}";
        }

        public void SetGrenadesCount(int count)
        {
            if (_grenadesText)
                _grenadesText.text = $"Grenades: {count}";
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}