using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

//����: https://ruyagames.tistory.com/m/23
//inputString: ���������� ǥ�õ� ����
//duration : ������ ��� ǥ�õɶ����� �ɸ��� �ð�
//ease : ��ȯ���� �ð��� ��ȭ�� �׷����� ����
public class TextAnimator : MonoBehaviour
{
    private Text targetText;
    public string inputString;
    public float duration;
    public Ease ease;

    private void Start()
    {
        targetText = GetComponent<Text>();
        targetText.text = "";
        targetText.DOText(inputString, duration).SetEase(ease);
    }
}