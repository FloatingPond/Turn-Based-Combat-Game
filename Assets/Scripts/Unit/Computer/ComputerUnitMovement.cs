using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG
{
    public class ComputerUnitMovement : UnitMovement
    {
        public override void CheckRemainingMovement()
        {
            base.CheckRemainingMovement();
        }

        public override float GetHeight(GameObject obj)
        {
            return base.GetHeight(obj);
        }

        public override void MoveToDestination()
        {
            base.MoveToDestination();
        }

        public override void Start()
        {
            base.Start();
        }

        public override void Update()
        {
            //base.Update();
        }
    }
}
