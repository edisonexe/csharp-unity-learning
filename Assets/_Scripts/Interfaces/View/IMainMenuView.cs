using System;

namespace _Scripts.Interfaces.View
{
    public interface IMainMenuView
    {
        event Action OnStart;
        event Action OnExit;
        void Show();
        void Hide();
    }
}