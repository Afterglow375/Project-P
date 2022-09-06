using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    int points = 0;
    public int force = 50;
    public Camera mainCam;

    private Rigidbody2D _body;

    // Start is called before the first frame update
    void Start()
    {
        _body = GetComponent<Rigidbody2D>();
        _body.constraints = RigidbodyConstraints2D.FreezePosition;
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
        _body.constraints = RigidbodyConstraints2D.None;
        _body.AddForce(shootDirection.normalized * force);
    }
}
