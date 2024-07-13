using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceptionistController : MonoBehaviour
{
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Talk()
    {
        animator.SetTrigger("Talk");
    }
}
