using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PG
{
    public class UIManager : MonoBehaviour
    {
        public TextMeshProUGUI distanceText, movementRemainingText, actionRemainingText;
        public Image projectedMovementIndicator;
        public GhostManager ghost;
        // Start is called before the first frame update
        void Start()
        {
            ghost = FindObjectOfType<GhostManager>();
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
