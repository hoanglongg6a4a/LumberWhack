using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace GameSystem
{
    public class Hunter : Enemy
    {
        [SerializeField] private Transform bulletSpawner;
        [SerializeField] private GameObject bulletPrefab;
        public override void SkillFrame(string animName)
        {
            AttackFrame(animName, true);
            m_Hits = 0;
        }
        public override void AttackFrame(string animName, bool isSpecial = false)
        {
            //if we can't reach the player anymore when it's time to damage, then that attack miss.
            if (m_Target.IsDeath) return;
            float attackCD = m_CharacterData.Stats.stats.attackCooldown;
            float attackAnimDuration = 1.067f;
            m_IsAttacking = true;
            m_Hits++;
            ChangeSkeletonAnimation(animName, false, () => {
                GameObject bullet = SimplePool.Spawn(bulletPrefab, bulletSpawner.position, Quaternion.identity);
                bullet.transform.localScale = isSpecial ? Vector3.one : Vector3.one * 0.7f;
                bullet.transform.DOMove(new Vector2(m_Target.transform.position.x, bulletSpawner.position.y), 1.067f / 2f).OnComplete(() => {
                    SimplePool.Despawn(bullet);
                    m_CharacterData.Attack(m_Target, isSpecial);
                    if (m_Target.IsDeath)
                    {
                        m_Target = null;
                    }
                    Idle();
                    m_IsAttacking = false;
                });
            });
            
            if (attackCD < attackAnimDuration) ChangeSkeletonAnimationTimeScale(m_CharacterData.Stats.stats.attackCooldown / 1.067f);
        }
    }
}
