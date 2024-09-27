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

    private static UIController m_instance; // �̱����� �Ҵ�� ����

    public Text smallGunammoText; // ź�� ǥ�ÿ� �ؽ�Ʈ
    public Text bigGunammoText; // ź�� ǥ�ÿ� �ؽ�Ʈ
    public Text CharAmmoText; // ĳ���� ź�� ǥ�� �ؽ�Ʈ
    public Text scoreText; // ���� ǥ�ÿ� �ؽ�Ʈ
    public Text waveText; // �� ���̺� ǥ�ÿ� �ؽ�Ʈ
    public Text lifeText;
    public Text leftEnemy;
    public Text hpText; // ĳ����â hp ����â
    public Text killsNum;
    public Text victoryMoney;
    public Text recordTimer;

    public GameObject gameoverUI; // ���� ������ Ȱ��ȭ�� UI 

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

        // ���� �غ���. 
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

    // ź�� �ؽ�Ʈ ����
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

    // ���� �ؽ�Ʈ ����
    public void UpdateMoneyText(int newMoney)
    {
        scoreText.text = "$ " + newMoney;
        victoryMoney.text = "Money:$" + newMoney;
    }

    // �� ���̺� �ؽ�Ʈ ����
    public void UpdateWaveText(int waves, int count)
    {
        waveText.text = waves.ToString();
        leftEnemy.text = count.ToString();
    }

    // ĳ����HPâ �� ������Ʈ
    public void UpdateHPText(float newHP)
    {
        hpText.text = newHP + "/" + "100";
    }

    // ������ �� ������Ʈ
    public void UpdateLifeText(int life)
    {
        lifeText.text = life.ToString();
    }

    // ���� ���� UI Ȱ��ȭ
    public void SetActiveGameoverUI(bool active)
    {
        gameoverUI.SetActive(active);
    }

    // ���� �����
    public void GameRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
