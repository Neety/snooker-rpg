using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBattle : MonoBehaviour
{
    [SerializeField] private float power;
    [SerializeField] private Vector2 minPower, maxPower;
    [SerializeField] private Vector3 charOffset;
    private Vector2 force, minVel;
    private Vector3 lineStart, lineEnd, dir;
    private Vector3 camOffset = new Vector3(0, 0, 10);
    private Rigidbody2D playerBody;
    private TrajectoryLineV2 trajectoryLine;
    private BattleSystemCMonkey battleSystem;
    private float dist;
    private string state;

    private void Start()
    {
        trajectoryLine = GetComponent<TrajectoryLineV2>();
        playerBody = GetComponent<Rigidbody2D>();
        battleSystem = GameObject.FindGameObjectWithTag("BattleSystem").GetComponent<BattleSystemCMonkey>();
    }

    private void OnMouseDrag()
    {
        if (state == "Waiting")
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
        if (state == "Waiting")
        {
            lineEnd = transform.position + dir.normalized * dist;
            force = new Vector2(Mathf.Clamp(transform.position.x - lineEnd.x, minPower.x, maxPower.x), Mathf.Clamp(transform.position.y - lineEnd.y, minPower.y, maxPower.y));
            GetComponent<Rigidbody2D>().AddForce(force * power, ForceMode2D.Impulse);
            trajectoryLine.EndLine();
        }
    }

    public bool Stopped()
    {
        if (playerBody.velocity.magnitude == 0)
        {
            return true;
        }
        return false;
    }

    private void Update()
    {
        state = battleSystem.GetState();

        if (playerBody.velocity.magnitude > 0)
        {
            if (playerBody.velocity.magnitude <= 0.2f)
            {
                playerBody.velocity = Vector2.zero;
            }
        }
    }
}
