using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystemCMonkey : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform enemy;
    private State state;
    private enum State
    {
        playerTurn, enemyTurn
    }

    private DragNShootV2 dragNShoot;

    private void Start()
    {
        SpawnCharacter(true);
        SpawnCharacter(false);
        state = State.playerTurn;

        dragNShoot = GameObject.FindGameObjectWithTag("Player").GetComponent<DragNShootV2>();
    }

    private void Update()
    {
        if (state == State.playerTurn)
        {

        }

        if (dragNShoot.Stopped())
        {
            if (state == State.playerTurn)
            {
                state = State.enemyTurn;
                Debug.Log("Enemy Turn");
            }
            else
            {
                state = State.playerTurn;
                Debug.Log("Player Turn");
            }

        }
    }
    private void SpawnCharacter(bool isPlayerTeam)
    {
        Vector3 position;

        if (isPlayerTeam)
        {
            position = new Vector3(-5, 0, 0);
            Instantiate(player, position, Quaternion.identity);
        }
        else
        {
            position = new Vector3(+5, 0, 0);
            Instantiate(enemy, position, Quaternion.identity);
        }


    }
}
