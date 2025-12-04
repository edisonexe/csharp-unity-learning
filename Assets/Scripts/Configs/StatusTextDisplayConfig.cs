using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "StatusTextDisplayConfig", menuName = "Configs/Status Text Display Config")]
    public class StatusTextDisplayConfig : ScriptableObject
    {
        [SerializeField] private Color _successColor = Color.green;
        [SerializeField] private Color _errorColor = Color.red;
        [SerializeField] private Color _warningColor = Color.yellow;
        [SerializeField] private Color _infoColor = Color.black;

        public Color SuccessColor => _successColor;
        public Color ErrorColor => _errorColor;
        public Color WarningColor => _warningColor;
        public Color InfoColor => _infoColor;
    }
}