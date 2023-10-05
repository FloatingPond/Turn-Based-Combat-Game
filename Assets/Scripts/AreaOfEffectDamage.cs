using UnityEngine;

namespace PG
{
    public class AreaOfEffectDamage : MonoBehaviour
    {
        [SerializeField] private float baseDamage = 5;
        [SerializeField] public float radius = 5;

        public void DoDamageInSphere(Vector3 center, float radius)
        {
            Collider[] hitColliders = Physics.OverlapSphere(center, radius);

            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i].TryGetComponent(out IDamageable damageable))
                {
                    float distanceFromDamageable = Vector3.Distance(transform.position, hitColliders[i].transform.position);

                    if (distanceFromDamageable <= radius)
                    {
                        damageable.TakeDamage(baseDamage / distanceFromDamageable);
                    }
                }
            }
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}