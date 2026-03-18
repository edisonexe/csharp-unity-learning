using _Project._Scripts.Gameplay.Match;
using Mirror;
using UnityEngine;

namespace _Project._Scripts.UI.Controllers
{
    public sealed class ReturnToLobbyButton : MonoBehaviour
    {
        [SerializeField] private MatchManager _matchManager;

        public void OnClickReturnToLobby()
        {
            if (!NetworkServer.active)
                return;

            if (!_matchManager)
                return;

            _matchManager.ReturnToLobby();
        }
    }
}