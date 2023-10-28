using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG
{
    public class PlayerUnitActions : UnitActions
    {
        public override void SwitchToGrenade()
        {
            base.SwitchToGrenade();
            UIManager.Instance.SwitchGrenadeIndicatorRenderer();

        }

        public override void ThrowGrenade()
        {
            base.ThrowGrenade();
            UIManager.Instance.ActionRemainingText.text = "Throwing Grenade";
            UIManager.Instance.SwitchGrenadeIndicatorRenderer();
        }

        public override void PerformAction(Action action)
        {
            base.PerformAction(action);
        }

        protected override void Shoot()
        {
            base.Shoot();
            UIManager.Instance.ActionRemainingText.text = "Shooting";
        }

    }
}
