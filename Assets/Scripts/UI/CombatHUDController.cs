using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay;
using Managers;
using TMPro;
using UnityEngine;

namespace UI
{
    public class CombatHUDController : MonoBehaviour
    {
        private TextMeshProUGUI _playerHealthTextMesh;
        private TextMeshProUGUI _pegScoreTextMesh;
        private TextMeshProUGUI _enemyHealthTextMesh;
        private int _playerMaxHp;
        private int _enemyMaxHp;
        private Coroutine _textAnimationCoroutine;
        private Color _origTextColor;
        private readonly Color _flashingTurnColor = new Color(0.1f, .25f, 1f);

        private void Awake()
        {
            CombatManager.PegScoreUpdateEvent += UpdatePegScoreText;
            CombatManager.PegBonusEvent += OnPegBonusEvent;
            CombatManager.PlayerHealthChangeEvent += OnPlayerHealthChange;
            CombatManager.EnemyHealthChangeEvent += OnEnemyHealthChange;
            CombatManager.PlayerTurnStartEvent += OnPlayerTurnStart;
            CombatManager.PlayerTurnEndEvent += OnPlayerTurnEnd;
            CombatManager.EnemyTurnStartEvent += OnEnemyTurnStart;
            CombatManager.EnemyTurnEndEvent += OnEnemyTurnEnd;
        }

        private void OnDestroy()
        {
            CombatManager.PegScoreUpdateEvent -= UpdatePegScoreText;
            CombatManager.PegBonusEvent -= OnPegBonusEvent;
            CombatManager.PlayerHealthChangeEvent -= OnPlayerHealthChange;
            CombatManager.EnemyHealthChangeEvent -= OnEnemyHealthChange;
            CombatManager.PlayerTurnStartEvent -= OnPlayerTurnStart;
            CombatManager.PlayerTurnEndEvent -= OnPlayerTurnEnd;
            CombatManager.EnemyTurnStartEvent -= OnEnemyTurnStart;
            CombatManager.EnemyTurnEndEvent -= OnEnemyTurnEnd;
        }

        void Start()
        {
            _playerHealthTextMesh = transform.Find("PlayerTextParent/PlayerText").GetComponent<TextMeshProUGUI>();
            _pegScoreTextMesh = transform.Find("PegScoreTextParent/PegScoreText").GetComponent<TextMeshProUGUI>();
            _enemyHealthTextMesh = transform.Find("EnemyTextParent/EnemyText").GetComponent<TextMeshProUGUI>();
            _origTextColor = _playerHealthTextMesh.color;
            _playerMaxHp = CombatManager.Instance.GetMaxPlayerHp();
            _enemyMaxHp = CombatManager.Instance.GetMaxEnemyHp();
            UpdatePlayerHealthText(_playerMaxHp);
            UpdateEnemyHealthText(_enemyMaxHp);
            UpdatePegScoreText();
        }

        private void UpdatePegScoreText(int pegScore = 0)
        {
            _pegScoreTextMesh.text = $"Peg score: {pegScore}";
        }
        
        private void OnPegBonusEvent(int pegScore)
        {
            UpdatePegScoreText(pegScore);
        }

        private void UpdatePlayerHealthText(int hp)
        {
            _playerHealthTextMesh.text = $"Player health: {hp}/{_playerMaxHp}";
        }

        private void UpdateEnemyHealthText(int hp)
        {
            _enemyHealthTextMesh.text = $"Enemy health: {hp}/{_enemyMaxHp}";
        }

        private void OnPlayerHealthChange(int hp)
        {
            UpdatePlayerHealthText(hp);
        }
        
        private void OnEnemyHealthChange(int hp)
        {
            UpdateEnemyHealthText(hp);
        }
        
        private void OnPlayerTurnStart()
        {
            StartTurnTextAnimation(_playerHealthTextMesh);
        }

        private void OnPlayerTurnEnd()
        {
            EndTurnTextAnimation(_playerHealthTextMesh);
        }

        private void OnEnemyTurnStart()
        {
            StartTurnTextAnimation(_enemyHealthTextMesh);
        }

        private void OnEnemyTurnEnd()
        {
            EndTurnTextAnimation(_enemyHealthTextMesh);
        }

        private void StartTurnTextAnimation(TextMeshProUGUI textMesh)
        {
            _textAnimationCoroutine = StartCoroutine(AnimateTurnText(textMesh));
            textMesh.fontSize += 10;
        }

        private void EndTurnTextAnimation(TextMeshProUGUI textMesh)
        {
            StopCoroutine(_textAnimationCoroutine);
            textMesh.color = _origTextColor;
            textMesh.fontSize -= 10;
        }

        // flashes text color using lerp with sine wave as time
        private IEnumerator AnimateTurnText(TextMeshProUGUI textMesh)
        {
            float timeAnimating = 0f;
            float shiftPhase = Mathf.PI * 1.5f;
            while (true)
            {
                float speed = 4f;
                float t = (Mathf.Sin(Mathf.PI * timeAnimating * speed + shiftPhase) + 1) / 2.0f;
                timeAnimating += Time.deltaTime;
                textMesh.color = Color.Lerp(Color.clear, _flashingTurnColor, t);
                yield return null;
            }
        }
    }
}
