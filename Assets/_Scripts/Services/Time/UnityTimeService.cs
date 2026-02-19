using _Scripts.Interfaces;

namespace _Scripts.Services.Time
{
    public class UnityTimeService : ITimeService
    {
        public void Pause() => UnityEngine.Time.timeScale = 0;

        public void Resume() => UnityEngine.Time.timeScale = 1;
    }
}