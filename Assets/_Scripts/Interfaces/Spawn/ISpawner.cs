namespace _Scripts.Interfaces.Spawn
{
    public interface ISpawner
    {
        void StartSpawning();
        void StopSpawning();
        void ClearSpawned();
    }
}