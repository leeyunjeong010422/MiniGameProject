using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] EnemyPool enemyPool;
    [SerializeField] Transform player;
    [SerializeField] float spawnDistance = 20f; //���� �Ÿ�
    [SerializeField] float spawnInterval = 5f; //���� ����

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            //�� ����
            GameObject enemy = enemyPool.GetEnemy();
            if (enemy != null)
            {
                Vector3 spawnPosition = new Vector3(player.position.x + spawnDistance, -26.8f, 0);
                enemy.transform.position = spawnPosition; //���� ��ġ ����

                //10�� �Ŀ� ���� ��ȯ
                StartCoroutine(ReturnEnemyAfterDelay(enemy, 10f));
            }
        }
    }

    private IEnumerator ReturnEnemyAfterDelay(GameObject enemy, float delay)
    {
        yield return new WaitForSeconds(delay);
        enemyPool.ReturnEnemy(enemy); //�� ��ȯ
    }
}
