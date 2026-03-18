using System.Collections.Generic;
using _Project._Scripts.Gameplay.Match;

namespace _Project._Scripts.Interfaces.Views
{
    public interface IMatchResultsView
    {
        void Show();
        void Hide();
        void SetRows(IReadOnlyList<LeaderboardEntryData> entries);
        void SetReturnButtonVisible(bool visible);
    }
}