using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
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
        private LineRenderer[] renderers;
        private RoundManager roundManager;
        private float distance = 0f;
        void Start()
        {
            renderers = GetComponentsInChildren<LineRenderer>();
            roundManager = FindObjectOfType<RoundManager>();
        }
        public void ClearPath()
        {
            renderers[0].positionCount = 0;
            renderers[1].positionCount = 0;
        }
        public void DrawPath(NavMeshAgent agent, Vector3 target)
        {
            agent.SetDestination(target);
            if (agent.path.status != NavMeshPathStatus.PathInvalid)
            {
                if (distance <= roundManager.unitTakingTurn.GetComponent<UnitMovement>().currentMovementRemaining)
                {
                    //WHITE
                    renderers[0].positionCount = agent.path.corners.Length;
                    renderers[1].positionCount = agent.path.corners.Length;
                    for (int i = 0; i < agent.path.corners.Length; i++)
                    {
                        renderers[0].SetPosition(i, agent.path.corners[i]);
                        renderers[1].SetPosition(i, agent.path.corners[i]);
                    }
                }
                else
                {
                    //RED

                    renderers[0].positionCount = 1;
                    renderers[0].SetPosition(0, agent.transform.position);
                    float overlapDistance = distance - roundManager.unitTakingTurn.GetComponent<UnitMovement>().currentMovementRemaining;
                    float percentage = overlapDistance / distance;

                    Vector3 maxMovementPoint = Vector3.zero;
                    
                    renderers[1].positionCount = agent.path.corners.Length;
                    if (renderers[1].positionCount <= 2)
                    {
                        renderers[0].positionCount = agent.path.corners.Length;
                        maxMovementPoint = Vector3.Lerp(renderers[1].GetPosition(0), renderers[1].GetPosition(renderers[1].positionCount - 1), 1.0f - percentage);
                    }
                    else
                    {
                        int z;
                        float lengthSoFar = 0f;
                        float lastPointLength = 0f;

                        for (z = 1; z < agent.path.corners.Length - 1; z++)
                        {
                            renderers[0].positionCount++;
                            lengthSoFar += Vector3.Distance(agent.path.corners[z], agent.path.corners[z + 1]);
                            if (lengthSoFar > roundManager.unitTakingTurn.GetComponent<UnitMovement>().currentMovementRemaining)
                            {
                                break;
                            }
                            lastPointLength = lengthSoFar;
                            renderers[0].SetPosition(z, agent.path.corners[z]);
                        }
                        maxMovementPoint = Vector3.Lerp(renderers[1].GetPosition(z), renderers[1].GetPosition(z - 1),
                        1.0f - ((roundManager.unitTakingTurn.GetComponent<UnitMovement>().currentMovementRemaining - lastPointLength) / roundManager.unitTakingTurn.GetComponent<UnitMovement>().currentMovementRemaining));
                    }
                    renderers[0].SetPosition(renderers[0].positionCount - 1, maxMovementPoint);
                    for (int i = 0; i < agent.path.corners.Length; i++)
                    {
                        renderers[1].SetPosition(i, agent.path.corners[i]);
                    }
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
