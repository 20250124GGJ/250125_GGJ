using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.Instance.AddScoreToRedPlayer(10);

            Debug.Log("Red : " + GameManager.Instance.GetRedPlayerScore);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            GameManager.Instance.ResetGame();

            Debug.Log("Reset : " + GameManager.Instance.GetRedPlayerScore);
        }
    }
}
