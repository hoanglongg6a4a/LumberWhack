using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem
{
    public class TreeRoot : BaseCharacterController
    {
        private void Awake()
        {
            ChangeLayerMask(Constants.EnemyMask);
        }
        // Update is called once per frame
        void Update()
        {
            //See the Update function of CharacterControl.cs for a comment on how we could replace
            //this (polling health) to a callback method.
            if (m_CharacterData.IsDeath)
            {
                DeathFrame();
                return;
            }
        }
        public void Hit()
        {

        }

        public override void SkillFrame(string animName)
        {

        }
    }
}

