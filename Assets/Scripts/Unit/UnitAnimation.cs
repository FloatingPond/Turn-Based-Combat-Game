using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace PG
{
    public class UnitAnimation : MonoBehaviour
    {
        private Animator animator;
        private UnitManager unitManager;
        private UnitMovement unitMovement;
        private UnitActions unitActions;
        private NavMeshAgent agent;
        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
            unitManager = GetComponent<UnitManager>();
            unitMovement = GetComponent<UnitMovement>();
            unitActions = GetComponent<UnitActions>();
            agent = GetComponent<NavMeshAgent>();
        }

        // Update is called once per frame
        void Update()
        {
            animator.SetFloat("MoveSpeed", agent.speed);
        }
    }
}
