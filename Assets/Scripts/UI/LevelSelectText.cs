using Managers;
using TMPro;
using UnityEngine;
using Utilities;

namespace UI
{
    public class LevelSelectText : MonoBehaviour
    {
        [SerializeField] private SceneLoader _sceneLoader;
        private TextMeshProUGUI _text;
        private Color _origColor;
        private bool _colliding;
    
        void Start()
        {
            _text = transform.GetComponentInChildren<TextMeshProUGUI>();
            _origColor = _text.color;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                _text.color = Color.white;
                _colliding = true;
            }
        }
    
        private void OnTriggerExit2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                _text.color = _origColor;
                _colliding = false;
            }
        }

        private void Update()
        {
            if (_colliding)
            {
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    GameManager.Instance.SaveRubyPosition(transform.position);
                    _sceneLoader.LoadScene(Scenes.Playground);
                }
            }
        }
    }
}
