using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattle : MonoBehaviour
{
    [SerializeField] private float power;
    [SerializeField] private Vector2 minPower, maxPower;
    [SerializeField] private Vector3 charOffset;
    private Vector2 force, minVel;
    private Vector3 lineStart, lineEnd, dir;
    private Vector3 camOffset = new Vector3(0, 0, 10);
    private int initiative, playerNum;
    private Rigidbody2D playerBody;
    private TrajectoryLineV2 trajectoryLine;
    private BattleHandler battleSystem;
    private GameHandler gameHandler;
    private float dist;
    private string state;
    private State playerState;
    private enum State
    {
        Active, Inactive
    }

    private void Start()
    {
        playerState = State.Inactive;
        trajectoryLine = GetComponent<TrajectoryLineV2>();
        playerBody = GetComponent<Rigidbody2D>();
        battleSystem = GameObject.FindGameObjectWithTag("BattleSystem").GetComponent<BattleHandler>();
        gameHandler = GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>();
    }

    private void OnMouseDrag()
    {
        if (battleSystem.GetState() == "Waiting" && playerState == State.Inactive)
        {
            lineStart = this.transform.position + charOffset;
            Vector3 currentPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition) + camOffset;
            dir = currentPoint - lineStart;
            dist = Vector3.Distance(currentPoint, lineStart);
            lineEnd = lineStart + dir.normalized * dist * -1;

            trajectoryLine.RenderLine(lineStart, lineEnd);
        }
    }

    private void OnMouseUp()
    {
        if (battleSystem.GetState() == "Waiting" && playerState == State.Inactive)
        {
            lineEnd = transform.position + dir.normalized * dist;
            force = new Vector2(Mathf.Clamp(lineStart.x - lineEnd.x, minPower.x, maxPower.x), Mathf.Clamp(lineStart.y - lineEnd.y, minPower.y, maxPower.y));
            playerBody.AddForce(force * power, ForceMode2D.Impulse);
            trajectoryLine.EndLine();
            playerState = State.Active;
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

                gameHandler.doDamage(Damage(), true, other.GetComponent<EnemyBattle>().GetEnemyNum());
            }
        }
    }

    private int Damage()
    {
        return (int)playerBody.velocity.magnitude * 5;
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

    private void FixedUpdate()
    {
        if (playerBody.velocity.magnitude > 0)
        {
            if (playerBody.velocity.magnitude < 0.2f)
            {
                playerBody.velocity = Vector2.zero;
            }
        }
    }

    private void Update()
    {
        if (playerState == State.Active)
        {
            if (playerBody.velocity == Vector2.zero)
            {
                playerState = State.Inactive;
                battleSystem.NextActive();
            }
        }
    }
}
