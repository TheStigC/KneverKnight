using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squire : MonoBehaviour
{
    public CharacterController2D controller;

    public float runSpeed = 40f;

    float horizontalMove = 0f;
    bool jump = false;

    private void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        if(Input.GetButtonDown("Jump"))
        {
            jump = true;
        }
    }
    private void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.deltaTime, false, jump);
        jump = false;
    }
}
