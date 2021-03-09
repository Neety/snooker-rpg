using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragNShootV2 : MonoBehaviour
{
    [SerializeField] private float power = 5f;

    [SerializeField] private Vector2 minPower;
    [SerializeField] private Vector2 maxPower;
    Camera cam;
    TrajectoryLineV2 trajectoryLine;
    Vector2 force;
    Vector3 lineStart, lineEnd, dir;
    float dist;
    Vector3 camOffset = new Vector3(0, 0, 10);
    Vector3 charOffset = new Vector3(0, -.2f, 0);



    private void Start()
    {
        trajectoryLine = GetComponent<TrajectoryLineV2>();
        cam = Camera.main;

    }

    // private void OnMouseDown()
    // {
    //     startPoint = cam.ScreenToWorldPoint(Input.mousePosition);
    // }

    private void OnMouseDrag()
    {
        lineStart = this.transform.position + charOffset;
        Vector3 currentPoint = cam.ScreenToWorldPoint(Input.mousePosition) + camOffset;
        dir = currentPoint - lineStart;
        dist = Mathf.Clamp(Vector3.Distance(currentPoint, lineStart), 0, 3);
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
}
