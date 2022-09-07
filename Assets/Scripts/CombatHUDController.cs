using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatHUDController : MonoBehaviour
{
    public float playerCurrentHealth;
    public float playerMaxHealth;
    public float enemyCurrentHealth;
    public float enemyMaxHealth;
    
    private TextMeshProUGUI _playerHealth;
    private TextMeshProUGUI _pegScore;
    private TextMeshProUGUI _enemyHealth;
    private string _pegScoreString;
    
    void Start()
    {
        _playerHealth = transform.Find("PlayerTextParent/PlayerText").GetComponent<TextMeshProUGUI>();
        _pegScore = transform.Find("PegScoreTextParent/PegScoreText").GetComponent<TextMeshProUGUI>();
        _enemyHealth = transform.Find("EnemyTextParent/EnemyText").GetComponent<TextMeshProUGUI>();
        UpdatePlayerHealthText();
        UpdateEnemyHealthText();
        UpdatePegScoreText();
    }

    public void UpdatePlayerHealthText(int damage = 0)
    {
        playerCurrentHealth += damage;
        _playerHealth.text = "Player health: " + playerCurrentHealth + "/" + playerMaxHealth;
    }
    
    public void UpdateEnemyHealthText(int damage = 0)
    {
        enemyCurrentHealth += damage;
        _enemyHealth.text = "Enemy health: " + enemyCurrentHealth + "/" + enemyMaxHealth;
    }

    public void UpdatePegScoreText(int pegScore = 0)
    {
        _pegScore.text = "Peg score: " + pegScore;
    }
}
