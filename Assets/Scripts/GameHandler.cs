using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    [SerializeField] private int MaxHP;
    [SerializeField] private Transform pfHealthBar;
    private Vector3 playerPos, enemyPos;
    private Transform playerHealthBarTransform, enemyHealthBarTransform;
    public Health health;
    private HealthBar playerHealthBar, enemyHealthBar;
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.01f);

        health = new Health(MaxHP);

        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position + new Vector3(0f, 0.8f, 0f);
        enemyPos = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Transform>().position + new Vector3(0f, 0.8f, 0f);

        playerHealthBarTransform = Instantiate(pfHealthBar, playerPos, Quaternion.identity);
        enemyHealthBarTransform = Instantiate(pfHealthBar, enemyPos, Quaternion.identity);
        playerHealthBar = playerHealthBarTransform.GetComponent<HealthBar>();
        enemyHealthBar = enemyHealthBarTransform.GetComponent<HealthBar>();

        playerHealthBar.Setup(health);
        enemyHealthBar.Setup(health);
    }
}
