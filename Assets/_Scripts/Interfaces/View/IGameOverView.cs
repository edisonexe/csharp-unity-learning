using System;

namespace _Scripts.Interfaces.View
{
    public interface IGameOverView
    {
        event Action OnRestart;
        event Action OnToMenu;
        void SetTitle(string title);
        void Show();
        void Hide();
    }
}