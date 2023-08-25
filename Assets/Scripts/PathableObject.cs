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
            if (roundManager.unitTakingTurn.GetComponent<UnitMovement>().unitMoving)
            {
                uIManager.distanceText.text = roundManager.unitTakingTurn.GetComponent<NavMeshAgent>().remainingDistance.ToString("F1") + "m";
            }
        }
        public void OnClick()
        {
            roundManager.unitTakingTurn.GetComponent<UnitMovement>().MoveToDestination();
        }

        public void OnHoverEnter()
        {

        }
        public void OnHoverStay()
        {

            if (!roundManager.unitTakingTurn.GetComponent<UnitMovement>().unitMoving)
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
