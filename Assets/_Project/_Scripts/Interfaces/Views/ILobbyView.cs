using System;
using System.Collections.Generic;
using _Project._Scripts.Network;
using UnityEngine;

namespace _Project._Scripts.Interfaces.Views
{
    public interface ILobbyView
    {
        event Action ReadyClicked;
        event Action StartGameClicked;
        event Action<string> NicknameChanged;
        event Action<Color> ColorChanged;
        
        void SetPlayers(IEnumerable<RoomPlayer> players, Func<RoomPlayer, bool> isHost);
        void SetStartGameAvailable(bool available);
        void SetStartGameVisible(bool value);
        void SetReadyButtonText(string text);
        void SetNickname(string nickname);
        void SetSelectedColor(Color color);
        
        void Show();
        void Hide();
    }
}