using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowEnemyFire : BaseEnemyFire
{
    //�ʿ��� �߰� ����� �ִٸ� ����
    //����� BaseEnemyFire�� �ִ� �ŷθ� ���

    protected override IEnumerator FireBullets()
    {
        while (true)
        {
            yield return new WaitForSeconds(fireRate);

            GameObject bullet = bulletPool.GetBullet();

            if (bullet != null)
            {
                bullet.transform.position = transform.position;
                bullet.transform.rotation = transform.rotation;

                bullet.SetActive(true);

                ArrowSound();

                StartCoroutine(DeactivateDelay(bullet, 2f));
            }
        }
    }
}