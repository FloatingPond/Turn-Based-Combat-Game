using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG
{
    [CreateAssetMenu]
    public class LevelData : ScriptableObject
    {
        public int ComputerUnitAmount = 4;
        public int PlayerUnitAmount = 4;
        public List<GameObject> ComputerUnits;
        public List<GameObject> PlayerUnits;
    }
}
