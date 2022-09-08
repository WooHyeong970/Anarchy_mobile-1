using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Cloud : MonoBehaviour
{
    Animator animator;
    CentralProcessor CP;

    private void Start()
    {
        CP = CentralProcessor.Instance;
        animator = this.gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Idle"))
        {
            if (CP.GetPlayer().GetLayer() == 7)
            {
                CP.UI.SetIdleState();
                CP.StartTimer();
            }
            else
            {
                CP.UI.SetNextState();
                CP.StopTimer();
            }
            this.gameObject.SetActive(false);
        }
    }

    
}