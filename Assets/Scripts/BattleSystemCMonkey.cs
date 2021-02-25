using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystemCMonkey : MonoBehaviour
{
    [SerializeField] private Transform pfCharacterBattle;
    public Texture2D playerSpritesheet;
    public Texture2D enemySpritesheet;

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
            position = new Vector3(-5, 0, +4);
        }
        else
        {
            position = new Vector3(+5, 0, +4);
        }

        Instantiate(pfCharacterBattle, position, Quaternion.identity);
    }
}
