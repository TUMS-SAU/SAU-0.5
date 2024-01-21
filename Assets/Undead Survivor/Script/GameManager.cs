using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
//using UnityEditor.SearchService;
using UnityEngine;
// 장면 관리를 사용하기 위해 SceneManagement 네임스페이스 추가
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //GameManager에 Player를 만들어서 관리한다. 
    //GameManager를 정적변수로 만들어서 관리하기 편하도록 할것이다. 
    //정적변수는 즉시 클래스에서 부를 수 있다는 편리함이 있다. 
    //Static으로 만든 변수는 하이라키에 보이지 않는다. 
    //GameManager를 메모리에 올려서 관리한다. 
    // static : 정적으로 사용하겠다는 키워드로, 바로 메모리에 얹어버린다
    // static으로 선언된 변수는 인스펙터에 나타나지 않는다
    // 정적 변수는 즉시 클래스에서 부를 수 있는 편리함이 있다
    public static GameManager instance;
    
    // Header : 인스펙터의 속성들을 이쁘게 구분시켜주는 타이틀
    [Header("# Game Control")]
    public bool isLive; //시간이 정지해 있는지를 판단하기 위해서 변수 선언
    
    //레벨을 조정하기 위해서 게임시간과 최대게임시간 변수를 선언
    public float gameTime;
    public float maxGameTime = 2 * 10f;

    [Header("# Player Info")]
    // 게임매니저에서 캐릭터 ID를 저장할 변수 선언
    public int playerId;
    // 게임 매니저에서 체력, 최대 체력 변수 선언
    // 게임매니저의 생명력 관련 변수는 float로 변경
    public float health;
    public float maxHealth = 100;
    // 게임 매니저에 레벨, 킬수, 경험치 변수 선언
    public int level;   
    public int kill;
    public int exp;
    //각 레벨의 필요 경험치를 보관할 배열 변수 선언 및 초기화
    // 테스트
    public int[] nextExp = {3, 5, 10, 100, 150, 210, 280, 360, 450, 600};
    //public int[] nextExp = { 10, 30, 60, 100, 150, 210, 280, 360, 450, 600 };

    [Header("#Game Object")]
    // 다양한 곳에서 쉽게 접근할 수 있도록 GameManager에 PoolManager 추가
    public PoolManager pool;
    // 플레이어 타입의 공개 변수 선언
    public Player player;
    // 게임매니저에 레벨업 변수 선언 및 초기화
    public LevelUp uiLevelUp;
    // 게임결과 UI 오브젝트를 저장할 변수 선언 및 초기화
    //public GameObject uiResult;
    // 게임매니저의 기존 변수의 타입을 스크립트로 변경
    public Result uiResult;
    // 게임매니저에서 조이스틱 오브젝트 변수 추가하고 초기화
    public Transform uiJoy;
    // 게임 승리할 때 적을 정리하는 클리너 변수 선언 및 초기화
    public GameObject enemyCleaner;


    //초기화를 스크립트 내에서 해줘야햠 
    void Awake()
    {
        // Awake 생명주기에서 인스턴스 변수를 자기자신 this로 초기화
        instance = this; //정적인 변수 자기자신을 집어넣어야 함
        //게임매니저에서 targetFrameRate속성을 직접 설정, 지정해주지 않으면 기본 30
        Application.targetFrameRate = 60; 
    }

    //void Start()
    // 게임매니저의 기존 Start 함수를 GameStart로 변경
    //public void GameStart()
    // 게임 시작 함수에는 int 매개변수 추가
    public void GameStart(int id) //int 변수를 추가
    {
        playerId = id;
        // 시작할 때 현재 체력과 최대 체력이 같도록 로직 추가
        health = maxHealth;

        // 게임 시작할 때 플레이어 활성화 후 기본 무기 지급
        //플레이어 활성화
        player.gameObject.SetActive(true);

        // 임시 스크립트 (첫 번째 캐릭터 선택)
        //uiLevelUp.Select(0);
        //기본 무기 지급을 위한 함수 호출에서 인자 값을 캐릭터 ID로 변경
        uiLevelUp.Select(playerId % 2);

        // 게임 시작 함수 내에 시간 재개 함수 호출
        Resume();

        // 게임 시작 부분과 종료 부분에 해당 함수 호출
        //배경음 시작을 게임 시작부분에 호출 
        AudioManager.instance.PlayBgm(true);
        //효과음을 재생할 부분마다 재생함수 호출
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
    }

    // 게임오버 담당 함수 작성
    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    //딜레이를 위해 게임오버 코루틴도 작성
    IEnumerator GameOverRoutine()
    {
        //작동을 멈추기
        isLive = false;

        //0.5초를 기다리고
        yield return new WaitForSeconds(0.5f);

        // 게임결과 UI 오브젝트를 게임오버 코루틴에서 활성화
        //uiResult.SetActive(true);
        // 게임매니저의 기존 변수의 타입을 스크립트로 변경
        uiResult.gameObject.SetActive(true);
        uiResult.Lose();
        //완전히 다 멈추기
        Stop();

        // 게임 시작 부분과 종료 부분에 해당 함수 호출
        //배경음 종료를 게임 종료부분에 호출 
        AudioManager.instance.PlayBgm(false);
        //효과음을 재생할 부분마다 재생함수 호출
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Lose);
    }

    // 게임 승리 로직은 게임오버 로직을 복사해서 편집
    public void GameVictory()
    {
        StartCoroutine(GameVictoryRoutine());
    }

    //딜레이를 위해 게임오버 코루틴도 작성
    IEnumerator GameVictoryRoutine()
    {
        //작동을 멈추기
        isLive = false;
        // 게임 승리 코루틴의 전반부에 적 클리너를 활성화
        enemyCleaner.SetActive(true);

        //0.5초를 기다리고
        yield return new WaitForSeconds(1.5f);

        uiResult.gameObject.SetActive(true);
        uiResult.Win();
        //완전히 다 멈추기
        Stop();

        //배경음 종료를 게임 종료부분에 호출 
        AudioManager.instance.PlayBgm(false);
        //효과음을 재생할 부분마다 재생함수 호출
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Win);
    }

    // 게임매니저에 게임재시작 함수 작성
    public void GameRetry()
    {
        // 장면 관리를 사용하기 위해 SceneManagement 네임스페이스 추가
        // LoadScene : 이름 혹은 인덱스로 장면을 새롭게 부르는 함수
        // File > Build Settings으로 가면 Scene의 인덱스를 볼 수 있다
        SceneManager.LoadScene(0);
    }

    // 종료 버튼의 기능을 담당하는 함수를 게임매니저에 추가
    public void GameQuit()
    {
        Application.Quit(); //게임을 종료하는 함수 실행 
        //에디터를 종료하는 기능이 아니므로, 빌드 버전에서만 작동
    }

     void Update()
    {
        // 각 스크립트의 Update 계열 로직에 조건 추가하기
        if (!isLive)
            return; //isLive가 아닌데 Update 시에 시간이 추가되지 않도록 조건 추가

        // Update에서 deltaTime 더하기
        gameTime += Time.deltaTime;

        //0.2초에 적 한마리씩 생성
        if (gameTime > maxGameTime){
            gameTime = maxGameTime;
            // 게임 시간이 최대 시간을 넘기는 때에 게임승리 함수 호출
            GameVictory();
        }

    }

    // 경험치 증가 함수 새로 작성
    public void GetExp()
    {
        // 경험치 얻는 함수에도 isLive 필터 추가
        if (!isLive) //EnemyCleaner 발동시 경험치 얻지 못하게 처리
            return;

        exp++;

        //if 조건으로 최대 필요 경험치에 도달하면 최대필요 경험치로 계속해서 레벨 업하도록 작성
        //레벨업 시 필요 경험치의 최대치가 100이면 그 다음 레벨업 시에도 똑같이 100으로 진행된다. 
        //if (exp == nextExp[level])
        // Min 함수를 사용하여 최고 경험치를 그대로 사용하도록 변경
        if (exp == nextExp[Mathf.Min(level, nextExp.Length-1)]){
            level ++;
            exp = 0;
            // 게임매니저의 레벨 업 로직에 창을 보여주는 함수 호출
            uiLevelUp.Show();
        }
    }

    // 시간 정지, 작동하는 함수 두 개 작성
    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0; //timeScale : 유니티의 시간 속도(배율)가 0배가 됨
        // 정지될 때는 크기를 0으로, 재개할 때는 크기를 1로 설정하도록 작성
        uiJoy.localScale = Vector3.zero; // 멈췄을 때 조이스틱 안보이게
    }

    public void Resume()
    {
        isLive = true;
        // 1 이상의 수를 넣으면 그만큼 빨라진다
        Time.timeScale = 1; //시간 속도 다시 1배 
        // 정지될 때는 크기를 0으로, 재개할 때는 크기를 1로 설정하도록 작성
        uiJoy.localScale = Vector3.one; // 다시 시작했을 때 조이스틱 보이게 
    }
}
