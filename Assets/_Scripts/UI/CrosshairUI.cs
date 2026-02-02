using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class CrosshairUI : MonoBehaviour
    {
        [SerializeField] private RectTransform _crosshair;

        private void Awake()
        {
            if  (!_crosshair)
                _crosshair = GetComponent<RectTransform>();
        }

        private void Update()
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            _crosshair.position = mousePos;
        }
    }
}
