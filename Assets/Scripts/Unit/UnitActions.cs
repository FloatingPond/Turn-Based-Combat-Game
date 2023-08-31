using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG
{
    public class UnitActions : MonoBehaviour, IInteractable
    {
        public bool usedAction = false;
        public action unitAction;
        public enum action { shoot, throwGrenade };
        public Transform gunBarrel;
        [SerializeField] private ParticleSystem gunSmoke;
        public GameObject myDamageableTarget;

        private void Start()
        {
            gunSmoke.Stop();
        }
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
            if (!usedAction)
            {
                gunSmoke.Play();
                usedAction = true;
                GunShotRenderer.Instance.ClearGunshot();
                myDamageableTarget.GetComponent<IDamageable>().TakeDamage(RoundManager.Instance.unitTakingTurn.unitData.damage);
            }
        }
        public void ThrowGrenade()
        {
            //usedAction = true;
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
            PerformAction(unitAction);
        }
    }
}
