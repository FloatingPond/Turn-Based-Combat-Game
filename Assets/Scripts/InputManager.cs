using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG
{
    public class InputManager : MonoBehaviour
    {
        private Interactable currentHover;
        [SerializeField]
        public Vector3 hoverWorldPosition;
        void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Interactable interactable = hit.collider.GetComponent<Interactable>();

                //If we have just hovered over something interactable:
                if (interactable != null)
                {
                    // Store the world position the player is hovering on
                    hoverWorldPosition = hit.point;

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
        }
    }

}
