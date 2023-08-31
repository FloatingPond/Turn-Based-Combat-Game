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
        public GhostManager ghostManager;
        public GunShotRenderer gunShotRenderer;
        // Start is called before the first frame update
        void Start()
        {
            ghostManager = FindObjectOfType<GhostManager>();
            gunShotRenderer = FindObjectOfType<GunShotRenderer>();
        }
    }
}
