using UnityEngine;
using UnityEngine.AI;

namespace PG
{
    public class UnitMovement : MonoBehaviour
    {
        [SerializeField] protected NavMeshAgent agent;
        [SerializeField] protected UnitController unitController;
        [SerializeField] protected LineRendererPath lineRendererPath;
        protected UnitData unitData;
        public bool UnitMoving;
        public bool MovementComplete;
        public float CurrentMovementRemaining;
        public Transform AimTargetIK;
        [SerializeField] protected float thisMovementCost;
        [SerializeField] protected float movementProgress;
        public virtual void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            unitController = GetComponent<UnitController>();
            lineRendererPath = FindObjectOfType<LineRendererPath>();
            unitData = unitController.UnitData;
            CurrentMovementRemaining = unitData.MaxMovementDistance;
        }
        public virtual void CheckRemainingMovement()
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
        public virtual void MoveToDestination()
        {
            RoundManager.Instance.TransitionCameraToUnit(RoundManager.Instance.unitTakingTurn_UnitController.OverShoulderVcam);
            RoundManager.Instance.unitTakingTurn_UnitController.UnitActions.PerformingAction = true;
            unitController.UnitAnimation.SetRigsForRunning();
            agent.speed = unitController.UnitData.MoveSpeed;
            UnitMoving = true;
            CurrentMovementRemaining -= thisMovementCost;
        }
        public virtual void Update()
        {
            if (!agent.pathPending && agent.remainingDistance < agent.stoppingDistance)
            {
                UnitMoving = false;
                agent.speed = 0;
                if (CurrentMovementRemaining <= 0)
                {
                    CurrentMovementRemaining = 0;
                    MovementComplete = true;
                }
            }
        }
        public virtual float GetHeight(GameObject obj)
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
