using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem
{
    public class WoodSawyer2 : Enemy
    {
        public override void SkillFrame(string animName)
        {
            base.AttackFrame(animName,true);
            m_Hits = 0;
        }
    }
}
