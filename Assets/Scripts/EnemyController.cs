using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public float enemyHP;
    [SerializeField] private float enemyShootInterval;
    [SerializeField] private float enemyDamagePerShoot;
    [SerializeField] private float enemyArrowSpeed;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private GameObject coinPrefab;
    private float timer;
    [SerializeField] private GameObject player;
    [SerializeField] private bool seePlayer;


    [SerializeField] private NavMeshAgent agent;
    private bool enemyDeath = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //destroy when enemy HP <0
        if (enemyHP <= 0)
        {
            enemyDeath = true;
            Destroy(gameObject);
        }

        player = GameObject.FindGameObjectWithTag("Player");

        //check behaviour

        if (!seePlayer)
        {
            agent.SetDestination(player.transform.position);
        }
        if(seePlayer)
        {
            agent.SetDestination(transform.position);
            transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position);
            playerShoot();
        }

        //generate coin when dead

        if(enemyDeath)
        {
            GameObject newCoin= Instantiate(coinPrefab, transform.position + new Vector3(0, 2.5f, 0), transform.rotation);
            enemyDeath = false;
        }

        FindPlayer();
    }

    //check is player vivble

    private void FindPlayer()
    {
        Ray ray = new Ray(transform.position, player.transform.position - transform.position);       
        Debug.DrawLine(transform.position, player.transform.position, Color.green);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f))
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                seePlayer = true;
            }
            else
            {
                seePlayer = false;
            }
        }
    }

    //shoot arrow with definite parameters

    private void playerShoot()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            GameObject newArrow = Instantiate(arrowPrefab, transform.position + new Vector3(0, 2.5f, 0), transform.rotation);
            newArrow.GetComponent<ArrowController>().destinationPoint = player.transform.position + new Vector3(0, 2.5f, 0);
            newArrow.GetComponent<ArrowController>().arrowDamage = enemyDamagePerShoot;
            newArrow.GetComponent<ArrowController>().arrowSpeed = enemyArrowSpeed;
            newArrow.GetComponent<ArrowController>().aimTrigger = "Player";
            timer = enemyShootInterval;
        }
    }


    //check collision with triggers 
    private void OnTriggerEnter(Collider col)
    {
        if(col.transform.root.gameObject.tag == "Arrow" && col.transform.root.gameObject.GetComponent<ArrowController>().aimTrigger == "Enemy")
        {
            enemyHP -= col.transform.gameObject.GetComponent<ArrowController>().arrowDamage;
        }
    }

}
