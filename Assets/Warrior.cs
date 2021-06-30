using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : MonoBehaviour
{
    [SerializeField] private GameObject pfWarriorGrave;

    private void Start()
    {
        PlayerBattle playerBattle = GetComponent<PlayerBattle>();
        playerBattle.setDead += SpawnGrave;
    }

    private void SpawnGrave(object sender, EventArgs e)
    {
        Instantiate(pfWarriorGrave, this.transform.position, Quaternion.identity);
    }

}
