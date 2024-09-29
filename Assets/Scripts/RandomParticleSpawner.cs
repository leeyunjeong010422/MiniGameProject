using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomParticleSpawner : MonoBehaviour
{
    [SerializeField] ParticleSystem particle;
    [SerializeField] float spawnInterval = 3f; //3�ʸ��� ����
    [SerializeField] Vector2 spawnArea = new Vector2(0f, 1f); //���� ����(Y�ุ ����)
    [SerializeField] float moveSpeed = 80f;
    [SerializeField] float particleLifetime = 5f; //��ƼŬ ���� �ð�

    private Camera mainCamera;
    private List<(ParticleSystem particle, float spawnTime)> activeParticles = new List<(ParticleSystem, float)>(); // ���� Ȱ��ȭ�� ��ƼŬ ��ϰ� ���� �ð��� ����

    private void Start()
    {
        mainCamera = Camera.main;
        StartCoroutine(SpawnParticles());
    }

    private IEnumerator SpawnParticles()
    {
        while (true)
        {
            //ī�޶� ����Ʈ���� ������ �� �κ��� ���� ��ǥ(����Ʈ ��ǥ (1, y, z))
            Vector3 spawnPosition = mainCamera.ViewportToWorldPoint(new Vector3(1, Random.Range(0f, 1f), 10));
            spawnPosition.z = 0;
            spawnPosition.y += Random.Range(-spawnArea.y, spawnArea.y); //Y�� �������� �����ϰ� ��ġ ����

            // ��ƼŬ ����
            ParticleSystem newParticle = Instantiate(particle, spawnPosition, Quaternion.identity);
            newParticle.Play();
            activeParticles.Add((newParticle, Time.time)); //������ ��ƼŬ�� ���� �ð� �߰�

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void FixedUpdate()
    {
        //��� Ȱ��ȭ�� ��ƼŬ �̵� ó��
        for (int i = activeParticles.Count - 1; i >= 0; i--)
        {
            var (particle, spawnTime) = activeParticles[i];

            if (particle != null && particle.isPlaying)
            {
                particle.transform.position += Vector3.left * moveSpeed * Time.deltaTime; //�������� �̵�

                //5�ʰ� ������ ��ƼŬ�� ����
                if (Time.time - spawnTime > particleLifetime)
                {
                    activeParticles.RemoveAt(i);
                    Destroy(particle.gameObject);
                }
            }
        }
    }
}
