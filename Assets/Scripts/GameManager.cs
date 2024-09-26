using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] int playerHealth = 3;

    [SerializeField] public Animator playerAnimator;
    [SerializeField] GameObject playerObject;

    private void Awake()
    {
        // �̱��� ���� ����
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

    public void TakeDamage()
    {
        playerHealth--;

        if (playerHealth <= 0) // 0 ���Ϸ� ����
        {
            playerAnimator.SetTrigger("Die");
            StartCoroutine(HandlePlayerDeath()); // �÷��̾� ��� ó�� �ڷ�ƾ ����
        }
    }

    private IEnumerator HandlePlayerDeath()
    {
        // �ִϸ��̼��� ���̸�ŭ ���
        yield return new WaitForSeconds(playerAnimator.GetCurrentAnimatorStateInfo(0).length);
        playerObject.SetActive(false); // �÷��̾� ������Ʈ ��Ȱ��ȭ
        Destroy(gameObject); // ���� �Ŵ��� ��Ȱ��ȭ (�ʿ��� ���)
    }

    public int GetPlayerHealth()
    {
        return playerHealth;
    }
}
