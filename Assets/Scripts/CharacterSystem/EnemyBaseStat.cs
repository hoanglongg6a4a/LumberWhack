using UnityEngine;

namespace GameSystem
{
    [CreateAssetMenu(fileName = "EnemyBaseStat", menuName = "Game System/New Enemy Base Stat", order = -999)]
    public class EnemyBaseStat : CharacterBaseStat
    {
        public LootSpawner.SpawnEvent[] Drops;
    }
}
