using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragNShootV2 : MonoBehaviour
{
    [SerializeField] private float power;
    [SerializeField] private Vector2 minPower, maxPower;
    [SerializeField] private Vector3 camOffset, charOffset;
    Rigidbody2D playerBody;
    TrajectoryLineV2 trajectoryLine;
    Vector2 force, minVel;
    Vector3 lineStart, lineEnd, dir;
    float dist;

    private void Start()
    {
        trajectoryLine = GetComponent<TrajectoryLineV2>();
        playerBody = GetComponent<Rigidbody2D>();
    }

    private void OnMouseDrag()
    {
        lineStart = this.transform.position + charOffset;
        Vector3 currentPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition) + camOffset;
        dir = currentPoint - lineStart;
        dist = Vector3.Distance(currentPoint, lineStart);
        lineEnd = lineStart + dir.normalized * dist * -1;

        trajectoryLine.RenderLine(lineStart, lineEnd);
    }

    private void OnMouseUp()
    {
        lineEnd = transform.position + dir.normalized * dist;
        force = new Vector2(Mathf.Clamp(transform.position.x - lineEnd.x, minPower.x, maxPower.x), Mathf.Clamp(transform.position.y - lineEnd.y, minPower.y, maxPower.y));
        GetComponent<Rigidbody2D>().AddForce(force * power, ForceMode2D.Impulse);
        trajectoryLine.EndLine();
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
        if (playerBody.velocity.magnitude > 0)
        {
            if (playerBody.velocity.magnitude <= 0.5f)
            {
                playerBody.velocity = Vector2.zero;
            }
        }

        Debug.Log(Stopped());
    }


}
