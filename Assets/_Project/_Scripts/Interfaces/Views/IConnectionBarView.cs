using System;

namespace _Project._Scripts.Interfaces.Views
{
    public interface IConnectionBarView
    {
        string Address { get; }
        event Action HostClicked;
        event Action ClientClicked;
        event Action StopClicked;
    
        void Show();
        void Hide();
        void ShowStatus(string status);
        void ShowError(string error);
        void ClearError();
    
        void SetConnectButtonsInteractable(bool value);
        void SetStopButtonInteractable(bool value);
        void SetAddressInteractable(bool value);
        void SetStopButtonVisible(bool value);

    }
}
