using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace PG
{
    public class InanimateObject : MonoBehaviour, IInteractable, IDamageable
    {
        public float currentHealth, maxHealth;
        [SerializeField] private float targetingHeightScale = 0.75f;
        [SerializeField] private ParticleSystem deathEffect;
        private MeshRenderer meshRenderer;
        private Collider objectCollider;
        private NavMeshObstacle navMeshObstacle;
        void Start()
        {
            if (TryGetComponent<NavMeshObstacle>(out NavMeshObstacle NMO))
            {
                navMeshObstacle = NMO;
            }
            if (TryGetComponent<Collider>(out Collider collider))
            { 
                objectCollider = collider;   
            }
            meshRenderer = GetComponent<MeshRenderer>();
            currentHealth = maxHealth;
            if (deathEffect != null) deathEffect.Stop();
        }
        public void Die()
        {
            if (deathEffect != null) deathEffect.Play();
            meshRenderer.enabled = false;
            objectCollider.enabled = false;
            navMeshObstacle.enabled = false;
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
