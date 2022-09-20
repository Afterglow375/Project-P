using System;
using Managers;
using UnityEngine;

namespace Gameplay
{
    public class LauncherController : MonoBehaviour
    {
        private Vector2 _shootDirection;
        private Vector3 _mousePos;
        private Camera _camera;
        private BallController _ballController;
        
        private void Start()
        {
            
            _camera = Camera.main;
            _ballController = transform.GetComponentInChildren<BallController>();
        }

        void Update()
        {
            if (GameManager.Instance.state == GameState.ReadyToShoot)
            {
                
                _mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
                _shootDirection = (_mousePos - transform.position).normalized;
                transform.up = _shootDirection;
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    _ballController.Shoot(_shootDirection);
                }
            }
        }
    }
}
