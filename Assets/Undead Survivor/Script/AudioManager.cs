using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class AudioManager : MonoBehaviour
{
    // 정적 메모리에 담기 위한 instance 변수 선언
    public static AudioManager instance;

    [Header("#BGM")]
    //배경음과 관련된 클립, 볼륨, 오디오소스 변수 선언
    public AudioClip bgmClip;
    public float bgmVolume;
    AudioSource bgmPlayer;
    AudioHighPassFilter bgmEffect; //배경음을 잠깐 멈추기 위한 변수 설정

    // 코드 복사하여 효과음 코드로 편집
    [Header("#SFX")]
    //효과음과 관련된 클립, 볼륨, 오디오소스 변수 선언
    public AudioClip[] sfxClips;
    public float sfxVolume;
    //동시다발적으로 다량의 효과음을 낼 수 있도록 채널 시스템 구축
    public int channels;
    AudioSource[] sfxPlayers;
    int channelIndex;

    // 효과음과 1:1 대응하는 열거형 데이터 선언
    // 열거형 데이터는 대응하는 숫자를 지정할 수 있어요.
    public enum Sfx { Dead, Hit, LevelUp = 3, Lose, Melee, Range = 7, Select, Win}

    void Awake()
    {
        instance = this;
        Init(); //초기화 함수
    }

    void Init()
    {
        //배경음 플레이어 초기화
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform; //배경음을 담당하는 자식 오브젝트 생성
        bgmPlayer = bgmObject.AddComponent<AudioSource>(); //AddComponent 함수로 오디오소스를 생성하고 변수에 저장
        bgmPlayer.playOnAwake = false; //시작하자마자 배경음이 나오면 안되므로 false로 설정
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;
        // 메인 카메라 접근은 Camera 클래스를 사용하면 간편해요.
        bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();

        //효과음 플레이어 초기화 
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform; //효과음을 담당하는 자식 오브젝트 생성
        //채널 개수만큼 오디오 소스를 다량으로 생성 
        sfxPlayers = new AudioSource[channels]; //채널 값을 사용하여 오디오소스 배열 초기화

        for (int index=0; index < sfxPlayers.Length; index++) //반목문으로 모든 효과음 오디오소스 생성하면서 저장
        {
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].playOnAwake = false;
            // 효과음 초기화 하는 부분에 byPassListenerEffects를 true로 변경
            sfxPlayers[index].bypassListenerEffects = true; //효과음은 배경음처럼 꺼지지 않도록 추가 설정  
            sfxPlayers[index].volume = sfxVolume;
        }
    }

    // 배경음을 재생하는 함수 작성
    public void PlayBgm(bool isPlay)
    {
        if (isPlay){
            bgmPlayer.Play();
        }
        else {
            bgmPlayer.Stop();
        }
    }

    // 필터를 켜고 끄는 함수도 작성
    public void EffectBgm(bool isPlay)
    {
        //레벨업 후 무기 선택시에 잠깐 배경음 줄이기
        bgmEffect.enabled = isPlay;
    }

    // 효과음 재생 함수 작성
    public void PlaySfx(Sfx sfx)
    {
        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            //채널 개수만큼 순회하도록 채널인덱스 변수 활용
            int loopIndex = (index + channelIndex) % sfxPlayers.Length;

            if (sfxPlayers[loopIndex].isPlaying)
                // continue : 반복문 도중 다음 루프로 건너뛰는 키워드
                continue; //반복문 도중 다음 루프로 건너뛰기

            //효과음이 2개 이상인 것은 랜덤 인덱스를 더하기
            int ranIndex = 0;
            if (sfx == Sfx.Hit || sfx == Sfx.Melee){
                ranIndex = Random.Range(0,2); //2개이므로 0과 1중 선택이 가능하도록
                //각각 개수가 다르면 switch로 진행
            }
            
            channelIndex = loopIndex;
            sfxPlayers[0].clip = sfxClips[(int)sfx + ranIndex];
            // 오디오소스의 클립을 변경하고 Play 함수 호출
            sfxPlayers[0].Play();
            break; //효과음 재생이 된 경우에는 break로 루프 종료
        }
        
        
    }
}
