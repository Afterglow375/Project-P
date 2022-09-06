using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallWithAimController : MonoBehaviour
{
    private Vector2 shootDirection;
    private Vector3 mousePos;
    private bool isLaunched = false; 

    public Camera mainCam;
    public BallController ball;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        shootDirection = (mousePos - transform.position).normalized;

        if (!isLaunched)
        {
            transform.up = shootDirection;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isLaunched = true;
            ball.Shoot(shootDirection);
        }
    }
}
