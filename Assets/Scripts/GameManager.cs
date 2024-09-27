using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] int playerHealth = 3;

    [SerializeField] public Animator playerAnimator;
    [SerializeField] GameObject playerObject;

    private bool isShieldActive;

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

        if (playerObject != null)
        {
            playerAnimator = playerObject.GetComponent<Animator>();
        }
    }

    public void TakeDamage()
    {
        if (isShieldActive) //���尡 Ȱ��ȭ�Ǿ� �ִٸ� ����X
            return;

        playerHealth--;

        if (playerHealth <= 0)
        {
            playerAnimator.SetTrigger("Die");
            StartCoroutine(HandlePlayerDeath());
        }
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
        //�ִϸ��̼��� ���̸�ŭ ���
        yield return new WaitForSeconds(playerAnimator.GetCurrentAnimatorStateInfo(0).length);
        playerObject.SetActive(false);
        //Destroy(gameObject);
    }

    public int GetPlayerHealth()
    {
        return playerHealth;
    }
}
