using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightAnimTriggers : MonoBehaviour
{

    private Knight knightScript;

    void Start()
    {
        knightScript = GetComponentInParent<Knight>();
    }


    public void DealDamage()
    {
        knightScript.enemyScript.TakeDamage(knightScript.attackDamage);

        if (knightScript.enemyScript.currentHealth <= 0)
        {
            knightScript.go.RemoveAt(0);
            knightScript.isMoving = true;
            knightScript.isAttacking = false;
        }
    }

}
