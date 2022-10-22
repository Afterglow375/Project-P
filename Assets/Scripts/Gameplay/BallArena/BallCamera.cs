using Cinemachine;
using UnityEngine;

namespace Gameplay.BallArena
{
    public class BallCamera : MonoBehaviour
    {
        private CinemachineVirtualCamera _vCam;
        private void Start()
        {
            _vCam = GetComponent<CinemachineVirtualCamera>();
            _vCam.Follow = GameObject.FindWithTag("Ball").transform;
        }
    }
}