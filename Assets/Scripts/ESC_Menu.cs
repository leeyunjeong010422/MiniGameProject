using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ESC_Menu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public Button resumeButton; //����ϱ�
    public Button restartButton; //�ٽ��ϱ�
    public Button menuButton; //�޴��� ���ư���
    private bool isPaused = false; //������ ������� ����

    private void Start()
    {
        resumeButton.onClick.AddListener(Resume);
        restartButton.onClick.AddListener(Restart);
        menuButton.onClick.AddListener(GoToStart);

        pauseMenuUI.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume(); //������ ��� ����
            }
            else
            {
                Pause(); //������ ����
            }
        }
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; //���� �ð� ����
        isPaused = true;
        SoundManager.Instance.StopBGM();
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        SoundManager.Instance.ResumeBGM();
    }

    public void Restart()
    {
        SoundManager.Instance.PlayMouseClickSound();
        Time.timeScale = 1f;
        SoundManager.Instance.SetStartBGM();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //���� �� �����
    }

    public void GoToStart()
    {
        SoundManager.Instance.PlayMouseClickSound();
        Time.timeScale = 1f;
        SceneManager.LoadScene("Start");
    }
}
