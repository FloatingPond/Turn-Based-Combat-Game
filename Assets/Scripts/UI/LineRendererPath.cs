using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace PG
{
    public class LineRendererPath : MonoBehaviour
    {
        private LineRenderer lineRenderer;

        void Start()
        {
            lineRenderer = FindObjectOfType<LineRenderer>();
        }
        public void ClearPath()
        {
            lineRenderer.positionCount = 0;
        }
        public void DrawPath(NavMeshAgent agent, Vector3 target)
        {
            agent.SetDestination(target);
            agent.speed = 0;
            if (agent.path.status != NavMeshPathStatus.PathInvalid)
            {
                lineRenderer.positionCount = agent.path.corners.Length;
                for (int i = 0; i < agent.path.corners.Length; i++)
                {
                    lineRenderer.SetPosition(i, agent.path.corners[i]);
                }
            }
        }
        public float GetPathDistance(NavMeshAgent agent, Vector3 target)
        {
            return (target - agent.transform.position).magnitude;
        }
    }
}
