using UnityEngine;

namespace PG
{
    public class UnitActions : MonoBehaviour, IInteractable
    {
        public bool usedAction = false;
        public action unitAction;
        public enum action { shoot, throwGrenade };
        public Transform gunBarrel;
        [SerializeField] private ParticleSystem gunSmoke;
        [SerializeField] private ParticleSystem bulletImpactFX;
        public GameObject myDamageableTarget;
        private Animator anim;

        private void Start()
        {
            anim = GetComponent<Animator>();
            gunSmoke.Stop();
            bulletImpactFX.Stop();
        }
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
            GunShotRenderer.Instance.ClearGunshot();
            myDamageableTarget.GetComponent<IDamageable>().TakeDamage(RoundManager.Instance.unitTakingTurn.unitData.damage);

            bulletImpactFX.transform.position = GunShotRenderer.Instance.hitPosition;

            Vector3 direction = myDamageableTarget.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction, transform.up);
            bulletImpactFX.transform.rotation = Quaternion.Euler(rotation.eulerAngles.x, rotation.eulerAngles.y - 90, rotation.eulerAngles.z);

            bulletImpactFX.Play();

            anim.SetTrigger("Shoot");
        }
        public void ThrowGrenade()
        {

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
            PerformAction(unitAction);
        }
    }
}
