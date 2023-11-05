using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG
{
    public class ComputerUnitActions : UnitActions, IInteractable
    {
        public List<UnitController> enemiesInSight;
        public List<InanimateObject> coverNearby, explosivesNearby;
        public override void PerformAction(Action action)
        {
            base.PerformAction(action);
        }

        public override void SwitchToGrenade()
        {
            base.SwitchToGrenade();
        }

        public override void ThrowGrenade()
        {
            base.ThrowGrenade();
        }

        protected override void Shoot()
        {
            base.Shoot();
        }
        void IInteractable.OnClick()
        {
            UIManager.Instance.UpdateActionText();
            RoundManager.Instance.TransitionCameraToUnit(RoundManager.Instance.unitTakingTurn_UnitController.OverShoulderVcam);
            StartCoroutine(RoundManager.Instance.unitTakingTurn_UnitController.UnitActions.LockInput(0));
            //Will want to provide details here (Think of when zooming in on an enemy in xcom 2 by clicking on them)
            //PerformAction(unitAction);
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
    }
}
