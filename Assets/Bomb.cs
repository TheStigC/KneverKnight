using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float power = 10f;
    public float minVelocity = -20f;
    public float maxVelocity = 20f;
    public float expRadius = 10f;
    public float expForce = 100f;
    public int damage = 10;
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

        StartCoroutine(BombTimer());
    }

    IEnumerator BombTimer ()
    {
        yield return new WaitForSeconds(5);

        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, expRadius, 1 << LayerMask.NameToLayer("Enemy"));

        foreach (Collider2D en in enemies)
        {
            Rigidbody2D rb = en.GetComponent<Rigidbody2D>();
            if(rb != null && rb.tag == "Enemy")
            {
                Enemy enemy = rb.GetComponent<Enemy>();
                if(enemy)
                enemy.TakeDamage(damage);

                Vector3 deltaPos = rb.transform.position - transform.position;

                Vector3 force = deltaPos.normalized * expForce;
                rb.AddForce(force);
            }
        }
        gameObject.SetActive(false);
        throwReady = true;
    }
}
