using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public int force = 50;
    public bool isLaunched = false;
    public GameObject trail;
    public int minEnemyDamage = 0;
    public int maxEnemyDamage = 30;
    
    private int _pegScore;
    private int _pegCount;
    private Rigidbody2D _body;
    private Vector2 _startPos;
    private TrailRenderer _trailRenderer;
    private CombatHUDController _combatHUD;

    // Start is called before the first frame update
    void Start()
    {
        _body = GetComponent<Rigidbody2D>();
        _trailRenderer = trail.GetComponent<TrailRenderer>();
        _combatHUD = GameObject.Find("HUD Parent").GetComponent<CombatHUDController>();
        _body.constraints = RigidbodyConstraints2D.FreezePosition;
        _startPos = _body.position;
    }

    private void OnCollisionEnter2D(Collision2D collision) // Hit peg
    {
        PegController peg = collision.gameObject.GetComponent<PegController>();

        if (peg != null)
        {
            // bonus 10 points for every 5 pegs hit in 1 shot
            _pegCount++;
            if (_pegCount == 5)
            {
                _pegScore += 10;
                _pegCount = 0;
            }
            
            _pegScore += peg.points;
            _combatHUD.UpdatePegScoreText(_pegScore);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) // Hit reset trigger
    {
        if (collision.gameObject.CompareTag("Respawn"))
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
            _combatHUD.UpdatePegScoreText();
            _pegScore = 0;
            _pegCount = 0;
        }
    }
    private void ResetPos()
    {
        _body.velocity = Vector2.zero;
        _body.transform.position = _startPos;
        _body.simulated = false;
        isLaunched = false;
        _trailRenderer.Clear();
        int enemyDamage = Random.Range(minEnemyDamage, maxEnemyDamage+1);
        _combatHUD.UpdatePlayerHealthText(-enemyDamage);
        _combatHUD.UpdateEnemyHealthText(-_pegScore);
    }
}