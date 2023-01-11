using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class charAnimations : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private float velocity;
    private bool grounded;


    public bool Grounded
    {
        set => grounded = value;
    }
    public float Velocity
    {
        set => velocity = value;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (grounded && velocity > 0)
            animator.Play("Run");
        else if (grounded && velocity  <=  0.8)
            animator.Play("Idle");
        else if (!grounded)
            animator.Play("CheckJump");
    }
}
