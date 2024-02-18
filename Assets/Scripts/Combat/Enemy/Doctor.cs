using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem
{
    public class Doctor : Enemy
    {
        public override void SkillFrame(string animName)
        {
            if (m_Target.IsDeath) return;
            CharacterData target = GetLowestHpCharacter();
            ShowDebug("Heal target " + target.name);

            float attackCD = m_CharacterData.Stats.stats.attackCooldown;
            float attackAnimDuration = 1.067f;
            m_IsAttacking = true;
            ChangeSkeletonAnimation(animName, false, () => {
                target.Stats.ChangeHealth(m_CharacterData.Stats.stats.strength * m_CharacterData.Stats.stats.multiplierSpecialAttack);
                m_IsAttacking = false;
                Idle();
            });
            if (attackCD < attackAnimDuration) ChangeSkeletonAnimationTimeScale(m_CharacterData.Stats.stats.attackCooldown / 1.067f);
            m_Hits = 0;
        }
        public CharacterData GetLowestHpCharacter()
        {
            int layerInt = LayerMask.NameToLayer(Constants.EnemyMask);
            LayerMask layer = 1 << layerInt;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, m_CharacterData.Stats.stats.range, layer);
            CharacterData target = m_CharacterData;
            foreach (var item in colliders)
            {
                CharacterData chara = item.GetComponent<CharacterData>();
                if (target.Stats.CurrentHealth < chara.Stats.CurrentHealth && !chara.IsDeath)
                {
                    target = chara;
                }
            }
            return target;
        }
    }
}
