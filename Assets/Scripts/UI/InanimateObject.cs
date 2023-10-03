using UnityEngine;
using UnityEngine.AI;

namespace PG
{
    public class InanimateObject : MonoBehaviour, IInteractable, IDamageable
    {
        public float currentHealth, maxHealth;
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
            float aimHeightTarget = GetHeight(gameObject) / 2;
            Vector3 aimHeightTargetVector = new Vector3 (transform.position.x, aimHeightTarget, transform.position.z);
            RoundManager.Instance.unitTakingTurn.unitActions.myDamageableTarget = gameObject;
            RoundManager.Instance.unitTakingTurn.myLookAtTarget = transform.position;
            RoundManager.Instance.unitTakingTurn.myLookAtTargetTransform = transform;
            if (!RoundManager.Instance.unitTakingTurn.unitActions.usedAction)
            {
                GunShotRenderer.Instance.DrawGunshot(RoundManager.Instance.unitTakingTurn.unitActions.gunBarrel.position, aimHeightTargetVector);
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
        float GetHeight(GameObject obj)
        {
            MeshRenderer meshRenderer = obj.GetComponent<MeshRenderer>();
            if (meshRenderer == null)
            {
                Debug.LogError("No MeshRenderer found on " + obj.name + ".");
                return 0;
            }
            Bounds bounds = meshRenderer.bounds;
            float height = bounds.size.y * obj.transform.localScale.y;
            return height;     
        }
    }
}
