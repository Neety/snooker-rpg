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
    private PlayerBattle player, activePlayer;
    private EnemyBattle enemy, activeEnemy;
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
        SpawnCharacter(true);
        SpawnCharacter(false);

        SetActivePlayer(player);

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBattle>();
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyBattle>();
    }

    private void SpawnCharacter(bool isPlayerTeam)
    {
        Vector3 position;

        if (isPlayerTeam)
        {
            position = new Vector3(-5, 0, 0);
            Instantiate(pfPlayer, position, Quaternion.identity);
        }
        else
        {
            position = new Vector3(+5, 0, 0);
            Instantiate(pfEnemy, position, Quaternion.identity);
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
}
