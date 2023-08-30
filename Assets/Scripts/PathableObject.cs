using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace PG
{
    public class PathableObject : MonoBehaviour, IInteractable
    {
        private RoundManager roundManager;
        private InputManager inputManager;
        private LineRendererPath lineRendererPath;
        private UIManager uIManager;

        // Start is called before the first frame update
        void Start()
        {
            roundManager = FindObjectOfType<RoundManager>();
            inputManager = FindObjectOfType<InputManager>();
            lineRendererPath = FindObjectOfType<LineRendererPath>();
            uIManager = FindObjectOfType<UIManager>();
        }
        private void Update()
        {
            UnitMovement unitTakingTurnMovement = roundManager.unitTakingTurn.GetComponent<UnitMovement>();
            NavMeshAgent agentTakingTurn = roundManager.unitTakingTurn.GetComponent<NavMeshAgent>();
            //Updates the distance UI to tick down the remaining distance as the unit moves toward it's destination
            if (unitTakingTurnMovement.unitMoving)
            {
                uIManager.distanceText.text = lineRendererPath.CalculatePathDistance(agentTakingTurn).ToString("F1") + "m";
            }
            //Clears the distance text once the unit has reached it's destination
            if (!unitTakingTurnMovement.unitMoving && agentTakingTurn.remainingDistance < 0.01)
            {
                ClearMovementUI();
            }
        }
        public void OnClick()
        {
            UnitMovement unitTakingTurnMovement = roundManager.unitTakingTurn.GetComponent<UnitMovement>();
            if (!unitTakingTurnMovement.movementComplete) unitTakingTurnMovement.CheckRemainingMovement();
        }

        public void OnHoverEnter()
        {

        }
        public void OnHoverStay()
        {
            UnitMovement unitTakingTurnMovement = roundManager.unitTakingTurn.GetComponent<UnitMovement>();
            NavMeshAgent agentTakingTurn = roundManager.unitTakingTurn.GetComponent<NavMeshAgent>();
            if (!unitTakingTurnMovement.unitMoving && !unitTakingTurnMovement.movementComplete)
            {
                //Update distance text position & content
                uIManager.distanceText.text = lineRendererPath.CalculatePathDistance(agentTakingTurn).ToString("F1") + "m";
                //Update projected movement indicator position & active state
                uIManager.projectedMovementIndicator.enabled = true;
                
                //Update ghost opacity
                uIManager.ghostManager.ShowGhost();

                if (unitTakingTurnMovement.currentMovementRemaining < lineRendererPath.CalculatePathDistance(agentTakingTurn))
                {
                    foreach (Renderer renderer in uIManager.ghostManager.renderers)
                    {
                        renderer.material.SetColor("_Color", Color.red);
                    }
                    uIManager.ghostManager.skinnedMesh.material.SetColor("_Color", Color.red);
                    uIManager.distanceText.color = Color.red;
                }
                else
                {
                    foreach (Renderer renderer in uIManager.ghostManager.renderers)
                    {
                        renderer.material.SetColor("_Color", Color.white);
                    }

                    uIManager.ghostManager.skinnedMesh.material.SetColor("_Color", Color.white);
                    uIManager.distanceText.color = Color.white;
                }

                Vector3 clampedVect3 = new Vector3 (Mathf.Sign(inputManager.hoverWorldPosition.x) * (Mathf.Abs((int)inputManager.hoverWorldPosition.x) + 0.5f),
                                                    roundManager.unitTakingTurn.transform.position.y,
                                                    Mathf.Sign(inputManager.hoverWorldPosition.z) * (Mathf.Abs((int)inputManager.hoverWorldPosition.z) + 0.5f));

                if (clampedVect3 != roundManager.unitTakingTurn.transform.position)
                {
                    uIManager.projectedMovementIndicator.transform.position = clampedVect3;
                    uIManager.ghostManager.transform.position = clampedVect3;
                }
                lineRendererPath.DrawPath(agentTakingTurn, uIManager.ghostManager.transform.position);
                #region Turn unit to face ghost
                Vector3 direction = roundManager.unitTakingTurn.transform.position - uIManager.ghostManager.transform.position;
                Quaternion rotation = Quaternion.LookRotation(direction);
                rotation *= Quaternion.Euler(0, 180, 0);
                roundManager.unitTakingTurn.transform.rotation = rotation;
                #endregion
            }
        }
        public void OnHoverExit()
        {
            UnitMovement unitTakingTurnMovement = roundManager.unitTakingTurn.GetComponent<UnitMovement>();
            if (!unitTakingTurnMovement.unitMoving)
            {
                ClearMovementUI();
            }
        }
        private void ClearMovementUI()
        {
            lineRendererPath.ClearPath();

            uIManager.distanceText.text = "";

            uIManager.projectedMovementIndicator.enabled = false;
            uIManager.projectedMovementIndicator.transform.position = Vector3.zero;

            uIManager.ghostManager.HideGhost();
            uIManager.ghostManager.transform.position = Vector3.zero;
        }

    }
}
