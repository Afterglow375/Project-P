using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauncherController : MonoBehaviour
{
    private Vector2 _shootDirection;
    private Vector3 _mousePos;
    private BallController _ballController;
    
    // Start is called before the first frame update
    void Start()
    {
        _ballController = transform.GetComponentInChildren<BallController>();
    }

    // Update is called once per frame
    void Update()
    {
        _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _shootDirection = (_mousePos - transform.position).normalized;

        if (!_ballController.isLaunched && !PauseMenu.isPaused)
        {
            transform.up = _shootDirection;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _ballController.Shoot(_shootDirection);
        }
    }
}
