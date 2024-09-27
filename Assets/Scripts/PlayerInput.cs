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

    // 값 할당은 내부에서만 가능
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
        // 게임오버 상태에서는 사용자 입력을 감지하지 않는다
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

        // 이게 원인이다. 
        //fire = false;
        //reload = false;
        jump = false;
    }
    public void touchFire()
    {
        fire = true;
        Debug.Log("총 발사 버튼");
    }

    public void touchReload()
    {
        reload = true;
        Debug.Log("리로드 버튼");
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

    // 이게 잘 전달이 안되나??
    public void ReturnRun()
    {
        run = false;
    }
    public void OnAiming()
    {
        Debug.Log("겨누고 있다.");
        //aimUi.color = Color.yellow;
        GameObject.Find("Follow Cam").GetComponent<CinemachineCameraOffset>().enabled = true;

    }

    public void ReturnAiming()
    { 
        GameObject.Find("Follow Cam").GetComponent<CinemachineCameraOffset>().enabled = false;
    }
}
