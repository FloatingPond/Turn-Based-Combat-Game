using UnityEngine;
using UnityEngine.AI;

namespace PG
{
    public class UnitController : MonoBehaviour, IDamageable, IInteractable
    {
        [Header("Unit Scripts")]
        public UnitActions UnitActions;
        public UnitAnimation UnitAnimation;
        public UnitMovement UnitMovement;
        [Space(10)]
        public team MyTeam;
        public UnitData UnitData;
        public int Initiative;
        public float CurrentHealth;
        public Vector3 MyLookAtTargetVector;
        [Space(10)]
        public Transform MyLookAtTargetTransform;
        public NavMeshAgent Agent;
        public Transform AimTargetIK;
        public enum team { player, computer };
       
        // Start is called before the first frame update
        void Start()
        {
            UnitActions = GetComponent<UnitActions>();
            UnitAnimation = GetComponent<UnitAnimation>();
            UnitMovement = GetComponent<UnitMovement>();
            Agent = GetComponent<NavMeshAgent>();
            CurrentHealth = UnitData.maxHealth;
        }
        public void Die()
        {
            //Play death animation & sound
            gameObject.SetActive(false);
        }

        public void TakeDamage(float damage)
        {
            //Play damage animation & sound
            CurrentHealth -= damage;
            if (CurrentHealth <= 0)
            {
                Die();
            }
        }

        void IInteractable.OnHoverEnter()
        {
           
        }

        void IInteractable.OnHoverStay()
        {

        }

        void IInteractable.OnHoverExit()
        {
            
        }

        void IInteractable.OnClick()
        {
            
        }
    }
}
