using UnityEngine;

namespace Gameplay
{
    public class RubyController : MonoBehaviour
    {
        public int maxHealth = 5;
        int currentHealth;
        Animator animator;
        Vector2 lookDirection = new Vector2(1,0);
    
        Rigidbody2D rigidbody2D;
        private float horizontal;
        private float vertical;

        // Start is called before the first frame update
        void Start()
        {
            rigidbody2D = GetComponent<Rigidbody2D>();
            currentHealth = maxHealth;
            animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
                
            Vector2 move = new Vector2(horizontal, vertical);
        
            if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
            {
                lookDirection.Set(move.x, move.y);
                lookDirection.Normalize();
            }
        
            animator.SetFloat("Look X", lookDirection.x);
            animator.SetFloat("Look Y", lookDirection.y);
            animator.SetFloat("Speed", move.magnitude);
        }

        private void FixedUpdate()
        {
            Vector2 position = rigidbody2D.position;
            position.x += 3.0f * horizontal * Time.deltaTime;
            position.y += 3.0f * vertical * Time.deltaTime;
            rigidbody2D.MovePosition(position);
        }
    }
}
