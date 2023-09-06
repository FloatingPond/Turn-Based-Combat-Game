using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
            foreach (var hitCollider in hitColliders)
            {
                Debug.Log("Found " + hitCollider.name + " in sphere.");
                if (hitCollider.TryGetComponent(out IDamageable damageable))
                {
                    float distanceFromDamageable = Vector3.Distance(transform.position, hitCollider.transform.position);
                    Debug.Log("Distance from damageable " + hitCollider.name + " = " + distanceFromDamageable);
                    if (distanceFromDamageable <= radius) 
                    {
                        Debug.Log("Dealing " + baseDamage / distanceFromDamageable + " damage to " + hitCollider.name);
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
