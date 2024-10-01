using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{

    public string sceneToLoadOnAnyKey;
    private void Update()
    {
        //���� ���� GameWay�� �� �ƹ� Ű�� ������ �� �� ��ȯ
        if (SceneManager.GetActiveScene().name == "GameWay" && Input.anyKeyDown && !string.IsNullOrEmpty(sceneToLoadOnAnyKey))
        {
            SceneManager.LoadScene("Game");
        }
    }

    public void ChangeGameWaySecene(string sceneName)
    {
        SoundManager.Instance.PlayMouseClickSound();
        SceneManager.LoadScene("GameWay");
    }

    public void ChangeStartSecene(string sceneName)
    {
        SoundManager.Instance.PlayMouseClickSound();
        SoundManager.Instance.SetStartBGM();
        SceneManager.LoadScene("Start");
    }

    public void ChangeStartGameSecene(string sceneName)
    {
        SoundManager.Instance.PlayMouseClickSound();
        SceneManager.LoadScene("Game");
    }

    public void ChangeGameSecene(string sceneName)
    {
        SoundManager.Instance.PlayMouseClickSound();
        SoundManager.Instance.SetStartBGM();
        SceneManager.LoadScene("Game");
    }
}
