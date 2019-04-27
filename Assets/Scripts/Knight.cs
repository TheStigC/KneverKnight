using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : MonoBehaviour
{

    public bool isAttacking;
    public bool isMoving, isAnimatingMovement;
    public bool isDead;
    public List<Transform> targets;
    public int currentHealth = 100;
    public int attackDamage = 5;
    public int shieldPower = 5;
    public float push = 10f;
    public float cooldownTimer = 2;
    public float timeBetweenAttacks = 0.5f;
    public float speed = 1;
    public string objectTag1 = "Goal";
    public string objectTag2 = "Enemy";
    public GameObject droppedSword;
    public GameObject droppedShield;
    public Transform sword;
    public Transform shield;
    public Enemy enemyScript;
    public List<GameObject> go;

    private Animator myAnim;
    private Transform selectedObject;
    private Transform myTransform;
    private Transform pickUpTrigger;
    private Collider2D col;
    private Rigidbody2D rb2d;
    private float step;
    private float timer;
    LevelManager levelManager;

    Item swordScript;
    Item shieldScript;

    private void Awake()
    {
        levelManager = GameObject.FindWithTag("GameController").GetComponent<LevelManager>();
        sword = transform.Find("Graphics").transform.Find("Knight_Body").transform.Find("Knight_ArmRight").transform.Find("Sword");
        sword.gameObject.SetActive(true);
        shield = transform.Find("Graphics").transform.Find("Knight_Body").transform.Find("Knight_ArmLeft").transform.Find("Shield");
        shield.gameObject.SetActive(true);
        pickUpTrigger = transform.Find("PickupTrigger");
        swordScript = droppedSword.GetComponent<Item>();
        shieldScript = droppedShield.GetComponent<Item>();
        myAnim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        targets = new List<Transform>();
        go = new List<GameObject>();
        myTransform = transform;
        AddAllObjects();
        SortTargetsByDistance();
        step = speed * Time.deltaTime;
        col = GetComponent<Collider2D>();
        rb2d = GetComponent<Rigidbody2D>();
        isAttacking = false;
        isDead = false;
        isMoving = true;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (isMoving && !isAttacking)
        {
            if (!isAnimatingMovement)
            {
                isAnimatingMovement = true;
                myAnim.SetTrigger("StartWalking");
            }

            transform.position = Vector3.MoveTowards(transform.position, targets[0].position, step);
        }
        else
        {
            isAnimatingMovement = false;
        }

        if (isAttacking && timer >= timeBetweenAttacks)
        {
            Attack();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            swordScript.Dropped();
        }

    }

    public void Attack()
    {
        timer = 0f;
        myAnim.SetTrigger("StartAttack");
        /*
        enemyScript.TakeDamage(attackDamage);

        if (enemyScript.currentHealth <= 0)
        {
            go.RemoveAt(0);
            isMoving = true;
            isAttacking = false;
        }
        */

    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount - shieldPower;

        Vector3 velocity = new Vector3(-push, 10f, 0f);
        rb2d.AddForce(velocity*25);

        float randValue = Random.value;

        if (randValue < .8f)
        {

        }
        else if (randValue < .9f && sword.gameObject.activeSelf)
        {
            //drop sword
            sword.gameObject.SetActive(false);
            pickUpTrigger.gameObject.SetActive(false);
            droppedSword.transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
            droppedSword.SetActive(true);
            swordScript.Dropped();
            attackDamage = 1;
            StartCoroutine(Cooldown());
        }
        else if (shield.gameObject.activeSelf)
        {
            //drop shield
            shield.gameObject.SetActive(false);
            pickUpTrigger.gameObject.SetActive(false);
            droppedShield.transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
            droppedShield.SetActive(true);
            shieldScript.Dropped();
            shieldPower = 0;
            StartCoroutine(Cooldown());

        }

        if (currentHealth <= 0 && !isDead)
        {
            Death();
        }
    }

    public void Death()
    {
        isMoving = false;
        isAttacking = false;
        isDead = true;
        transform.gameObject.tag = "Dead";
        col.enabled = false;
        myAnim.SetTrigger("StartDying");
        Destroy(gameObject.GetComponent<Rigidbody2D>());
        StartCoroutine(WaitForDeathScene());
    }

    IEnumerator WaitForDeathScene()
    {
        yield return new WaitForSeconds(5);
        levelManager.LoadLevel("GameOver");
    }

    IEnumerator WaitForWinScene()
    {
        yield return new WaitForSeconds(5);
        levelManager.LoadNextLevel();
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cooldownTimer);
        pickUpTrigger.gameObject.SetActive(true);
    }

    public void AddAllObjects()
    {
        go.Clear();
        targets.Clear();

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

    public void SortTargetsByDistance()
    {
        targets.Sort(delegate (Transform t1, Transform t2)
        {
            return Vector3.Distance(t1.position, myTransform.position).CompareTo(Vector3.Distance(t2.position, myTransform.position));
        });
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == objectTag2)
        {
            isMoving = false;
            isAttacking = true;
            print("enemy hit");
            enemyScript = collision.gameObject.GetComponent<Enemy>();
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == objectTag1)
        {
            print("goal hit");
            //play fanfare
            isMoving = false;
            StartCoroutine(WaitForWinScene());
        }
    }


}
