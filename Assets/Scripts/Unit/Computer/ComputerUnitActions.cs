using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG
{
    public class ComputerUnitActions : UnitActions
    {
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
    }
}
