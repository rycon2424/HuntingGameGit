using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class IKHead : MonoBehaviour
{
    protected Animator animator;

    public Transform lookObj = null;
    public FPSPlayer fps;
    [Header("Intensity")]
    [Range(0, 1f)] public float intensity = 1;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    //a callback for calculating IK
    void OnAnimatorIK()
    {
        if (fps.sprinting)
        {
            animator.SetLookAtWeight(0);
        }
        else
        {
            animator.SetLookAtWeight(intensity);
        }
        animator.SetLookAtPosition(lookObj.position);
    }
}