using Cinemachine;
using Gameplay.BallArena;
using Managers;
using UnityEngine;

namespace UI.BallArena
{
    public class CinemachineBrainController : MonoBehaviour
    {
        [SerializeField] private CinemachineBrain _brain;
        [SerializeField] private BallTrackingCameraController _ballTrackingCameraController;
        [SerializeField] private BallOriginCameraController _ballOriginCameraController;
        private bool _blendStarted;
        
        private void Update()
        {
            if (GameManager.Instance.state == GameState.ResettingBall)
            {
                if (!_brain.IsBlending)
                {
                    if (_blendStarted)
                    {
                        Debug.Log("Finished resetting ball");
                        _ballOriginCameraController.gameObject.SetActive(false);
                        _ballTrackingCameraController.gameObject.SetActive(true);
                        _blendStarted = false;
                        _ballTrackingCameraController.ResetBallSettings();
                        GameManager.Instance.UpdateGameState(GameState.ReadyToShoot);
                    }
                    else
                    {
                        // start panning back to ball origin
                        _ballTrackingCameraController.gameObject.SetActive(false);
                        _ballOriginCameraController.gameObject.SetActive(true);
                    }
                }
                else if (!_blendStarted)
                {
                    _blendStarted = true;
                    float orthoSize = _ballTrackingCameraController.GetCamOrthoSize();
                    // set ortho size and posiition of ball origin cam to match zoom level of ball tracking cam 
                    _ballOriginCameraController.AdjustCameraPosition(orthoSize);
                }
            }
        }
    }
}