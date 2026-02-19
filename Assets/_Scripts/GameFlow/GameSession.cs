using _Scripts.Interfaces;
using UnityEngine;

namespace _Scripts.GameFlow
{
    public class GameSession : IGameSession
    {
        private readonly IHealth _health;
        private readonly IScore _score;
        private readonly IGameResult _result;
        private readonly ISpawner _spawner;
        private readonly Rigidbody _playerRb;
        private readonly Transform _playerStart;

        public GameSession(IHealth health, IScore score, IGameResult result, ISpawner spawner, Rigidbody playerRb,
            Transform playerStart)
        {
            _health = health;
            _score = score;
            _result = result;
            _spawner = spawner;
            _playerRb = playerRb;
            _playerStart = playerStart;
        }

        public void Restart()
        {
            _spawner.StopSpawning();
            _spawner.ClearSpawned();

            
            _health.Reset();
            _score.Reset();
            _result.Set(GameResult.None);

            _playerRb.linearVelocity = Vector3.zero;
            _playerRb.angularVelocity = Vector3.zero;
            _playerRb.transform.position = _playerStart.position;
        }
    }
}