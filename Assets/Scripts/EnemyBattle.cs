using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBattle : MonoBehaviour
{
    [SerializeField] private float power;
    [SerializeField] private Vector2 minPower, maxPower;
    [SerializeField] private Vector3 charOffset;
    private Vector2 force;
    private Rigidbody2D enemyBody;
    private BattleHandler battleSystem;
    private GameHandler gameHandler;
    private Vector3 playerPos, enemyPos;
    private State enemyState;
    private enum State
    {
        Active, Inactive
    }

    private void Start()
    {
        this.enemyState = State.Inactive;
        enemyBody = GetComponent<Rigidbody2D>();
        battleSystem = GameObject.FindGameObjectWithTag("BattleSystem").GetComponent<BattleHandler>();
        gameHandler = GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>();
    }

    public void Attack()
    {
        if (battleSystem.GetState() == "Busy" && enemyState == State.Inactive)
        {
            enemyPos = this.transform.position + charOffset;
            playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position;
            force = new Vector2(Mathf.Clamp(playerPos.x - enemyPos.x, minPower.x, maxPower.x), Mathf.Clamp(playerPos.y - enemyPos.y, minPower.y, maxPower.y));
            enemyBody.AddForce(force * power, ForceMode2D.Impulse);
            this.enemyState = State.Active;
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
                gameHandler.doDamage(Damage(), false);
            }
        }
    }

    private int Damage()
    {
        return (int)enemyBody.velocity.magnitude * 5;
    }

    private void FixedUpdate()
    {
        if (this.enemyBody.velocity.magnitude > 0)
        {
            if (this.enemyBody.velocity.magnitude <= 0.2f)
            {
                this.enemyBody.velocity = Vector2.zero;
            }
        }
    }

    private void Update()
    {
        if (this.enemyState == State.Active)
        {
            if (this.enemyBody.velocity == Vector2.zero)
            {
                this.enemyState = State.Inactive;
                battleSystem.NextActive();
            }
        }
    }
}