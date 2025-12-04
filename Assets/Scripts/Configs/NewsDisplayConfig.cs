using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "NewsDisplayConfig", menuName = "Configs/News Display Config")]
    public class NewsDisplayConfig : ScriptableObject
    {
        [Header("Colors")]
        [SerializeField] private Color _titleColor = Color.blue;
        [SerializeField] private Color _contentColor = Color.black;
        [SerializeField] private Color _dateColor = Color.cyan;

        public Color TitleColor => _titleColor;
        public Color ContentColor => _contentColor;
        public Color DateColor => _dateColor;
    }
}
