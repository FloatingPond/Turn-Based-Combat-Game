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

                if (interactable != null)
                {
                    // Store the world position the player is hovering on
                    hoverWorldPosition = hit.point;
                    if (currentHover == null)
                    {
                        currentHover = interactable;
                        currentHover.OnHoverEnter();
                    }
                    else if (currentHover != interactable)
                    {
                        currentHover.OnHoverExit();
                        currentHover = interactable;
                        currentHover.OnHoverEnter();
                    }
                    else if (currentHover == interactable)
                    {
                        currentHover.OnHoverStay();
                    }

                    if (Input.GetMouseButtonDown(0))
                    {
                        currentHover.OnClick();
                    }
                }
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
