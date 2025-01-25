using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : MonoBehaviour
{
    public int team;

    private Rigidbody rb;

    public void AttackBottle(float InPower)
    {
        Vector3 fwd = transform.forward;
        fwd.Normalize();
        Debug.Log(fwd);

        rb.AddForce(fwd * InPower, ForceMode.Impulse);
    }

    public Vector3 GetForwardVector()
    {
        return transform.forward;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
}
