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
                Vector2 spawnPosition = mainCamera.ViewportToWorldPoint(new Vector2(1, Random.Range(0.2f, 0.8f)));
                spawnPosition.y += Random.Range(-spawnArea.y, spawnArea.y); //Y�� �������� �����ϰ� ��ġ ����

                // ��ƼŬ ����
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
}
