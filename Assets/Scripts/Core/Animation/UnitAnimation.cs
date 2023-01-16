using UnityEngine;

namespace RTSEngine
{
    [RequireComponent(typeof(UnitView))]
    public class UnitAnimation : MonoBehaviour, IAnimated
    {
        private Animator animator;
        private NavMovementController navMovemnent;
        private UnitView unit;

        private void Start()
        {
            unit = GetComponent<UnitView>();
            animator = unit.model.GetComponentInChildren<Animator>();
            navMovemnent = GetComponent<NavMovementController>();
        }

        public void PlayAnimation(AnimType animType)
        {
            string playTrigger = animType.ToString();

            animator.SetTrigger(playTrigger);
        }

        public void Update()
        {
            if (!animator || !navMovemnent)
                return;

            animator.SetFloat("Speed", navMovemnent.Agent.desiredVelocity.magnitude);
        }
    }

}