using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] Animator animator;

    [SerializeField] Vector2 findSize = new Vector2(); //170, 160�� ���� ������ �����

    private WaitForSeconds delay;

    private void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        delay = new WaitForSeconds(1f);
    }

    private void Update()
    {
        BombOverlapBox();
    }

    //����ĳ��Ʈ�� �ߴ��� �÷��̾ �����ϴ� ��쿡�� �÷��̾ ã�� ���Ͽ� �ִϸ��̼� ���� X
    private void BombOverlapBox()
    {
        int playerLayerMask = LayerMask.GetMask("Player");
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, 70f, playerLayerMask);
        //Debug.DrawRay(transform.position, Vector2.left * 70f, Color.red);

        Collider2D hit = Physics2D.OverlapBox(transform.position, findSize, 0, playerLayerMask);

        if (hit != null && hit.CompareTag("Player"))
        {
            PlayerController playerController = hit.GetComponent<PlayerController>();

            if (playerController != null)
            {
                animator.Play("Bomb");
                StartCoroutine(DeactivateBomb());
            }
        }
    }

    private IEnumerator DeactivateBomb()
    {
        SoundManager.Instance.PlayBombSound();
        yield return delay;
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, findSize);
    }
}
