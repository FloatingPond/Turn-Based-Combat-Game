using UnityEngine;

namespace PG
{
    public class UnitActions : MonoBehaviour, IInteractable
    {
        public Transform gunBarrel;
        [SerializeField] private ParticleSystem gunSmoke;
        [SerializeField] private ParticleSystem bulletImpactFX;
        public GameObject myDamageableTarget;
        private Animator anim;

        private void Start()
        {
            anim = RoundManager.Instance.unitTakingTurn_UnitController.unitAnimation.animator;
            gunSmoke.Stop();
            bulletImpactFX.Stop();
        }
        public bool usedAction = false;
        public action unitAction;
        public enum action { shoot, throwGrenade };
        public void PerformAction(action _action)
        {
            if (!usedAction)
            {
                switch (_action)
                {
                    case action.shoot:
                        Shoot();
                        break;
                    case action.throwGrenade:
                        ThrowGrenade();
                        break;
                    default:
                        break;
                }
                usedAction = true;
            }
        }

        public void Shoot()
        {
            gunSmoke.Play();
            WeaponTrajectoryIndicator.Instance.ClearRendererPositions();
            myDamageableTarget.GetComponent<IDamageable>().TakeDamage(RoundManager.Instance.unitTakingTurn_UnitController.unitData.damage);
            bulletImpactFX.transform.position = WeaponTrajectoryIndicator.Instance.hitPosition;
            #region Rotate bullet impact fx (particle system) to face the player
            Vector3 direction = myDamageableTarget.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction, transform.up);
            bulletImpactFX.transform.rotation = Quaternion.Euler(rotation.eulerAngles.x, rotation.eulerAngles.y - 90, rotation.eulerAngles.z);
            #endregion
            bulletImpactFX.Play();

            anim.SetTrigger("Shoot");
        }
        public void ThrowGrenade()
        {
            anim.SetTrigger("ThrowGrenade");
            anim.SetBool("AimGrenade", false);
        }

        public void SwitchToGrenade()
        {
            unitAction = action.throwGrenade;
            anim.SetBool("AimGrenade", true);
            RoundManager.Instance.unitTakingTurn_UnitController.unitAnimation.SetRigsForRunning();
        }
        public void SwitchToRifle()
        {
            unitAction = action.shoot;
            anim.SetBool("AimGrenade", false);
            RoundManager.Instance.unitTakingTurn_UnitController.unitAnimation.SetRigsForAiming();
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
