using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    int points = 0;
    public int force = 50;
    public Camera mainCam;
    private Vector3 startPos;
    private Vector3 mousePos;
    private Rigidbody2D body;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        startPos = body.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PegController peg = collision.gameObject.GetComponent<PegController>();

        if (peg != null)
        {
            points += peg.points;
            Debug.Log(points);
        }
    }

    private void Shoot()
    {
        body.simulated = true;
        Vector2 shootDirection = (mousePos - body.transform.position);
        body.AddForce(shootDirection * force);
    }
}
