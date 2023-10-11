using UnityEngine;

namespace PG
{
    [CreateAssetMenu]
    public class WeaponData : ScriptableObject
    {
        public string Name = "Rifle";
        public weaponType Type = weaponType.rifle;
        public float MaxRange = 20f;
        public float Damage = 1f;
        public int ClipSize = 3;
        public int ReloadSpeed = 1;
        public int AreaOfEffectDamage = 0;
        public int AreaOfEffectRange = 0;
        public enum weaponType { rifle, pistol, grenade, shotgun };
    }
}
