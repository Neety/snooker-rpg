using System;
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
    private Vector3 camOffset = new Vector3(0, 0, 9);

    private int startInitiative, currIntiative, playerNum;
    private Rigidbody2D playerBody;
    private TrajectoryLineV2 trajectoryLine;
    private BattleHandler battleSystem;
    private MoveToActive activeEntity;
    private GameHandler gameHandler;
    private float dist;
    private string state;
    private bool active, hit, enter, start, dead;
    public event EventHandler triggerNextTurn;

    private void Start()
    {
        this.hit = false;
        this.enter = false;
        this.trajectoryLine = GetComponent<TrajectoryLineV2>();
        this.playerBody = GetComponent<Rigidbody2D>();
        battleSystem = GameObject.FindGameObjectWithTag("BattleSystem").GetComponent<BattleHandler>();
        activeEntity = GameObject.FindGameObjectWithTag("Active").GetComponent<MoveToActive>();
    }

    private void OnMouseDrag()
    {
        if (this.active == true && this.hit == false && battleSystem.GetActive() == "Player")
        {
            this.lineStart = this.transform.position + this.charOffset;
            this.currentPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition) + camOffset;
            this.dir = this.lineStart - this.currentPoint;
            this.dist = Vector3.Distance(this.currentPoint, this.lineStart);
            this.lineEnd = this.lineStart + this.dir.normalized * this.dist;
            this.trajectoryLine.RenderLine(this.lineStart, this.lineEnd);
        }
    }
    private void OnMouseUp()
    {
        if (this.active == true && this.hit == false && battleSystem.GetActive() == "Player")
        {
            // this.lineStart = this.transform.position + this.charOffset;
            // this.currentPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition) + camOffset;
            // this.dir = this.lineStart - this.currentPoint;
            this.playerBody.AddForce(this.dir.normalized * power, ForceMode2D.Impulse);
            this.trajectoryLine.EndLine();
            Debug.Log("Current Point: " + this.currentPoint + " Line Start: " + this.lineStart);
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
                    battleSystem.doDamage(Damage(), true, other.gameObject.GetComponent<EnemyBattle>().GetEnemyNum(), other.gameObject.transform.Find("HealthBar").transform.position);
                    Debug.Log(Damage());
                    Instantiate(battleSystem.pfImpact, other.transform.position, Quaternion.identity);
                }
            }


        }
    }

    public void SetDead()
    {
        this.dead = true;
    }

    private int Damage()
    {
        return (int)Mathf.Ceil(this.playerBody.velocity.magnitude);
    }

    public int GetInitiative()
    {
        return this.currIntiative;
    }
    public void SetInitiative(int init, bool start)
    {
        if (start == true)
        {
            this.startInitiative = init;
            this.currIntiative = this.startInitiative;
        }
        else
            this.currIntiative = init;
    }
    public int GetPlayerNum()
    {
        return this.playerNum;
    }
    public void SetPlayerNum(int num)
    {
        this.playerNum = num;
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

    public bool GetActive()
    {
        return this.active;
    }

    private IEnumerator TurnDelay()
    {
        this.enter = true;
        yield return new WaitForSeconds(1f);
        this.transform.Find("PlayerSelect").gameObject.SetActive(false);
        battleSystem.OrderList(false);
        triggerNextTurn?.Invoke(this, EventArgs.Empty);
        this.enter = false;
    }

    private void FixedUpdate()
    {
        if (this.active == true)
        {
            if (this.hit == true)
            {
                activeEntity.moving = true;

                if (this.playerBody.velocity.magnitude < 0.2f)
                {
                    this.playerBody.velocity = Vector3.zero;

                    activeEntity.moving = false;

                    if (this.dead == true)
                    {
                        // StartCoroutine(battleSystem.TurnDelay(2f));
                        this.gameObject.SetActive(false);
                        battleSystem.DestroyEntity<PlayerBattle>(GetPlayerNum());
                        battleSystem.OrderList(false);
                        triggerNextTurn?.Invoke(this, EventArgs.Empty);
                    }
                    else
                    {
                        this.active = false;
                        this.hit = false;
                        this.currIntiative = this.startInitiative;
                        if (this.enter == false) StartCoroutine(TurnDelay());
                    }
                }
            }
        }
        else
        {
            if (this.playerBody.velocity.magnitude < 0.2f)
            {
                if (this.dead == true)
                {
                    this.playerBody.velocity = Vector3.zero;
                    this.gameObject.SetActive(false);
                    battleSystem.DestroyEntity<PlayerBattle>(GetPlayerNum());
                    battleSystem.OrderList(false);
                }
            }
        }
    }
}
