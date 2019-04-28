using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimTriggers : MonoBehaviour
{
    private Enemy enemyScript;

    private void Start()
    {
        enemyScript = GetComponentInParent<Enemy>();
    }

    public void DealDamage()
    {
        if (enemyScript.currentTarget == enemyScript.objectTag1)
        {
            enemyScript.knight.TakeDamage(enemyScript.attackDamage);
        }
        else if (enemyScript.currentTarget == enemyScript.objectTag2)
        {
            enemyScript.squire.TakeDamage(enemyScript.attackDamage * 2);
        }
    }

}
