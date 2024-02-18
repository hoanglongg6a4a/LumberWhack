using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem
{
    public abstract class CharacterBaseStat : ScriptableObject
    {
        public StatSystem.Stats BaseStat;
        public StatSystem.Stats LevelUpStat;
        public int level = 0;
        public bool unlockSkill = false;

        public StatSystem.Stats GetStatsAtLevel(int level)
        {
            StatSystem.Stats newStats = new StatSystem.Stats();
            newStats.Copy(BaseStat);
            newStats.health += LevelUpStat.health * level;
            newStats.strength += LevelUpStat.strength * level;
            newStats.moveSpeed += LevelUpStat.moveSpeed * level;
            newStats.attackCooldown -= LevelUpStat.attackCooldown * level;
            newStats.range += LevelUpStat.range * level;

            return newStats;
        }
        public StatSystem.Stats GetStats()
        {
            return GetStatsAtLevel(level);
        }
    }
}
