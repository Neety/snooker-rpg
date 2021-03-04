using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystemCMonkey : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform enemy;

    private void Start()
    {
        SpawnCharacter(true);
        SpawnCharacter(false);
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
