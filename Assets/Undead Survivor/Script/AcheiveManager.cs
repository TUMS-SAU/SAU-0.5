using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Rendering;

public class AcheiveManager : MonoBehaviour
{
    // 잠금, 해금 버튼들을 담을 변수 추가 및 초기화
    public GameObject[] lockCharacter;
    public GameObject[] unlockCharacter;
    // 알림 오브젝트를 저장할 변수 선언 및 초기화
    public GameObject uiNotice;
    enum Acheive {UnlockPotato, UnlockBean} //업적 데이터와 같은 열거형 enum 작성
    Acheive[] acheives; //업적 데이터들을 저장해둘 배열 선언 및 초기화
    WaitForSecondsRealtime wait; //멈추지 않는 시간 동안 시간이 갈 수 있도록 변수 설정

    void Awake()
    {
        //Enum값을 acheives 배열에 넣기
        ///앞에 Acheive[]를 추가해서 배열이라고 명시하기
        acheives = (Acheive[])Enum.GetValues(typeof(Acheive));
        //Enum.GetValues : 주어진 열거형의 데이터를 모두 가져오는 함수
        // Enum.GetValues 앞에 타입을 명시적으로 지정하여 타입 맞추기

        wait = new WaitForSecondsRealtime(5); //5초간 해금 알림 UI 띄워주기 위한 변수

        //HasKey 함수로 데이터 유무 체크 후 초기화 실행
        if(!PlayerPrefs.HasKey("MyData")){
            Init(); //저장된 데이터가 있으면 초기화 함수를 진행하지 않음
        }
    }

    // 저장 데이터 초기화 함수 작성
    void Init()
    {
        // PlayerPrefs : 간단한 저장 기능을 제공하는 유니티 제공 클래스
        PlayerPrefs.SetInt("MyData",1);
        //데이터 저장을 시작했다는 의미의 int형 데이터
        //SetInt 함수를 사용하여 key와 연결된 int형 데이터를 저장

        // foreach를 활용하여 순차적으로 데이터 저장
        //아직 달성하지 않은 데이터의 key를 0으로 저장
        //0 : false, 1 : true
        foreach (Acheive acheive in acheives) {
            PlayerPrefs.SetInt(acheive.ToString(),0);
        }
        //이렇게 일일히 할 필요 없이 위에 처럼 하면 됨
        // 업적 데이터와 동일한 이름의 key로 0을 저장
        //PlayerPrefs.SetInt("UnlockPotato",0);
        //PlayerPrefs.SetInt("UnlockBean",0);
    }

    void Start()
    {
        UnlockCharacter();
    }

    // 캐릭터 버튼 해금을 위한 함수 새롭게 작성
    void UnlockCharacter()
    {
        // 잠금 버튼 배열을 순회하면서 인덱스에 해당하는 업적 이름 가져오기
        for (int index = 0; index < lockCharacter.Length; index++){
            string acheiveName = acheives[index].ToString();
            bool isUnlock = PlayerPrefs.GetInt(acheiveName) == 1;  //isUnlock 변수를 true로 설정

            //GetInt 함수로 저장된 업적 상태를 가져와서 버튼 활성화에 적용

            lockCharacter[index].SetActive(!isUnlock);  //잠겨있으면 false
            unlockCharacter[index].SetActive(isUnlock);  //열려있으면 true
        }
    }

    void LateUpdate()
    {
        // 모든 업적 확인을 위한 반복문을 LateUpdate에 작성
        foreach (Acheive acheive in acheives){
            CheckAcheive(acheive);
        }
    }

    // 업적 달성을 위한 함수 새롭게 작성
    void CheckAcheive(Acheive acheive)
    {
        bool isAcheive = false;

        // switch~case문으로 각 업적 달성 조건을 작성
        switch (acheive){
            case Acheive.UnlockPotato: //킬수가 10마리 이상이면 언락
                if(GameManager.instance.isLive){ //단, 마무리시에 일괄 죽임으로는 안되도록 
                    isAcheive = GameManager.instance.kill >= 10;} 
                break;
    
            case Acheive.UnlockBean: //최대시간까지 버티면 언락
                isAcheive = GameManager.instance.gameTime == GameManager.instance.maxGameTime;
                break;
        }

        //해당 업적이 처음 달성했다는 조건을 if문에 작성
        if (isAcheive && PlayerPrefs.GetInt(acheive.ToString())==0){
            PlayerPrefs.SetInt(acheive.ToString(), 1);
            for (int index = 0; index < uiNotice.transform.childCount; index++){
                bool isActive = index == (int)acheive; //알림 창의 자식 오브젝트를 순회하면서 순번이 맞으면 활성화
                uiNotice.transform.GetChild(index).gameObject.SetActive(isActive);
            }
            StartCoroutine(NoticeRoutine());
        }
    }

    // 알림 창을 활성화했다가 일정 시간 이후 비활성화하는 코루틴 생성
    IEnumerator NoticeRoutine()
    {
        uiNotice.SetActive(true);

        //효과음을 재생할 부분마다 재생함수 호출
        AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);
        
        ///yield return new WaitForSeconds(5);
        yield return wait;

        uiNotice.SetActive(false);
    }
}
