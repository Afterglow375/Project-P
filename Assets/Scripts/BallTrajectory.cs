using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTrajectory : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    private Rigidbody2D _ballRb;
    private BallController _ballController;
    private float _force;
    private float _mass;
    private float _vel;

    public float maxDuration;
    public float timeStepInterval;

    // Start is called before the first frame update
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _ballController = GetComponentInChildren<BallController>();
        _ballRb = GetComponentInChildren<Rigidbody2D>();

        _mass = _ballRb.mass;
        _force = _ballController.force;
        _vel = _force / _mass * Time.fixedDeltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_ballController.isLaunched)
        {
            DrawTrajectory();
        }
    }

    private void DrawTrajectory()
    {
        _lineRenderer.positionCount = SimulateArc().Count;
        for (int a = 0; a < _lineRenderer.positionCount; a++)
        {
            _lineRenderer.SetPosition(a, SimulateArc()[a]); //Add each Calculated Step to a LineRenderer to display a Trajectory. Look inside LineRenderer in Unity to see exact points and amount of them
        }
    }

    private List<Vector2> SimulateArc() //A method happening via this List
    {
        List<Vector2> lineRendererPoints = new List<Vector2>(); //Reset LineRenderer List for new calculation

        int maxSteps = (int)(maxDuration / timeStepInterval);//Calculates amount of steps simulation will iterate for
        Vector2 directionVector = transform.up; //INPUT launch direction (This Vector2 is automatically normalized for us, keeping it in low and communicable terms)
        Vector2 launchPosition = transform.position; //INPUT launch origin (Important to make sure RayCast is ignoring some layers (easiest to use default Layer 2))

        _vel = _force / _mass * Time.fixedDeltaTime; //Initial Velocity, or Velocity Modifier, with which to calculate Vector Velocity

        for (int i = 0; i < maxSteps; ++i)
        {
            //Remember f(t) = (x0 + x*t, y0 + y*t - 9.81t²/2)
            //calculatedPosition = Origin + (transform.up * (speed * which step * the length of a step);
            Vector2 calculatedPosition = launchPosition + directionVector * _vel * i * timeStepInterval; //Move both X and Y at a constant speed per Interval
            calculatedPosition.y += Physics2D.gravity.y / 2 * Mathf.Pow(i * timeStepInterval, 2); //Subtract Gravity from Y

            lineRendererPoints.Add(calculatedPosition); //Add this to the next entry on the list
        }
        return lineRendererPoints;
    }
}
