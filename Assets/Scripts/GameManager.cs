using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public static GameManager instance
    {
        get
        {
            // ���� �̱��� ������ ���� ������Ʈ�� �Ҵ���� �ʾҴٸ�
            if (m_instance == null)
            {
                // ������ GameManager ������Ʈ�� ã�� �Ҵ�
                m_instance = FindObjectOfType<GameManager>();
            }

            // �̱��� ������Ʈ�� ��ȯ
            return m_instance;
        }
    }

    private static GameManager m_instance; // �̱����� �Ҵ�� static ����

    private int money = 0; // ���� ���� ����
    public Text timerAttack; // 60�� �ð� ����
    public Text gameTimer;

    private DemonSpawner spawner;
    private PlayerHP playerHP;
    public float timeCnt = 60f;
    private float timer = 0f;
    public float recordTime;
    public bool isGameover { get; private set; } // ���� ���� ����
    // ��ź ��ġ �Ϸ� ��
    public bool bombInstall = false;

    private void Awake()
    {
        // ���� �̱��� ������Ʈ�� �� �ٸ� GameManager ������Ʈ�� �ִٸ�
        if (instance != this)
        {
            // �ڽ��� �ı�
            Destroy(gameObject);
        }
        spawner = FindObjectOfType<DemonSpawner>();
        playerHP = FindObjectOfType<PlayerHP>();
    }

    private void Start()
    {
        // �÷��̾� ĳ������ ��� �̺�Ʈ �߻��� ���� ����
        FindObjectOfType<PlayerHP>().onDeath += EndGame;

    }

    // ������ �߰��ϰ� UI ����
    public void AddScore(int newMoney)
    {
        // ���� ������ �ƴ� ���¿����� ���� ���� ����
        if (!isGameover)
        {
            // ���� �߰�
            money += newMoney;
            // ���� UI �ؽ�Ʈ ����
            UIController.instance.UpdateMoneyText(newMoney);
        }
    }

    // ���� ���� ó��
    public void EndGame()
    {
        // ���� ���� ���¸� ������ ����
        isGameover = true;
        // ���� ���� UI�� Ȱ��ȭ
        UIController.instance.SetActiveGameoverUI(true);
    }

    private void Update()
    {
        // ���� ���� �Ⱓ ���� �ð� �帣��(�ϴ� �غ���)
        timer += Time.deltaTime;
        gameTimer.text = TruncateTime(timer).ToString();

        // ��ź�� ��ġ�ȴٸ� ī��Ʈ �ٿ� ����, ����� �︮��
        if (bombInstall)
        {
            gameTimer.gameObject.SetActive(false);
            recordTime = timer;

            spawner.wave = 7;
            timeCnt -= Time.deltaTime;
            timerAttack.text = TruncateTime(timeCnt).ToString();

            // ��ź ���� �� �÷��̾�, ���� ���, ���� ����
            // ��ź ������ �Ҹ�
            if (timeCnt <= 0f && timeCnt > -0.05f)
            {
                timerAttack.text = "00:00";
                // �ٵ� ��� ������Ʈ �Ǿ ������ ȿ�� ���ӵǾ 3�� �Ŀ� ȿ�� ��.
                playerHP.explosionEffect.Play();
                // �÷��̾� ���
                playerHP.dead = true;
                EndGame();
                playerHP.Die();
            }
            else if(timeCnt <= -0.05f && timeCnt >= -3f)
            {
                timerAttack.text = "00:00";
                // �ٵ� ��� ������Ʈ �Ǿ ������ ȿ�� ���ӵǾ 3�� �Ŀ� ȿ�� ��.
                playerHP.explosionEffect.Play();
                // �÷��̾� ���
                playerHP.dead = true;
                EndGame();
            }
            else if(timeCnt <= -3f)
            {
                timerAttack.text = "00:00";
                playerHP.explosionEffect.Stop();
                playerHP.dead = true;
                EndGame();
            }
            
        }
    }

    public float TruncateTime(float time)
    {
        return (float)Mathf.Floor(time * 100) / 100f;
    }
}
