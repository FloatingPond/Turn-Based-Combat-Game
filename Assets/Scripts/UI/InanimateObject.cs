using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG
{
    public class InanimateObject : MonoBehaviour, IInteractable, IDamageable
    {
        public float currentHealth, maxHealth;
        [SerializeField] private float targetingHeightScale = 0.75f;
        [SerializeField] private ParticleSystem deathEffect;
        private MeshRenderer meshRenderer;
        void Start()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            currentHealth = maxHealth;
            if (deathEffect != null) deathEffect.Stop();
        }
        public void Die()
        {
            if (deathEffect != null) deathEffect.Play();
            meshRenderer.enabled = false;
        }

        void IInteractable.OnClick()
        {
            RoundManager.Instance.unitTakingTurn.unitActions.Shoot();
            if (TryGetComponent<AreaOfEffectDamage>(out AreaOfEffectDamage aoe)) aoe.DoDamageInSphere(transform.position, aoe.radius);
        }

        void IInteractable.OnHoverEnter()
        {
            RoundManager.Instance.unitTakingTurn.unitActions.myDamageableTarget = gameObject;
            RoundManager.Instance.unitTakingTurn.myLookAtTarget = transform.position;
            RoundManager.Instance.unitTakingTurn.myLookAtTargetTransform = transform;
            if (!RoundManager.Instance.unitTakingTurn.unitActions.usedAction)
            {
                GunShotRenderer.Instance.DrawGunshot(RoundManager.Instance.unitTakingTurn.unitActions.gunBarrel.position, new Vector3(transform.position.x, transform.position.y + (transform.localScale.y * targetingHeightScale), transform.position.z));
            }
        }

        void IInteractable.OnHoverExit()
        {
            RoundManager.Instance.unitTakingTurn.unitActions.myDamageableTarget = null;
            RoundManager.Instance.unitTakingTurn.myLookAtTargetTransform = null;
            GunShotRenderer.Instance.ClearGunshot();
        }

        void IInteractable.OnHoverStay()
        {
            
        }
        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
            if (currentHealth <= 0) Die();
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
