using System.Collections.Generic;
using UnityEngine;

namespace GameSystem
{
    [CreateAssetMenu(fileName = "SoldierBaseStat", menuName = "Game System/New Soldier Base Stat", order = -999)]
    public class SoldierBaseStat : CharacterBaseStat
    {
        public int unlockPrice;
        public List<int> upgradePrices;
    }
}
