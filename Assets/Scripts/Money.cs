using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour, IItem
{
    public int score = 200; // ������ ����

    public void Use(GameObject target)
    {
        // ���� �Ŵ����� ������ ���� �߰�
        GameManager.instance.AddScore(score);
        // ���Ǿ����Ƿ�, �ڽ��� �ı�
        Destroy(gameObject);
    }
}
