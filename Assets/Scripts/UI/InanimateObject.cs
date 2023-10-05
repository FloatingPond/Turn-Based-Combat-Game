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
        void IInteractable.OnClick()
        {
            RoundManager.Instance.unitTakingTurn_UnitController.unitActions.PerformAction(RoundManager.Instance.unitTakingTurn_UnitController.unitActions.unitAction);
        }

        void IInteractable.OnHoverEnter()
        {
            float aimHeightTarget = GetHeight(gameObject) / 2; //Half the object's height to get the middle of the object
            Vector3 aimHeightTargetVector = new Vector3 (transform.position.x, aimHeightTarget, transform.position.z);
            RoundManager.Instance.unitTakingTurn_UnitController.unitActions.myDamageableTarget = gameObject;
            RoundManager.Instance.unitTakingTurn_UnitController.myLookAtTarget = transform.position;
            RoundManager.Instance.unitTakingTurn_UnitController.myLookAtTargetTransform = transform;
            if (!RoundManager.Instance.unitTakingTurn_UnitController.unitActions.usedAction)
            {
                if (RoundManager.Instance.unitTakingTurn_UnitController.unitAnimation.animator.GetBool("AimGrenade"))
                {
                    //Draw grenade trajectory
                }
                else
                {
                    WeaponTrajectoryIndicator.Instance.DrawProjectedGunshot(RoundManager.Instance.unitTakingTurn_UnitController.unitActions.gunBarrel.position, aimHeightTargetVector);
                }
                
            }
        }

        void IInteractable.OnHoverExit()
        {
            RoundManager.Instance.unitTakingTurn_UnitController.unitActions.myDamageableTarget = null;
            RoundManager.Instance.unitTakingTurn_UnitController.myLookAtTargetTransform = null;
            WeaponTrajectoryIndicator.Instance.ClearRendererPositions();
        }

        void IInteractable.OnHoverStay()
        {
            
        }
        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
            if (currentHealth <= 0) Die();
        }
        public void Die()
        {
            if (deathEffect != null) deathEffect.Play();
            meshRenderer.enabled = false;
            objectCollider.enabled = false;
            navMeshObstacle.enabled = false;
            if (TryGetComponent<AreaOfEffectDamage>(out AreaOfEffectDamage aoe)) aoe.DoDamageInSphere(transform.position, aoe.radius);
        }
        /// <summary>
        /// Gets the local height of the object, for the purposes of aiming correctly in the middle of the object.  	
        /// </summary>
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
