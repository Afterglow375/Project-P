using Managers;
using TMPro;
using UnityEngine;
using Utilities;

namespace UI
{
    public class LevelSelectText : MonoBehaviour
    {
        [SerializeField] private GameObject[] _nextLevelObjects;
        [SerializeField] private bool _firstLvl;
        [SerializeField] private bool _lastLvl;
        [SerializeField] private SceneLoader _sceneLoader;
        private LevelStatus _status;
        private TextMeshProUGUI _text;
        private string _lvlName;
        private Color _origColor;
        private bool _isPlayerColliding;
        private LineRenderer[] _lines;
        private readonly Color _accessibleColor = Color.black;
        private readonly Color _inaccessibleColor = new Color(0f, 0f, 0f, 0.6f);
        private readonly Color _completedColor = Color.green;
        private readonly Color _hightlightColor = Color.white;

        private void Awake()
        {
            _text = transform.GetComponentInChildren<TextMeshProUGUI>();
            _lvlName = _text.text;
            // on first overworld load, create Level objects
            if (!GameManager.Instance.LevelsInitialized())
            {
                
                _status = _firstLvl ? LevelStatus.Accessible : LevelStatus.Inaccessible;
                Level lvl = new Level(_lvlName, _status, _firstLvl, _lastLvl, _nextLevelObjects);
                GameManager.Instance.AddLevel(lvl);
            }
            else
            {
                _status = GameManager.Instance.GetLevelStatus(_lvlName);
            }
        }

        void Start()
        {
            // compute level colors from the status
            Color lineColor = _inaccessibleColor;
            Color textColor = _inaccessibleColor;
            if (_status == LevelStatus.Accessible)
            {
                textColor = _accessibleColor;
            }
            else if (_status == LevelStatus.Complete)
            {
                lineColor = _accessibleColor;
                textColor = _completedColor;
            }
            
            _text.color = textColor;
            _origColor = textColor;
            
            // create lines connecting to the next levels and color them
            GameObject linePrefab = Resources.Load<GameObject>("Prefabs/Overworld/Line");
            foreach (var nextLvl in _nextLevelObjects)
            {
                LineRenderer line = Instantiate(linePrefab, transform).GetComponent<LineRenderer>();
                Vector3 colliderCenter1 = transform.position;
                Vector3 colliderCenter2 = nextLvl.gameObject.transform.position;
                Vector3 firstPoint = Vector3.MoveTowards(colliderCenter1, colliderCenter2, 1f);
                Vector3 secondPoint = Vector3.MoveTowards(colliderCenter2,colliderCenter1, 1f);
                firstPoint.z = 1;
                secondPoint.z = 1;
                line.SetPosition(0, firstPoint);
                line.SetPosition(1, secondPoint);
                line.startColor = lineColor;
                line.endColor = lineColor;
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (IsLevelPlayable() && col.gameObject.CompareTag("Player"))
            {
                _text.color = _hightlightColor;
                _isPlayerColliding = true;
            }
        }
    
        private void OnTriggerExit2D(Collider2D col)
        {
            if (IsLevelPlayable() && col.gameObject.CompareTag("Player"))
            {
                _text.color = _origColor;
                _isPlayerColliding = false;
            }
        }

        private void Update()
        {
            if (IsLevelPlayable() && _isPlayerColliding)
            {
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    GameManager.Instance.SaveRubyPosition(transform.position);
                    _sceneLoader.LoadScene(_text.text);
                }
            }
        }

        private bool IsLevelPlayable()
        {
            return _status != LevelStatus.Inaccessible;
        }
    }
}
