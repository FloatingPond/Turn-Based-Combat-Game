using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG
{
    public class UnitManager : MonoBehaviour, Damageable
    {
        public UnitData unitData;
        public int initiative;
        public float currentHealth;
        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
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
