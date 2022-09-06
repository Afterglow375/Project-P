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
    private Vector2 shootDirection;
    private Rigidbody2D body;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        body.constraints = RigidbodyConstraints2D.FreezePosition;
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public void Shoot(Vector2 shootDirection)
    {
        body.constraints = RigidbodyConstraints2D.None;
        body.AddForce(shootDirection * force);
    }
}
