using System.Collections.Generic;
using UnityEngine;

namespace Actors.Enemy.AI
{
    public interface IEnemyStrategy
    {
        public void Init(Transform player, IReadOnlyList<Transform> patrolPoints, float speed, float stoppingDistance);
        public void Update(EnemyView enemy, float deltaTime);
    }    
}
