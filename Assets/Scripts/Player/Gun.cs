using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] BulletPool bulletPool;
    [SerializeField] float fireRate; //�Ѿ� �߻� ����

    private void Start()
    {
        StartCoroutine(FireBullets());
    }

    public IEnumerator FireBullets()
    {
        while (true) //����ؼ� �Ѿ��� �߻���
        {
            //����Ƽ���� ������ ���ݸ�ŭ ���
            yield return new WaitForSeconds(fireRate);

            //�Ѿ��� ������
            GameObject bullet = bulletPool.GetBullet();

            if (bullet != null)
            {
                //�Ѿ��� ���� ��ġ, ���� ����� ��ġ��Ŵ
                bullet.transform.position = transform.position;
                bullet.transform.rotation = transform.rotation;

                bullet.SetActive(true); //�߻��� �Ѿ� Ȱ��ȭ

                StartCoroutine(DeactivateDelay(bullet, 1f)); //�Ѿ��� 1�� �Ŀ� ��Ȱ��ȭ�ϰ� Ǯ�� ��ȯ
            }
        }
    }

    //������ �ð���ŭ ��� �Ŀ� ����� �Ѿ��� ��Ȱ��ȭ�ϰ� �ٽ� Ǯ�� ��ȯ����
    private IEnumerator DeactivateDelay(GameObject bullet, float time)
    {
        yield return new WaitForSeconds(time);
        bulletPool.ReturnBullet(bullet); //�Ѿ��� Ǯ�� ��ȯ�Ͽ� ��Ȱ��ȭ��
    }
}
