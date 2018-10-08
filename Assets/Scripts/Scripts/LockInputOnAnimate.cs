using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockInputOnAnimate : MonoBehaviour
{

    Animator m_Animator;

    void Start()
    {
        m_Animator = gameObject.GetComponent<Animator>();
    }

    // Use this for initialization
    void OnStateEnter()
    {
        m_Animator.SetBool("DisableTransitions", true);

    }
}
