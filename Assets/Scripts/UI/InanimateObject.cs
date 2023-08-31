using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG
{
    public class InanimateObject : MonoBehaviour, IInteractable, IDamageable
    {
        public float currentHealth, maxHealth;
        void Start()
        {
            currentHealth = maxHealth;
        }
        public void Die()
        {
            
        }

        void IInteractable.OnClick()
        {
            
        }

        void IInteractable.OnHoverEnter()
        {
            RoundManager.Instance.unitTakingTurn.unitActions.myTarget = this.transform;
        }

        void IInteractable.OnHoverExit()
        {
            RoundManager.Instance.unitTakingTurn.unitActions.myTarget = null;
            RoundManager.Instance.unitTakingTurn.myLookAtTarget = RoundManager.Instance.unitTakingTurn.transform.position;
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
