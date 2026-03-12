using _Project._Scripts.Gameplay.Player;
using UnityEngine;

namespace _Project._Scripts.Network.Player
{
    public sealed class GamePlayerPresentation
    {
        private readonly GamePlayerView _view;

        public GamePlayerPresentation(GamePlayerView view)
        {
            _view = view;
        }

        public void Apply(string nickname, Color color, bool isLocalPlayer)
        {
            if (_view == null)
                return;

            _view.SetNickname(nickname);
            _view.SetColor(color);
            _view.SetLocalState(isLocalPlayer);
        }

        public void SetNickname(string nickname)
        {
            _view?.SetNickname(nickname);
        }

        public void SetColor(Color color)
        {
            _view?.SetColor(color);
        }
    }
}