using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG
{
    public class Grenade : MonoBehaviour
    {
        [SerializeField] private AnimationCurve trajectory;
        [SerializeField] private AreaOfEffectDamage aoe;
        [SerializeField] private ParticleSystem grenadeExplosionFX;
        [SerializeField] private List<MeshRenderer> meshRenderers;
        [SerializeField] private float throwStrength = 10f;
        public float verticalForce = 5f;
        private Rigidbody rb;
        private Transform parent;
        private Vector3 originalPosition;

        private void OnEnable()
        {
            parent = transform.parent;
            originalPosition = transform.localPosition;
            rb = GetComponent<Rigidbody>();
        }
        private void ResetGrenade()
        {
            transform.SetParent(parent);
            transform.localPosition = originalPosition;
        }
        public void Throw(Vector3 direction, float verticalForce)
        {
            transform.SetParent(null);
            ChangeGrenadeRenderers(true);
            rb.isKinematic = false;
            Vector3 finalDirection = new Vector3(direction.x, verticalForce, direction.z);
            rb.AddForce(finalDirection.normalized * throwStrength, ForceMode.Impulse);
        }
        public void ChangeGrenadeRenderers(bool newVal)
        {
            for (int i = 0; i < meshRenderers.Count; i++)
            {
                meshRenderers[i].enabled = newVal;
            }
        }
        private void OnCollisionEnter(Collision collision)
        {
            aoe.DoDamageInSphere(transform.position, aoe.radius);
            rb.isKinematic = true;
            grenadeExplosionFX.transform.rotation = new Quaternion(-45, 0, 0, 0);
            grenadeExplosionFX.Play();
            ChangeGrenadeRenderers(false);
            Invoke(nameof(ResetGrenade), 3.5f);
        }
    }
}
