using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    //�ν����Ϳ� Cinemachine Impulse Listener + Cinemachine Impulse Source �߰��ϱ�
    //Duration: ���ӽð�, Amplitude: ��鸲 ����, Frequency: ���ļ�(��鸲 �ӵ�) �˸°� �����ϱ� 
    public Cinemachine.CinemachineImpulseSource impulseSource; //ī�޶� ��鸮�� �ϱ�

    //�ʱ� �������� �����ϴ� ����
    //�긦 �� �� �ָ� ��������� �� ����� �̻��ϰ� ���޵�
    [SerializeField] private int initialPlayerHealth = 3;

    private int playerHealth;
    [SerializeField] private Image[] hearts;

    public int totalPoint; //���� ��ü���� ���� ����
    //����� ���� �ǹ̰� ������ ���������� �����ϸ� �������� �� �� ������ �� �� ����
    public int stagePoint; //���� ������������ ���� ����

    [SerializeField] PlayerController player;
    [SerializeField] public Animator playerAnimator;
    [SerializeField] GameObject playerObject;
    [SerializeField] TextMeshProUGUI scoreText;

    private bool isShieldActive; //���� ���� ����
    private WaitForSeconds delay;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            //���ο� ���� �ε�� ������ OnSceneLoaded�� ȣ���ϵ��� �̺�Ʈ ���
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        //���� ���� �� ������ (����� ����) ����� �ʱⰪ���� ������
        playerHealth = initialPlayerHealth;
    }

    private void Start()
    {
        InitializeGameObjects(); //���� ������Ʈ�� �ʱ�ȭ

        UpdateScoreUI();
        UpdateHealthUI();
    }

    //���� ���� �ε�� ������ �÷��̾��� ����� �ʱⰪ���� ����
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        playerHealth = initialPlayerHealth;
        InitializeGameObjects();
        UpdateHealthUI();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    //���ӿ� �ʿ��� ������Ʈ �� ������Ʈ���� �ʱ�ȭ
    private void InitializeGameObjects()
    {
        player = FindObjectOfType<PlayerController>();

        if (player != null)
        {
            playerObject = player.gameObject;
            playerAnimator = player.GetComponent<Animator>();
        }

        scoreText = FindObjectOfType<TextMeshProUGUI>();

        //Image������Ʈ�� hearts �迭�� ����
        hearts = GameObject.FindGameObjectsWithTag("Heart")
            .Select(heart => heart.GetComponent<Image>())
            .ToArray();

        impulseSource = FindObjectOfType<Cinemachine.CinemachineImpulseSource>();

        float animationLength = playerAnimator != null ? playerAnimator.GetCurrentAnimatorStateInfo(0).length : 0;
        delay = new WaitForSeconds(animationLength);
    }

    public void TakeDamage()
    {
        if (isShieldActive) //���尡 Ȱ��ȭ�Ǿ� �ִٸ� ����X
            return;

        if (playerHealth >= 0)
        {
            if (player.gameObject.layer == 7)
                return;

            impulseSource.GenerateImpulse();
            SoundManager.Instance.PlayAttackSound();
            playerHealth--;
            UpdateHealthUI();
            PlayerDamaged();
        }
        
        else
        {
            Die();
        }
    }

    private void Die()
    {
        SoundManager.Instance.PlayGameOverSound();
        playerAnimator.SetTrigger("Die");
        StartCoroutine(HandlePlayerDeath());
    }

    private void PlayerDamaged()
    {
        player.gameObject.layer = 7;
        player.playerSpriteRenderer.color = new Color(1, 1, 1, 0.4f);
        player.LeftArmSpriteRenderer.color = new Color(1, 1, 1, 0.4f);
        player.RightArmSpriteRenderer.color = new Color(1, 1, 1, 0.4f);
        player.Invoke("OffDamaged", 3);
    }

    public void ActivateShield()
    {
        isShieldActive = true;
    }

    public void DeactivateShield()
    {
        isShieldActive = false;
    }

    private IEnumerator HandlePlayerDeath()
    {
        yield return delay;

        if (playerObject != null)
        {
            playerObject.SetActive(false);
        }

        SoundManager.Instance.StopBGM();
        SceneManager.LoadScene("GameOver");
    }

    public int GetPlayerHealth()
    {
        return playerHealth;
    }

    public void AddScore(int points)
    {
        stagePoint += points;
        totalPoint += points;
        UpdateScoreUI();
    }

    //���� UI ������Ʈ
    public void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + stagePoint;
        }
    }

    //��� UI ������Ʈ
    private void UpdateHealthUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < playerHealth)
            {
                hearts[i].gameObject.SetActive(true);
            }
            else
            {
                hearts[i].gameObject.SetActive(false);
            }
        }
    }
}
