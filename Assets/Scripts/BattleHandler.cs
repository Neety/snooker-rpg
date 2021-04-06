using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class BattleHandler : MonoBehaviour
{
    private static BattleHandler inst;
    public static BattleHandler GetInstance()
    {
        return inst;
    }
    [SerializeField] private Transform pfPlayer, pfEnemy, pfHealthBar;
    [SerializeField] private GameObject playerSelect, enemySelect, pfDamageText;

    private int numOfPlayers;
    private List<Transform> players = new List<Transform>();
    private List<Health> playerHealth = new List<Health>();
    private List<HealthBar> playerHealthBar = new List<HealthBar>();
    private List<Transform> playerHealthBarTransform = new List<Transform>();

    private int numOfEnemies;
    private List<Transform> enemies = new List<Transform>();
    private List<Health> enemyHealth = new List<Health>();
    private List<HealthBar> enemyHealthBar = new List<HealthBar>();
    private List<Transform> enemyHealthBarTransform = new List<Transform>();

    private Active active;
    private enum Active
    {
        Player, Enemy
    }

    private void Awake()
    {
        inst = this;

        numOfPlayers = 3;
        numOfEnemies = 3;
    }
    private void Start()
    {
        for (int i = 0; i < numOfPlayers; i++)
        {
            SpawnCharacter(true, i);
        }

        for (int i = 0; i < numOfEnemies; i++)
        {
            SpawnCharacter(false, i);
        }

        players = players.OrderBy(p => p.GetComponent<PlayerBattle>().GetInitiative()).ToList();
        enemies = enemies.OrderBy(e => e.GetComponent<EnemyBattle>().GetInitiative()).ToList();

        for (int i = 0; i < 3; i++)
        {
            Debug.Log("Player: " + i + " " + players[i].GetComponent<PlayerBattle>().GetInitiative());
            Debug.Log("Enemy: " + i + " " + enemies[i].GetComponent<EnemyBattle>().GetInitiative());
        }

        SetActive<PlayerBattle>(players[0].GetComponent<PlayerBattle>());
    }

    private void SpawnCharacter(bool isPlayerTeam, int i)
    {
        Vector3 position;

        if (isPlayerTeam)
        {
            position = new Vector3(Random.Range(-8f, -4f), Random.Range(-4f, 0), 0);
            players.Insert(i, Instantiate(pfPlayer, position, Quaternion.identity));
            players[i].GetComponent<PlayerBattle>().SetPlayerNum(i);
            playerHealth.Insert(i, new Health(100));
            playerHealthBarTransform.Insert(i, Instantiate(pfHealthBar, players[i].position + new Vector3(0f, 1f, 0f), Quaternion.identity, players[i]));
            playerHealthBar.Insert(i, playerHealthBarTransform[i].GetComponent<HealthBar>());
            playerHealthBar[i].Setup(playerHealth[i]);
            Instantiate(playerSelect, players[i].position + new Vector3(0f, -0.65f, 0f), Quaternion.identity, players[i]);
            GenerateInitiative<PlayerBattle>(numOfPlayers, i);
        }
        else
        {
            position = new Vector3(Random.Range(4f, 8f), Random.Range(0, 4f), 0);
            enemies.Insert(i, Instantiate(pfEnemy, position, Quaternion.identity));
            enemies[i].GetComponent<EnemyBattle>().SetEnemyNum(i);
            enemyHealth.Insert(i, new Health(100));
            enemyHealthBarTransform.Insert(i, Instantiate(pfHealthBar, enemies[i].position + new Vector3(0f, 1.15f, 0f), Quaternion.identity, enemies[i]));
            enemyHealthBar.Insert(i, enemyHealthBarTransform[i].GetComponent<HealthBar>());
            enemyHealthBar[i].Setup(enemyHealth[i]);
            GenerateInitiative<EnemyBattle>(numOfEnemies, i);
        }
    }

    private void SetActive<T>(T activeEntity)
    {
        if (typeof(T) == typeof(PlayerBattle))
        {
            PlayerBattle aP = (PlayerBattle)(object)activeEntity;
            aP.SetState(true);
            active = Active.Player;
        }
        else if (typeof(T) == typeof(EnemyBattle))
        {
            EnemyBattle aE = (EnemyBattle)(object)activeEntity;
            aE.SetState(true);
            active = Active.Enemy;
        }
    }

    public void NextActive()
    {
        if (active == Active.Player)
        {
            // SetActiveEnemy(enemies[0]);
            SetActive<EnemyBattle>(enemies[0].GetComponent<EnemyBattle>());
            enemies[0].GetComponent<EnemyBattle>().Attack();
        }
        else
        {
            // SetActivePlayer(players[0]);
            SetActive<PlayerBattle>(players[0].GetComponent<PlayerBattle>());
        }
    }

    public void doDamage(int damage, bool isEnemy, int num, Vector3 pos)
    {
        if (isEnemy)
        {
            enemyHealth[num].Damage(damage);
            ShowDamage(damage.ToString(), pos + new Vector3(0f, 1.15f, 0f));
        }
        else
        {
            playerHealth[num].Damage(damage);
            ShowDamage(damage.ToString(), pos + new Vector3(0f, 1f, 0f));
        }
    }

    private void ShowDamage(string text, Vector3 pos)
    {
        GameObject damageText;

        if (pfDamageText)
        {
            damageText = Instantiate(pfDamageText, pos, Quaternion.identity);
            damageText.GetComponentInChildren<TextMeshPro>().text = text;
        }
    }

    // private Vector3 GetPos(bool isEnemy, int num)
    // {
    //     Vector3 pos;

    //     if (isEnemy)
    //     {
    //         pos = enemies[num].GetComponent<Transform>().position + new Vector3(0f, 1.15f, 0f);
    //     }
    //     else
    //     {
    //         pos = players[num].GetComponent<Transform>().position + new Vector3(0f, 1f, 0f);
    //     }

    //     return pos;
    // }

    public string GetActive()
    {
        return active.ToString();
    }

    private void GenerateInitiative<T>(int numOfInits, int i)
    {
        int init;

        init = Random.Range(10, numOfInits * 10 + 1);

        if (typeof(T) == typeof(PlayerBattle))
        {
            while (players.Any(p => p.GetComponent<PlayerBattle>().GetInitiative() == init))
            {
                init = Random.Range(10, numOfInits * 10 + 1);
            }

            players[i].GetComponent<PlayerBattle>().SetInitiative(init);
        }
        else
        {
            while (enemies.Any(e => e.GetComponent<EnemyBattle>().GetInitiative() == init))
            {
                init = Random.Range(10, numOfInits * 10 + 1);
            }

            enemies[i].GetComponent<EnemyBattle>().SetInitiative(init);
        }
    }
}
