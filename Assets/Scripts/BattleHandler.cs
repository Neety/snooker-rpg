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
    [SerializeField] private int numOfPlayers;
    [SerializeField] private int numOfEnemies;
    private List<int> playerInits = new List<int>();
    private List<int> enemyInits = new List<int>();
    private List<PlayerBattle> players = new List<PlayerBattle>();
    private List<EnemyBattle> enemies = new List<EnemyBattle>();
    private PlayerBattle activePlayer;
    private EnemyBattle activeEnemy;
    private State state;
    private enum State
    {
        Waiting, Busy
    }
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
        GenerateInitiative(playerInits, numOfPlayers);
        for (int i = 0; i < numOfPlayers; i++)
        {
            SpawnCharacter(true, playerInits[i], i);
        }

        GenerateInitiative(enemyInits, numOfEnemies);
        for (int i = 0; i < numOfEnemies; i++)
        {
            SpawnCharacter(false, enemyInits[i], i);
        }

        SetActivePlayer(player);

        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            players.Add(player.GetComponent<PlayerBattle>());
        }

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemies.Add(enemy.GetComponent<EnemyBattle>());
        }
    }

    private void SpawnCharacter(bool isPlayerTeam, int initiative, int num)
    {
        Vector3 position;

        if (isPlayerTeam)
        {
            position = new Vector3(Random.Range(-8f, -4f), Random.Range(-4f, 0), 0);
            Instantiate(pfPlayer, position, Quaternion.identity);
            players[num].SetInitiative(initiative);
            players[num].SetPlayerNum(num);
        }
        else
        {
            position = new Vector3(Random.Range(4f, 8f), Random.Range(0, 4f), 0);
            Instantiate(pfEnemy, position, Quaternion.identity);
            enemies[num].SetInitiative(initiative);
            enemies[num].SetEnemyNum(num);
        }
    }

    private void SetActivePlayer(PlayerBattle activeP)
    {
        activePlayer = activeP;
        active = Active.Player;
        state = State.Waiting;
    }

    private void SetActiveEnemy(EnemyBattle activeE)
    {
        activeEnemy = activeE;
        active = Active.Enemy;
        state = State.Busy;
    }

    public void NextActive()
    {
        if (active == Active.Player)
        {
            SetActiveEnemy(enemy);
            enemy.Attack();
        }
        else
        {
            SetActivePlayer(player);
            state = State.Waiting;
        }
    }

    public string GetState()
    {
        return state.ToString();
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
    private void GenerateInitiative(List<int> inits, int numOfInits)
    {
        int init;

        for (int i = 0; i < numOfInits + 1; i++)
        {
            init = Random.Range(1, numOfInits);

            while (inits.Contains(init))
            {
                init = Random.Range(1, numOfInits);
            }

            inits[i] = init * 5;
        }
    }
}
