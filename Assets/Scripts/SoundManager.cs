using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioSource startBGM;
    [SerializeField] private AudioSource gunClip;

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
}
