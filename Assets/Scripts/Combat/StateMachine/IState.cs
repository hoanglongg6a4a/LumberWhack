using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem
{
    public interface IState
    {
        void OnEnter(BaseCharacterController character);
        void OnExecute(BaseCharacterController character);
        void OnExit(BaseCharacterController character);
    }
}