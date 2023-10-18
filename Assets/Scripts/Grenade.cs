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
            grenadeExplosionFX.transform.rotation = new Quaternion(-90, 0, 0, 0);
            grenadeExplosionFX.Play();
            for (int i = 0; i < meshRenderers.Count; i++)
            {
                meshRenderers[i].enabled = false;
            }
        }
    }
}
