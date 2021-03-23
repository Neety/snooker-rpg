using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameHandler : MonoBehaviour
{
    [SerializeField] private int MaxHP;
    [SerializeField] private Transform pfHealthBar;
    [SerializeField] private GameObject pfDamageText;
    private GameObject damageText;
    private Vector3 playerPos, enemyPos, pos;
    private Transform playerHealthBarTransform, enemyHealthBarTransform, player, enemy;
    public Health playerHealth, enemyHealth;
    private HealthBar playerHealthBar, enemyHealthBar;
    private BattleHandler battleSystem;
    private IEnumerator Start()
    {
        battleSystem = GameObject.FindGameObjectWithTag("BattleSystem").GetComponent<BattleHandler>();

        yield return new WaitForSeconds(0.01f);

        playerHealth = new Health(MaxHP);
        enemyHealth = new Health(MaxHP);

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Transform>();

        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position + new Vector3(0f, 0.85f, 0f);
        enemyPos = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Transform>().position + new Vector3(0f, 0.85f, 0f);

        playerHealthBarTransform = Instantiate(pfHealthBar, playerPos, Quaternion.identity, player);
        enemyHealthBarTransform = Instantiate(pfHealthBar, enemyPos, Quaternion.identity, enemy);

        playerHealthBar = playerHealthBarTransform.GetComponent<HealthBar>();
        enemyHealthBar = enemyHealthBarTransform.GetComponent<HealthBar>();

        playerHealthBar.Setup(playerHealth);
        enemyHealthBar.Setup(enemyHealth);
    }

    public void doDamage(int damage, bool isEnemy)
    {
        if (isEnemy)
        {
            enemyHealth.Damage(damage);
            ShowDamage(damage.ToString(), GetPos(isEnemy));
        }
        else
        {
            playerHealth.Damage(damage);
            ShowDamage(damage.ToString(), GetPos(isEnemy));
        }
    }

    private void ShowDamage(string text, Vector3 pos)
    {
        if (pfDamageText)
        {
            damageText = Instantiate(pfDamageText, pos, Quaternion.identity);
            damageText.GetComponentInChildren<TextMeshPro>().text = text;
        }
    }

    private Vector3 GetPos(bool isEnemy)
    {
        if (isEnemy)
        {
            pos = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Transform>().position + new Vector3(0f, 0.85f, 0f);
        }
        else
        {
            pos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position + new Vector3(0f, 0.85f, 0f);
        }

        return pos;

    }
}
