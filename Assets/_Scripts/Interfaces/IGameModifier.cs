namespace _Scripts.Interfaces
{
    public interface IGameModifier
    {
        void OnEnterGameplay();
        void OnExitGameplay();
        void Tick(float deltaTime);
    }
}