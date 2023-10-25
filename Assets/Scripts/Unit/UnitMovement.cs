using UnityEngine;
using UnityEngine.AI;

namespace PG
{
    public class UnitMovement : MonoBehaviour
    {
        private NavMeshAgent agent;
        private UnitController unitController;
        private LineRendererPath lineRendererPath;
        private UnitData unitData;
        public bool UnitMoving;
        public bool MovementComplete;
        public float CurrentMovementRemaining;
        public Transform AimTargetIK;
        [SerializeField] private float thisMovementCost;
        [SerializeField] private float movementProgress;
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            unitController = GetComponent<UnitController>();
            lineRendererPath = FindObjectOfType<LineRendererPath>();
            unitData = unitController.UnitData;
            CurrentMovementRemaining = unitData.MaxMovementDistance;
            UIManager.Instance.MovementRemainingText.text = "Movement Remaining: " + CurrentMovementRemaining.ToString("F1") + "m";
        }
        public void CheckRemainingMovement()
        {
            thisMovementCost = lineRendererPath.CalculatePathDistance(agent);
            if (thisMovementCost > CurrentMovementRemaining)
            {
                Debug.Log("Not enough movement!"); //Need to add UI/feedback to the player here
            }
            else
            {
                Debug.Log("I have enough movement, moving.");
                MoveToDestination();
            }
        }
        public void MoveToDestination()
        {
            RoundManager.Instance.TransitionCameraToUnit(unitController);
            RoundManager.Instance.unitTakingTurn_UnitController.UnitActions.PerformingAction = true;
            unitController.UnitAnimation.SetRigsForRunning();
            agent.speed = unitController.UnitData.MoveSpeed;
            UnitMoving = true;
            CurrentMovementRemaining -= thisMovementCost;
        }

        public void LookAtMyTarget()
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
        void Update()
        {
            if (unitController.MyTeam == UnitController.Team.computer && RoundManager.Instance.CurrentTurnOwner == RoundManager.TurnOwner.Computer) return;
            if (!UnitMoving && !RoundManager.Instance.unitTakingTurn_UnitController.UnitActions.PerformingAction) LookAtMyTarget();
            else if (UnitMoving)
            {
                movementProgress = lineRendererPath.CalculatePathDistance(RoundManager.Instance.unitTakingTurn_UnitController.Agent);

                if (RoundManager.Instance.unitTakingTurn_UnitController.UnitMovement.CurrentMovementRemaining < 1)
                { RoundManager.Instance.unitTakingTurn_UnitController.UnitMovement.CurrentMovementRemaining = 0; }

                float temp = RoundManager.Instance.unitTakingTurn_UnitController.UnitMovement.CurrentMovementRemaining + movementProgress;

                string distanceFromRemaining = "Movement Remaining: " + temp.ToString("F1") + "m";

                UIManager.Instance.MovementRemainingText.text = distanceFromRemaining.ToString();
            }

            if (!agent.pathPending && agent.remainingDistance < agent.stoppingDistance)
            {
                UnitMoving = false;
                agent.speed = 0;
                if (CurrentMovementRemaining <= 0)
                {
                    CurrentMovementRemaining = 0;
                    MovementComplete = true;
                    UIManager.Instance.MovementRemainingText.text = "Movement Complete";
                }
            }
        }
        float GetHeight(GameObject obj)
        {
            MeshRenderer meshRenderer = obj.GetComponent<MeshRenderer>();
            if (meshRenderer == null)
            {
                Debug.LogError("No MeshRenderer found on " + obj.name + ".");
                return 0;
            }
            Bounds bounds = meshRenderer.bounds;
            float height = bounds.size.y * obj.transform.localScale.y;
            return height;
        }
    }
}
