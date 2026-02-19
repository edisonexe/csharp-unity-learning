using System;

namespace _Scripts.Interfaces.View
{
    public interface IPauseView
    {
        event Action OnResume;
        event Action OnToMenu;
        void Show();
        void Hide();
    }
}