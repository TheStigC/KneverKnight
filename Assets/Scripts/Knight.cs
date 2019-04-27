using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : MonoBehaviour
{

    public bool isAttacking;
    public bool isMoving;
    public bool isDead;
    public List<Transform> targets;
    public int currentHealth = 100;
    public int attackDamage = 5;
    public float cooldownTimer = 2;
    public float timeBetweenAttacks = 0.5f;
    public float speed = 1;
    public string objectTag1 = "Goal";
    public string objectTag2 = "Enemy";
    public GameObject droppedSword;
    public Transform sword;

    private List<GameObject> go;
    private Transform selectedObject;
    private Transform myTransform;
    private Transform pickUpTrigger;
    private float step;
    private float timer;
    LevelManager levelManager;
    Enemy enemyScript;
    Item swordScript;

    private void Awake()
    {
        levelManager = gameObject.GetComponent<LevelManager>();
        sword = transform.Find("Sword");
        sword.gameObject.SetActive(true);
        pickUpTrigger = transform.Find("PickupTrigger");
        swordScript = droppedSword.GetComponent<Item>();
    }

    private void Start()
    {
        targets = new List<Transform>();
        go = new List<GameObject>();
        myTransform = transform;
        AddAllObjects();
        SortTargetsByDistance();
        step = speed * Time.deltaTime;
        isAttacking = false;
        isDead = false;
        isMoving = true;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targets[0].position, step);
        }

        if (isAttacking && timer >= timeBetweenAttacks)
        {
            Attack();
        }

    }

    public void Attack()
    {
        timer = 0f;

        enemyScript.TakeDamage(attackDamage);

        if(enemyScript.currentHealth <= 0)
        {
            go.RemoveAt(0);
            isMoving = true;
            isAttacking = false;
        }

    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        float randValue = Random.value;

        if (randValue < .8f)
        {

        }
        else if (sword.gameObject.activeSelf)
        {
            //drop sword
            sword.gameObject.SetActive(false);
            pickUpTrigger.gameObject.SetActive(false);
            StartCoroutine(Cooldown());
            Instantiate(droppedSword, transform.position, Quaternion.identity);
            swordScript.Dropped();
            attackDamage = 1;
        }

        if (currentHealth <= 0 && !isDead)
        {
            Death();
        }
    }

    public void Death()
    {

    }

    IEnumerator Cooldown ()
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
        
        for(int i = 0; i < enemyArray.Length; i++)
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
        targets.Sort(delegate (Transform t1, Transform t2) {
            return Vector3.Distance(t1.position, myTransform.position).CompareTo(Vector3.Distance(t2.position, myTransform.position));
        });
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == objectTag2)
        {
            isMoving = false;
            isAttacking = true;
            print("enemy hit");
            enemyScript = collision.gameObject.GetComponent<Enemy>();

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == objectTag1)
        {
            print("goal hit");
        }
    }

}
