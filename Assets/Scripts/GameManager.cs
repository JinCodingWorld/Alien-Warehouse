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
            // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
            if (m_instance == null)
            {
                // 씬에서 GameManager 오브젝트를 찾아 할당
                m_instance = FindObjectOfType<GameManager>();
            }

            // 싱글톤 오브젝트를 반환
            return m_instance;
        }
    }

    private static GameManager m_instance; // 싱글톤이 할당될 static 변수

    private int money = 0; // 현재 게임 점수
    public Text timerAttack; // 60초 시간 제한
    public Text gameTimer;

    private DemonSpawner spawner;
    private PlayerHP playerHP;
    public float timeCnt = 60f;
    private float timer = 0f;
    public float recordTime;
    public bool isGameover { get; private set; } // 게임 오버 상태
    // 폭탄 설치 완료 시
    public bool bombInstall = false;

    private void Awake()
    {
        // 씬에 싱글톤 오브젝트가 된 다른 GameManager 오브젝트가 있다면
        if (instance != this)
        {
            // 자신을 파괴
            Destroy(gameObject);
        }
        spawner = FindObjectOfType<DemonSpawner>();
        playerHP = FindObjectOfType<PlayerHP>();
    }

    private void Start()
    {
        // 플레이어 캐릭터의 사망 이벤트 발생시 게임 오버
        FindObjectOfType<PlayerHP>().onDeath += EndGame;

    }

    // 점수를 추가하고 UI 갱신
    public void AddScore(int newMoney)
    {
        // 게임 오버가 아닌 상태에서만 점수 증가 가능
        if (!isGameover)
        {
            // 점수 추가
            money += newMoney;
            // 점수 UI 텍스트 갱신
            UIController.instance.UpdateMoneyText(newMoney);
        }
    }

    // 게임 오버 처리
    public void EndGame()
    {
        // 게임 오버 상태를 참으로 변경
        isGameover = true;
        // 게임 오버 UI를 활성화
        UIController.instance.SetActiveGameoverUI(true);
    }

    private void Update()
    {
        // 게임 진행 기간 동안 시간 흐르기(일단 해보기)
        timer += Time.deltaTime;
        gameTimer.text = TruncateTime(timer).ToString();

        // 폭탄이 설치된다면 카운트 다운 시작, 경고음 울리기
        if (bombInstall)
        {
            gameTimer.gameObject.SetActive(false);
            recordTime = timer;

            spawner.wave = 7;
            timeCnt -= Time.deltaTime;
            timerAttack.text = TruncateTime(timeCnt).ToString();

            // 폭탄 터진 후 플레이어, 몬스터 사망, 전부 멈춤
            // 폭탄 터지는 소리
            if (timeCnt <= 0f && timeCnt > -0.05f)
            {
                timerAttack.text = "00:00";
                // 근데 계속 업데이트 되어서 터지는 효과 지속되어서 3초 후에 효과 끔.
                playerHP.explosionEffect.Play();
                // 플레이어 사망
                playerHP.dead = true;
                EndGame();
                playerHP.Die();
            }
            else if(timeCnt <= -0.05f && timeCnt >= -3f)
            {
                timerAttack.text = "00:00";
                // 근데 계속 업데이트 되어서 터지는 효과 지속되어서 3초 후에 효과 끔.
                playerHP.explosionEffect.Play();
                // 플레이어 사망
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
