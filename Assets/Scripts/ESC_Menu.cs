using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ESC_Menu : MonoBehaviour
{
    public GameObject pauseMenuUI; // UI �г�
    public Button resumeButton; // ����ϱ� ��ư
    public Button restartButton; // �ٽ��ϱ� ��ư
    public Button menuButton; // �޴��� ���ư��� ��ư
    private bool isPaused = false; // ������ ������� ����

    private void Start()
    {
        // ��ư Ŭ�� �̺�Ʈ ����
        resumeButton.onClick.AddListener(Resume);
        restartButton.onClick.AddListener(Restart);
        menuButton.onClick.AddListener(GoToStart);

        // UI �ʱ� ���� ����
        pauseMenuUI.SetActive(false);
    }

    private void Update()
    {
        // ESC Ű �Է� Ȯ��
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume(); // ������ ��� ����
            }
            else
            {
                Pause(); // ������ ����
            }
        }
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true); // UI Ȱ��ȭ
        Time.timeScale = 0f; // ���� �ð� ����
        isPaused = true; // ���� ������Ʈ
        SoundManager.Instance.StopBGM();
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false); // UI ��Ȱ��ȭ
        Time.timeScale = 1f; // ���� �ð� �簳
        isPaused = false; // ���� ������Ʈ
        SoundManager.Instance.ResumeBGM();
    }

    public void Restart()
    {
        Time.timeScale = 1f; // ���� �ð� �簳
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // ���� �� �����
    }

    public void GoToStart()
    {
        Time.timeScale = 1f; // ���� �ð� �簳
        SceneManager.LoadScene("Start"); // "Start" ������ ��ȯ
    }
}
