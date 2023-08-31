using System.Net;
using UnityEngine;
using UnityEngine.AI;

namespace PG
{
    public class LineRendererPath : MonoBehaviour
    {
        [SerializeField]
        private LineRenderer lineRenderer;
        private RoundManager roundManager;
        private float distance = 0f;
        private Gradient gradient;
        private float percentage;
        void Start()
        {
            lineRenderer = GetComponent<LineRenderer>();
            roundManager = FindObjectOfType<RoundManager>();
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
            distance = 0;
            for (int i = 0; i < agent.path.corners.Length - 1; i++)
            {
                distance += Vector3.Distance(agent.path.corners[i], agent.path.corners[i + 1]);
            }
            distance = Mathf.Round(distance * 10) / 10;
            return distance;
        }
    }
}
