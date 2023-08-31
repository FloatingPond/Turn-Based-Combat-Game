using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG
{
    public class UnitActions : MonoBehaviour
    {
        public bool usedAction = false;
        public action unitAction;
        public enum action { shoot, throwGrenade };

        public void PerformAction(action _action)
        {
            if (!usedAction)
            {
                switch (_action)
                {
                    case action.shoot:
                        Shoot();
                        break;
                    case action.throwGrenade:
                        ThrowGrenade();
                        break;
                    default:
                        break;
                }
            }
        }

        public void Shoot()
        {
        }
        public void ThrowGrenade()
        {
        }
    }
}
