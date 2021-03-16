using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TrajectoryLineV2 : MonoBehaviour
{
    LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;
    }

    public void RenderLine(Vector3 startPoint, Vector3 endPoint)
    {
        lineRenderer.positionCount = 2;
        Vector3[] points = new Vector3[2];
        points[0] = startPoint;
        points[1] = Vector3.MoveTowards(startPoint, endPoint, 3f);
        lineRenderer.SetPositions(points);

        float fade = Mathf.Clamp(Vector2.Distance(startPoint, endPoint), 0f, 3f) / 100 * 7;

        lineRenderer.startColor = new Color(1f - fade, fade + 1 - (fade * 1), 0f + (fade / 1.5f), 1f);
        lineRenderer.endColor = new Color(1f, fade + 1 - (fade * 4.5f), 0f, 1f);
    }

    public void EndLine()
    {
        lineRenderer.positionCount = 0;
    }
}
