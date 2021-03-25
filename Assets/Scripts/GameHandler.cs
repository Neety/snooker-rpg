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
    private Vector3 pos;
    private List<Vector3> playerPos = new List<Vector3>();
    private List<Vector3> enemyPos = new List<Vector3>();
    private List<Transform> playerHealthBarTransform = new List<Transform>();
    private List<Transform> enemyHealthBarTransform = new List<Transform>();
    private List<Transform> player = new List<Transform>();
    private List<Transform> enemy = new List<Transform>();
    public List<Health> playerHealth = new List<Health>();
    public List<Health> enemyHealth = new List<Health>();
    private List<HealthBar> playerHealthBar = new List<HealthBar>();
    private List<HealthBar> enemyHealthBar = new List<HealthBar>();
    private BattleHandler battleSystem;
    private IEnumerator Start()
    {
        battleSystem = GameObject.FindGameObjectWithTag("BattleSystem").GetComponent<BattleHandler>();

        yield return new WaitForSeconds(0.01f);

        foreach (GameObject playerHBTransform in GameObject.FindGameObjectsWithTag("Player"))
        {
            playerHealthBarTransform.Add(playerHBTransform.GetComponent<Transform>());
        }

        foreach (GameObject enemyHBTransform in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemyHealthBarTransform.Add(enemyHBTransform.GetComponent<Transform>());
        }

        foreach (GameObject playerTransform in GameObject.FindGameObjectsWithTag("Player"))
        {
            player.Add(playerTransform.GetComponent<Transform>());
        }

        foreach (GameObject enemyTransform in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemy.Add(enemyTransform.GetComponent<Transform>());
        }

        for (int i = 0; i < battleSystem.GetNumOfPlayers(); i++)
        {
            playerHealth[i] = new Health(MaxHP);
            playerPos[i] = player[i].position + new Vector3(0f, 0.85f, 0f);
            playerHealthBarTransform[i] = Instantiate(pfHealthBar, playerPos[i], Quaternion.identity, player[i]);
            playerHealthBar[i] = playerHealthBarTransform[i].GetComponent<HealthBar>();
            playerHealthBar[i].Setup(playerHealth[i]);
        }

        for (int i = 0; i < battleSystem.GetNumOfPlayers(); i++)
        {
            enemyHealth[i] = new Health(MaxHP);
            enemyPos[i] = enemy[i].position + new Vector3(0f, 0.85f, 0f);
            enemyHealthBarTransform[i] = Instantiate(pfHealthBar, enemyPos[i], Quaternion.identity, enemy[i]);
            enemyHealthBar[i] = enemyHealthBarTransform[i].GetComponent<HealthBar>();
            enemyHealthBar[i].Setup(enemyHealth[i]);
        }
    }

    public void doDamage(int damage, bool isEnemy, int num)
    {
        if (isEnemy)
        {
            enemyHealth[num].Damage(damage);
            ShowDamage(damage.ToString(), GetPos(isEnemy, num));
        }
        else
        {
            playerHealth[num].Damage(damage);
            ShowDamage(damage.ToString(), GetPos(isEnemy, num));
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

    private Vector3 GetPos(bool isEnemy, int num)
    {
        if (isEnemy)
        {
            pos = player[num].position + new Vector3(0f, 0.85f, 0f);
        }
        else
        {
            pos = player[num].position + new Vector3(0f, 0.85f, 0f);
        }

        return pos;

    }
}
