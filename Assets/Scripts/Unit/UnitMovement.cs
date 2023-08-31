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
        private UnitManager unitManager;
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
            unitManager = GetComponent<UnitManager>();
            lineRendererPath = FindObjectOfType<LineRendererPath>();

            unitData = unitManager.unitData;
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
            agent.speed = unitManager.unitData.moveSpeed;
            unitMoving = true;
        }
        // Update is called once per frame
        void Update()
        {
            if (unitManager.myTeam == UnitManager.team.computer) return;
            unitManager.uIManager.movementRemainingText.text = "Movement Remaining: " + currentMovementRemaining.ToString("F1") + "m";
            if (!agent.pathPending && agent.remainingDistance < agent.stoppingDistance)
            {
                unitMoving = false;
                agent.speed = 0;
                currentMovementRemaining -= thisMovementCost;
                if (currentMovementRemaining <= 0)
                {
                    currentMovementRemaining = 0;
                    movementComplete = true;
                    unitManager.uIManager.movementRemainingText.text = "Movement Complete";
                }
            }
        }
    }
}
