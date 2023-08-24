using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace PG
{
    public class InteractableObject : MonoBehaviour, IInteractable
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
            lineRendererPath.DrawPath(roundManager.unitTakingTurn.GetComponent<NavMeshAgent>(), inputManager.hoverWorldPosition);
        }
        public void OnHoverExit()
        {
            lineRendererPath.ClearPath();
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
