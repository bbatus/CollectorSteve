using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    private Animator anim;
    void Start()
    {
        
    }
    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }
    void Update()   
    {
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
        PlayerAnimController();
        }
        else 
        {
            PlayerAnimUp();
        }
#else 
if (Mathf.Abs(horizontalInput) > 0.1f || Mathf.Abs(verticalInput) > 0.1f)
    {
        PlayerAnimController();
    }
    else
    {
        PlayerAnimUp();
    }

#endif
    }

    private void PlayerAnimController() {
        if (anim != null && UIManager.instance.levelState == LevelState.Playing) {
        anim.SetBool("isTouched", true);
        }
    }
       private void PlayerAnimUp() {
        anim.SetBool("isTouched", false);
    }

}
