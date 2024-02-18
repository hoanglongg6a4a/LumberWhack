using UnityEngine;
using GameSystemInternal;
using Spine.Unity;
using UnityEngine.Events;

namespace GameSystem
{
    public abstract class BaseCharacterController : MonoBehaviour
    {
        public enum State
        {
            IDLE,
            WALKING,
            ATTACKING,
            NONE
        }

        public enum MoveDirection
        {
            LEFT,
            RIGHT,
            STOP
        }

        [SerializeField] private MoveDirection moveDirection;
        [SerializeField] private SkeletonAnimation skeletonAnimation;
        [SerializeField] private bool isDebug;

        protected LayerMask targetMask;
        protected IState m_CurrentStateMachine;
        protected CharacterData m_CharacterData;
        protected CharacterData m_Target;
        protected CharacterAudio m_CharacterAudio;
        protected State m_State;
        protected State m_LastState = State.NONE;
        protected LootSpawner m_LootSpawner;
        protected bool m_IsDeath;
        protected bool m_IsAttacking;
        protected int m_Hits = 0;

        // Start is called before the first frame update
        void Start()
        {
            m_CharacterData = GetComponent<CharacterData>();
            m_CharacterData.Init();

            m_CharacterAudio = GetComponentInChildren<CharacterAudio>();

            m_CharacterData.OnDamage += () =>
            {
                //m_Animator.SetTrigger(m_HitAnimHash);
                m_CharacterAudio.Hit(transform.position);
            };

            m_LootSpawner = GetComponent<LootSpawner>();
            m_State = State.WALKING;
        }
        public void ChangeLayerMask(string name)
        {
            int layer = LayerMask.NameToLayer(name);
            targetMask = 1 << layer;
        }

        public CharacterData GetClosestCharacter(LayerMask layerMask)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position , m_CharacterData.Stats.stats.range , layerMask);
            if (colliders.Length == 0)
            {
                return null;
            }
            float curdis = 100;
            CharacterData m_Target = null;
            foreach (var item in colliders)
            {
                float distance = Vector2.Distance(item.transform.position , transform.position);
                CharacterData chara = item.GetComponent<CharacterData>();
                if (distance < curdis && !chara.IsDeath)
                {
                    curdis = distance;
                    m_Target = chara;
                }
            }
            if (m_Target != null) ShowDebug("Find target " + m_Target.name);
            return m_Target;
        }
        public void ChangeSkeletonAnimation(string state , bool isLoop = false , UnityAction endAction = null)
        {
            if (m_LastState == m_State || skeletonAnimation == null) return;
            ChangeSkeletonAnimationTimeScale(1);
            m_LastState = m_State;
            ShowDebug(this.name + " " + m_State);
            Spine.TrackEntry entry = skeletonAnimation.AnimationState.SetAnimation(0, state, isLoop);
            entry.Complete += (entry) => {
                endAction?.Invoke();
            };
        }
        public void ChangeSkeletonAnimationTimeScale(float scale)
        {
            if (skeletonAnimation == null) return;
            skeletonAnimation.timeScale = scale;
        }
        public void Walk()
        {
            ChangeSkeletonAnimation(Constants.SoldierWalkAnimation,true);
            ChangeSkeletonAnimationTimeScale(m_CharacterData.Stats.stats.moveSpeed / 1.067f);
            Vector2 dir = moveDirection == MoveDirection.LEFT ? Vector2.left : Vector2.right;
            float step = m_CharacterData.Stats.stats.moveSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + dir, step);
        }
        public void Idle()
        {
            ChangeSkeletonAnimation(Constants.SoldierIdleAnimation, true);
        }
        public virtual void AttackFrame(string animName, bool isSpecial = false)
        {
            //if we can't reach the player anymore when it's time to damage, then that attack miss.
            if (m_Target.IsDeath) return;
            float attackCD = m_CharacterData.Stats.stats.attackCooldown;
            float attackAnimDuration = 1.067f;
            m_IsAttacking = true;
            m_Hits++;
            ChangeSkeletonAnimation(animName, false, () => {
                m_CharacterData.Attack(m_Target , isSpecial);
                if (m_Target.IsDeath)
                {
                    m_Target = null;
                }
                m_IsAttacking = false;
                Idle();
            });
            if (attackCD < attackAnimDuration) ChangeSkeletonAnimationTimeScale(m_CharacterData.Stats.stats.attackCooldown / 1.067f);
        }
        public abstract void SkillFrame(string animName);
        public virtual void DeathFrame()
        {
            if (m_IsDeath) return;
            ChangeSkeletonAnimation(Constants.SoldierDieAnimation, false, () => {
                if (m_LootSpawner != null) m_LootSpawner.SpawnLoot();
                SimplePool.Despawn(gameObject);
                ShowDebug(this.name + " dead");
            });

            m_IsDeath = true;
            m_CharacterAudio.Death(transform.position);
            m_CharacterData.Death();
        }
        protected void ShowDebug(string log)
        {
            if (!isDebug) return;
            Logger.Debug(log);
        }
    }
}