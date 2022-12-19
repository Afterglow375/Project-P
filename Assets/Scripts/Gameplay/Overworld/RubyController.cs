using UnityEngine;

namespace Gameplay.Overworld
{
    public class RubyController : MonoBehaviour
    {
        private Animator _animator;
        private Vector2 _lookDirection = new Vector2(1,0);
        private Rigidbody2D _rigidbody2D;
        private float _horizontal;
        private float _vertical;

        void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        void Update()
        {
            _horizontal = Input.GetAxis("Horizontal");
            _vertical = Input.GetAxis("Vertical");
                
            Vector2 move = new Vector2(_horizontal, _vertical);
        
            if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
            {
                _lookDirection.Set(move.x, move.y);
                _lookDirection.Normalize();
            }
        
            _animator.SetFloat("Look X", _lookDirection.x);
            _animator.SetFloat("Look Y", _lookDirection.y);
            _animator.SetFloat("Speed", move.magnitude);
        }

        private void FixedUpdate()
        {
            Vector2 position = _rigidbody2D.position;
            position.x += 4.0f * _horizontal * Time.fixedDeltaTime;
            position.y += 4.0f * _vertical * Time.fixedDeltaTime;
            _rigidbody2D.MovePosition(position);
        }
    }
}
