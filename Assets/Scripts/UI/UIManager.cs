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
        public WeaponTrajectoryIndicator gunShotRenderer;
        #region Singleton
        public static UIManager Instance { get; private set; }

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
        // Start is called before the first frame update
        void Start()
        {
            ghostManager = FindObjectOfType<GhostManager>();
            gunShotRenderer = FindObjectOfType<WeaponTrajectoryIndicator>();
        }

        public void SwitchWeapon()
        {
            RoundManager.Instance.unitTakingTurn_UnitController.unitActions.SwitchToGrenade(); //Need to take in weapon to switch to at later point
        }
    }
}
