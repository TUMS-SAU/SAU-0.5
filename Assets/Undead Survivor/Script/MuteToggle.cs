using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuteToggle : MonoBehaviour
{
    public Toggle muteToggle; // 음소거 토글
    public Button soundButton; // 음소거 버튼
    public Sprite soundOnSprite; // 소리 켜진 버튼 이미지
    public Sprite soundOffSprite; // 음소거된 버튼 이미지

    private bool isMuted = false; // 음소거 상태 여부

    void Start()
    {
        // 토글에 이벤트 리스너 추가
        muteToggle.onValueChanged.AddListener(OnMuteToggleChanged);

        // 버튼에 클릭 이벤트 추가
        soundButton.onClick.AddListener(OnSoundButtonClick);
    }

    // 음소거 토글 상태 변경 시 호출되는 메서드
    void OnMuteToggleChanged(bool value)
    {
        isMuted = value;

        // 음소거 상태에 따라 버튼 이미지 변경
        if (isMuted)
        {
            soundButton.image.sprite = soundOffSprite;
        }
        else
        {
            soundButton.image.sprite = soundOnSprite;
        }

        // 여기에 음소거 상태에 따른 추가적인 처리를 추가할 수 있음
    }

    // 버튼 클릭 시 호출되는 메서드
    void OnSoundButtonClick()
    {
        // 음소거 상태 토글
        isMuted = !isMuted;

        // 음소거 상태에 따라 버튼 이미지 변경
        if (isMuted)
        {
            soundButton.image.sprite = soundOffSprite;
        }
        else
        {
            soundButton.image.sprite = soundOnSprite;
        }

        // Toggle의 상태도 변경
        muteToggle.isOn = isMuted;

        // 여기에 음소거 상태에 따른 추가적인 처리를 추가할 수 있음
    }
}

