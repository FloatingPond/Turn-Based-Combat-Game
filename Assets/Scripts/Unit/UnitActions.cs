using UnityEngine;

namespace PG
{
    public class UnitActions : MonoBehaviour, IInteractable
    {
        public Transform GunBarrel;
        [SerializeField] private ParticleSystem gunSmoke;
        [SerializeField] private ParticleSystem bulletImpactFX;
        public GameObject MyDamageableTarget;
        public WeaponData CurrentWeapon;
        public Grenade grenade;
        [SerializeField] private WeaponData rifleData, grenadeData;
        public bool PerformingAction = false;
        private void Start()
        {
            gunSmoke.Stop();
            bulletImpactFX.Stop();
            grenade.ChangeGrenadeRenderers(false);
        }
        public bool UsedAction = false;
        public Action UnitAction;
        public enum Action { shoot, throwGrenade };
        public void PerformAction(Action action)
        {
            if (!UsedAction && !PerformingAction)
            {
                PerformingAction = true;
                RoundManager.Instance.TransitionCameraToUnit(RoundManager.Instance.unitTakingTurn_UnitController);
                switch (action)
                {
                    case Action.shoot:
                        Shoot();
                        break;
                    case Action.throwGrenade:
                        GetCurrentUnitAnimator().SetTrigger("ThrowGrenade");
                        break;
                    default:
                        break;
                }
                UsedAction = true;
                UIManager.Instance.ActionRemainingText.text = "No Actions Remaining";
            }
        }
        private Animator GetCurrentUnitAnimator()
        {
            return RoundManager.Instance.unitTakingTurn_UnitController.UnitAnimation.animator;
        }
        public void Shoot()
        {
            gunSmoke.Play();
            WeaponTrajectoryIndicator.Instance.ClearRendererPositions();
            bulletImpactFX.transform.position = WeaponTrajectoryIndicator.Instance.hitPosition;
            #region Rotate bullet impact fx (particle system) to face the player
            Vector3 direction = MyDamageableTarget.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction, transform.up);
            bulletImpactFX.transform.rotation = Quaternion.Euler(rotation.eulerAngles.x, rotation.eulerAngles.y - 90, rotation.eulerAngles.z);
            #endregion
            bulletImpactFX.Play();
            MyDamageableTarget.GetComponent<IDamageable>().TakeDamage(RoundManager.Instance.unitTakingTurn_UnitController.UnitData.damage);
            GetCurrentUnitAnimator().SetTrigger("Shoot");
        }
        public void ThrowGrenade()
        {
            UIManager.Instance.SwitchGrenadeIndicatorRenderer();
            Vector3 direction = RoundManager.Instance.unitTakingTurn_UnitController.MyLookAtTargetVector - transform.position;
            float forwardForce = Vector3.Distance(RoundManager.Instance.unitTakingTurn_UnitController.MyLookAtTargetVector, transform.position);
            float verticalForce = forwardForce / 2;
            grenade.Throw(direction, forwardForce, verticalForce);
        }

        public void SwitchToGrenade()
        {
            CurrentWeapon = grenadeData;
            UnitAction = Action.throwGrenade;
            grenade.ChangeGrenadeRenderers(true);
            GetCurrentUnitAnimator().SetBool("AimGrenade", true);
            RoundManager.Instance.unitTakingTurn_UnitController.UnitAnimation.SetRigsForRunning();
            UIManager.Instance.SwitchGrenadeIndicatorRenderer();
        }
        public void SwitchToRifle()
        {
            CurrentWeapon = rifleData;
            UnitAction = Action.shoot;
            GetCurrentUnitAnimator().SetBool("AimGrenade", false);
            RoundManager.Instance.unitTakingTurn_UnitController.UnitAnimation.SetRigsForAiming();
        }
        void IInteractable.OnHoverEnter()
        {
            
        }

        void IInteractable.OnHoverStay()
        {
            
        }

        void IInteractable.OnHoverExit()
        {
           
        }

        void IInteractable.OnClick()
        {
            //Will want to provide details here (Think of when zooming in on an enemy in xcom 2 by clicking on them)
            //PerformAction(unitAction);
        }
    }
}
