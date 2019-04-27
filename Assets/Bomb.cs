using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float power = 10f;
    public float minVelocity = -20f;
    public float maxVelocity = 20f;
    public bool throwReady = true;

    public Rigidbody2D rb2d;
    private float throwPower;

    CharacterController2D controller;

    private void Awake()
    {
        controller = GameObject.FindWithTag("Squire").GetComponent<CharacterController2D>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    public void Thrown()
    {
        throwReady = false;

        if (controller.m_FacingRight)
        {
            throwPower = 50f;
        }
        else
        {
            throwPower = -50f;
        }

        Vector3 velocity = new Vector3(throwPower, 20f, 0.0f);
        float torque = Random.Range(minVelocity, maxVelocity);

        rb2d.AddTorque(torque);
        rb2d.AddForce(velocity * power);
    }
}
