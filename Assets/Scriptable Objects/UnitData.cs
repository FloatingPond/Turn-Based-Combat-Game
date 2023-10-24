using UnityEngine;

namespace PG
{
    [CreateAssetMenu]
    public class UnitData : ScriptableObject
    {
        public string UnitName = "Soldier";
        public float MaxMovementDistance = 5f;
        public float MoveSpeed = 3.5f;
        public float MaxHealth = 3f;
        public float Damage = 1f;
        public int ClipSize = 3;
        public int ReloadSpeed = 1;
        public int MaxActions = 1;
    }
}
