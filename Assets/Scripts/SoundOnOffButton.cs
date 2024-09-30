using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundOnOffButton : MonoBehaviour
{
    [SerializeField] private Sprite soundOnSprite;
    [SerializeField] private Sprite soundOffSprite;
    [SerializeField] private Button soundToggleButton;

    private Image buttonImage;

    private void Start()
    {
        //���� ���� �ִٰ� �ٽ� ó�� ȭ������ ���ƿ��� ������ ��� �� �Ǿ �߰�
        SoundManager.Instance.SetStartBGM();
        buttonImage = soundToggleButton.GetComponent<Image>();
        UpdateButtonImage();
        soundToggleButton.onClick.AddListener(ToggleSound);
    }

    private void ToggleSound()
    {
        SoundManager.Instance.ButtonToggle();
        UpdateButtonImage();
    }

    private void UpdateButtonImage()
    {
        //���� ���忡 ���� �̹��� ����
        if (SoundManager.Instance.IsSoundMuted())
        {
            buttonImage.sprite = soundOffSprite;
        }
        else
        {
            buttonImage.sprite = soundOnSprite;
        }
    }
}
