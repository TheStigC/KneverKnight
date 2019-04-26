using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool isAttacking;
    public bool isMoving;
    public bool isDead;
    public List<Transform> targets;
    public int currentHealth = 10;
    public float speed = 1;
    public string objectTag1 = "Knight";
    public string objectTag2 = "Squire";
    private List<GameObject> go;
    private Transform selectedObject;
    private Transform myTransform;
    private float step;
    private Collider2D col;
    Knight knight;

    private void Awake()
    {
        knight = GameObject.FindWithTag("Knight").GetComponent<Knight>(); 
    }

    private void Start()
    {
        targets = new List<Transform>();
        go = new List<GameObject>();
        myTransform = transform;
        AddAllObjects();
        step = speed * Time.deltaTime;
        isAttacking = false;
        isDead = false;
        isMoving = true;
        col = GetComponent<Collider2D>();
    }

    private void Update()
    {
        SortTargetsByDistance();

        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targets[0].position, step);
        }

    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0 && !isDead)
        {
            Death();
        }
    }

    public void Death()
    {
        transform.gameObject.tag = "Dead";
        col.enabled = false;
        Destroy(gameObject.GetComponent<Rigidbody2D>());
        knight.AddAllObjects();
        knight.SortTargetsByDistance();
    }

    public void AddAllObjects()
    {
        GameObject[] enemyArray = GameObject.FindGameObjectsWithTag(objectTag2);
        GameObject goal = GameObject.FindGameObjectWithTag(objectTag1);
        go.Add(goal);

        for (int i = 0; i < enemyArray.Length; i++)
        {
            go.Add(enemyArray[i]);
        }

        foreach (GameObject enemies in go)
        {
            AddTarget(enemies.transform);
        }
    }

    public void AddTarget(Transform goals)
    {
        targets.Add(goals);
    }

    private void SortTargetsByDistance()
    {
        targets.Sort(delegate (Transform t1, Transform t2) {
            return Vector3.Distance(t1.position, myTransform.position).CompareTo(Vector3.Distance(t2.position, myTransform.position));
        });
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == objectTag1)
        {
            isMoving = false;
            isAttacking = true;
            print("knight hit");
        } else if (collision.gameObject.tag == objectTag2)
        {
            isMoving = false;
            isAttacking = true;
            print("squire hit");
        }
    }

}
