using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour
{
    public string moveVertical = "Vertical"; 
    public string moveHorizontal = "Horizontal";
    //public string JumpButtonName = "Jump";
    //public string fireButtonName = "Fire1";
    //public string reloadButtonName = "Reload"; 

    public FixedJoystick joystick;
    //public Image aimUi;
    //public Image runUi;

    //public Gun gun;

    // �� �Ҵ��� ���ο����� ����
    public float moveV { get; private set; } 
    public float moveH { get; private set; }

    public bool fire = false;
    public bool reload = false;
    public bool jump {  get; private set; }

    public bool run {  get; private set; }

    private void Start()
    {
        //animator = GetComponent<Animator>();
        run = false;
    }
    private void Update()
    {
        // ���ӿ��� ���¿����� ����� �Է��� �������� �ʴ´�
        //if (GameManager.instance != null && GameManager.instance.isGameover)
        //{
        //    moveV = 0;
        //    moveH = 0;
        //    fire = false;
        //    reload = false;
        //    return;
        //}

        moveV = joystick.Vertical;
        moveH = joystick.Horizontal;

        // �̰� �����̴�. 
        //fire = false;
        //reload = false;
        jump = false;
    }
    public void touchFire()
    {
        fire = true;
        Debug.Log("�� �߻� ��ư");
    }

    public void touchReload()
    {
        reload = true;
        Debug.Log("���ε� ��ư");
    }

    public void OnJump()
    {
        jump = true;
    }

    public void OnRun()
    {
        run = true;
        //runUi.color = Color.yellow;
    }

    // �̰� �� ������ �ȵǳ�??
    public void ReturnRun()
    {
        run = false;
    }
    public void OnAiming()
    {
        Debug.Log("�ܴ��� �ִ�.");
        //aimUi.color = Color.yellow;
        GameObject.Find("Follow Cam").GetComponent<CinemachineCameraOffset>().enabled = true;

    }

    public void ReturnAiming()
    { 
        GameObject.Find("Follow Cam").GetComponent<CinemachineCameraOffset>().enabled = false;
    }
}
