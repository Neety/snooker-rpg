using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattle : MonoBehaviour
{
    [SerializeField] private float power;
    [SerializeField] private Vector2 minPower, maxPower;
    [SerializeField] private Vector3 charOffset;
    private Transform select;
    private Vector2 force, minVel;
    private Vector3 currentPoint, lineStart, lineEnd, dir;
    private Vector3 camOffset = new Vector3(0, 0, 10);
    private int initiative, playerNum;
    private Rigidbody2D playerBody;
    private TrajectoryLineV2 trajectoryLine;
    private BattleHandler battleSystem;
    private GameHandler gameHandler;
    private float dist;
    private string state;
    private bool active, hit;

    private void Start()
    {
        this.hit = false;
        this.trajectoryLine = GetComponent<TrajectoryLineV2>();
        this.playerBody = GetComponent<Rigidbody2D>();
        battleSystem = GameObject.FindGameObjectWithTag("BattleSystem").GetComponent<BattleHandler>();
        gameHandler = GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>();
    }

    private void OnMouseDrag()
    {
        if (this.active == true && this.hit == false && battleSystem.GetActive() == "Player")
        {
            this.lineStart = this.transform.position + this.charOffset;
            this.currentPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition) + camOffset;
            this.dir = this.currentPoint - this.lineStart;
            this.dist = Vector3.Distance(this.currentPoint, this.lineStart);
            this.lineEnd = this.lineStart + this.dir.normalized * this.dist * -1;
            this.trajectoryLine.RenderLine(this.lineStart, this.lineEnd);
        }
    }

    private void OnMouseUp()
    {
        if (this.active == true && this.hit == false && battleSystem.GetActive() == "Player")
        {
            this.lineEnd = this.transform.position + this.dir.normalized * this.dist;
            this.force = new Vector2(Mathf.Clamp(this.lineStart.x - this.lineEnd.x, this.minPower.x, this.maxPower.x), Mathf.Clamp(this.lineStart.y - this.lineEnd.y, this.minPower.y, this.maxPower.y));
            this.playerBody.AddForce(this.force * power, ForceMode2D.Impulse);
            this.trajectoryLine.EndLine();
            this.hit = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        ProcessCollision(other.gameObject);
    }

    private void ProcessCollision(GameObject other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (battleSystem.GetActive() == "Player")
            {
                if (this.active == true)
                {
                    battleSystem.doDamage(Damage(), true, other.gameObject.GetComponent<EnemyBattle>().GetEnemyNum(), other.gameObject.GetComponent<Transform>().position);

                }
            }
        }
    }

    private int Damage()
    {
        return (int)Mathf.Ceil(this.playerBody.velocity.magnitude) * 5;
    }

    public int GetInitiative()
    {
        return this.initiative;
    }
    public void SetInitiative(int init)
    {
        this.initiative = init;
    }
    public int GetPlayerNum()
    {
        return this.playerNum;
    }
    public void SetPlayerNum(int num)
    {
        this.playerNum = num;
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
                if (this.playerBody.velocity.magnitude < 0.2f)
                {
                    this.playerBody.velocity = Vector3.zero;
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
