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
    public GameObject pfImpact;

    private int numOfPlayers;
    private int activePlayerNum;
    private List<GameObject> players = new List<GameObject>();

    private int numOfEnemies;
    private int activeEnemyNum;
    private List<GameObject> enemies = new List<GameObject>();

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
    }
    private void Start()
    {
        numOfPlayers = UnityEngine.Random.Range(1, 5);
        numOfEnemies = UnityEngine.Random.Range(1, 5);

        for (int i = 0; i < numOfPlayers; i++)
        {
            SpawnCharacter(true, i);
        }

        for (int i = 0; i < numOfEnemies; i++)
        {
            SpawnCharacter(false, i);
        }

        players = players.OrderBy(p => p.GetComponent<PlayerBattle>().GetInitiative()).ToList();
        enemies = enemies.OrderBy(e => e.GetComponent<EnemyBattle>().GetInitiative()).ToList();

        foreach (GameObject player in players)
        {
            player.GetComponent<PlayerBattle>().triggerNextTurn += NextActive;
        }

        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<EnemyBattle>().triggerNextTurn += NextActive;
        }

        SetActive<PlayerBattle>(players[0].GetComponent<PlayerBattle>());
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
        }
        else
        {
            position = new Vector3(UnityEngine.Random.Range(6f, 12f), UnityEngine.Random.Range(0, 8f), 0);
            enemies.Insert(i, Instantiate(pfEnemy, position, Quaternion.identity));
            enemies[i].GetComponent<EnemyBattle>().SetEnemyNum(i);
            enemies[i].GetComponentInChildren<HealthBar>().Setup(enemies[i].GetComponent<Health>());
            GenerateInitiative<EnemyBattle>(numOfEnemies, i);
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
            int initativeDiff = activePlayer.GetInitiative();

            NextInitiative(initativeDiff);

            players[0].transform.Find("PlayerSelect").gameObject.SetActive(true);
        }
        else
        {
            active = Active.Enemy;

            activeEnemy = (EnemyBattle)(object)activeEntity;
            Debug.Log("Set Active Enemy");
            activeEnemy.SetActive(true);
            int initativeDiff = activeEnemy.GetInitiative();

            NextInitiative(initativeDiff);

            enemies[0].transform.Find("EnemySelect").gameObject.SetActive(true);
        }
    }

    private void NextActive(object sender, EventArgs e)
    {
        if (enemies[0].GetComponent<EnemyBattle>().GetInitiative() < players[0].GetComponent<PlayerBattle>().GetInitiative())
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
            enemies = enemies.OrderBy(e => e.GetComponent<EnemyBattle>().GetInitiative()).ToList();
        }
        else
        {
            players = players.OrderBy(p => p.GetComponent<PlayerBattle>().GetInitiative()).ToList();
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
        yield return new WaitForSeconds(2f);
    }

    private void NextInitiative(int initiativeDiff)
    {
        int currInitiative;

        foreach (GameObject player in players)
        {
            currInitiative = player.GetComponent<PlayerBattle>().GetInitiative();
            player.GetComponent<PlayerBattle>().SetInitiative(currInitiative - initiativeDiff, false);
        }

        foreach (GameObject enemy in enemies)
        {
            currInitiative = enemy.GetComponent<EnemyBattle>().GetInitiative();
            enemy.GetComponent<EnemyBattle>().SetInitiative(currInitiative - initiativeDiff, false);
        }
    }

    private void GenerateInitiative<T>(int numOfInits, int i)
    {
        int init;

        init = UnityEngine.Random.Range(10, numOfInits * 10 + 1);

        if (typeof(T) == typeof(PlayerBattle))
        {
            while (players.Any(p => p.GetComponent<PlayerBattle>().GetInitiative() == init || enemies.Any(e => e.GetComponent<EnemyBattle>().GetInitiative() == init)))
            {
                init = UnityEngine.Random.Range(10, numOfInits * 10 + 1);
            }

            players[i].GetComponent<PlayerBattle>().SetInitiative(init, true);
        }
        else
        {
            while (enemies.Any(e => e.GetComponent<EnemyBattle>().GetInitiative() == init || players.Any(p => p.GetComponent<PlayerBattle>().GetInitiative() == init)))
            {
                init = UnityEngine.Random.Range(10, numOfInits * 10 + 1);
            }

            enemies[i].GetComponent<EnemyBattle>().SetInitiative(init, true);
        }
    }
}
