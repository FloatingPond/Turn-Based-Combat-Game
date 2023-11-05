using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG
{
    public class PlayerUnitMovement : UnitMovement
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
            UIManager.Instance.MovementRemainingText.text = "Movement Remaining: " + CurrentMovementRemaining.ToString("F1") + "m";
        }

        public override void Update()
        {
            base.Update();
            if (MovementComplete)
            {
                UIManager.Instance.MovementRemainingText.text = "Movement Complete";
                LookAtMyTarget();
                return;
            }
            if (!UnitMoving && !RoundManager.Instance.unitTakingTurn_UnitController.UnitActions.PerformingAction) LookAtMyTarget();
            else if (UnitMoving)
            {
                movementProgress = lineRendererPath.CalculatePathDistance(RoundManager.Instance.unitTakingTurn_UnitController.Agent);

                if (CurrentMovementRemaining < 1) CurrentMovementRemaining = 0;

                float temp = RoundManager.Instance.unitTakingTurn_UnitController.UnitMovement.CurrentMovementRemaining + movementProgress;

                string distanceFromRemaining = "Movement Remaining: " + temp.ToString("F1") + "m";

                UIManager.Instance.MovementRemainingText.text = distanceFromRemaining.ToString();
            }
        }
        protected void LookAtMyTarget()
        {
            #region Turn unit to face ghost
            if (unitController.UnitAnimation.animator.GetBool("AimGrenade"))
            {
                unitController.UnitAnimation.SetRigsForAimingGrenade();
            }
            else
            {
                unitController.UnitAnimation.SetRigsForAiming();
            }

            Vector3 direction;
            if (unitController.UnitActions.MyDamageableTarget == null)
            {
                direction = transform.position - unitController.MyLookAtTargetVector;
                AimTargetIK.transform.position = unitController.MyLookAtTargetVector;
            }
            else
            {
                float aimHeightTarget = GetHeight(unitController.UnitActions.MyDamageableTarget) / 2;
                Vector3 aimHeightTargetVector = new(unitController.UnitActions.MyDamageableTarget.transform.position.x, aimHeightTarget, unitController.UnitActions.MyDamageableTarget.transform.position.z);
                direction = transform.position - unitController.UnitActions.MyDamageableTarget.transform.position;
                AimTargetIK.transform.position = aimHeightTargetVector;
            }
            Quaternion rotation = Quaternion.LookRotation(direction);
            rotation *= Quaternion.Euler(0, 180, 0);
            transform.rotation = rotation;
            #endregion
        }
    }
}
