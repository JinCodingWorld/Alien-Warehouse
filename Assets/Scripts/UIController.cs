using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class UIController : MonoBehaviour
{
    public static UIController instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<UIController>();
            }

            return m_instance;
        }
    }

    private static UIController m_instance; // 싱글톤이 할당될 변수

    public Text smallGunammoText; // 탄약 표시용 텍스트
    public Text bigGunammoText; // 탄약 표시용 텍스트
    public Text CharAmmoText; // 캐릭터 탄약 표시 텍스트
    public Text scoreText; // 점수 표시용 텍스트
    public Text waveText; // 적 웨이브 표시용 텍스트
    public Text lifeText;
    public Text leftEnemy;
    public Text hpText; // 캐릭터창 hp 상태창
    public Text killsNum;
    public Text victoryMoney;
    public Text recordTimer;

    public GameObject gameoverUI; // 게임 오버시 활성화할 UI 

    public GameObject turnOffSmallGunPanel;
    public GameObject turnOffBigGunPanel;

    public GameObject smallGunSelect;
    public GameObject bigGunSelect;
    public GameObject victoryUI;

    private void Start()
    {
        if(AsunaGun.gunNum == 0)
        {
            smallGunSelect.SetActive(true);
            bigGunSelect.SetActive(false);
            turnOffSmallGunPanel.SetActive(false);
        }
        else
        {
            smallGunSelect.SetActive(false);
            bigGunSelect.SetActive(true);
            turnOffBigGunPanel.SetActive(false);
        }

    }

    public void UpdateRecordTimer(float recordTime)
    {
        recordTimer.text = "Clear Time:" + TruncateTime(recordTime);

        // 도전 해본다. 
        //recordTimer.text = "Clear Time:" + (int)recordTime + "'" + (TruncateTime(recordTime) - (int)recordTime) + "''";
    }

    public float TruncateTime(float time)
    {
        return (float)Mathf.Floor(time * 100) / 100f;
    }
    public void UpdateKillsNum(int deadAlienNum)
    {
        killsNum.text = "Kills:" + deadAlienNum;
    }

    // 탄약 텍스트 갱신
    public void UpdateAmmoText(int magAmmo, int remainAmmo)
    {
        if (AsunaGun.gunNum == 0)
        {
            smallGunammoText.text = magAmmo + "/" + remainAmmo;
        }
        else if(AsunaGun.gunNum == 1)
        {
            bigGunammoText.text = magAmmo + "/" + remainAmmo;
        }

        CharAmmoText.text = magAmmo + "/" + remainAmmo;
    }

    // 점수 텍스트 갱신
    public void UpdateMoneyText(int newMoney)
    {
        scoreText.text = "$ " + newMoney;
        victoryMoney.text = "Money:$" + newMoney;
    }

    // 적 웨이브 텍스트 갱신
    public void UpdateWaveText(int waves, int count)
    {
        waveText.text = waves.ToString();
        leftEnemy.text = count.ToString();
    }

    // 캐릭터HP창 수 업데이트
    public void UpdateHPText(float newHP)
    {
        hpText.text = newHP + "/" + "100";
    }

    // 라이프 수 업데이트
    public void UpdateLifeText(int life)
    {
        lifeText.text = life.ToString();
    }

    // 게임 오버 UI 활성화
    public void SetActiveGameoverUI(bool active)
    {
        gameoverUI.SetActive(active);
    }

    // 게임 재시작
    public void GameRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
