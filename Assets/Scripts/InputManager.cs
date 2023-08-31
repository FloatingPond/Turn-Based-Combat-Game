using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace PG
{
    public class InputManager : MonoBehaviour
    {
        private IInteractable currentHover;
        [SerializeField]
        public Vector3 hoverWorldPosition;
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

        void Update()
        {
            #region Interactable
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();

                //If we have just hovered over something interactable:
                if (interactable != null)
                {
                    // Store the world position the player is hovering on
                    
                    Vector3 groundNavMeshHeightOffset = hit.point;
                    groundNavMeshHeightOffset.y = 1;
                    NavMeshHit navMeshHit;
                    NavMesh.SamplePosition(groundNavMeshHeightOffset, out navMeshHit, 1, NavMesh.AllAreas);
                    if (navMeshHit.position.x == hit.point.x && navMeshHit.position.z == hit.point.z) hoverWorldPosition = hit.point;
                    else { currentHover.OnHoverExit(); hoverWorldPosition = Vector3.zero; return; }

                    RoundManager.Instance.unitTakingTurn.myLookAtTarget = hoverWorldPosition;
                    //If we haven't hovered over something yet, make the thing we hovered over this frame our current hover
                    if (currentHover == null)
                    {
                        currentHover = interactable;
                        currentHover.OnHoverEnter();
                    }
                    //If the thing we've hovered over is interactable but different from the interactable from last frame, call exit then enter again
                    else if (currentHover != interactable)
                    {
                        currentHover.OnHoverExit();
                        currentHover = interactable;
                        currentHover.OnHoverEnter();
                    }
                    //If we're hovering over the same interactable from last frame, call stay
                    else if (currentHover == interactable)
                    {
                        currentHover.OnHoverStay();
                    }
                    //If we click, call click
                    if (Input.GetMouseButtonDown(0))
                    {
                        currentHover.OnClick();
                    }
                }
                //If it's not interactable, call exit
                else if (currentHover != null)
                {
                    currentHover.OnHoverExit();
                    currentHover = null;
                    hoverWorldPosition  = Vector3.zero;
                }
            }
            else if (currentHover != null)
            {
                currentHover.OnHoverExit();
                currentHover = null;
                hoverWorldPosition = Vector3.zero;
            }
            #endregion

        }
    }

}
