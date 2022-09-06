using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauncherController : MonoBehaviour
{
    private Vector2 _shootDirection;
    private Vector3 _mousePos;
    private bool _isLaunched = false;
    private BallController _ballController;

    public Camera mainCam;

    // Start is called before the first frame update
    void Start()
    {
        _ballController = transform.GetComponentInChildren<BallController>();
    }

    // Update is called once per frame
    void Update()
    {
        _mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        _shootDirection = (_mousePos - transform.position).normalized;

        if (!_isLaunched)
        {
            transform.up = _shootDirection;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isLaunched = true;
            _ballController.Shoot(_shootDirection);
        }
    }
}
