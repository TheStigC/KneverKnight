using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool isAttacking;
    public bool isMoving, isAnimatingMovement;
    public bool isDead;
    public List<Transform> targets;
    public int currentHealth = 10;
    public int attackDamage = 10;
    public float timeBetweenAttacks = 0.5f;
    public float speed = 1;
    public float seeDistance = 5f;
    public float push = 10f;
    public string objectTag1 = "Knight";
    public string objectTag2 = "Squire";
    public string currentTarget;

    private List<GameObject> go;
    private Transform selectedObject;
    private Transform myTransform;
    private float step;
    private float timer;
    private Collider2D col;
    private Rigidbody2D rb2d;
    private bool m_FacingRight = false;
    private Animator myAnim;
    public Knight knight;
    public Squire squire;


    private void Awake()
    {
        myAnim = GetComponentInChildren<Animator>();
        knight = GameObject.FindWithTag(objectTag1).GetComponent<Knight>();
        squire = GameObject.FindWithTag(objectTag2).GetComponent<Squire>();
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
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        SortTargetsByDistance();

        if (isMoving && !isDead)
        {
            if (!isAnimatingMovement)
            {
                isAnimatingMovement = true;
                myAnim.SetTrigger("StartWalking");
            }


            transform.position = Vector3.MoveTowards(transform.position, targets[0].position, step);

            if (transform.position.x < targets[0].position.x && m_FacingRight)
            {
                Flip();
            }
            else if (transform.position.x > targets[0].position.x && !m_FacingRight)
            {
                Flip();
            }
        }
        else
        {
            isAnimatingMovement = false;
        }


        if (isAttacking && timer >= timeBetweenAttacks)
        {
            Attack();
        }

    }

    public void Attack()
    {
        timer = 0f;

        myAnim.SetTrigger("StartAttacking");
        /*
        if (currentTarget == objectTag1)
        {
            knight.TakeDamage(attackDamage);
        }
        else if (currentTarget == objectTag2)
        {
            squire.TakeDamage(attackDamage * 2);
        }
        */
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        Vector3 velocity = new Vector3(push, 10f, 0f);
        rb2d.AddForce(velocity * 25);

        if (currentHealth <= 0 && !isDead)
        {
            Death();
        }
    }

    public void Death()
    {
        isAttacking = false;
        isDead = true;
        transform.gameObject.tag = "Dead";
        col.enabled = false;
        myAnim.SetTrigger("StartDying");
        Destroy(gameObject.GetComponent<Rigidbody2D>());
        knight.AddAllObjects();
        knight.SortTargetsByDistance();
    }

    private void Flip()
    {
        m_FacingRight = !m_FacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
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
        targets.Sort(delegate (Transform t1, Transform t2)
        {
            return Vector3.Distance(t1.position, myTransform.position).CompareTo(Vector3.Distance(t2.position, myTransform.position));
        });

        if (Vector3.Distance(targets[0].position, transform.position) > seeDistance)
        {
            isMoving = false;
        }
        else if (!isAttacking)
        {
            isMoving = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == objectTag1)
        {
            isMoving = false;
            isAttacking = true;
            print("knight hit");
            currentTarget = objectTag1;

        }
        else if (collision.gameObject.tag == objectTag2)
        {
            isMoving = false;
            isAttacking = true;
            print("squire hit");
            currentTarget = objectTag2;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!isDead)
        {
            isAttacking = false;
            isMoving = true;
        }
    }

}
