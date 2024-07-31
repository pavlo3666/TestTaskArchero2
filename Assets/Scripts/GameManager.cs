using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> allEnemies;
    [SerializeField] private List<GameObject> allCoins;
    [SerializeField] private GameObject player;
    public TMP_Text stats;
    [SerializeField] private GameObject enemyPrefab;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Find all enemies, player and coins
        allEnemies.Clear();
        FindEnemies();
        allCoins.Clear();
        FindCoins();
        player = GameObject.FindGameObjectWithTag("Player");

        //Spawn enemies if less then 2
        if (allEnemies.Count < 2)
        {
            GameObject newEnemy = Instantiate(enemyPrefab, new Vector3(Random.Range(-10, 10), 0, Random.Range(0, 12)), transform.rotation);
        }

        //Stats on screen
        stats.text = "Player HP = " + player.GetComponent<PlayerController>().playerHP + "<br>" +
                     "Player Coins = " + player.GetComponent<PlayerController>().playerCoins + "<br>";
        foreach(GameObject enemy in allEnemies)
        {
            stats.text += enemy.name + " HP = " + enemy.GetComponent<EnemyController>().enemyHP + "<br>";
        }
        foreach (GameObject coin in allCoins)
        {
            coin.transform.position = Vector3.MoveTowards(coin.transform.position, player.transform.position, 5 * Time.deltaTime);
            if(Vector3.Distance(coin.transform.position, player.transform.position) < 1)
            {
                player.GetComponent<PlayerController>().playerCoins++;
                Destroy(coin);
            }
        }


    }

    private void FindEnemies()
    {
        allEnemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
    }
    private void FindCoins()
    {
        allCoins.AddRange(GameObject.FindGameObjectsWithTag("Coin"));
    }
    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
