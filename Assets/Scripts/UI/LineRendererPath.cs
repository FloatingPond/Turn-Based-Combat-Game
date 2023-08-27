using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
        private RoundManager roundManager;
        private float distance = 0f;
        public Gradient gradient;
        void Start()
        {
            lineRenderer = FindObjectOfType<LineRenderer>();
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

        public void ChangeColor(NavMeshAgent agent)
        {
            bool isRed = false;
            if (distance > roundManager.unitTakingTurn.GetComponent<UnitMovement>().currentMovementRemaining)
            {
                float overlapDistance = distance - roundManager.unitTakingTurn.GetComponent<UnitMovement>().currentMovementRemaining;
                float percentage = overlapDistance / distance;
                if (!isRed)
                {
                    isRed = true;
                    if (lineRenderer.positionCount <= 2)
                    {
                        Debug.Log("POSITION: " + Vector3.Lerp(lineRenderer.GetPosition(0), lineRenderer.GetPosition(lineRenderer.positionCount - 1), 1.0f - percentage));
                    }
                    else
                    {
                        int i = 0;
                        float pathLength = 0f;
                        float lastPointLength = 0f;
                        for (i = 0; i < agent.path.corners.Length - 1; i++)
                        {
                            pathLength += Vector3.Distance(agent.path.corners[i], agent.path.corners[i + 1]);
                            if (pathLength > roundManager.unitTakingTurn.GetComponent<UnitMovement>().currentMovementRemaining)
                            {
                                break;
                            }
                            lastPointLength = pathLength;
                        }
                        Vector3.Lerp(lineRenderer.GetPosition(lineRenderer.positionCount - 1), lineRenderer.GetPosition(lineRenderer.positionCount - 2),
                            1.0f - ((roundManager.unitTakingTurn.GetComponent<UnitMovement>().currentMovementRemaining - lastPointLength) / roundManager.unitTakingTurn.GetComponent<UnitMovement>().currentMovementRemaining));
                    }
                }
                // Create a new gradient with color keys
                gradient = new Gradient();
                gradient.SetKeys(
                    new GradientColorKey[] { new GradientColorKey(Color.white, 0.0f), new GradientColorKey(Color.white, (1.0f - percentage) - 0.02f), new GradientColorKey(Color.red, (1.0f - percentage) + 0.02f), new GradientColorKey(Color.red, 1.0f) },
                    new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) }
                );

                // Assign the gradient to the LineRenderer
                lineRenderer.colorGradient = gradient;
            }
            else
            {
                isRed = false;
                lineRenderer.startColor = Color.white;
                lineRenderer.endColor = Color.white;
            }
        }
    }
}
