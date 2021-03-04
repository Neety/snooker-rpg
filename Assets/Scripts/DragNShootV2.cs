using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragNShootV2 : MonoBehaviour
{
    [SerializeField] private float power = 10f;

    [SerializeField] private Vector2 minPower;
    [SerializeField] private Vector2 maxPower;
    Camera cam;

    TrajectoryLineV2 trajectoryLine;
    Vector2 force;
    Vector3 startPoint;
    Vector3 endPoint;

    private void Start()
    {
        trajectoryLine = GetComponent<TrajectoryLineV2>();
        cam = Camera.main;
    }
    private void OnMouseDown()
    {
        startPoint = cam.ScreenToWorldPoint(Input.mousePosition);
        startPoint.z = 15;
    }

    private void OnMouseDrag()
    {
        Vector3 currentPoint = cam.ScreenToWorldPoint(Input.mousePosition);
        float currDist = Vector3.Distance(currentPoint, transform.position);
        currentPoint.z = 15;
        Vector3 dimXY = currentPoint - transform.position;
        float diff = dimXY.magnitude;
        Vector3 lineStart = transform.position + ((dimXY / diff) * currDist * -1);

        trajectoryLine.RenderLine(lineStart, transform.position);
    }

    private void OnMouseUp()
    {
        endPoint = cam.ScreenToWorldPoint(Input.mousePosition);
        endPoint.z = 15;
        force = new Vector2(Mathf.Clamp(startPoint.x - endPoint.x, minPower.x, maxPower.x), Mathf.Clamp(startPoint.y - endPoint.y, minPower.y, maxPower.y));
        GetComponent<Rigidbody2D>().AddForce(force * power, ForceMode2D.Impulse);
        trajectoryLine.EndLine();
    }
}
