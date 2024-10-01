using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BR_PlayerController : MonoBehaviour
{
    public int maxHP = 100;
    private int currentHP;

    public Slider hpBar;
    private bool isInvincible = false;
    private SpriteRenderer spriteRenderer;
    [SerializeField] SpriteRenderer rightSpriteRenderer; 
    [SerializeField] SpriteRenderer leftSpriteRenderer;

    public Cinemachine.CinemachineImpulseSource impulseSource; //ī�޶� ��鸮�� �ϱ�

    private void Start()
    {
        hpBar = GameObject.Find("PlayerHP").GetComponent<Slider>();
        currentHP = maxHP;
        UpdateHPBar();

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("BossAttack") && !isInvincible) //���� ���°� �ƴ� ���� ������ �ޱ�
        {
            TakeDamage(10);
            StartCoroutine(ActivateInvincibility(3f)); //3�ʰ� ���� ���� ����
        }

        if (other.CompareTag("BossRandomParticle") && !isInvincible)
        {
            TakeDamage(10);
            StartCoroutine(ActivateInvincibility(3f));
        }
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        UpdateHPBar();

        if (currentHP <= 0)
        {
            SoundManager.Instance.StopBGM();
            SoundManager.Instance.PlayGameOverSound();
            SceneManager.LoadScene("GameOver");
        }
    }

    private void UpdateHPBar()
    {
        if (hpBar != null)
        {
            hpBar.value = currentHP;
        }
    }

    private IEnumerator ActivateInvincibility(float duration)
    {
        isInvincible = true;

        impulseSource.GenerateImpulse();
        //������ ȿ�� ����
        Color originalColor = spriteRenderer.color; //���� ���� ����
        Color rightOriginalColor = rightSpriteRenderer.color;
        Color leftOriginalColor = leftSpriteRenderer.color;

        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.5f); //���� �� 0.5�� ����
        rightSpriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.5f);
        leftSpriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.5f);

        yield return new WaitForSeconds(duration); //duration ���� ���

        //���� �������� ����
        spriteRenderer.color = originalColor;
        rightSpriteRenderer.color = rightOriginalColor;
        leftSpriteRenderer.color = leftOriginalColor;

        isInvincible = false; // ���� ���� ��Ȱ��ȭ
    }
}
