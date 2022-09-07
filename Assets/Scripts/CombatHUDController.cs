using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatHUDController : MonoBehaviour
{
    private TextMeshProUGUI _playerHealth;
    private TextMeshProUGUI _pegScore;
    private TextMeshProUGUI _enemyHealth;

    public float playerCurrentHealth;
    public float playerMaxHealth;
    private string _pegScoreString;
    public float enemyCurrentHealth;
    public float enemyMaxHealth;
    
    void Start()
    {
        _playerHealth = transform.Find("PlayerTextParent/PlayerText").GetComponent<TextMeshProUGUI>();
        _pegScore = transform.Find("PegScoreTextParent/PegScoreText").GetComponent<TextMeshProUGUI>();
        _enemyHealth = transform.Find("EnemyTextParent/EnemyText").GetComponent<TextMeshProUGUI>();
        UpdatePlayerHeaalthText();
        UpdateEnemyHeaalthText();
        UpdatePegScoreText();
    }

    public void UpdatePlayerHeaalthText(int damage = 0)
    {
        playerCurrentHealth += damage;
        _playerHealth.text = "Player health: " + playerCurrentHealth + "/" + playerMaxHealth;
    }
    
    public void UpdateEnemyHeaalthText(int damage = 0)
    {
        enemyCurrentHealth += damage;
        _enemyHealth.text = "Enemy health: " + enemyCurrentHealth + "/" + enemyMaxHealth;
    }

    public void UpdatePegScoreText(int pegScore = 0)
    {
        _pegScore.text = "Peg score: " + pegScore;
    }
}
