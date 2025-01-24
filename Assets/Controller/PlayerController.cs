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
            if(Bottle)
            {
                Bottle.GetComponent<Bottle>().AttackBottle(1.0f);   
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            GameManager.Instance.NextTurn();
        }
    }
}
