using GameSystem;
using UnityEngine;

/// <summary>
/// Base class of all effect you can add on a weapon to specialize it. See documentation on How to write a new
/// Weapon Effect.
/// </summary>
public abstract class AttackEffect : ScriptableObject
{
    public string Description;

    //return the amount of physical damage. If no change, just return physicalDamage passed as parameter
    public virtual void OnAttack(CharacterData target, CharacterData user, ref AttackData data) { }

    //called after all weapon effect where applied, allow to react to the total amount of damage applied
    public virtual void OnPostAttack(CharacterData target, CharacterData user, AttackData data) { }

    public virtual string GetDescription()
    {
        return Description;
    }
}
