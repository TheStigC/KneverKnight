using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squire : MonoBehaviour
{
    public CharacterController2D controller;

    public float runSpeed = 40f;
    public bool isCarrying = false;
    public int currentHealth = 100;
    public Transform sword;
    public Transform shield;

    private float horizontalMove = 0f;
    private bool jump = false;
    private bool isDead = false;
    Item carriedItem;

    private void Awake()
    {
        sword = transform.Find("Sword");
        shield = transform.Find("Shield");
        sword.gameObject.SetActive(false);
        shield.gameObject.SetActive(false);
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
            if(sword.gameObject.activeSelf)
            {
                sword.gameObject.SetActive(false);
            } else if (shield.gameObject.activeSelf)
            {
                shield.gameObject.SetActive(false);
            }

            isCarrying = false;
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

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0 && !isDead)
        {
            Death();
        }
    }

    private void Death()
    {

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
            else if (collision.gameObject.tag == "Shield")
            {
                print("picked up shield");
                shield.gameObject.SetActive(true);
            }
        }
    }
}
