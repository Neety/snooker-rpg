using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TrajectoryLine : MonoBehaviour
{
    public LineRenderer lr;

    [SerializeField] private Color startColor = new Color(0f, 1f, .5f, 1f);
    [SerializeField] private Color endColor = new Color(0f, 1f, .5f, 1f);


    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    public void RenderLine(Vector3 startPoint, Vector3 endPoint)
    {
        lr.positionCount = 2;
        Vector3[] points = new Vector3[2];
        points[0] = startPoint;
        points[1] = Vector3.MoveTowards(startPoint, endPoint, 3f);
        lr.SetPositions(points);

        float fade = Vector2.Distance(startPoint, endPoint) / 100 * 15;

        lr.startColor = new Color(0f, fade + 1 - (fade * 2), .5f, 1f);
        lr.endColor = new Color(1f, fade + 1 - (fade * 2), 0f, 1f);
    }

    public void EndLine()
    {
        lr.positionCount = 0;
    }
}