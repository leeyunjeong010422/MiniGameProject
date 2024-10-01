using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField] Sprite warningSprite;
    [SerializeField] GameObject[] attackParticlePrefabs; //���� Ÿ�� �迭
    [SerializeField] float warningDuration = 2f; //�����̴� �ð�
    [SerializeField] float blinkInterval = 0.2f; //�����̴� ����
    [SerializeField] Vector2 warningSize = new Vector2(10f, 10f); //��� �̹��� ũ��

    private void Start()
    {
        StartCoroutine(DelayedStart());
    }

    private IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(3f);
        StartCoroutine(AttackPattern());
    }

    //���� ���� ó�� �ڷ�ƾ
    private IEnumerator AttackPattern()
    {
        while (true)
        {
            //���� ��ġ�� �������� ����
            Vector2 attackPosition = new Vector2(-10, Random.Range(-35f, 40f));

            //���� ���� ��ġ�� ��� �̹����� ����
            GameObject warning = CreateWarning(attackPosition);

            SoundManager.Instance.PlayWarningSound();

            //2�� ���� ������
            yield return StartCoroutine(BlinkEffect(warning));

            //���� ��ƼŬ ���� �� ��� (���� ����)
            int randomIndex = Random.Range(0, attackParticlePrefabs.Length);
            //�������� ������ ���������� ��ƼŬ ����
            GameObject particle = Instantiate(attackParticlePrefabs[randomIndex], attackPosition, Quaternion.identity);
            ParticleSystem particleSystem = particle.GetComponent<ParticleSystem>();

            if (particleSystem != null)
            {
                SoundManager.Instance.PlayBossSound();
                particleSystem.Play(); //��ƼŬ ���
            }

            Destroy(warning); //��� �̹��� ����

            //���� ��ƼŬ�� ��� ���� ������ ���
            yield return new WaitForSeconds(particleSystem.main.duration);
            Destroy(particle); //��ƼŬ ����

            //���� �� ��� ��� (n�� �� �ٽ� ����)
            yield return new WaitForSeconds(2f);
        }
    }

    private GameObject CreateWarning(Vector3 position)
    {
        //��� �̹����� ǥ���� GameObject ����
        GameObject warning = new GameObject("AttackWarning");
        SpriteRenderer spriteRenderer = warning.AddComponent<SpriteRenderer>();

        spriteRenderer.sprite = warningSprite; //��������Ʈ ����
        spriteRenderer.color = Color.red; //������ ����
        warning.transform.localScale = warningSize; //�̹��� ũ�� ����
        warning.transform.position = position; //��ġ����

        return warning;
    }

    private IEnumerator BlinkEffect(GameObject warning)
    {
        SpriteRenderer renderer = warning.GetComponent<SpriteRenderer>(); //�̹��� ��������

        float elapsedTime = 0f;
        bool isVisible = true;

        while (elapsedTime < warningDuration) //����� �ð��� warningDuration ���� ���� ��
        {
            renderer.enabled = isVisible;
            isVisible = !isVisible;
            elapsedTime += blinkInterval;
            yield return new WaitForSeconds(blinkInterval);
        }

        renderer.enabled = true;
    }
}
