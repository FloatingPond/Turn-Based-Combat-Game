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
        private Transform parent;
        private Vector3 originalPosition;

        private void OnEnable()
        {
            parent = transform.parent;
            originalPosition = transform.localPosition;
        }
        private void ResetGrenade()
        {
            transform.SetParent(parent);
            transform.localPosition = originalPosition;
        }
        public IEnumerator ThrowGrenade(Vector3 targetPosition, float duration)
        {
            transform.SetParent(null);
            
            float elapsedTime = 0;
            Vector3 startPosition = transform.position;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float progress = elapsedTime / duration;

                float height = trajectory.Evaluate(progress);
                Vector3 newPosition = Vector3.Lerp(startPosition, targetPosition, progress);
                newPosition.y += height;

                transform.position = newPosition;
                yield return null;
            }
            aoe.DoDamageInSphere(transform.position, aoe.radius);
            grenadeExplosionFX.transform.rotation = new Quaternion(-45, 0, 0, 0);
            grenadeExplosionFX.Play();
            ChangeGrenadeRenderers(false);
            Invoke(nameof(ResetGrenade), 3.5f);
        }
        public void ChangeGrenadeRenderers(bool newVal)
        {
            for (int i = 0; i < meshRenderers.Count; i++)
            {
                meshRenderers[i].enabled = newVal;
            }
        }
    }
}
