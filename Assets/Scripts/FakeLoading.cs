using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//���� �� ����� �ڵ� ����
public class FakeLoading : MonoBehaviour
{
    [SerializeField] Image loadingImage;
    [SerializeField] Slider loadingBar;

    private void Start()
    {
        //���������� �ڵ� �ε� ����
        StartCoroutine(LoadBossRoom());
    }

    private IEnumerator LoadBossRoom()
    {
        //�ε� �̹��� Ȱ��ȭ
        loadingImage.gameObject.SetActive(true);
        loadingBar.value = 0;

        //����ũ �ε� �ð� ����
        float fakeLoadingTime = 3f;
        float elapsedTime = 0f;

        //����ũ �ε� �� ����
        while (elapsedTime < fakeLoadingTime)
        {
            elapsedTime += Time.deltaTime;
            loadingBar.value = elapsedTime / fakeLoadingTime;
            yield return null;
        }

        //�񵿱� �� �ε� ����
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("BossRoom");
        asyncLoad.allowSceneActivation = false;

        //���� �ε� ���� ��Ȳ ������Ʈ
        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress < 0.9f)
            {
                loadingBar.value = asyncLoad.progress;
            }
            else
            {
                loadingBar.value = 1; //�ε� �Ϸ�� ǥ��
                break; //�ε� �Ϸ�
            }

            yield return null;
        }

        //�� ��ȯ
        asyncLoad.allowSceneActivation = true;

        //�ε� �̹��� ��Ȱ��ȭ
        loadingImage.gameObject.SetActive(false);
    }
}
