using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG
{
    public class UnitController : MonoBehaviour, IDamageable, IInteractable
    {
        [Header("Unit Scripts")]
        public UnitActions unitActions;
        public UnitAnimation unitAnimation;
        public UnitMovement unitMovement;
        [Space(10)]
        public team myTeam;
        public UnitData unitData;
        public int initiative;
        public float currentHealth;
        public UIManager uIManager;
        private Transform myTarget;
        public enum team { player, computer };
       
        // Start is called before the first frame update
        void Start()
        {
            unitActions = GetComponent<UnitActions>();
            unitAnimation = GetComponent<UnitAnimation>();
            unitMovement = GetComponent<UnitMovement>();
            uIManager = FindObjectOfType<UIManager>();
            currentHealth = unitData.maxHealth;
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        public void Die()
        {
            //Play death animation & sound
            gameObject.SetActive(false);
        }

        public void TakeDamage(float damage)
        {
            //Play damage animation & sound
            currentHealth -= damage;
            if (currentHealth <= 0)
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
