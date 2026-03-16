using System;
using _Project._Scripts.Gameplay.Combat;
using _Project._Scripts.Gameplay.Inventory;
using _Project._Scripts.Interfaces;
using _Project._Scripts.Interfaces.Views;

namespace _Project._Scripts.UI.Controllers
{
    public sealed class PlayerHudPresenter : IDisposable
    {
        private readonly IPlayerHudView _playerHudView;
        private readonly Health _health;
        private readonly PlayerInventory _inventory;

        public PlayerHudPresenter(IPlayerHudView playerHudView, Health health, PlayerInventory inventory)
        {
            _playerHudView = playerHudView;
            _health = health;
            _inventory = inventory;

            _health.HpChanged += OnHpChanged;
            _inventory.MedkitsCountChanged += OnMedkitsChanged;
            _inventory.GrenadesCountChanged += OnGrenadesChanged;

            _playerHudView.SetHp(_health.CurrentHp, _health.MaxHp);
            _playerHudView.SetMedkitsCount(_inventory.MedkitsCount);
            _playerHudView.SetGrenadesCount(_inventory.GrenadesCount);
        }

        private void OnHpChanged(int current, int max)
        {
            _playerHudView.SetHp(current, max);
        }

        private void OnMedkitsChanged(int value)
        {
            _playerHudView.SetMedkitsCount(value);
        }

        private void OnGrenadesChanged(int value)
        {
            _playerHudView.SetGrenadesCount(value);
        }

        public void Dispose()
        {
            _health.HpChanged -= OnHpChanged;
            _inventory.MedkitsCountChanged -= OnMedkitsChanged;
            _inventory.GrenadesCountChanged -= OnGrenadesChanged;
        }
    }
}