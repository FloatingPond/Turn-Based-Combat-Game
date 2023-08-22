using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG
{
    public class InteractableObject : MonoBehaviour, Interactable
    {
        private RoundManager rm;

        public void OnClick()
        {
            
        }

        public void OnHoverEnter()
        {
            Debug.Log(rm.unitTakingTurn);
        }

        public void OnHoverExit()
        {
            
        }

        // Start is called before the first frame update
        void Start()
        {
            rm = FindObjectOfType<RoundManager>();
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
