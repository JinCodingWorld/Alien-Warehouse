using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class IntroLetter : MonoBehaviour
{
    public Text introtext;

    private void Start()
    {
        introtext.DOText("������ �������� 21,292�� 2�� 29��" +
            "\r\n����� ���� �༺ ���ζ�� �����ϴ� ���ּ�\r\n'��Ÿ����Ʈȣ'�� ž�����Դϴ�." +
            "\r\n�츮�� �����Ӱ� �����ϴ� ��, ���� �ܰ��� ������\r\n�´ڶ߸��� �Ǿ���, �츮 �η��� ���⿡ ó�� �ֽ��ϴ�." +
            " \r\n����� �ӹ��� �ܰ��ε��� �ݸ��ϸ鼭 �̼���" + "\r\n�����Ͽ� ���ּ��� ����κ��� ���س��� �մϴ�." +
            "\r\n�츮 �η��� ����� ����� �տ� �޷��ֽ��ϴ�.\r\n����� ���ϴ�. Asuna ����", 10).SetEase(Ease.Linear).SetAutoKill(false).Pause();
    }
    public void StartTweens()
    {
        DOTween.PlayAll();
    }
}
