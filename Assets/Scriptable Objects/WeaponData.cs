using UnityEngine;

namespace PG
{
    [CreateAssetMenu]
    public class WeaponData : ScriptableObject
    {
        public string Name = "Rifle";
        public weaponType type = weaponType.rifle;
        public float maxRange = 20f;
        public float damage = 1f;
        public int clipSize = 3;
        public int reloadSpeed = 1;
        public int areaOfEffectDamage = 0;
        public int areaOfEffectRange = 0;
        public enum weaponType { rifle, pistol, grenade, shotgun };
    }
}
