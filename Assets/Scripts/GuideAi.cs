using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideAi : MonoBehaviour {
    public LineRenderer lineRenderer;
    public Transform point0, point1, point2;

    public int numPoints = 50;
    public Vector3[] positions = new Vector3[50];
    public int index;
    public float vel;
    public bool startMove;
    public int count = 1;
    public Transform[] area1WayP;
    public Transform[,] pointsAreas = new Transform[4,3];
    public int curArea;
    private void Start()
    {
        lineRenderer.positionCount = numPoints;

        pointsAreas[0, 0] = point0;
        pointsAreas[0, 1] = point1;
        pointsAreas[0, 2] = point2;

        pointsAreas[1, 0] = area1WayP[0];
        pointsAreas[1, 1] = area1WayP[1];
        pointsAreas[1, 2] = area1WayP[2];

    }

    public void Update()
    {
        if(index == positions.Length && startMove)
        {
            startMove = false;
            index = 0;
        }
    }

    public void ChangeWaypoints(int x)
    {
        point0 = pointsAreas[x, 0];
        point1 = pointsAreas[x, 1];
        point2 = pointsAreas[x, 2];
        curArea = x;
        startMove = true;
    }

    private void LateUpdate()
    {
        if(startMove)
            move();

        DrawQuadraticCurve();
    }

    private void move()
    {
        if (index < positions.Length)
        {
            float range = Vector2.Distance(transform.position, positions[index]);

            if (range > 0)
                transform.position = Vector2.MoveTowards(transform.position, positions[index], vel * Time.deltaTime);

            else
                index++;
        }
    }

    private void DrawQuadraticCurve()
    {
        for (int i = 1; i < numPoints + 1; i++)
        {
            float t = i / (float)numPoints;
            positions[i - 1] = CalculateQuadraticBezierPoint(t, point0.position, point1.position, point2.position);
        }
        lineRenderer.SetPositions(positions);
    }

    private Vector3 CalculateLinearBezierPoint(float t, Vector3 p0, Vector3 p1)
    {
        return p0 + t * (p1 - p0);
    }

    private Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;

        return p;
    }
}
