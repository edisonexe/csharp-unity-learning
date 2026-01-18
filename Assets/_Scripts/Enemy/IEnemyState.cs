namespace Enemy
{
    public interface IEnemyState
    {
        void Update(EnemyView enemy, float deltaTime);
    }    
}
