using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemonSpawner : MonoBehaviour
{
    public Demon demonPrefab;
    public Demon pigMonsterPrefab;
    public Demon ScavengerPrefab;
    public GameObject bomb;
    public GameObject missionTitle;
    public GameObject levelUp;

    public Text charLevelText;
    public static int deadAlienNum = 0;

    public DemonData[] demonDatas;

    public Transform[] spawnPoints;

    private List<Demon> demons = new List<Demon>();
    public int wave;

    private void Update()
    {
        if (GameManager.instance != null && GameManager.instance.isGameover)
        {
            return;
        }

        // ���� ��� ����ģ ��� ���� ���� ����
        if (demons.Count <= 0)
        {
            SpawnWave();
        }

        // UI ����
        UpdateUI();

        // �ܰ��� �Ҹ�, �����̴��� ���缭 �ǽð����� ���� �����Ѱ�? �ƽ��Ե� �ǽð����δ� �ȵǳ�
        // �� �׷���???(�� �ȵȴ�.)
        Demon[] demonList = FindObjectsOfType<Demon>();

        for(int j=0;j< demonList.Length;j++)
        {
            AudioSource audioSource = demonList[j].gameObject.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.volume = SoundManager.instance.audioSourceRandom.volume;
            }
        }

        // level Up UI ����ֱ�
        if(deadAlienNum == 15)
        {
            levelUp.SetActive(true);
            charLevelText.text = "2";

            Destroy(levelUp, 3f);
        }
        
    }

    //public void offLevelUpUI()
    //{
    //    levelUp.SetActive(false);
    //}

    private void UpdateUI()
    {
        // ���� ���̺�� ���� �� �� ǥ��
        UIController.instance.UpdateWaveText(wave, demons.Count);
        UIController.instance.UpdateKillsNum(deadAlienNum);

        // ��� �ð� ����
        //UpdateRecordTimer
        UIController.instance.UpdateRecordTimer(GameManager.instance.recordTime);

    }

    // ���� ���̺꿡 ���� ������� ����
    private void SpawnWave()
    {
        if (wave <= 5)
        {
            wave++;
        }

        int spawnCount = Mathf.RoundToInt(wave * 2.0f);

        if (wave == 1)
        {
            for (int i = 0; i < spawnCount; i++)
            {
                // 2���� ����
                CreatePigMonster();
            }
        }
        else if (wave == 2)
        {
            // 4����
            for (int i = 0; i < spawnCount; i++)
            {
                CreatePigMonster();
            }
        }
        else if (wave == 3)
        {
            // 3����
            for (int i = 0; i < spawnCount / 2; i++)
            {
                CreatePigMonster();
            }
            // 3����
            for (int i = 0; i < spawnCount / 2; i++)
            {
                ScavengerMonster();
            }
        }
        else if (wave == 4)
        {
            // 4����
            for (int i = 0; i < spawnCount / 2; i++)
            {
                ScavengerMonster();
            }
            // 4����
            for (int i = 0; i < spawnCount / 2; i++)
            {
                CreateDemon();
            }
        }
        else if (wave == 5)
        {
            // 5����, ��ź �̼� ��ŸƮ
            for (int i = 0; i < spawnCount / 2; i++)
            {
                CreateDemon();
            }

            missionTitle.SetActive(true);
            bomb.SetActive(true);

            Invoke("offMissionTitle", 4f);
        }
        else if (wave == 6)
        {
            CreatePigMonster();
            CreateDemon();
            ScavengerMonster();
            CreatePigMonster();
            CreateDemon();
            ScavengerMonster();
            CreatePigMonster();
            CreateDemon();
            ScavengerMonster();
            CreatePigMonster();
            CreateDemon();
            ScavengerMonster();
            CreatePigMonster();
            CreateDemon();
            ScavengerMonster();

            // �Ҹ� ����???
            Demon[] demons = FindObjectsOfType<Demon>();
            for (int i = 0; i < demons.Length; i++)
            {
                AudioSource audioSource = demons[i].gameObject.GetComponent<AudioSource>();
                if (audioSource != null)
                {
                    audioSource.Pause();
                }
            }
        }
    }

    public void offMissionTitle()
    {
        missionTitle.SetActive(false);
    }
public void CreateDemon()
    {
        DemonData demonData = demonDatas[Random.Range(4, 6)];
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        Demon demon = Instantiate(demonPrefab, spawnPoint.position, spawnPoint.rotation);
        demon.Setup(demonData);
        demons.Add(demon);

        demon.onDeath += () => demons.Remove(demon);

        // zomebie.gameObject ���� gameObject ���� ��� �ǳ�?
        demon.onDeath += () => Destroy(demon.gameObject, 10f);
        demon.onDeath += () => GameManager.instance.AddScore(100);


    }

    public void CreatePigMonster()
    {
        DemonData demonData = demonDatas[Random.Range(0, 2)];
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        Demon demon = Instantiate(pigMonsterPrefab, spawnPoint.position, spawnPoint.rotation);
        demon.Setup(demonData);
        demons.Add(demon);

        demon.onDeath += () => demons.Remove(demon);

        // zomebie.gameObject ���� gameObject ���� ��� �ǳ�?
        demon.onDeath += () => Destroy(demon.gameObject, 10f);
        demon.onDeath += () => GameManager.instance.AddScore(30);

    }

    public void ScavengerMonster()
    {
        DemonData demonData = demonDatas[Random.Range(2, 4)];
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        Demon demon = Instantiate(ScavengerPrefab, spawnPoint.position, spawnPoint.rotation);
        demon.Setup(demonData);
        demons.Add(demon);

        demon.onDeath += () => demons.Remove(demon);

        // zomebie.gameObject ���� gameObject ���� ��� �ǳ�?
        demon.onDeath += () => Destroy(demon.gameObject, 10f);
        demon.onDeath += () => GameManager.instance.AddScore(50);

    }


}
