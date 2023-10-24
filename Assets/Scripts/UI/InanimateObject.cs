using UnityEngine;
using UnityEngine.AI;

namespace PG
{
    public class InanimateObject : MonoBehaviour, IInteractable, IDamageable
    {
        public float CurrentHealth, MaxHealth;
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
            CurrentHealth = MaxHealth;
            if (deathEffect != null) deathEffect.Stop();
        }
        void IInteractable.OnClick()
        {
            RoundManager.Instance.unitTakingTurn_UnitController.UnitActions.PerformAction(RoundManager.Instance.unitTakingTurn_UnitController.UnitActions.UnitAction);
        }

        void IInteractable.OnHoverEnter()
        {
            if (RoundManager.Instance.unitTakingTurn_UnitController.UnitActions.PerformingAction) return;
            float aimHeightTarget = GetHeight(gameObject) / 2; //Half the object's height to get the middle of the object
            Vector3 aimHeightTargetVector = new(transform.position.x, aimHeightTarget, transform.position.z);
            RoundManager.Instance.unitTakingTurn_UnitController.UnitActions.MyDamageableTarget = gameObject;
            RoundManager.Instance.unitTakingTurn_UnitController.MyLookAtTargetVector = transform.position;
            RoundManager.Instance.unitTakingTurn_UnitController.MyLookAtTargetTransform = transform;
            if (RoundManager.Instance.unitTakingTurn_UnitController.UnitActions.actionsRemaining != 0)
            {
                if (RoundManager.Instance.unitTakingTurn_UnitController.UnitAnimation.animator.GetBool("AimGrenade"))
                {
                    //Draw grenade trajectory
                }
                else
                {
                    WeaponTrajectoryIndicator.Instance.DrawProjectedGunshot(RoundManager.Instance.unitTakingTurn_UnitController.UnitActions.GunBarrel.position, aimHeightTargetVector);
                }
                
            }
        }

        void IInteractable.OnHoverExit()
        {
            RoundManager.Instance.unitTakingTurn_UnitController.UnitActions.MyDamageableTarget = null;
            RoundManager.Instance.unitTakingTurn_UnitController.MyLookAtTargetTransform = null;
            WeaponTrajectoryIndicator.Instance.ClearRendererPositions();
        }

        void IInteractable.OnHoverStay()
        {
            
        }
        public void TakeDamage(float damage)
        {
            CurrentHealth -= damage;
            if (CurrentHealth <= 0) Die();
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
