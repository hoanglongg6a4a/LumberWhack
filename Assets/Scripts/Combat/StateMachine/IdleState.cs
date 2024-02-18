using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem
{
    public class IdleState : IState
    {
        float timer;
        float randomTime;

        public void OnEnter(BaseCharacterController character)
        {
            //randomTime = Random.Range(0.3f, 0.6f);
            //timer = 0;
            //if (!enemy.IsAttack || !enemy.IsDeath) enemy.Attack();
        }


        public void OnExecute(BaseCharacterController character)
        {
            //timer += Time.deltaTime;
            //if (timer >= randomTime)
            //{
            //    enemy.ChangeState(new IdleState());
            //}
        }

        public void OnExit(BaseCharacterController character)
        {

        }
    }
}
