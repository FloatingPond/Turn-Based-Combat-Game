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
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            unitController = GetComponent<UnitController>();
            lineRendererPath = FindObjectOfType<LineRendererPath>();
            unitData = unitController.UnitData;
            CurrentMovementRemaining = unitData.maxMovementDistance;
        }
        public void CheckRemainingMovement()
        {
            thisMovementCost = lineRendererPath.CalculatePathDistance(agent);
            if (thisMovementCost > CurrentMovementRemaining)
            {
                Debug.Log("Not enough movement!");
            }
            else
            {
                Debug.Log("I have enough movement, moving.");
                MoveToDestination();
            }
        }
        public void MoveToDestination()
        {
            unitController.UnitAnimation.SetRigsForRunning();
            agent.speed = unitController.UnitData.moveSpeed;
            UnitMoving = true;
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

            Vector3 direction = Vector3.zero;
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
            if (unitController.MyTeam == UnitController.team.computer) return;
            if (!UnitMoving) LookAtMyTarget();
            UIManager.Instance.MovementRemainingText.text = "Movement Remaining: " + CurrentMovementRemaining.ToString("F1") + "m";
            if (!agent.pathPending && agent.remainingDistance < agent.stoppingDistance)
            {
                UnitMoving = false;
                agent.speed = 0;
                CurrentMovementRemaining -= thisMovementCost;
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
