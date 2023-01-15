using RTSEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UnitAnimation : MonoBehaviour, IAnimated
{
    [SerializeField]
    private Animator animator;

    private NavMovementController navMovemnent;
    private void Awake()
    {
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

        animator.SetFloat("Speed", navMovemnent._navAgent.desiredVelocity.magnitude);
    }
}
