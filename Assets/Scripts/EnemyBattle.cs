using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBattle : MonoBehaviour
{
    [SerializeField] private float power;
    [SerializeField] private Vector2 minPower, maxPower;
    [SerializeField] private Vector3 charOffset;
    private Vector2 force;
    private Vector3 playerPos, enemyPos;
    private int startInitiative, currIntiative, enemyNum;
    private Rigidbody2D enemyBody;
    private BattleHandler battleSystem;
    private GameHandler gameHandler;
    private bool active, hit, enter, start;
    public event EventHandler triggerNextTurn;

    private void Start()
    {
        this.hit = false;
        this.enter = false;
        this.start = false;
        this.enemyBody = GetComponent<Rigidbody2D>();
        battleSystem = GameObject.FindGameObjectWithTag("BattleSystem").GetComponent<BattleHandler>();
        gameHandler = GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>();
    }

    public void Attack()
    {
        if (this.active == true && battleSystem.GetActive() == "Enemy")
        {
            this.enemyPos = this.transform.position + charOffset;
            playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position;
            this.force = new Vector2(Mathf.Clamp(playerPos.x - this.enemyPos.x, this.minPower.x, this.maxPower.x), Mathf.Clamp(playerPos.y - this.enemyPos.y, this.minPower.y, this.maxPower.y));
            this.enemyBody.AddForce(this.force * power, ForceMode2D.Impulse);
            this.hit = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        ProcessCollision(other.gameObject);
    }

    private void ProcessCollision(GameObject other)
    {
        if (other.CompareTag("Player"))
        {
            if (battleSystem.GetActive() == "Enemy")
            {
                if (this.active == true)
                {
                    battleSystem.doDamage(Damage(), false, other.gameObject.GetComponent<PlayerBattle>().GetPlayerNum(), other.gameObject.transform.Find("HealthBar").transform.position);

                }
            }
        }
    }

    private int Damage()
    {
        return (int)Mathf.Ceil(this.enemyBody.velocity.magnitude);
    }

    public int GetInitiative(bool start)
    {
        if (start == true)
            return this.startInitiative;
        else
            return this.currIntiative;
    }
    public void SetInitiative(int init)
    {
        if (start == false)
        {
            this.startInitiative = init;
            this.currIntiative = this.startInitiative;
            start = true;
        }
        else
            this.currIntiative = init;
    }
    public int GetEnemyNum()
    {
        return this.enemyNum;
    }
    public void SetEnemyNum(int num)
    {
        this.enemyNum = num;
    }
    public bool GetActive()
    {
        return this.active;
    }
    public void SetActive(bool active)
    {
        if (active == true)
        {
            this.active = true;
        }
        else
            this.active = false;
    }

    private IEnumerator TurnDelay()
    {
        this.enter = true;
        yield return new WaitForSeconds(1f);
        triggerNextTurn?.Invoke(this, EventArgs.Empty);
        this.enter = false;
    }

    private void FixedUpdate()
    {
        if (this.active == true)
        {
            if (this.hit == true)
            {
                if (this.enemyBody.velocity.magnitude < 0.2f)
                {
                    this.enemyBody.velocity = Vector3.zero;
                    this.active = false;
                    this.hit = false;
                    this.currIntiative = this.startInitiative;
                    if (this.enter == false) StartCoroutine(TurnDelay());
                }
            }
        }
    }
}