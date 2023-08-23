using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG
{
    public class InteractableObject : MonoBehaviour, Interactable
    {
        private RoundManager rm;
        private InputManager inputManager;

        public void OnClick()
        {
            
        }

        public void OnHoverEnter()
        {

        }
        public void OnHoverStay()
        {
            Debug.Log("Distance from unit taking turn to target: " + Vector3.Distance(rm.unitTakingTurn.transform.position, inputManager.hoverWorldPosition) + "m.");
        }
        public void OnHoverExit()
        {
            
        }

        // Start is called before the first frame update
        void Start()
        {
            rm = FindObjectOfType<RoundManager>();
            inputManager = FindObjectOfType<InputManager>();
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
