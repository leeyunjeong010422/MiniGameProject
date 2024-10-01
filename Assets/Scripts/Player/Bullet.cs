using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float bulletSpeed; //80�� ������ �� ����

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    private void Update()
    {
        //�Ѿ��� ���������� �߻�
        rb.velocity = transform.right * bulletSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //�ٸ� ������Ʈ�� �浹 �� �Ѿ� ��Ȱ��ȭ
        gameObject.SetActive(false);
    }
}
