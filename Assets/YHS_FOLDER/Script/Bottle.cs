using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : MonoBehaviour
{
    public int team;
    //Blue Red
    public Material[] OutLineMaterials;

    private Rigidbody rb;

    public void AttachmentOutline()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

        if(meshRenderer)
        {
            Material[] sharedMaterials = meshRenderer.sharedMaterials;

            // ���ο� �迭 ���� (���� �迭 + 1 ũ��)
            Material[] updatedMaterials = new Material[sharedMaterials.Length + 1];

            // ���� Material �迭 ����
            for (int i = 0; i < sharedMaterials.Length; i++)
            {
                updatedMaterials[i] = sharedMaterials[i];
            }

            // �� Material �߰�
            updatedMaterials[sharedMaterials.Length] = OutLineMaterials[team - 1];

            // MeshRenderer�� Material �迭 ������Ʈ
            meshRenderer.sharedMaterials = updatedMaterials;
        }
    }

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
