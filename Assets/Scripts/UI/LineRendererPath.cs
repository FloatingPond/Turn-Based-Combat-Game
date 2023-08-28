using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;
using UnityEngine.Events;

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

        public Vector3 CalculateMaxMovementPoint(NavMeshAgent agent)
        {
            percentage = 0;
            Vector3 maxMovementPoint = Vector3.zero;

            Vector3 previousCorner = agent.path.corners[0];
            float lengthSoFar = 0.0F;
            int i = 1;
            while (i < agent.path.corners.Length)
            {
                Vector3 currentCorner = agent.path.corners[i];
                lengthSoFar += Vector3.Distance(previousCorner, currentCorner);
                if (lengthSoFar >= roundManager.unitTakingTurn.GetComponent<UnitMovement>().currentMovementRemaining) break;
                previousCorner = currentCorner;
                i++;
            }
            float overlapDistance = lengthSoFar - roundManager.unitTakingTurn.GetComponent<UnitMovement>().currentMovementRemaining;
            percentage = overlapDistance / distance;
            if (lineRenderer.positionCount <= 2)
            {
                maxMovementPoint = Vector3.Lerp(lineRenderer.GetPosition(0), lineRenderer.GetPosition(lineRenderer.positionCount - 1), 1.0f - percentage);
            }
            else
            {
                maxMovementPoint = Vector3.Lerp(lineRenderer.GetPosition(i - 2), lineRenderer.GetPosition(i - 1), 1.0f - percentage);
            }
            return maxMovementPoint;
        }

        public void ChangeGradientColour()
        {
            // Create a new gradient with color keys
            gradient = new Gradient();
            gradient.mode = GradientMode.Fixed;
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(Color.white, 0.0f), new GradientColorKey(Color.white, 1.0f - percentage), new GradientColorKey(Color.red, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) }
            );

            // Assign the gradient to the LineRenderer
            lineRenderer.colorGradient = gradient;
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
