using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PegController : MonoBehaviour
{
    public int points;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collided");
        gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
