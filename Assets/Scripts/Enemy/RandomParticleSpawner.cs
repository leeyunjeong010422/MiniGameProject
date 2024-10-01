using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���� ȭ�� ������ ������ ���� ��ġ�� ��ƼŬ�� �����ϰ� �������� �̵���Ŵ
public class RandomParticleSpawner : MonoBehaviour
{
    [SerializeField] ParticleSystem particle;
    [SerializeField] float spawnInterval = 5f; //3�ʸ��� ��ƼŬ ����
    [SerializeField] Vector2 spawnArea = new Vector2(0f, 0.5f); //���� ����(Y�ุ ����)
    [SerializeField] float moveSpeed = 80f;
    [SerializeField] float particleLifetime = 3f; //��ƼŬ ���� �ð� (5�� �ڿ� �ı�)

    [SerializeField] Sprite warningSprite;
    [SerializeField] Vector2 warningSize = new Vector2(10f, 10f);
    [SerializeField] float warningDuration = 2f; //���� �ð�
    [SerializeField] float blinkInterval = 0.2f; //������ ����

    private Camera mainCamera;
    private Queue<(ParticleSystem particle, float spawnTime)> activeParticles = new Queue<(ParticleSystem, float)>(); //���� Ȱ��ȭ�� ��ƼŬ�� �ð��� ����
    private WaitForSeconds waitForSpawnInterval; //���� �ֱ� ��⸦ ���� �ڷ�ƾ
    private WaitForSeconds startRandomParticle; //ó���� ��ƼŬ ������ ���� ������ ���� ȣ���ϴ� �ڷ�ƾ

    private void Start()
    {
        mainCamera = Camera.main; //���� ī�޶� ������
        waitForSpawnInterval = new WaitForSeconds(spawnInterval);
        startRandomParticle = new WaitForSeconds(5f); //5�� �ڿ� ��ƼŬ ������ �����Ѵٴ� ��
        StartCoroutine(StartRandomParticle());
    }

    private IEnumerator StartRandomParticle()
    {
        yield return startRandomParticle; //���� ������ �ð�(5��) ��� ��
        StartCoroutine(SpawnParticles()); //��ƼŬ ���� �ڷ�ƾ ����
    }

    private IEnumerator SpawnParticles()
    {
        while (true)
        {
            int randomCount = Random.Range(1, 3);

            for (int j = 0; j < randomCount; j++)
            {

                //ī�޶� ����Ʈ���� ������ �� �κ��� ���� ��ǥ(����Ʈ ��ǥ (1, y, z))
                //ī�޶� ����: https://ansohxxn.github.io/unitydocs/camera/
                //          : https://m.blog.naver.com/corncho456/221727952827
                Vector2 warningPosition = mainCamera.ViewportToWorldPoint(new Vector2(0.9f, Random.Range(0.2f, 0.8f)));
                warningPosition.y += Random.Range(-spawnArea.y, spawnArea.y); //Y�� �������� �����ϰ� ��ġ ����

                //���� ���� ��ġ�� ��� �̹��� ����
                SoundManager.Instance.PlayWarningSound();
                GameObject warning = CreateWarning(warningPosition);
                yield return StartCoroutine(BlinkEffect(warning));
                Destroy(warning);

                //��ƼŬ ���� (��ġ: ���� warningPosition�� �߻��� ��ġ���� ���� �� �����ʿ��� ����)
                Vector2 spawnPosition = mainCamera.ViewportToWorldPoint(new Vector2(1.1f, mainCamera.WorldToViewportPoint(warningPosition).y));
                ParticleSystem newParticle = Instantiate(particle, spawnPosition, Quaternion.identity);
                newParticle.Play();
                activeParticles.Enqueue((newParticle, Time.time)); //������ ��ƼŬ�� ���� �ð� �߰�
            }

            yield return waitForSpawnInterval;
        }
    }

    private void FixedUpdate()
    {
        int particleCount = activeParticles.Count;

        //��� Ȱ��ȭ�� ��ƼŬ �̵� ó��
        for (int i = 0; i < particleCount; i++)
        {
            var (particle, spawnTime) = activeParticles.Dequeue(); //ť���� ù��° ��ƼŬ�� �����ð��� ����

            if (particle != null && particle.isPlaying)
            {
                particle.transform.position += Vector3.left * moveSpeed * Time.deltaTime; //�������� �̵�

                //5�ʰ� ������ ��ƼŬ�� ����
                if (Time.time - spawnTime > particleLifetime)
                {
                    Destroy(particle.gameObject);
                }
                else
                {
                    //���� �����ð�(5��)�� ���������� ��ƼŬ�� ť�� �ٽ� �߰�
                    //����ؼ� �������� �������� �ϱ� ������
                    activeParticles.Enqueue((particle, spawnTime));
                }
            }
        }
    }

    private GameObject CreateWarning(Vector3 position)
    {
        //��� �̹����� ǥ���� GameObject ����
        GameObject warning = new GameObject("ParticleWarning");
        SpriteRenderer spriteRenderer = warning.AddComponent<SpriteRenderer>();

        spriteRenderer.sprite = warningSprite;
        spriteRenderer.color = Color.red;
        warning.transform.localScale = warningSize; //�̹��� ũ�� ����
        warning.transform.position = position; //��ġ ����

        warning.transform.SetParent(mainCamera.transform);

        return warning;
    }

    private IEnumerator BlinkEffect(GameObject warning)
    {
        SpriteRenderer renderer = warning.GetComponent<SpriteRenderer>(); //�̹��� ��������

        float elapsedTime = 0f;
        bool isVisible = true;

        while (elapsedTime < warningDuration) //����� �ð��� warningDuration���� ���� ��
        {
            renderer.enabled = isVisible;
            isVisible = !isVisible;
            elapsedTime += blinkInterval;
            yield return new WaitForSeconds(blinkInterval);
        }

        renderer.enabled = true;
    }
}
