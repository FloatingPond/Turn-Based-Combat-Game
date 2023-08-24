using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG
{
    public interface IDamageable 
    {
        void TakeDamage(float damage);
        void Die();
    }
}
