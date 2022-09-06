using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    int points = 0;
    public int force = 50;
    public Camera mainCam;
    public bool isLaunched = false;

    private Rigidbody2D _body;
    private Vector2 _startPos;

    // Start is called before the first frame update
    void Start()
    {
        _body = GetComponent<Rigidbody2D>();
        _body.constraints = RigidbodyConstraints2D.FreezePosition;
        _startPos = _body.position;
    }

    private void OnCollisionEnter2D(Collision2D collision) // Hit peg
    {
        PegController peg = collision.gameObject.GetComponent<PegController>();

        if (peg != null)
        {
            points += peg.points;
            Debug.Log(points);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) // Hit reset trigger
    {
        if (collision.gameObject.tag == "Respawn")
        {
            ResetPos();
        }
    }

    public void Shoot(Vector2 shootDirection)
    {
        if (!isLaunched)
        {
            _body.simulated = true;
            _body.constraints = RigidbodyConstraints2D.None;
            _body.AddForce(shootDirection.normalized * force);
            isLaunched = true;
        }
    }
    private void ResetPos()
    {
        _body.velocity = Vector2.zero;
        _body.transform.position = _startPos;
        _body.simulated = false;
        isLaunched = false;
    }
}
