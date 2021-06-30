using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ogre : MonoBehaviour
{
    [SerializeField] private GameObject pfOgreGrave;

    private void Start()
    {
        EnemyBattle enemyBattle = GetComponent<EnemyBattle>();
        enemyBattle.setDead += SpawnGrave;
    }

    private void SpawnGrave(object sender, EventArgs e)
    {
        Instantiate(pfOgreGrave, this.transform.position, Quaternion.identity);
    }
}
