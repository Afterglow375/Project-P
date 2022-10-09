using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.WSA;
using Utilities;

namespace UI
{
    public class LevelSelectText : MonoBehaviour
    {
        [SerializeField] private SceneLoader _sceneLoader;
        [SerializeField] private GameObject _previousLevel;
        private TextMeshProUGUI _text;
        private Color _origColor;
        private bool _colliding;
        private bool _isActive;
        private LineRenderer _line;
        private readonly Color _inactiveColor = new Color(0f, 0f, 0f, 0.6f);
        
        void Start()
        {
            _text = transform.GetComponentInChildren<TextMeshProUGUI>();
            _origColor = _text.color;
            _line = GetComponent<LineRenderer>();
            
            if (_previousLevel != null)
            {
                _line.SetPosition(0, transform.position);
                _line.SetPosition(1, _previousLevel.transform.position);
                _isActive = false;
                _text.alpha = 0.6f;
                _line.startColor = _inactiveColor;
                _line.endColor = _inactiveColor;
            }
            else
            {
                _isActive = true;
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (_isActive && col.gameObject.CompareTag("Player"))
            {
                _text.color = Color.white;
                _colliding = true;
            }
        }
    
        private void OnTriggerExit2D(Collider2D col)
        {
            if (_isActive && col.gameObject.CompareTag("Player"))
            {
                _text.color = _origColor;
                _colliding = false;
            }
        }

        private void Update()
        {
            if (_isActive && _colliding)
            {
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    GameManager.Instance.SaveRubyPosition(transform.position);
                    _sceneLoader.LoadScene(_text.text);
                }
            }
        }

        public void Activate()
        {
            _isActive = true;
            _text.alpha = 1f;
            _line.startColor = Color.black;
            _line.endColor = Color.black;
        }
    }
}
