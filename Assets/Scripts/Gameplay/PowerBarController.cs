using Managers;
using UnityEngine;
using UnityEngine.UI;

public class PowerBarController : MonoBehaviour
{
    [SerializeField]
    private Image _powerBar;
    private GameObject _powerBarCanvas;

    public float shotPowerModifier;
    public float scrollIncrement = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        _powerBarCanvas = GameObject.Find("PowerBarCanvas");
    }

    // Update is called once per frame
    void Update()
    {
        shotPowerModifier = _powerBar.fillAmount;

        if (GameManager.Instance.state == GameState.ReadyToShoot)
        {
            _powerBarCanvas.SetActive(true);

            if (Input.mouseScrollDelta.y > 0)
            {
                _powerBar.fillAmount += scrollIncrement;
            }
            else if (Input.mouseScrollDelta.y < 0)
            {
                _powerBar.fillAmount -= scrollIncrement;
            }
        }
        else
        {
            _powerBarCanvas.SetActive(false);
        }
    }
}
