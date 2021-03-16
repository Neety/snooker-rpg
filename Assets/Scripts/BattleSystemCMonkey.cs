using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystemCMonkey : MonoBehaviour
{
    private static BattleSystemCMonkey inst;
    public static BattleSystemCMonkey GetInstance()
    {
        return inst;
    }
    [SerializeField] private Transform pfPlayer, pfEnemy;
    private CharacterBattle player, enemy, active;
    private State state;
    private enum State
    {
        Waiting, Busy
    }
    private void Awake()
    {
        inst = this;
    }
    private void Start()
    {
        SpawnCharacter(true);
        SpawnCharacter(false);

        SetActive(player);
        state = State.Waiting;

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBattle>();
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<CharacterBattle>();
    }

    private void Update()
    {
        if (player.Stopped())
        {
            NextActive();
        }

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

    private void SetActive(CharacterBattle activeChar)
    {
        active = activeChar;
    }

    private void NextActive()
    {
        if (active == player)
        {
            SetActive(enemy);
            state = State.Busy;
            // Enemy Battle Logic
            NextActive();
        }
        else
        {
            SetActive(player);
            state = State.Waiting;
        }
    }

    public string GetState()
    {
        return state.ToString();
    }
}
