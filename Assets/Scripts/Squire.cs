using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squire : MonoBehaviour
{
    public CharacterController2D controller;

    public float runSpeed = 40f;
    public bool isCarrying = false;
    public Transform sword;

    private float horizontalMove = 0f;
    private bool jump = false;
    Item carriedItem;

    private void Awake()
    {
        sword = transform.Find("Sword");
        sword.gameObject.SetActive(false);
    }

    private void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        if(Input.GetButtonDown("Jump"))
        {
            jump = true;
        }

        if(Input.GetKeyDown(KeyCode.E) && isCarrying)
        {
            isCarrying = false;
            sword.gameObject.SetActive(false);
            carriedItem.transform.position = transform.position;
            carriedItem.gameObject.SetActive(true);
            carriedItem.Thrown();
        }
    }
    private void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.deltaTime, false, jump);
        jump = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 12 && !isCarrying)
        {
            carriedItem = collision.gameObject.GetComponent<Item>();
            collision.gameObject.SetActive(false);
            isCarrying = true;

            if (collision.gameObject.tag == "Sword")
            {
                print("picked up sword");
                sword.gameObject.SetActive(true);
            }
        }
    }
}
