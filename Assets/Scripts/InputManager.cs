using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

namespace PG
{
    public class InputManager : MonoBehaviour
    {
        private IInteractable currentHover;
        public GameObject test;
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

        private bool CheckPositionOnNavMesh(Vector3 point)
        {
            Vector3 groundNavMeshHeightOffset = point;
            groundNavMeshHeightOffset.y = 1;
            NavMeshHit navMeshHit;
            NavMesh.SamplePosition(groundNavMeshHeightOffset, out navMeshHit, 1, NavMesh.AllAreas);
            if (navMeshHit.position.x == point.x && navMeshHit.position.z == point.z) return true;
            else return false;
        }
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

                    if (CheckPositionOnNavMesh(hit.point))
                    {
                        hoverWorldPosition = hit.point;
                        RoundManager.Instance.unitTakingTurn.myLookAtTarget = hoverWorldPosition;
                        //If we haven't hovered over something yet, make the thing we hovered over this frame our current hover
                        if (currentHover == null)
                        {
                            currentHover = interactable;
                            test = hit.transform.gameObject;
                            currentHover.OnHoverEnter();
                        }
                        //If the thing we've hovered over is interactable but different from the interactable from last frame, call exit then enter again
                        else if (currentHover != interactable)
                        {
                            currentHover.OnHoverExit();
                            currentHover = interactable;
                            test = hit.transform.gameObject;
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
                    else
                    {
                        if (currentHover == null)
                        {
                            currentHover = interactable;
                            currentHover.OnHoverEnter();
                        }
                        else
                        {
                            currentHover.OnHoverExit();
                            currentHover = interactable;
                            currentHover.OnHoverEnter();
                        }
                    }
                }
                //If it's not interactable, call exit
                else
                {
                    if (currentHover != null) { currentHover.OnHoverExit(); currentHover = null; }
                }
            }
            else
            {
                if (currentHover != null) { currentHover.OnHoverExit(); currentHover = null; }
            }
            #endregion
            
        }
    }

}
