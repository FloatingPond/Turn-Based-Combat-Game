using UnityEngine;

namespace PG
{
    public class PathableObject : MonoBehaviour, IInteractable
    {
        private LineRendererPath lineRendererPath;

        void Start()
        {
            lineRendererPath = FindObjectOfType<LineRendererPath>();
        }
        private void Update()
        { 
            if (RoundManager.Instance.unitTakingTurn_UnitController.myTeam == UnitController.team.computer || RoundManager.Instance.unitTakingTurn_UnitController.unitAnimation.animator.GetBool("AimGrenade")) { ClearMovementUI(); return; }
            //Updates the distance UI to tick down the remaining distance as the unit moves toward it's destination
            if (RoundManager.Instance.unitTakingTurn_UnitController.unitMovement.unitMoving)
            {
                UIManager.Instance.DistanceText.text = lineRendererPath.CalculatePathDistance(RoundManager.Instance.unitTakingTurn_UnitController.agent).ToString("F1") + "m";
            }
            //Clears the distance text once the unit has reached it's destination
            else
            {
                ClearMovementUI();
            }
        }
        public void OnClick()
        {
            if (RoundManager.Instance.unitTakingTurn_UnitController.myTeam == UnitController.team.computer || RoundManager.Instance.unitTakingTurn_UnitController.unitAnimation.animator.GetBool("AimGrenade")) { ClearMovementUI(); return; }
            if (!RoundManager.Instance.unitTakingTurn_UnitController.unitMovement.movementComplete) RoundManager.Instance.unitTakingTurn_UnitController.unitMovement.CheckRemainingMovement();
        }

        public void OnHoverEnter()
        {

        }
        public void OnHoverStay()
        {
            if (RoundManager.Instance.unitTakingTurn_UnitController.myTeam == UnitController.team.computer || RoundManager.Instance.unitTakingTurn_UnitController.unitAnimation.animator.GetBool("AimGrenade")) { ClearMovementUI(); return; }
            if (!RoundManager.Instance.unitTakingTurn_UnitController.unitMovement.unitMoving && !RoundManager.Instance.unitTakingTurn_UnitController.unitMovement.movementComplete)
            {
                //Update distance text position & content
                UIManager.Instance.DistanceText.text = lineRendererPath.CalculatePathDistance(RoundManager.Instance.unitTakingTurn_UnitController.agent).ToString("F1") + "m";
                //Update projected movement indicator position & active state
                UIManager.Instance.ProjectedMovementIndicator.enabled = true;

                //Update ghost opacity
                UIManager.Instance.GhostManager.ShowGhost();

                if (RoundManager.Instance.unitTakingTurn_UnitController.unitMovement.currentMovementRemaining < lineRendererPath.CalculatePathDistance(RoundManager.Instance.unitTakingTurn_UnitController.agent))
                {
                    foreach (Renderer renderer in UIManager.Instance.GhostManager.renderers)
                    {
                        renderer.material.SetColor("_Color", Color.red);
                    }
                    UIManager.Instance.GhostManager.skinnedMesh.material.SetColor("_Color", Color.red);
                    UIManager.Instance.DistanceText.color = Color.red;
                }
                else
                {
                    foreach (Renderer renderer in UIManager.Instance.GhostManager.renderers)
                    {
                        renderer.material.SetColor("_Color", Color.white);
                    }

                    UIManager.Instance.GhostManager.skinnedMesh.material.SetColor("_Color", Color.white);
                    UIManager.Instance.DistanceText.color = Color.white;
                }

                Vector3 clampedVect3 = new Vector3 (Mathf.Sign(InputManager.Instance.hoverWorldPosition.x) * (Mathf.Abs((int)InputManager.Instance.hoverWorldPosition.x) + 0.5f),
                                                    RoundManager.Instance.unitTakingTurn_UnitController.transform.position.y,
                                                    Mathf.Sign(InputManager.Instance.hoverWorldPosition.z) * (Mathf.Abs((int)InputManager.Instance.hoverWorldPosition.z) + 0.5f));

                if (clampedVect3 != RoundManager.Instance.unitTakingTurn_UnitController.transform.position)
                {
                    UIManager.Instance.ProjectedMovementIndicator.transform.position = clampedVect3;
                    UIManager.Instance.GhostManager.transform.position = clampedVect3;
                    UIManager.Instance.GhostManager.transform.rotation = RoundManager.Instance.unitTakingTurn_UnitController.transform.rotation;
                }
                lineRendererPath.DrawPath(RoundManager.Instance.unitTakingTurn_UnitController.agent, UIManager.Instance.GhostManager.transform.position);
            }
        }
        public void OnHoverExit()
        {
            if (!RoundManager.Instance.unitTakingTurn_UnitController.unitMovement.unitMoving)
            {
                ClearMovementUI();
            }
        }
        private void ClearMovementUI()
        {
            lineRendererPath.ClearPath();

            UIManager.Instance.DistanceText.text = "";

            UIManager.Instance.ProjectedMovementIndicator.enabled = false;
            UIManager.Instance.ProjectedMovementIndicator.transform.position = Vector3.zero;

            UIManager.Instance.GhostManager.HideGhost();
            UIManager.Instance.GhostManager.transform.position = Vector3.zero;
        }   

    }
}
