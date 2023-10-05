using UnityEngine;

namespace PG
{
    public class PathableObject : MonoBehaviour, IInteractable
    {
        private LineRendererPath lineRendererPath;

        // Start is called before the first frame update
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
                UIManager.Instance.distanceText.text = lineRendererPath.CalculatePathDistance(RoundManager.Instance.unitTakingTurn_UnitController.agent).ToString("F1") + "m";
            }
            //Clears the distance text once the unit has reached it's destination
            if (!RoundManager.Instance.unitTakingTurn_UnitController.unitMovement.unitMoving && RoundManager.Instance.unitTakingTurn_UnitController.agent.remainingDistance < 0.01)
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
                UIManager.Instance.distanceText.text = lineRendererPath.CalculatePathDistance(RoundManager.Instance.unitTakingTurn_UnitController.agent).ToString("F1") + "m";
                //Update projected movement indicator position & active state
                UIManager.Instance.projectedMovementIndicator.enabled = true;

                //Update ghost opacity
                UIManager.Instance.ghostManager.ShowGhost();

                if (RoundManager.Instance.unitTakingTurn_UnitController.unitMovement.currentMovementRemaining < lineRendererPath.CalculatePathDistance(RoundManager.Instance.unitTakingTurn_UnitController.agent))
                {
                    foreach (Renderer renderer in UIManager.Instance.ghostManager.renderers)
                    {
                        renderer.material.SetColor("_Color", Color.red);
                    }
                    UIManager.Instance.ghostManager.skinnedMesh.material.SetColor("_Color", Color.red);
                    UIManager.Instance.distanceText.color = Color.red;
                }
                else
                {
                    foreach (Renderer renderer in UIManager.Instance.ghostManager.renderers)
                    {
                        renderer.material.SetColor("_Color", Color.white);
                    }

                    UIManager.Instance.ghostManager.skinnedMesh.material.SetColor("_Color", Color.white);
                    UIManager.Instance.distanceText.color = Color.white;
                }

                Vector3 clampedVect3 = new Vector3 (Mathf.Sign(InputManager.Instance.hoverWorldPosition.x) * (Mathf.Abs((int)InputManager.Instance.hoverWorldPosition.x) + 0.5f),
                                                    RoundManager.Instance.unitTakingTurn_UnitController.transform.position.y,
                                                    Mathf.Sign(InputManager.Instance.hoverWorldPosition.z) * (Mathf.Abs((int)InputManager.Instance.hoverWorldPosition.z) + 0.5f));

                if (clampedVect3 != RoundManager.Instance.unitTakingTurn_UnitController.transform.position)
                {
                    UIManager.Instance.projectedMovementIndicator.transform.position = clampedVect3;
                    UIManager.Instance.ghostManager.transform.position = clampedVect3;
                    UIManager.Instance.ghostManager.transform.rotation = RoundManager.Instance.unitTakingTurn_UnitController.transform.rotation;
                }
                lineRendererPath.DrawPath(RoundManager.Instance.unitTakingTurn_UnitController.agent, UIManager.Instance.ghostManager.transform.position);
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

            UIManager.Instance.distanceText.text = "";

            UIManager.Instance.projectedMovementIndicator.enabled = false;
            UIManager.Instance.projectedMovementIndicator.transform.position = Vector3.zero;

            UIManager.Instance.ghostManager.HideGhost();
            UIManager.Instance.ghostManager.transform.position = Vector3.zero;
        }   

    }
}
