using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBattle : MonoBehaviour
{
    [SerializeField] private float power;
    [SerializeField] private Vector2 minPower, maxPower;
    [SerializeField] private Vector3 charOffset;
    [SerializeField] private Transform select;
    private Vector2 force;
    private Vector3 playerPos, enemyPos;
    private int initiative, enemyNum;
    private Rigidbody2D enemyBody;
    private BattleHandler battleSystem;
    private GameHandler gameHandler;
    private bool active, hit;

    private void Start()
    {
        this.hit = false;
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
                    battleSystem.doDamage(Damage(), false, other.gameObject.GetComponent<PlayerBattle>().GetPlayerNum(), other.gameObject.GetComponent<Transform>().position);

                }
            }
        }
    }

    private int Damage()
    {
        return (int)Mathf.Ceil(this.enemyBody.velocity.magnitude) * 5;
    }

    public int GetInitiative()
    {
        return this.initiative;
    }
    public void SetInitiative(int init)
    {
        this.initiative = init;
    }
    public int GetEnemyNum()
    {
        return this.enemyNum;
    }
    public void SetEnemyNum(int num)
    {
        this.enemyNum = num;
    }
    public void SetState(bool active)
    {
        if (active == true)
        {
            this.active = true;
        }
        else
            this.active = false;
    }

    public bool GetState()
    {
        return this.active;
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
                }
            }
        }
    }

    private void LateUpdate()
    {
        if (this.hit == true && this.active == false)
        {
            battleSystem.NextActive();
            this.hit = false;
        }
    }
}