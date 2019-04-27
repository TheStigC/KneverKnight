using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{

    public GameObject sword;
    public GameObject shield;
    Knight knight;
    Squire squire;

    private void Awake()
    {
        knight = GameObject.FindWithTag("Knight").GetComponent<Knight>();
        squire = GameObject.FindWithTag("Squire").GetComponent<Squire>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Sword")
        {
            knight.sword.gameObject.SetActive(true);
            knight.attackDamage = 5;
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag =="Shield")
        {
            knight.shield.gameObject.SetActive(true);
            knight.shieldPower = 5;
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Squire" && squire.isCarrying)
        {
            if (squire.sword.gameObject.activeSelf)
            {
                knight.sword.gameObject.SetActive(true);
                knight.attackDamage = 5;
                squire.sword.gameObject.SetActive(false);
            }
            else if (squire.shield.gameObject.activeSelf)
            {
                knight.shield.gameObject.SetActive(true);
                knight.shieldPower = 5;
                squire.shield.gameObject.SetActive(false);
            }
        }
    }

}
