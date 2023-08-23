using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG
{
    public class InteractableObject : MonoBehaviour, Interactable
    {
        private RoundManager roundManager;
        private InputManager inputManager;
        private LineRendererPath lineRendererPath;

        public void OnClick()
        {
            
        }

        public void OnHoverEnter()
        {

        }
        public void OnHoverStay()
        {
            lineRendererPath.PathToTransform(roundManager.unitTakingTurn.transform.position, inputManager.hoverWorldPosition);
            //Debug.Log("Distance from unit taking turn to target: " + Vector3.Distance(roundManager.unitTakingTurn.transform.position, inputManager.hoverWorldPosition) + "m.");
        }
        public void OnHoverExit()
        {
            
        }

        // Start is called before the first frame update
        void Start()
        {
            roundManager = FindObjectOfType<RoundManager>();
            inputManager = FindObjectOfType<InputManager>();
            lineRendererPath = FindObjectOfType<LineRendererPath>();
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
