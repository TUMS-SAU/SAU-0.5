using UnityEngine;
using UnityEngine.UI;

public class Alert : MonoBehaviour
{
    public GameObject alertPanel; // 경고창을 담고 있는 패널
    public Text alertText; // 경고창에 표시할 텍스트

    private void Start()
    {
        // 시작 시 경고창을 숨김
        alertPanel.SetActive(false);
    }

    public void OnButtonClick()
    {
        // 버튼 클릭 시 호출되는 함수
        ShowAlert("다음 업데이트 때 추가될 예정입니다!");
    }

    public void ShowAlert(string message)
    {
        // 경고창을 보여주는 함수
        alertText.text = message; // 경고창에 메시지 설정
        alertPanel.SetActive(true); // 경고창을 활성화

        // 2초 후에 경고창을 숨김
        Invoke("HideAlert", 2f);
    }

    private void HideAlert()
    {
        // 경고창을 숨기는 함수
        alertPanel.SetActive(false);
    }
}
