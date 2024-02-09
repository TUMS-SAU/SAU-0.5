using UnityEngine;
using UnityEngine.SceneManagement;

public class PrevButton : MonoBehaviour
{
    // 뒤로가기 버튼 클릭 시 호출할 함수
    public void GoBack()
    {
        // 현재 Scene의 인덱스를 가져옵니다.
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // 이전 Scene의 인덱스로 돌아갑니다. 만약 현재 Scene이 첫 번째 Scene이라면 아무 동작도 수행하지 않습니다.
        if (currentSceneIndex > 0)
        {
            SceneManager.LoadScene(currentSceneIndex - 1);
        }
    }
}
