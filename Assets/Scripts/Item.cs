using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public float power = 10f;
    public float minVelocity = -20f;
    public float maxVelocity = 20f;

    public Rigidbody2D rb2d;
    private float throwPower;

    CharacterController2D controller;

    private void Awake()
    {
        controller = GameObject.FindWithTag("Squire").GetComponent<CharacterController2D>();
        rb2d = GetComponent<Rigidbody2D>();
        gameObject.layer = 12;
        gameObject.SetActive(true);
        Debug.Log("Awake");

        Debug.Log("Active? " + gameObject.activeInHierarchy);
    }

    public void Dropped()
    {
        Debug.Log("Dropped");

        Debug.Log("Active? " + gameObject.activeInHierarchy);

        Vector3 velocity = new Vector3(Random.Range(minVelocity, maxVelocity), 20f, 0.0f);
        float torque = Random.Range(minVelocity, maxVelocity);

        rb2d.AddTorque(torque);
        rb2d.AddForce(velocity * power);

    }

    public void Thrown()
    {
        gameObject.layer = 13;
        StartCoroutine(Cooldown());

        if (controller.m_FacingRight)
        {
            throwPower = 20f;
        } else
        {
            throwPower = -20f;
        }

        Vector3 velocity = new Vector3(throwPower, 20f, 0.0f);
        float torque = Random.Range(minVelocity, maxVelocity);

        rb2d.AddTorque(torque);
        rb2d.AddForce(velocity * power);

        StartCoroutine(Cooldown());

    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(1);
        gameObject.layer = 12;
    }

}
