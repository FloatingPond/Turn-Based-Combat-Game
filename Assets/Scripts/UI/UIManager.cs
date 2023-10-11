using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PG
{
    public class UIManager : MonoBehaviour
    {
        public TextMeshProUGUI DistanceText, MovementRemainingText, ActionRemainingText;
        public Image ProjectedMovementIndicator;
        public GhostManager GhostManager;
        public Transform AimTargetIK;
        private MeshRenderer grenadeIndicator;
        public WeaponTrajectoryIndicator WeaponTrajectoryIndicator;
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
        void Start()
        {
            GhostManager = FindObjectOfType<GhostManager>();
            WeaponTrajectoryIndicator = FindObjectOfType<WeaponTrajectoryIndicator>();
        }

        public void SwitchWeapon()
        {
            RoundManager.Instance.unitTakingTurn_UnitController.unitActions.SwitchToGrenade(); //Need to take in weapon to switch to at later point
        }

        public void SwitchGrenadeIndicatorRenderer()
        {
            if (AimTargetIK.TryGetComponent<MeshRenderer>(out MeshRenderer meshRenderer)) grenadeIndicator = meshRenderer; else grenadeIndicator = null;
            grenadeIndicator.enabled = !grenadeIndicator.enabled;
        }
    }
}
