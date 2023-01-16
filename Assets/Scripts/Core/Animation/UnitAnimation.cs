using RTSEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UnitAnimation : MonoBehaviour, IAnimated
{
    private Animator animator;
    private NavMovementController navMovemnent;
    private Transform model;

    private void Awake()
    {
        model = transform.GetChild(0);
        animator = model.GetComponentInChildren<Animator>();
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
