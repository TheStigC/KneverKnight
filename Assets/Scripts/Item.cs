using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public float power = 10f;
    public float minVelocity = 10f;
    public float maxVelocity = 20f;

    private Rigidbody2D rb2d;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();

        Vector3 velocity = new Vector3(Random.Range(minVelocity, maxVelocity), 10f, 0.0f);
        rb2d.AddForce(velocity * power);
    }
}
