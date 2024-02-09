using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneAudioController : MonoBehaviour
{
    private static SceneAudioController instance;

    // Scene 전환 시에 파괴되지 않도록 설정할 오디오 소스
    public AudioSource sceneAudio;

    void Awake()
    {
        // SceneAudioController를 싱글톤으로 만들어 중복 생성을 방지
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (sceneAudio == null)
        {
            sceneAudio = GetComponent<AudioSource>();
            if (sceneAudio == null)
            {
                sceneAudio = gameObject.AddComponent<AudioSource>();
            }
        }

        PlayAudio();
    }

    void PlayAudio()
    {
        sceneAudio.Play();
    }

    // Scene 전환 시 호출되는 이벤트
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Scene 전환 시 호출되는 함수
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 현재 Scene이 SampleScene이라면 이전 Scene의 오디오를 정지
        if (scene.name == "SampleScene")
        {
            // 기존 Scene의 오디오를 멈추거나 필요에 따라 조절
            StopPreviousSceneAudio();
        }

        // 새로운 Scene에도 SceneAudioController를 유지하도록 설정
        if (sceneAudio != null)
        {
            DontDestroyOnLoad(sceneAudio.gameObject);
        }
    }

    void StopPreviousSceneAudio()
    {
        // 기존 Scene의 오디오를 정지하는 로직을 추가
        if (sceneAudio != null)
        {
            sceneAudio.Stop();
        }
    }
}