using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

namespace PG
{
    public class UnitAnimation : MonoBehaviour
    {
        public Animator animator;
        private NavMeshAgent agent;
        [SerializeField] private MultiAimConstraint[] multiAimConstraints;
        [SerializeField] private MultiAimConstraint headAim, bodyAim, aimConstraint;
        [SerializeField] private TwoBoneIKConstraint handAim;
        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();
        }

        // Update is called once per frame
        void Update()
        {
            animator.SetFloat("MoveSpeed", agent.speed);
        }

        public void ChangeMultiAimConstraintWeight(MultiAimConstraint multiAimConstraint, float newVal)
        {
            multiAimConstraint.weight = newVal;
        }

        public void ChangeListOfMultiAimConstraints(float newVal)
        {
            foreach (MultiAimConstraint mac in multiAimConstraints)
            {
                ChangeMultiAimConstraintWeight(mac, newVal);
            }
        }

        public void SetRigsForRunning()
        {
            ChangeMultiAimConstraintWeight(bodyAim, 0);
            ChangeMultiAimConstraintWeight(headAim, 0);
            ChangeMultiAimConstraintWeight(aimConstraint, 0.4f);
            handAim.weight = 1;
        }

        public void SetRigsForAiming()
        {
            ChangeMultiAimConstraintWeight(bodyAim, 0.65f);
            ChangeMultiAimConstraintWeight(headAim, 1);
            ChangeMultiAimConstraintWeight(aimConstraint, 1);
            handAim.weight = 1;
        }
        public void SetRigsForAimingGrenade()
        {
            ChangeMultiAimConstraintWeight(bodyAim, 0.65f);
            ChangeMultiAimConstraintWeight(headAim, 1);
            ChangeMultiAimConstraintWeight(aimConstraint, 1);
            handAim.weight = 0;
        }
    }
}
