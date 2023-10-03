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
        public bool unitMoving;
        public bool movementComplete;
        public float currentMovementRemaining;
        [SerializeField]
        private float thisMovementCost;
        public Transform aimTargetIK;
        // Start is called before the first frame update
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            unitController = GetComponent<UnitController>();
            lineRendererPath = FindObjectOfType<LineRendererPath>();
            unitData = unitController.unitData;
            currentMovementRemaining = unitData.maxMovementDistance;
        }
        public void CheckRemainingMovement()
        {
            thisMovementCost = lineRendererPath.CalculatePathDistance(agent);
            if (thisMovementCost > currentMovementRemaining)
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
            agent.speed = unitController.unitData.moveSpeed;
            unitMoving = true;
        }

        public void LookAtMyTarget()
        {
            #region Turn unit to face ghost
            Vector3 direction = Vector3.zero;
            if (unitController.unitActions.myDamageableTarget == null)
            {
                direction = transform.position - unitController.myLookAtTarget;
                aimTargetIK.transform.position = unitController.myLookAtTarget;
            }
            else
            {
                float aimHeightTarget = GetHeight(unitController.unitActions.myDamageableTarget) / 2;
                Vector3 aimHeightTargetVector = new(unitController.unitActions.myDamageableTarget.transform.position.x, aimHeightTarget, unitController.unitActions.myDamageableTarget.transform.position.z);
                direction = transform.position - unitController.unitActions.myDamageableTarget.transform.position;
                aimTargetIK.transform.position = aimHeightTargetVector;
            }
            Quaternion rotation = Quaternion.LookRotation(direction);
            rotation *= Quaternion.Euler(0, 180, 0);
            transform.rotation = rotation;
            #endregion
        }
        // Update is called once per frame
        void Update()
        {
            if (unitController.myTeam == UnitController.team.computer) return;
            if (!unitMoving) LookAtMyTarget();
            UIManager.Instance.movementRemainingText.text = "Movement Remaining: " + currentMovementRemaining.ToString("F1") + "m";
            if (!agent.pathPending && agent.remainingDistance < agent.stoppingDistance)
            {
                unitMoving = false;
                agent.speed = 0;
                currentMovementRemaining -= thisMovementCost;
                if (currentMovementRemaining <= 0)
                {
                    currentMovementRemaining = 0;
                    movementComplete = true;
                    UIManager.Instance.movementRemainingText.text = "Movement Complete";
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
