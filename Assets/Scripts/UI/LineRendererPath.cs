using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

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
            if (agent.path.status != NavMeshPathStatus.PathInvalid)
            {
                lineRenderer.positionCount = agent.path.corners.Length;
                for (int i = 0; i < agent.path.corners.Length; i++)
                {
                    lineRenderer.SetPosition(i, agent.path.corners[i]);
                }
            }
        }
        public float CalculatePathDistance(NavMeshAgent agent)
        {
            float distance = 0f;
            for (int i = 0; i < agent.path.corners.Length - 1; i++)
        {
                distance += Vector3.Distance(agent.path.corners[i], agent.path.corners[i + 1]);
            }
            return distance;
        }

        public void ChangeColor(Color color)
        {
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;
        }
    }
}
