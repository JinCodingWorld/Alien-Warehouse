using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private float Xmove;
    private float Ymove;
    // �� �Ʒ� ȸ��
    private float verticalRotation = 0f; 
    [SerializeField] private Transform PlayerBody;
    public Vector2 LookAxis;
    public float Sensitivity = 10f;

    // ���� ���� ����
    public float maxVerticalAngle = 90f; 

    void Update()
    {
        // �¿� ������
        Xmove = LookAxis.x * Sensitivity * Time.deltaTime;
        PlayerBody.Rotate(Vector3.up * Xmove);

        // �� �Ʒ� ������
        Ymove = LookAxis.y * Sensitivity * Time.deltaTime;

        //Debug.Log("LookAxis: " + LookAxis);
        // �� �Ʒ� ���� ����
        verticalRotation -= Ymove;
        verticalRotation = Mathf.Clamp(verticalRotation, -maxVerticalAngle, maxVerticalAngle);

        // ī�޶� ���� ȸ�� ����
        transform.localEulerAngles = new Vector3(verticalRotation, transform.localEulerAngles.y, 0f);
    }
}
