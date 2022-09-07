using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Cloud : MonoBehaviour
{
    Animator animator;

    private void Start()
    {
        animator = this.gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Idle"))
        {
            if (CentralProcessor.Instance.GetPlayer().GetLayer() == 7)
            {
                CentralProcessor.Instance.UI.SetIdleState();
                CentralProcessor.Instance.StartTimer();
            }
            else
            {
                CentralProcessor.Instance.UI.SetNextState();
                CentralProcessor.Instance.StopTimer();
            }
            this.gameObject.SetActive(false);
        }
    }

    
}