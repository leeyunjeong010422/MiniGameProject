using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] AudioSource audioSource;

    [SerializeField] AudioClip startBGM;

    //���� BGM ��ġ �����صξ ����ٰ� �ٽ� �����ص� �̾ �����ϵ��� ��
    private float previousTime = 0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
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

    public void PlayBGM(AudioClip bgm)
    {
        audioSource.clip = bgm;
        audioSource.loop = true;
        audioSource.Play();
        previousTime = 0f;
    }

    public void StopBGM()
    {
        previousTime = audioSource.time;
        audioSource.Stop();
    }

    public void ResumeBGM()
    {
        audioSource.time = previousTime;
        audioSource.Play();
    }

    public void ButtonToggle()
    {
        audioSource.mute = !audioSource.mute;
    }

    public bool IsSoundMuted()
    {
        return audioSource.mute;
    }
}
