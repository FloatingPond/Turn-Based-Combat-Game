using System.Collections;
using UnityEngine;

namespace PG
{
    public class UnitActions : MonoBehaviour, IInteractable
    {
        public Transform GunBarrel;
        [SerializeField] private ParticleSystem gunSmoke;
        [SerializeField] private ParticleSystem bulletImpactFX;
        public GameObject BulletTrail;
        public GameObject MyDamageableTarget;
        public WeaponData CurrentWeapon;
        public Grenade grenade;
        [SerializeField] private WeaponData rifleData, grenadeData;
        public bool PerformingAction = false;
        public int actionsRemaining = 1;
        private void Start()
        {
            if (gunSmoke != null) gunSmoke.Stop();
            if (bulletImpactFX != null) bulletImpactFX.Stop();
            if (grenade != null) grenade.ChangeGrenadeRenderers(false);
            TryGetComponent(out UnitController unitController);
            actionsRemaining = unitController.UnitData.MaxActions;
        }
        public Action UnitAction;
        public enum Action { shoot, throwGrenade };
        public virtual void PerformAction(Action action)
        {
            if (actionsRemaining != 0)
            {
                actionsRemaining--;
                PerformingAction = true;
                RoundManager.Instance.TransitionCameraToUnit(RoundManager.Instance.unitTakingTurn_UnitController, RoundManager.Instance.unitTakingTurn_UnitController.OverShoulderVcam);
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
            }
        }
        private Animator GetCurrentUnitAnimator()
        {
            return RoundManager.Instance.unitTakingTurn_UnitController.UnitAnimation.animator;
        }
        protected virtual void Shoot()
        {
            WeaponTrajectoryIndicator.Instance.ClearRendererPositions();
            gunSmoke.Play();
            bulletImpactFX.transform.position = WeaponTrajectoryIndicator.Instance.hitPosition;
            #region Rotate bullet impact fx (particle system) to face the player
            Vector3 direction = MyDamageableTarget.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction, transform.up);
            bulletImpactFX.transform.rotation = Quaternion.Euler(rotation.eulerAngles.x, rotation.eulerAngles.y - 90, rotation.eulerAngles.z);
            #endregion
            bulletImpactFX.Play();
            BulletTrail.SetActive(true);
            float bulletDistance = Vector3.Distance(GunBarrel.position, MyDamageableTarget.transform.position);
            BulletTrail.transform.localScale = new Vector3(1f, 1f, 0.65f + bulletDistance);
            BulletTrail.transform.localPosition = new Vector3(0, 0, 0.65f + (bulletDistance / 2));
            MyDamageableTarget.GetComponent<IDamageable>().TakeDamage(RoundManager.Instance.unitTakingTurn_UnitController.UnitData.Damage);
            GetCurrentUnitAnimator().SetTrigger("Shoot");
        }
        public virtual void ThrowGrenade()
        {
            Vector3 direction = RoundManager.Instance.unitTakingTurn_UnitController.MyLookAtTargetVector - transform.position;
            float forwardForce = Vector3.Distance(RoundManager.Instance.unitTakingTurn_UnitController.MyLookAtTargetVector, transform.position);
            float verticalForce = forwardForce / 2;
            grenade.Throw(direction, forwardForce, verticalForce);
        }

        public virtual void SwitchToGrenade()
        {
            CurrentWeapon = grenadeData;
            UnitAction = Action.throwGrenade;
            grenade.ChangeGrenadeRenderers(true);
            GetCurrentUnitAnimator().SetBool("AimGrenade", true);
            RoundManager.Instance.unitTakingTurn_UnitController.UnitAnimation.SetRigsForRunning();
        }
        public void SwitchToRifle()
        {
            CurrentWeapon = rifleData;
            UnitAction = Action.shoot;
            GetCurrentUnitAnimator().SetBool("AimGrenade", false);
            RoundManager.Instance.unitTakingTurn_UnitController.UnitAnimation.SetRigsForAiming();
        }
        public IEnumerator UnlockInput(float delay)
        {
            yield return new WaitForSecondsRealtime(delay);
            RoundManager.Instance.unitTakingTurn_UnitController.UnitActions.PerformingAction = false;
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
