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
            //Updates the distance UI to tick down the remaining distance as the unit moves toward it's destination
            if (roundManager.unitTakingTurn.GetComponent<UnitMovement>().unitMoving)
            {
                uIManager.distanceText.text = lineRendererPath.CalculatePathDistance(roundManager.unitTakingTurn.GetComponent<NavMeshAgent>()).ToString("F1") + "m";
            }
            //Clears the distance text once the unit has reached it's destination
            if (!roundManager.unitTakingTurn.GetComponent<UnitMovement>().unitMoving && roundManager.unitTakingTurn.GetComponent<NavMeshAgent>().remainingDistance < 0.01)
            {
                ClearMovementUI();
            }
        }
        public void OnClick()
        {
            if (!roundManager.unitTakingTurn.GetComponent<UnitMovement>().movementComplete) roundManager.unitTakingTurn.GetComponent<UnitMovement>().CheckRemainingMovement();
        }

        public void OnHoverEnter()
        {

        }
        public void OnHoverStay()
        {

            if (!roundManager.unitTakingTurn.GetComponent<UnitMovement>().unitMoving && !roundManager.unitTakingTurn.GetComponent<UnitMovement>().movementComplete)
            {
                lineRendererPath.DrawPath(roundManager.unitTakingTurn.GetComponent<NavMeshAgent>(), inputManager.hoverWorldPosition);
                //Update distance text position & content
                uIManager.distanceText.text = lineRendererPath.CalculatePathDistance(roundManager.unitTakingTurn.GetComponent<NavMeshAgent>()).ToString("F1") + "m";
                //Update projected movement indicator position & active state
                uIManager.projectedMovementIndicator.enabled = true;
                uIManager.projectedMovementIndicator.transform.position = inputManager.hoverWorldPosition;
                //Update ghost position & opacity
                uIManager.ghost.ShowGhost();
                uIManager.ghost.transform.position = inputManager.hoverWorldPosition;

                if (roundManager.unitTakingTurn.GetComponent<UnitMovement>().currentMovementRemaining < lineRendererPath.CalculatePathDistance(roundManager.unitTakingTurn.GetComponent<NavMeshAgent>()))
                {
                    foreach (Renderer renderer in uIManager.ghost.renderers)
                    {
                        renderer.material.SetColor("_Color",Color.red);
                    }
                    uIManager.ghost.skinnedMesh.material.SetColor("_Color", Color.red);
                    uIManager.distanceText.color = Color.red;
                }
                else
                {
                    foreach (Renderer renderer in uIManager.ghost.renderers)
                    {
                        renderer.material.SetColor("_Color", Color.white);
                    }
                    uIManager.ghost.skinnedMesh.material.SetColor("_Color", Color.white);
                    uIManager.distanceText.color = Color.white;
                }
                lineRendererPath.ChangeColor(roundManager.unitTakingTurn.GetComponent<NavMeshAgent>());
                //Turn unit to face ghost
                Vector3 direction = roundManager.unitTakingTurn.transform.position - uIManager.ghost.transform.position;
                Quaternion rotation = Quaternion.LookRotation(direction);
                rotation *= Quaternion.Euler(0, 180, 0);
                roundManager.unitTakingTurn.transform.rotation = rotation;
            }
        }
        public void OnHoverExit()
        {
            if (!roundManager.unitTakingTurn.GetComponent<UnitMovement>().unitMoving)
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

            uIManager.ghost.HideGhost();
            uIManager.ghost.transform.position = Vector3.zero;
        }

    }
}
