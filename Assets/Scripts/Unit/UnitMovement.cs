using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

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
            Vector3 direction = transform.position - unitController.myLookAtTarget;
            Quaternion rotation = Quaternion.LookRotation(direction);
            rotation *= Quaternion.Euler(0, 180, 0);
            transform.rotation = rotation;
            #endregion
        }
        // Update is called once per frame
        void Update()
        {
            if (unitController.myTeam == UnitController.team.computer) return;
            LookAtMyTarget();
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
    }
}
