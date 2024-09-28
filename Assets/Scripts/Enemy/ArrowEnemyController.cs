using UnityEngine;

//ȭ���� ��� ��
public class ArrowEnemyController : BaseEnemyController
{
    [SerializeField] private GameObject armObject;
    [SerializeField] private GameObject arrowObject;

    protected override void Start()
    {
        base.Start();
        armObject.SetActive(true);
        arrowObject.SetActive(true);
    }

    public override void Die()
    {
        base.Die();
        armObject.SetActive(false);
        arrowObject.SetActive(false);
    }
}
