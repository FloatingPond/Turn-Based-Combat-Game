using Cinemachine;
using UnityEngine;
using UnityEngine.AI;

namespace PG
{
    public class UnitController : MonoBehaviour, IDamageable
    {
        [Header("Unit Scripts")]
        public UnitActions UnitActions;
        public UnitAnimation UnitAnimation;
        public UnitMovement UnitMovement;
        [Space(10)]
        public Team MyTeam;
        public UnitData UnitData;
        public float CurrentHealth;
        public Vector3 MyLookAtTargetVector;
        [Space(10)]
        public Transform MyLookAtTargetTransform;
        [Header("References")]
        public NavMeshAgent Agent;
        public Transform AimTargetIK;
        public CinemachineVirtualCamera OverShoulderVcam;
        public enum Team { player, computer };
       
        // Start is called before the first frame update
        void Start()
        {
            UnitActions = GetComponent<UnitActions>();
            UnitAnimation = GetComponent<UnitAnimation>();
            UnitMovement = GetComponent<UnitMovement>();
            Agent = GetComponent<NavMeshAgent>();
            CurrentHealth = UnitData.MaxHealth;
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
    }
}
