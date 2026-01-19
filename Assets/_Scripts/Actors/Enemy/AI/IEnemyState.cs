namespace Actors.Enemy.AI
{
    public interface IEnemyState
    {
        void Update(EnemyView enemy, float deltaTime);
    }    
}
