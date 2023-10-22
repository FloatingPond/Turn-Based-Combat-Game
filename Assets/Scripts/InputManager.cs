using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace PG
{
    public class InputManager : MonoBehaviour
    {
        private IInteractable currentHover, currentInteractable;
        private RaycastHit hit;
        public Vector3 hoverWorldPosition;
        [SerializeField] private EventSystem eventSystem;
        LayerMask worldspaceLayer;
        #region Singleton
        public static InputManager Instance { get; private set; }

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        #endregion
        private void Start()
        {
            worldspaceLayer = LayerMask.GetMask("WorldspaceUI");
        }
        private bool CheckIfPositionIsOnNavMesh(Vector3 point)
        {
            Vector3 groundNavMeshHeightOffset = point;
            groundNavMeshHeightOffset.y = 1;
            NavMeshHit navMeshHit;
            NavMesh.SamplePosition(groundNavMeshHeightOffset, out navMeshHit, 1, NavMesh.AllAreas);
            return (navMeshHit.position.x == point.x && navMeshHit.position.z == point.z);
        }
        private void Update()
        {
            if (RoundManager.Instance.unitTakingTurn_UnitController.UnitActions.PerformingAction 
                || RoundManager.Instance.unitTakingTurn_UnitController.MyTeam == UnitController.Team.computer) return;
            //If we have just hovered over something interactable:
            if (currentInteractable != null)
            {
                // Store the world position the player is hovering on

                if (CheckIfPositionIsOnNavMesh(hit.point))
                {
                    hoverWorldPosition = hit.point;
                    RoundManager.Instance.unitTakingTurn_UnitController.MyLookAtTargetVector = hoverWorldPosition;
                    //If we haven't hovered over something yet, make the thing we hovered over this frame our current hover
                    if (currentHover == null)
                    {
                        currentHover = currentInteractable;
                        currentHover.OnHoverEnter();
                    }
                    //If the thing we've hovered over is interactable but different from the interactable from last frame, call exit then enter again
                    else if (currentHover != currentInteractable)
                    {
                        currentHover.OnHoverExit();
                        currentHover = currentInteractable;
                        currentHover.OnHoverEnter();
                    }
                    //If we're hovering over the same interactable from last frame, call stay
                    else if (currentHover == currentInteractable)
                    {
                        currentHover.OnHoverStay();
                    }
                    //If we click, call click
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (!eventSystem.IsPointerOverGameObject() || (eventSystem.IsPointerOverGameObject() && hit.collider.gameObject.layer != worldspaceLayer))
                            currentHover.OnClick();
                    }
                }
                else
                {
                    //Position we're hovering over isn't on the navmesh
                    if (currentHover == null)
                    {
                        currentHover = currentInteractable;
                        currentHover.OnHoverEnter();
                    }
                    else
                    {
                        currentHover.OnHoverExit();
                        currentHover = currentInteractable;
                        currentHover.OnHoverEnter();
                    }
                    //If we click, call click
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (!eventSystem.IsPointerOverGameObject() || (eventSystem.IsPointerOverGameObject() && hit.collider.gameObject.layer != worldspaceLayer))
                            currentHover.OnClick();
                    }
                }
            }
            //If it's not interactable, call exit
            else
            {
                if (currentHover != null) { currentHover.OnHoverExit(); currentHover = null; }
            }
        }
        void FixedUpdate()
        {
            #region Interactable
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (eventSystem.IsPointerOverGameObject() && hit.collider.gameObject.layer != worldspaceLayer)
                {
                    currentInteractable = null;
                    return;
                }

                _ = hit.collider.TryGetComponent(out IInteractable interactable);
                currentInteractable = interactable;
            }
            else //If we hit nothing, it's not interactable so call exit
            {
                currentHover?.OnHoverExit();
                currentHover = null;
            }
            #endregion
        }
    }

}
