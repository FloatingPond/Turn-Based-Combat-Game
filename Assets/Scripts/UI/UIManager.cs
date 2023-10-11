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
            RoundManager.Instance.unitTakingTurn_UnitController.UnitActions.SwitchToGrenade(); //Need to take in weapon to switch to at later point
        }

        public void SwitchGrenadeIndicatorRenderer()
        {
            AimTargetIK = RoundManager.Instance.unitTakingTurn_UnitController.AimTargetIK;
            if (AimTargetIK.TryGetComponent<MeshRenderer>(out MeshRenderer meshRenderer)) grenadeIndicator = meshRenderer; else grenadeIndicator = null;

            if (grenadeIndicator != null) 
            {
                float grenadeExplosionSize = RoundManager.Instance.unitTakingTurn_UnitController.UnitActions.CurrentWeapon.AreaOfEffectRange;
                Vector3 grenadeExplosionSizeVector = new (grenadeExplosionSize, grenadeExplosionSize, grenadeExplosionSize);
                grenadeIndicator.enabled = !grenadeIndicator.enabled;
                AimTargetIK.localScale += grenadeExplosionSizeVector;
            }
        }
    }
}
