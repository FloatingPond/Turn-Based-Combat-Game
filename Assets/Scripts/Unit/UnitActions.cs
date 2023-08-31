using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG
{
    public class UnitActions : MonoBehaviour, IInteractable
    {
        private UnitController unitManager;
        public bool usedAction = false;
        public action unitAction;
        public enum action { shoot, throwGrenade };
        [SerializeField] private Transform gunBarrel;
        [SerializeField] private ParticleSystem gunSmoke;
        public Transform myTarget;

        private void Start()
        {
            unitManager = GetComponent<UnitController>();
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
            unitManager.uIManager.gunShotRenderer.DrawGunshot(gunBarrel.position);
            gunSmoke.Play();
            //usedAction = true;
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
