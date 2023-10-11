using UnityEngine;

namespace PG
{
    [CreateAssetMenu]
    public class UnitData : ScriptableObject
    {
        public string unitName = "Soldier";
        public float maxMovementDistance = 5f;
        public float moveSpeed = 3.5f;
        public float maxHealth = 3f;
        public float damage = 1f;
        public int clipSize = 3;
        public int reloadSpeed = 1;
    }
}
