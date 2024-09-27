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
        introtext.DOText("오늘은 은하제국 21,292년 2월 29일" +
            "\r\n당신은 지금 행성 오로라로 이주하는 우주선\r\n'스타더스트호'에 탑승중입니다." +
            "\r\n우리는 순조롭게 향해하던 중, 낯선 외계인 무리를\r\n맞닥뜨리게 되었고, 우리 인류는 위기에 처해 있습니다." +
            " \r\n당신의 임무는 외계인들을 격멸하면서 미션을" + "\r\n수행하여 우주선을 위기로부터 구해내야 합니다." +
            "\r\n우리 인류의 희망은 당신의 손에 달려있습니다.\r\n행운을 빕니다. Asuna 대위", 10).SetEase(Ease.Linear).SetAutoKill(false).Pause();
    }
    public void StartTweens()
    {
        DOTween.PlayAll();
    }
}
