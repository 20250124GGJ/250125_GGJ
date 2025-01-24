using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameObject Bottle;

    public void SetBottle(GameObject InBottle)
    {
        Bottle = InBottle;
    }

    // Update is called once per frame
    private void Start()
    {
        GameManager.Instance.SetPlayerController(this);
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Check : " + Bottle.ToString());
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            GameManager.Instance.NextTurn();

            Debug.Log("Reset : " + GameManager.Instance.GetRedPlayerScore);
        }
    }
}
