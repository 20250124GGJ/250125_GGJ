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

            // 새로운 배열 생성 (기존 배열 + 1 크기)
            Material[] updatedMaterials = new Material[sharedMaterials.Length + 1];

            // 기존 Material 배열 복사
            for (int i = 0; i < sharedMaterials.Length; i++)
            {
                updatedMaterials[i] = sharedMaterials[i];
            }

            // 새 Material 추가
            updatedMaterials[sharedMaterials.Length] = OutLineMaterials[team - 1];

            // MeshRenderer의 Material 배열 업데이트
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
