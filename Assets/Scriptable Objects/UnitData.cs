using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG
{
    [CreateAssetMenu]
    public class UnitData : ScriptableObject
    {
        public string unitName = "Soldier";
        public float movement = 5f;
        public float health = 3f;
        public float damage = 1f;
        public int clipSize = 3;
        public int reloadSpeed = 1;
    }
}
