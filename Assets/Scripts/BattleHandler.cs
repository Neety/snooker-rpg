using TMPro;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleHandler : MonoBehaviour
{
    private static BattleHandler inst;
    public static BattleHandler GetInstance()
    {
        return inst;
    }
    [SerializeField] private GameObject pfPlayer, pfEnemy, pfDamageText;

    private int numOfPlayers;
    private int activePlayerNum;
    private List<GameObject> players = new List<GameObject>();
    // private List<Health> playerHealth = new List<Health>();
    // private List<HealthBar> playerHealthBar = new List<HealthBar>();
    // private List<Transform> playerHealthBarTransform = new List<Transform>();
    // private List<GameObject> playerSelectTransform = new List<GameObject>();

    private int numOfEnemies;
    private int activeEnemyNum;
    private List<GameObject> enemies = new List<GameObject>();
    // private List<Health> enemyHealth = new List<Health>();
    // private List<HealthBar> enemyHealthBar = new List<HealthBar>();
    // private List<Transform> enemyHealthBarTransform = new List<Transform>();
    // private List<GameObject> enemySelectTransform = new List<GameObject>();
    private PlayerBattle playerBattle, activePlayer;
    private EnemyBattle enemyBattle, activeEnemy;
    int playerStartInitiative, enemyStartInitiative;

    private Active active;
    private enum Active
    {
        Player, Enemy
    }

    private void Awake()
    {
        inst = this;

        numOfPlayers = 3;
        numOfEnemies = 3;
    }
    private void Start()
    {
        for (int i = 0; i < numOfPlayers; i++)
        {
            SpawnCharacter(true, i);
        }

        for (int i = 0; i < numOfEnemies; i++)
        {
            SpawnCharacter(false, i);
        }

        players = players.OrderBy(p => p.GetComponent<PlayerBattle>().GetInitiative(true)).ToList();
        enemies = enemies.OrderBy(e => e.GetComponent<EnemyBattle>().GetInitiative(true)).ToList();

        SetActive<PlayerBattle>(players[0].GetComponent<PlayerBattle>());

        foreach (GameObject player in players)
        {
            player.GetComponent<PlayerBattle>().triggerNextTurn += NextActive;
        }

        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<EnemyBattle>().triggerNextTurn += NextActive;
        }

    }

    private void SpawnCharacter(bool isPlayerTeam, int i)
    {
        Vector3 position;

        if (isPlayerTeam)
        {
            position = new Vector3(UnityEngine.Random.Range(-12f, -6f), UnityEngine.Random.Range(-8f, 0), 0);
            players.Insert(i, Instantiate(pfPlayer, position, Quaternion.identity));
            players[i].GetComponent<PlayerBattle>().SetPlayerNum(i);
            players[i].GetComponentInChildren<HealthBar>().Setup(players[i].GetComponent<Health>());
            GenerateInitiative<PlayerBattle>(numOfPlayers, i);
            // playerHealth.Insert(i, new Health(100));
            // playerHealthBarTransform.Insert(i, Instantiate(pfHealthBar, players[i].position + new Vector3(0f, 1f, 0f), Quaternion.identity, players[i]));
            // playerHealthBar.Insert(i, playerHealthBarTransform[i].GetComponent<HealthBar>());
            // playerHealthBar[i].Setup(playerHealth[i]);
            // playerSelectTransform.Insert(i, Instantiate(playerSelect, players[i].position + new Vector3(0f, -1f, 0f), Quaternion.identity, players[i]));
            // playerSelectTransform[i].SetActive(false);
        }
        else
        {
            position = new Vector3(UnityEngine.Random.Range(6f, 12f), UnityEngine.Random.Range(0, 8f), 0);
            enemies.Insert(i, Instantiate(pfEnemy, position, Quaternion.identity));
            enemies[i].GetComponent<EnemyBattle>().SetEnemyNum(i);
            enemies[i].GetComponentInChildren<HealthBar>().Setup(enemies[i].GetComponent<Health>());
            GenerateInitiative<EnemyBattle>(numOfEnemies, i);
            // enemyHealth.Insert(i, new Health(100));
            // enemyHealthBarTransform.Insert(i, Instantiate(pfHealthBar, enemies[i].position + new Vector3(0f, 1f, 0f), Quaternion.identity, enemies[i]));
            // enemyHealthBar.Insert(i, enemyHealthBarTransform[i].GetComponent<HealthBar>());
            // enemyHealthBar[i].Setup(enemyHealth[i]);
            // enemySelectTransform.Insert(i, Instantiate(enemySelect, enemies[i].position + new Vector3(0f, -1f, 0f), Quaternion.identity, enemies[i]));
            // enemySelectTransform[i].SetActive(false);
        }
    }

    private void SetActive<T>(T activeEntity)
    {
        if (typeof(T) == typeof(PlayerBattle))
        {
            active = Active.Player;

            activePlayer = (PlayerBattle)(object)activeEntity;
            Debug.Log("Set Active Player");
            activePlayer.SetActive(true);
            int playerStartInitiative = activePlayer.GetInitiative(true);
            int initativeDiff = activePlayer.GetInitiative(false);

            int currInitiative;
            foreach (GameObject player in players)
            {
                currInitiative = player.GetComponent<PlayerBattle>().GetInitiative(false);
                player.GetComponent<PlayerBattle>().SetInitiative(currInitiative - initativeDiff, false);
                // Debug.Log("Player Current Initiative: " + player.GetComponent<PlayerBattle>().GetInitiative(false));
            }

            players[0].transform.Find("PlayerSelect").gameObject.SetActive(true);
        }
        else
        {
            active = Active.Enemy;

            activeEnemy = (EnemyBattle)(object)activeEntity;
            Debug.Log("Set Active Enemy");
            activeEnemy.SetActive(true);
            int enemyStartInitiative = activeEnemy.GetInitiative(true);
            int initativeDiff = activeEnemy.GetInitiative(false);

            int currInitiative;
            foreach (GameObject enemy in enemies)
            {
                currInitiative = enemy.GetComponent<EnemyBattle>().GetInitiative(false);
                enemy.GetComponent<EnemyBattle>().SetInitiative(currInitiative - initativeDiff, false);
                // Debug.Log("Enemy Current Initiative: " + enemy.GetComponent<EnemyBattle>().GetInitiative(false));
            }

            enemies[0].transform.Find("EnemySelect").gameObject.SetActive(true);
        }
    }

    private void NextActive(object sender, EventArgs e)
    {
        if (active == Active.Player)
        {
            SetActive<EnemyBattle>(enemies[0].GetComponent<EnemyBattle>());
            enemies[0].GetComponent<EnemyBattle>().Attack();
        }
        else
        {
            SetActive<PlayerBattle>(players[0].GetComponent<PlayerBattle>());
        }
    }

    public void doDamage(int damage, bool isEnemy, int num, Vector3 pos)
    {
        if (isEnemy)
        {
            foreach (GameObject enemy in enemies)
            {
                if (enemy.GetComponent<EnemyBattle>().GetEnemyNum() == num)
                {
                    enemies[enemies.IndexOf(enemy)].GetComponent<Health>().Damage(damage);
                    ShowDamage(damage.ToString(), pos);
                }
            }
        }
        else
        {
            foreach (GameObject player in players)
            {
                if (player.GetComponent<PlayerBattle>().GetPlayerNum() == num)
                {
                    players[players.IndexOf(player)].GetComponent<Health>().Damage(damage);
                    ShowDamage(damage.ToString(), pos);
                }
            }
        }
    }

    private void ShowDamage(string text, Vector3 pos)
    {
        GameObject damageText;

        if (pfDamageText)
        {
            damageText = Instantiate(pfDamageText, pos, Quaternion.identity);
            damageText.GetComponentInChildren<TextMeshPro>().text = text;
        }
    }

    public string GetActive()
    {
        return active.ToString();
    }

    public void OrderList(bool isEnemy)
    {
        if (isEnemy == true)
        {
            enemies = enemies.OrderBy(e => e.GetComponent<EnemyBattle>().GetInitiative(false)).ToList();

            // foreach (GameObject enemy in enemies)
            // {
            //     Debug.Log("Orderlist() | Enemy Current Initiative: " + enemy.GetComponent<EnemyBattle>().GetInitiative(false));
            // }

        }
        else
        {
            players = players.OrderBy(p => p.GetComponent<PlayerBattle>().GetInitiative(false)).ToList();

            // foreach (GameObject player in players)
            // {
            //     Debug.Log("Orderlist() | Player Current Initiative: " + player.GetComponent<PlayerBattle>().GetInitiative(false));
            // }
        }
    }

    public void DestroyEntity<T>(int num)
    {
        if (typeof(T) == typeof(PlayerBattle))
        {
            foreach (GameObject player in players.ToList())
            {
                if (player.GetComponent<PlayerBattle>().GetPlayerNum() == num)
                {
                    players.RemoveAt(players.IndexOf(player));
                }
            }
        }
        else
        {
            foreach (GameObject enemy in enemies.ToList())
            {
                if (enemy.GetComponent<EnemyBattle>().GetEnemyNum() == num)
                {
                    enemies.RemoveAt(enemies.IndexOf(enemy));
                }
            }
        }
    }

    public IEnumerator TurnDelay()
    {
        yield return new WaitForSeconds(5f);
    }

    private void GenerateInitiative<T>(int numOfInits, int i)
    {
        int init;

        init = UnityEngine.Random.Range(10, numOfInits * 10 + 1);

        if (typeof(T) == typeof(PlayerBattle))
        {
            while (players.Any(p => p.GetComponent<PlayerBattle>().GetInitiative(false) == init || enemies.Any(e => e.GetComponent<EnemyBattle>().GetInitiative(false) == init)))
            {
                init = UnityEngine.Random.Range(10, numOfInits * 10 + 1);
            }

            players[i].GetComponent<PlayerBattle>().SetInitiative(init, true);
        }
        else
        {
            while (enemies.Any(e => e.GetComponent<EnemyBattle>().GetInitiative(false) == init || players.Any(p => p.GetComponent<PlayerBattle>().GetInitiative(false) == init)))
            {
                init = UnityEngine.Random.Range(10, numOfInits * 10 + 1);
            }

            enemies[i].GetComponent<EnemyBattle>().SetInitiative(init, true);
        }
    }
}
