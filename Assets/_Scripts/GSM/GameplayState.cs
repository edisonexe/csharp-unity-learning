using System.Collections.Generic;
using _Scripts.Configs;
using _Scripts.GameFlow;
using _Scripts.Gameplay.GameModifiers;
using _Scripts.Interfaces;
using _Scripts.Interfaces.Input;
using _Scripts.Interfaces.Spawn;
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
        private readonly ModifierRunner _modifierRunner;
        private readonly IReadOnlyList<string> _modifierNames;

        public GameplayState(ILoggerService logger, IGameplayHud hud, GameStateMachine gsm, ISpawner spawner,
            IInputModeProvider inputMode, IHealth health, IScore score, GameRulesConfig gameCfg, 
            IGameResult gameResult, IInputToggle inputToggle, ModifierRunner modifierRunner, 
            IReadOnlyList<string> modifierNames)
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
            _modifierRunner = modifierRunner;
            _modifierNames = modifierNames;
        }
        
        public void Enter()
        {
            _inputToggle.Enable();
            
            _health.Changed += HealthHandler;
            _health.Died += DeathHandler;
            _score.Changed += ScoreHandler;
            _hud.SetModifiers(_modifierNames);
            
            _hud.Show();
            
            _hud.SetHp(_health.Current, _health.Max);
            _hud.SetScore(_score.Current);
            _hud.SetInputMode(_inputMode.CurrentMode.ToString());
            
            _modifierRunner.Activate();
            
            _spawner.StartSpawning();
            _logger.Log("[GameplayState]: Enter");
        }

        public void Exit()
        {
            _spawner.StopSpawning();
            _modifierRunner.Deactivate();
            
            _health.Changed -= HealthHandler;
            _health.Died -= DeathHandler;
            _score.Changed -= ScoreHandler;
            
            _hud.Hide();
            _logger.Log("[GameplayState]: Exit");
        }

        private void HealthHandler(int cur, int max) => _hud.SetHp(cur, max);

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