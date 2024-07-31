using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private float playerSpeed;
    public float playerHP;
    public float playerCoins;
    [SerializeField] private float playerShootInterval;
    [SerializeField] private float playerDamagePerShoot;
    [SerializeField] private float playerArrowSpeed;
    [SerializeField] private InputActionReference movePlayer;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private List<GameObject> allEnemies;
    [SerializeField] private List<GameObject> visibleEnemies;
    [SerializeField] private GameObject mainAim;
    private float timer;
    public Vector2 joystickDirection;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //destroy player if HP < 0
        if (playerHP <= 0)
        {
            Destroy(gameObject);
        }

        //Player controller

        joystickDirection = movePlayer.action.ReadValue<Vector2>();
        Vector3 moveDirection = new Vector3(joystickDirection.x, 0, joystickDirection.y);

        transform.Translate(moveDirection * playerSpeed * Time.deltaTime, Space.World);

        //set player orientation
        if(joystickDirection != new Vector2(0, 0))
        {
            transform.rotation = Quaternion.LookRotation(moveDirection);
        }
        else if (mainAim != null)
        {
            LookAtEnemy();
            Shoot();
        }

        //find all enemies and check visibility, select mainAim
        allEnemies.Clear();
        FindEnemies();
        visibleEnemies.Clear();
        mainAim = null;
        FindAim();        

    }

    //set player orientation on enemy if visible 
    private void LookAtEnemy()
    {
        transform.rotation = Quaternion.LookRotation(mainAim.transform.position - transform.position);
        transform.hasChanged = false;
    }

    //shoot arrow with definite parameters
    private void Shoot()
    {        
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            GameObject newArrow = Instantiate(arrowPrefab, transform.position + new Vector3(0, 2.5f,0), transform.rotation);
            newArrow.GetComponent<ArrowController>().destinationPoint = mainAim.transform.position  + new Vector3(0, 2.5f,0);
            newArrow.GetComponent<ArrowController>().arrowDamage = playerDamagePerShoot;
            newArrow.GetComponent<ArrowController>().arrowSpeed = playerArrowSpeed;
            newArrow.GetComponent<ArrowController>().aimTrigger = "Enemy";
            timer = playerShootInterval;
        }
    }


    private void FindEnemies()
    {
        allEnemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
    }

    //check if enemy visible
    private void FindAim()
    {
        foreach (GameObject enemy in allEnemies)
        {
            Ray ray = new Ray(transform.position, enemy.transform.position - transform.position);
            Debug.DrawLine(transform.position, enemy.transform.position, Color.red);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f))
            {
                //Debug.LogError(hit.collider.gameObject.name);
                if (hit.collider.gameObject.tag == "Enemy")
                {
                    visibleEnemies.Add(hit.collider.gameObject);
                }
            }
        }

        //choose closest enemy
        if(visibleEnemies.Count > 0)
        {
            mainAim = visibleEnemies[0];
            foreach (GameObject enemy in visibleEnemies)
            {
                if(Vector3.Distance(enemy.transform.position, transform.position) < Vector3.Distance(mainAim.transform.position, transform.position))
                {
                    mainAim = enemy;
                }
            }
        }
    }

    //check collision with triggers 
    private void OnTriggerEnter(Collider col)
    {
        if (col.transform.root.gameObject.tag == "Arrow" && col.transform.root.gameObject.GetComponent<ArrowController>().aimTrigger == "Player")
        {
            playerHP -= col.transform.gameObject.GetComponent<ArrowController>().arrowDamage;
        }
        if (col.transform.root.gameObject.tag == "Enemy")
        {
            playerHP --;
        }
    }

}
