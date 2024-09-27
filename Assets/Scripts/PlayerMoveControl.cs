using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveControl : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpPower = 2f;

    private PlayerInput playerInput;
    private Rigidbody playerRigidbody; 
    private Animator playerAnimator; 

    private void Start()
    {
        // ����� ������Ʈ���� ������ ��������
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Move();
        playerAnimator.SetFloat("Horizontal", playerInput.moveH);
        playerAnimator.SetFloat("Vertical", playerInput.moveV);

    }

    private void Update()
    {
        if (playerInput.jump)
        {
            playerRigidbody.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
            Debug.Log("���� �Ŀ�");
        }

        if (playerInput.run)
        {
            moveSpeed = 8f;
            // �ٴ� �ִϸ��̼����� �ٲ�� ����
            playerAnimator.SetBool("Running", playerInput.run);
        }
        else
        {
            moveSpeed = 5f;
            playerAnimator.SetBool("Running", playerInput.run);
        }
    }
    private void Move()
    {
        Vector3 moveDistance = playerInput.moveV * transform.forward + playerInput.moveH * transform.right;
        playerRigidbody.MovePosition(playerRigidbody.position + moveDistance * moveSpeed * Time.deltaTime);
    }
}
