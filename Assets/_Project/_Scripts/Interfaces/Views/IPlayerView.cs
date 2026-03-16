using UnityEngine;

namespace _Project._Scripts.Interfaces.Views
{
    public interface IPlayerView
    {
        void SetNickname(string nickname);
        void SetColor(Color color);
        void SetLocalState(bool isLocal);
    }
}