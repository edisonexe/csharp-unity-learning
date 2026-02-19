using _Scripts.Configs;
using _Scripts.GameFlow;
using _Scripts.Interfaces;
using _Scripts.Interfaces.Input;
using _Scripts.Interfaces.View;

namespace _Scripts.GSM
{
    public sealed class GameplayState : IGameState
    {
        private readonly IGameplayHud _hud;
        private readonly GameStateMachine _gsm;
        private readonly ISpawner _spawner;
        private readonly IInputModeProvider _inputMode;
        private readonly IHealth _health;
        private readonly IScore _score;
        private readonly ILoggerService _logger;
        private readonly GameRulesConfig _gameCfg;
        private readonly IGameResult _gameResult;
        private readonly IInputToggle _inputToggle;

        public GameplayState(ILoggerService logger, IGameplayHud hud, GameStateMachine gsm, ISpawner spawner,
            IInputModeProvider inputMode, IHealth health, IScore score, GameRulesConfig gameCfg, 
            IGameResult gameResult, IInputToggle inputToggle)
        {
            _hud = hud;
            _gsm = gsm;
            _spawner = spawner;
            _health = health;
            _score = score;
            _inputMode = inputMode;
            _logger = logger;
            _gameCfg = gameCfg;
            _gameResult = gameResult;
            _inputToggle = inputToggle;
        }
        
        public void Enter()
        {
            _inputToggle.Enable();
            _hud.Show();
            _hud.SetHp(_health.Current, _health.Max);
            _hud.SetScore(_score.Current);
            _hud.SetInputMode(_inputMode.CurrentMode.ToString());

            _health.Damaged += HealthHandler;
            _health.Died += DeathHandler;
            _score.Changed += ScoreHandler;
            _logger.Log("Enter GameplayState");
            _spawner.StartSpawning();
            
        }

        public void Exit()
        {
            _health.Damaged -= HealthHandler;
            _health.Died -= DeathHandler;
            _score.Changed -= ScoreHandler;
            _spawner.StopSpawning();
            _hud.Hide();
            _logger.Log("Exit GameplayState");
        }

        private void HealthHandler(int amount)
        {
            _hud.SetHp(_health.Current, _health.Max);
        }

        private void DeathHandler()
        {
            _gameResult.Set(GameResult.Lose);
            _gsm.ChangeState<GameOverState>();
        }
        private void ScoreHandler(int amount)
        {
            _hud.SetScore(amount);
            if (amount < _gameCfg.TargetScore) return;
            _gameResult.Set(GameResult.Win);
            _gsm.ChangeState<GameOverState>();
        }
    }
}