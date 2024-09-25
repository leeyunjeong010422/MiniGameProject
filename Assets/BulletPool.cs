using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] int poolSize = 10; //�Ѿ��� ���� (Ǯ ũ��)

    private Queue<GameObject> bulletPool; //�Ѿ��� ����? ����?�� ť ����

    private void Awake()
    {
        bulletPool = new Queue<GameObject>();

        //�Ѿ��� �����ϰ� ��Ȱ��ȭ ��Ű�� ť�� �߰���
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            bulletPool.Enqueue(bullet);
        }
    }

    public GameObject GetBullet()
    {
        if (bulletPool.Count > 0) //�Ѿ��� Ǯ�� ������� ���� ��
        {
            //�Ѿ��� ������ Ȱ��ȭ
            GameObject bullet = bulletPool.Dequeue();
            bullet.SetActive(true);
            return bullet; //���� �Ѿ� ��ȯ
        }
        return null; //����� �� �ִ� �Ѿ��� ������ null ��ȯ
                     //(����� �� �ִ� �Ѿ��� 10���̰� 0.5�ʸ��� �����̱� ������ ������ ���� ������ �ʿ� X)
    }

    //�Ѿ��� ��Ȱ��ȭ ��Ű�� �ٽ� ť�� �߰���
    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        bulletPool.Enqueue(bullet);
    }
}
