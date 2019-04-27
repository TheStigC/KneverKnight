using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squire : MonoBehaviour
{
    public CharacterController2D controller;

    public float runSpeed = 40f;
    public bool isCarrying = false;
    public int currentHealth = 100;
    public int bombs = 2;
    public Transform sword;
    public Transform shield;
    public GameObject bomb;

    private float horizontalMove = 0f;
    private bool jump = false;
    private bool isDead = false;
    private Transform top;
    Item carriedItem;
    Bomb bombScript;

    private void Awake()
    {
        sword = transform.Find("Sword");
        shield = transform.Find("Shield");
        top = transform.Find("Ceilingcheck");
        bombScript = bomb.GetComponent<Bomb>();
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
            carriedItem.transform.position = top.transform.position;
            carriedItem.gameObject.SetActive(true);
            carriedItem.Thrown();
        }

        if(Input.GetKeyDown(KeyCode.F) && bombs > 0)
        {
            bombs -= 1;
            bomb.transform.position = top.transform.position;
            bomb.gameObject.SetActive(true);
            bombScript.Thrown();
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

        if (isCarrying)
        {
            if (sword.gameObject.activeSelf)
            {
                sword.gameObject.SetActive(false);
            } else if (shield.gameObject.activeSelf)
            {
                shield.gameObject.SetActive(false);
            }

            isCarrying = false;
            carriedItem.transform.position = top.transform.position;
            carriedItem.gameObject.layer = 13;
            carriedItem.gameObject.SetActive(true);
            carriedItem.Dropped();
        }

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
