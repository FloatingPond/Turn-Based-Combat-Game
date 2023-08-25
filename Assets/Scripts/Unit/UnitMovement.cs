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
        public bool unitMoving;
        // Start is called before the first frame update
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            unitManager = GetComponent<UnitManager>();
        }

        public void MoveToDestination()
        {
            agent.speed = unitManager.unitData.moveSpeed;
            unitMoving = true;
        }
        // Update is called once per frame
        void Update()
        {
            if (agent.remainingDistance < 0.01)
            {
                unitMoving = false;
                agent.speed = 0;
            }
        }
    }
}
