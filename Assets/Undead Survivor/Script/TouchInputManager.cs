using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TouchInputManager : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // 터치 입력이 발생하는지 확인
        if (Input.touchCount > 0)
        {
            // 첫 번째 터치를 가져옴
            Touch touch = Input.GetTouch(0);

            // 터치 상태가 시작된 경우
            if (touch.phase == TouchPhase.Began)
            {
                // UI 버튼을 누른 경우
                if (IsPointerOverUIObject(touch.position))
                {
                  
                }
                else
                {
                    SceneManager.LoadScene("CharacterScene");
                    // 다음 장면으로 전환
                    //SwitchToNextScene();
                }
                // 다음 장면으로 전환
                //SwitchToNextScene();
            }
        }
    }

    void SwitchToNextScene()
    {
        // 현재 장면의 인덱스를 가져와서 다음 장면의 인덱스로 전환
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = (currentSceneIndex + 1) % SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene(nextSceneIndex);
    }

    // UI 버튼 위에 터치가 있는지 확인하는 메서드
    bool IsPointerOverUIObject(Vector2 touchPosition)
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = touchPosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        return results.Count > 0;
    }
}





