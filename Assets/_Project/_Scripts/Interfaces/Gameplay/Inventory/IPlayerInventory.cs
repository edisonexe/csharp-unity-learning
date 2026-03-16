using System;

namespace _Project._Scripts.Interfaces.Gameplay.Inventory
{
    public interface IPlayerInventory
    {
        int MedkitsCount { get; }
        int GrenadesCount { get; }
        
        event Action<int> MedkitsCountChanged;
        event Action<int> GrenadesCountChanged;
    }
}