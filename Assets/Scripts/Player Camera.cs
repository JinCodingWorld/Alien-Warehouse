using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private float Xmove;
    private float Ymove;
    // 위 아래 회전
    private float verticalRotation = 0f; 
    [SerializeField] private Transform PlayerBody;
    public Vector2 LookAxis;
    public float Sensitivity = 10f;

    // 보는 각도 제한
    public float maxVerticalAngle = 90f; 

    void Update()
    {
        // 좌우 움직임
        Xmove = LookAxis.x * Sensitivity * Time.deltaTime;
        PlayerBody.Rotate(Vector3.up * Xmove);

        // 위 아래 움직임
        Ymove = LookAxis.y * Sensitivity * Time.deltaTime;

        //Debug.Log("LookAxis: " + LookAxis);
        // 위 아래 각도 제한
        verticalRotation -= Ymove;
        verticalRotation = Mathf.Clamp(verticalRotation, -maxVerticalAngle, maxVerticalAngle);

        // 카메라 수직 회전 적용
        transform.localEulerAngles = new Vector3(verticalRotation, transform.localEulerAngles.y, 0f);
    }
}
