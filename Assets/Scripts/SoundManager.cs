using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioSource startBGM;
    [SerializeField] private AudioSource gunClip;
    [SerializeField] private AudioSource coinClip;
    [SerializeField] private AudioSource itemClip;
    [SerializeField] private AudioSource attackClip;
    [SerializeField] private AudioSource gameOverClip;
    [SerializeField] private AudioSource mouseClickClip;
    [SerializeField] private AudioSource enemyGunClip;
    [SerializeField] private AudioSource enemyArrowClip;
    [SerializeField] private AudioSource bombClip;
    [SerializeField] private AudioSource enemyDyingClip;
    [SerializeField] private AudioSource bossSoundClip;
    [SerializeField] private AudioSource warningClip;
    [SerializeField] private AudioSource gameClearClip;
    [SerializeField] private AudioSource playerJumpClip;

    //���� BGM ��ġ ����
    private float previousTime = 0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetStartBGM()
    {
        PlayBGM(startBGM);
    }

    public void PlayBGM(AudioSource bgm)
    {
        bgm.loop = true;
        bgm.Play();
        previousTime = 0f;
    }

    public void StopBGM()
    {
        previousTime = startBGM.time; //startBGM�� ���� �ð� ����
        startBGM.Stop();
    }

    public void ResumeBGM()
    {
        startBGM.time = previousTime; //���� �ð����� ����
        startBGM.Play();
    }

    public void ButtonToggle()
    {
        if (startBGM != null)
        {
            startBGM.mute = !startBGM.mute;
        }
    }

    public bool IsSoundMuted()
    {
        return startBGM != null && startBGM.mute; //���Ұ� ���� ��ȯ
    }

    public void PlayGunSound()
    {
        gunClip.PlayOneShot(gunClip.clip); //�� �Ҹ� ���
    }

    public void PlayCoinSound()
    {
        coinClip.PlayOneShot(coinClip.clip); //���� �Դ� �Ҹ� ���
    }

    public void PlayItemSound()
    {
        itemClip.PlayOneShot(itemClip.clip); //������ �Դ� �Ҹ� ���
    }

    public void PlayAttackSound()
    {
        attackClip.PlayOneShot(attackClip.clip); //�÷��̾ �ǰ� ������ �� ���� �Ҹ� ���
    }

    public void PlayGameOverSound()
    {
        gameOverClip.PlayOneShot(gameOverClip.clip); //���ӿ����� �� ���� �Ҹ� ���
    }

    public void PlayMouseClickSound()
    {
        mouseClickClip.PlayOneShot(mouseClickClip.clip); //���콺 Ŭ���� �� ���� �Ҹ�
    }

    public void PlayEnemyGunSound()
    {
        enemyGunClip.PlayOneShot(enemyGunClip.clip); //�� ��� �� ���� �Ҹ� ���
    }

    public void PlayEnemyArrowSound()
    {
        enemyArrowClip.PlayOneShot(enemyArrowClip.clip); //Ȱ ��� �� ���� �Ҹ� ���
    }

    public void PlayBombSound()
    {
        bombClip.PlayOneShot(bombClip.clip); //��ź ������ �Ҹ� ���
    }

    public void PlayEnemyDyingSound()
    {
        enemyDyingClip.PlayOneShot(enemyDyingClip.clip); //�� �ǰ� �Ҹ� ���
    }

    public void PlayBossSound()
    {
        bossSoundClip.PlayOneShot(bossSoundClip.clip); //���� ���� �Ҹ� ���
    }

    public void PlayWarningSound()
    {
        warningClip.PlayOneShot(warningClip.clip); //��� �Ҹ� ���
    }

    public void PlayGameClaerSound()
    {
        gameClearClip.PlayOneShot(gameClearClip.clip); //���� Ŭ���� �Ҹ� ���
    }

    public void PlayPlayerJumpSound()
    {
        playerJumpClip.PlayOneShot(playerJumpClip.clip); //�÷��̾� ���� �Ҹ� ���
    }
}
