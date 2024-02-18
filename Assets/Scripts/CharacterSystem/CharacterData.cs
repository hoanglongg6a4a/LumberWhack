using System;
using UnityEngine;
using UnityEngine.UI;
using GameSystemInternal;

namespace GameSystem 
{
    /// <summary>
    /// This defines a character in the game .
    /// can be attacked and have some stats including health. It could also be an inanimate object like a breakable box.
    /// </summary>
    public class CharacterData : MonoBehaviour
    {
        public string CharacterName;

        public CharacterBaseStat characterBaseStat;
        public StatSystem Stats;
        public Slider hpBar;
        public bool isDebug;
    
        /// <summary>
        /// Callback for when that CharacterData receive damage. E.g. used by the player character to trigger the right
        /// animation
        /// </summary>
        public Action OnDamage { get; set; }

        /// <summary>
        /// Will return true if the attack cooldown have reached 0. False otherwise.
        /// </summary>
        public bool CanAttack
        {
            get { return m_AttackCoolDown <= 0.0f; }
        }
        public bool IsDeath => Stats.CurrentHealth <= 0;
        float m_AttackCoolDown;

        public void Init()
        {
            Stats.Init(this,characterBaseStat.GetStats());
        }

        void Awake()
        {

        }
        private void OnEnable()
        {
            m_AttackCoolDown = 0;
        }
        // Update is called once per frame
        void Update()
        {
            Stats.Tick();
            hpBar.value = Stats.CurrentPercentageHealth;

            if (m_AttackCoolDown > 0.0f) m_AttackCoolDown -= Time.deltaTime;
        }
        private void OnValidate()
        {
            Stats.Init(this,characterBaseStat.GetStats());
        }

        /// <summary>
        /// Will check if that CharacterData can reach the given target with its currently equipped weapon. Will rarely
        /// be called, as the function CanAttackTarget will call this AND also check if the cooldown is finished.
        /// </summary>
        /// <param name="target">The CharacterData you want to reach</param>
        /// <returns>True if you can reach the target, False otherwise</returns>
        public bool CanAttackReach(CharacterData target)
        {
            return Vector2.Distance(transform.position , target.transform.position) <= Stats.stats.range + 1.5f;
        }

        /// <summary>
        /// Will check if the target is attackable. This in effect check :
        /// - If the target is in range of the weapon
        /// - If this character attack cooldown is finished
        /// - If the target isn't already dead
        /// </summary>
        /// <param name="target">The CharacterData you want to reach</param>
        /// <returns>True if the target can be attacked, false if any of the condition isn't met</returns>
        public bool CanAttackTarget(CharacterData target)
        {
            if (target.Stats.CurrentHealth == 0)
                return false;
        
            if (!CanAttackReach(target))
                return false;

            if (m_AttackCoolDown > 0.0f)
                return false;

            return true;
        }

        /// <summary>
        /// Call when the character die (health reach 0).
        /// </summary>
        public void Death()
        {
            Stats.Death();
        }

        /// <summary>
        /// Attack the given target. NOTE : this WON'T check if the target CAN be attacked, you should make sure before
        /// with the CanAttackTarget function.
        /// </summary>
        /// <param name="target">The CharacterData you want to attack</param>
        public void Attack(CharacterData target, bool isSpecial = false)
        {
            //TODO
            AttackData attackData = new AttackData(target, this);
            float damage = isSpecial ? Stats.stats.strength * Stats.stats.multiplierSpecialAttack : Stats.stats.strength;
            attackData.AddDamage(StatSystem.DamageType.Physical, damage);
            target.Damage(attackData);
        }

        /// <summary>
        /// This need to be called as soon as an attack is triggered, it will start the cooldown. This is separate from
        /// the actual Attack function as AttackTriggered will be called at the beginning of the animation while the
        /// Attack function (doing the actual attack and damage) will be called by an animation event to match the animation
        /// </summary>
        public void AttackTriggered()
        {
            m_AttackCoolDown = Stats.stats.attackCooldown;
        }

        /// <summary>
        /// Damage the Character by the AttackData given as parameter. See the documentation for that class for how to
        /// add damage to that attackData. (this will be done automatically by weapons, but you may need to fill it
        /// manually when writing special elemental effect)
        /// </summary>
        /// <param name="attackData"></param>
        public void Damage(AttackData attackData)
        {
            ShowDebug(attackData.Source.name + " attack "+ attackData.Target.name + " : " + attackData.GetFullDamage() + " dame");
            Stats.Damage(attackData);
            ShowDebug(attackData.Target.name + " HP : "+ Stats.CurrentHealth);
            
            OnDamage?.Invoke();
        }
        private void ShowDebug(string log) {
            if (!isDebug) return;
            Logger.Debug(log);
        }
    }
}