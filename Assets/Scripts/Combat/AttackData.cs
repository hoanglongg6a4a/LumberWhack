using GameSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackData
{
    public CharacterData Target => m_Target;
    public CharacterData Source => m_Source;

    CharacterData m_Target;
    CharacterData m_Source;

    float[] m_Damages = new float[System.Enum.GetValues(typeof(StatSystem.DamageType)).Length];

    /// <summary>
    /// Build a new AttackData. All AttackData need a target, but source is optional. If source is null, the
    /// damage is assume to be from a non CharacterData source (elemental effect, environment) and no boost will
    /// be applied to damage (target defense is still taken in account).
    /// </summary>
    /// <param name="target"></param>
    /// <param name="source"></param>
    public AttackData(CharacterData target, CharacterData source = null)
    {
        m_Target = target;
        m_Source = source;
    }

    /// <summary>
    /// Add an amount of damage given in the given type. The source (if non null, see class documentation for
    /// info) boost will be applied and the target defense will be removed from that amount.
    /// </summary>
    /// <param name="damageType">The type of damage</param>
    /// <param name="amount">The amount of damage</param>
    /// <returns></returns>
    public float AddDamage(StatSystem.DamageType damageType, float amount)
    {
        float addedAmount = amount;

        //Physical damage are increase by 1% for each point of strength
        if (damageType == StatSystem.DamageType.Physical)
        {
            //source cna be null when it's elemental or effect damage
            if (m_Source != null)
                addedAmount += Mathf.FloorToInt(addedAmount * m_Source.Stats.stats.strength * 0.01f);
        }

        m_Damages[(int)damageType] += addedAmount;

        return addedAmount;
    }

    /// <summary>
    /// Return the current amount of damage of the given type stored in that AttackData. This is the *effective*
    /// amount of damage, boost and defense have already been applied.
    /// </summary>
    /// <param name="damageType">The type of damage</param>
    /// <returns>How much damage of that type is stored in that AttackData</returns>
    public float GetDamage(StatSystem.DamageType damageType)
    {
        return m_Damages[(int)damageType];
    }

    /// <summary>
    /// Return the total amount of damage across all type stored in that AttackData. This is *effective* damage,
    /// that mean all boost/defense was already applied.
    /// </summary>
    /// <returns>The total amount of damage across all type in that Attack Data</returns>
    public float GetFullDamage()
    {
        float totalDamage = 0f;
        for (int i = 0; i < m_Damages.Length; ++i)
        {
            totalDamage += m_Damages[i];
        }

        return totalDamage;
    }
}