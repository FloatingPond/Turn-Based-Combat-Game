using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG
{
    public class UnitManager : MonoBehaviour, Damageable
    {
        public team myTeam;
        public UnitData unitData;
        public int initiative;
        public float currentHealth;
        public enum team { player, computer };
        public void Die()
        {
            gameObject.SetActive(false);
        }

        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                Die();
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            currentHealth = unitData.maxHealth;
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
