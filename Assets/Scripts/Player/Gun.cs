using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] BulletPool bulletPool;
    [SerializeField] float fireRate; //�Ѿ� �߻� ����

    private WaitForSeconds fireRateDelay;
    private WaitForSeconds deactivateDelay;

    private void Start()
    {
        fireRateDelay = new WaitForSeconds(fireRate);
        deactivateDelay = new WaitForSeconds(1.3f);

        StartCoroutine(FireBullets());
    }

    public IEnumerator FireBullets()
    {
        while (true) //����ؼ� �Ѿ��� �߻���
        {
            //����Ƽ���� ������ ���ݸ�ŭ ���
            yield return fireRateDelay;

            //�Ѿ��� ������
            GameObject bullet = bulletPool.GetBullet();

            if (bullet != null)
            {
                //�Ѿ��� ���� ��ġ, ���� ����� ��ġ��Ŵ
                bullet.transform.position = transform.position;
                bullet.transform.rotation = transform.rotation;

                bullet.SetActive(true); //�߻��� �Ѿ� Ȱ��ȭ

                SoundManager.Instance.PlayGunSound(); //�߻��� ������ ���� �߻�

                StartCoroutine(DeactivateDelay(bullet)); //�Ѿ��� 1�� �Ŀ� ��Ȱ��ȭ�ϰ� Ǯ�� ��ȯ
            }
        }
    }

    //������ �ð���ŭ ��� �Ŀ� ����� �Ѿ��� ��Ȱ��ȭ�ϰ� �ٽ� Ǯ�� ��ȯ����
    private IEnumerator DeactivateDelay(GameObject bullet)
    {
        yield return deactivateDelay;
        bulletPool.ReturnBullet(bullet); //�Ѿ��� Ǯ�� ��ȯ�Ͽ� ��Ȱ��ȭ��
    }
}
