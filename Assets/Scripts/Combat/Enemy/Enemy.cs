using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem
{
    public abstract class Enemy : BaseCharacterController
    {
        private void Awake()
        {
            ChangeLayerMask(Constants.PlayerMask);
        }
        void Update()
        {
            //See the Update function of CharacterControl.cs for a comment on how we could replace
            //this (polling health) to a callback method.
            if (m_CharacterData.IsDeath)
            {
                DeathFrame();
                return;
            }

            if (m_Target == null) m_Target = GetClosestCharacter(targetMask);

            switch (m_State)
            {
                case State.WALKING:
                    {
                        if (m_Target != null)
                        {
                            m_State = State.ATTACKING;
                        }
                        else
                        {
                            Walk();
                        }
                    }
                    break;
                case State.ATTACKING:
                    {
                        if (!m_CharacterData.CanAttackReach(m_Target) || m_Target.IsDeath)
                        {
                            m_State = State.WALKING;
                            m_Target = null;
                        }
                        else
                        {
                            if (m_CharacterData.CanAttackTarget(m_Target))
                            {
                                m_CharacterData.AttackTriggered();
                                if(m_Hits == m_CharacterData.Stats.stats.specialHitAttack)
                                {
                                    SkillFrame(Constants.EnemyAttack2Animation);
                                }
                                else
                                {
                                    AttackFrame(Constants.EnemyAttack1Animation);
                                }
                                if (m_Target.IsDeath)
                                {
                                    m_State = State.WALKING;
                                }
                                else
                                {
                                    m_State = State.IDLE;
                                }
                            }
                        }
                    }
                    break;
                case State.IDLE:
                    {
                        if (m_IsAttacking) return;
                        if (m_Target == null || m_Target.IsDeath || !m_CharacterData.CanAttackReach(m_Target))
                        {
                            m_State = State.WALKING;
                        }
                        else if (m_CharacterData.CanAttackTarget(m_Target))
                        {
                            m_State = State.ATTACKING;
                        }
                    }
                    break;
            }
        }
    }
}
