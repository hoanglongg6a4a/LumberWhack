using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem
{
    public abstract class Soldier : BaseCharacterController
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
                                AttackFrame(Constants.SoldierAttack1Animation);
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
                        if (m_Target == null || !m_CharacterData.CanAttackReach(m_Target))
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

