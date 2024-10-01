using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs; //�� ������ �迭 (���� ������ �� �߰� ����)
    [SerializeField] private int poolSize = 5; //Ǯ ũ��
    private Queue<GameObject> enemyPool;

    private void Awake()
    {
        enemyPool = new Queue<GameObject>();

        //Ǯ�� �� ����
        for (int i = 0; i < poolSize; i++)
        {
            //�����ϰ� �� ������ ����
            GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.transform.parent = transform;
            enemy.SetActive(false);
            enemyPool.Enqueue(enemy);
        }
    }

    //���� �������� �޼���
    public GameObject GetEnemy()
    {
        if (enemyPool.Count > 0)
        {
            GameObject enemy = enemyPool.Dequeue();
            enemy.SetActive(true);
            return enemy;
        }
        return null; //����� �� �ִ� ���� ����
    }

    //���� ��ȯ�ϴ� �޼���
    public void ReturnEnemy(GameObject enemy)
    {
        GunEnemyController enemyController = enemy.GetComponent<GunEnemyController>();
        if (enemyController != null)
        {
            enemyController.Reset();
        }

        ArrowEnemyController enemyController1 = enemy.GetComponent<ArrowEnemyController>();
        if (enemyController1 != null)
        {
            enemyController1.Reset();
        }

        enemy.SetActive(false);
        enemyPool.Enqueue(enemy); //ť�� �ٽ� �߰�
    }
}
