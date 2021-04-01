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
    [SerializeField] private Transform pfPlayer, pfEnemy;
    private int numOfPlayers;
    private int numOfEnemies;
    private List<int> playerInits = new List<int>();
    private List<int> enemyInits = new List<int>();
    private List<PlayerBattle> players = new List<PlayerBattle>();
    private List<EnemyBattle> enemies = new List<EnemyBattle>();
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

        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            players.Add(player.GetComponent<PlayerBattle>());
        }
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemies.Add(enemy.GetComponent<EnemyBattle>());
        }

        for (int i = 0; i < numOfPlayers; i++)
        {
            players[i].SetPlayerNum(i);
        }
        for (int i = 0; i < numOfEnemies; i++)
        {
            enemies[i].SetEnemyNum(i);
        }

        GenerateInitiative<PlayerBattle>(players, numOfPlayers);
        GenerateInitiative<EnemyBattle>(enemies, numOfEnemies);

        players = players.OrderBy(p => p.GetInitiative()).ToList();
        enemies = enemies.OrderBy(e => e.GetInitiative()).ToList();

        // SetActivePlayer(players[0]);
        SetActive<PlayerBattle>(players[0]);
    }

    private void SpawnCharacter(bool isPlayerTeam, int num)
    {
        Vector3 position;

        if (isPlayerTeam)
        {
            position = new Vector3(Random.Range(-8f, -4f), Random.Range(-4f, 0), 0);
            Instantiate(pfPlayer, position, Quaternion.identity);
        }
        else
        {
            position = new Vector3(Random.Range(4f, 8f), Random.Range(0, 4f), 0);
            Instantiate(pfEnemy, position, Quaternion.identity);
        }
    }

    // private void SetActivePlayer(PlayerBattle activeP)
    // {
    //     activeP.SetState(true);
    //     active = Active.Player;
    // }

    // private void SetActiveEnemy(EnemyBattle activeE)
    // {
    //     activeE.SetState(true);
    //     active = Active.Enemy;
    // }

    private void SetActive<T>(T activeEntity)
    {
        if (typeof(T) == typeof(PlayerBattle))
        {
            PlayerBattle aP = (PlayerBattle)(object)activeEntity;
            aP.SetState(true);
            active = Active.Player;
        }
        else if (typeof(T) == typeof(EnemyBattle))
        {
            EnemyBattle aE = (EnemyBattle)(object)activeEntity;
            aE.SetState(true);
            active = Active.Enemy;
        }
    }

    public void NextActive()
    {
        if (active == Active.Player)
        {
            // SetActiveEnemy(enemies[0]);
            SetActive<EnemyBattle>(enemies[0]);
            enemies[0].Attack();
        }
        else
        {
            // SetActivePlayer(players[0]);
            SetActive<PlayerBattle>(players[0]);
        }
    }

    public string GetActive()
    {
        return active.ToString();
    }
    public int GetNumOfPlayers()
    {
        return numOfPlayers;
    }
    public int GetNumOfEnemies()
    {
        return numOfEnemies;
    }
    public List<PlayerBattle> GetPlayers()
    {
        return players;
    }
    public List<EnemyBattle> GetEnemies()
    {
        return enemies;
    }

    private void GenerateInitiative<T>(List<T> entities, int numOfInits)
    {
        int init;

        for (int i = 0; i < numOfInits; i++)
        {
            init = Random.Range(1, numOfInits);

            if (typeof(T) == typeof(PlayerBattle))
            {
                List<PlayerBattle> players = (List<PlayerBattle>)(object)entities;

                while (players.Any(p => p.GetInitiative() == init))
                {
                    init = Random.Range(1, numOfInits + 1);
                }

                players[i].SetInitiative(init);
            }
            else
            {
                List<EnemyBattle> enemies = (List<EnemyBattle>)(object)entities;

                while (enemies.Any(e => e.GetInitiative() == init))
                {
                    init = Random.Range(1, numOfInits + 1);
                }

                enemies[i].SetInitiative(init);
            }
        }
    }

    private int SortInitiative<T>(T a, T b)
    {
        if (typeof(T) == typeof(PlayerBattle))
        {
            PlayerBattle alpha = (PlayerBattle)(object)a;
            PlayerBattle beta = (PlayerBattle)(object)b;

            if (alpha.GetInitiative() < beta.GetInitiative())
            {
                return -1;
            }
            else if (alpha.GetInitiative() > beta.GetInitiative())
            {
                return 1;
            }
            return 0;
        }
        else
        {
            EnemyBattle alpha = (EnemyBattle)(object)a;
            EnemyBattle beta = (EnemyBattle)(object)b;

            if (alpha.GetInitiative() < beta.GetInitiative())
            {
                return -1;
            }
            else if (alpha.GetInitiative() > beta.GetInitiative())
            {
                return 1;
            }
            return 0;
        }
    }
}
