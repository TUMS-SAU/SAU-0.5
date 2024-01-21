using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;
// 이미지 사용하기 위해 아래 꼭 입력
using UnityEngine.UI; //이걸 받아오지 않으면 Image 사용 불가

public class Item : MonoBehaviour
{
    // 아이템 관리에 필요한 변수들 선언
    public ItemData data;  //설정한 아이템 데이터 가져오기
    public int level;
    public Weapon weapon;
    // 버튼 스크립트에서 새롭게 작성한 장비 타입의 변수 선언
    public Gear gear;

    Image icon;
    Text textLevel;
    // 아이템 스크립트에 이름과 설명 텍스트 변수 추가 및 초기화
    Text textName;
    Text textDesc;

    void Awake()
    {
        icon = GetComponentsInChildren<Image>()[1]; 
        //자식 오브젝트의 컴포넌트가 필요하므로 GetComponentsInChildren 사용
        //GetComponentsInChildren에서 두번째 값 가져오기 (첫번째는 자기자신)
        icon.sprite = data.itemIcon;

        Text[] texts = GetComponentsInChildren<Text>();
        //GetComponents의 순서는 inspector 상 계층 구조의 순서를 따라간다. 
        //따라서 inspector 상의 순서를 제대로 해줘야 text의 순서가 바뀌지 않는다. 
        textLevel = texts[0]; 
        textName = texts[1]; 
        textDesc = texts[2]; 
        textName.text = data.itemName;
    }

    // 레벨 텍스트 로직은 OnEnable로 이동
    //사라졌다가 나타났다가 할 것이므로 활성화되었을 때 자동으로 실행되는 이벤트 함수를 활용
    void OnEnable(){
        // LateUpdate에서 레벨 텍스트 갱신
        textLevel.text = "Lv." + (level + 1);

        // 아이템 타입에 따라 switch case문으로 로직 분리
        //아이템 타입별로 설명 유무가 다르므로 switch로 케이스 나누기
        switch (data.itemType){
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100, data.counts[level]);
                //데미지 % 상승을 보여줄 땐 100 곱하기
                break;
            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                textDesc.text = string.Format(data.itemDesc, data.damages[level]);
                break;
            default:
                textDesc.text = string.Format(data.itemDesc);
                break;
        }

    }
    //void LateUpdate()
    //{
    // LateUpdate에서 레벨 텍스트 갱신
    //// textLevel.text = "LV." + (level + 1); //이렇게 하면 레벨 1부터 시작
    //textLevel.text = "LV." + level; //이렇게 하면 레벨 0부터 시작
    //무기가 몇 레벨인지 측정
    //}

    // 버튼 클릭 이벤트와 연결할 함수 추가
    public void OnClick()
    {
        // 아이템 타입을 통해 switch case문 작성해두기
        switch (data.itemType){
            //여러개의 case를 붙여서 로직을 실행하게 할 수 있음
            case ItemData.ItemType.Melee: 
            case ItemData.ItemType.Range:
                //같은 로직이므로 같이 묶어서 코드 실행
                if (level == 0) {
                    GameObject newWeapon = new GameObject(); //새로운 게임오브젝트를 코드로 생성
                    //AddComponent<T> : 게임 오브젝트에 T 컴포넌트를 추가하는 모습
                    //AddComponenet 함수 반환 값을 미리 선언한 변수에 저장
                    weapon = newWeapon.AddComponent<Weapon>();
                    // Ctrl + 마우스 클릭으로 함수를 클릭하면 해당 함수 내용으로 바로 이동
                    // Weapon 초기화 함수에 스크립터블 오브젝트를 매개변수로 받아 활용
                    weapon.Init(data); //weapon의 데이터를 초기화
                }
                else {
                    float nextDamage = data.baseDamage;
                    int nextCount = 0;

                    //처음 이후의 레벨업은 데미지와 횟수를 계산
                    nextDamage += data.baseDamage * data.damages[level];
                    nextCount += data.counts[level];

                    //weapon에 작성된 레벨업 함수를 활용하여 레벨업 적용
                    weapon.LevelUp(nextDamage, nextCount);
                }
                // 레벨 값을 올리는 로직을 무기, 장비 case 안쪽으로 이동
                level++;
                break;
            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                // 무기 로직과 마찬가지로 최초 레벨업은 게임오브젝트 생성 로직을 작성
                if (level == 0) {
                    GameObject newGear = new GameObject();
                    gear = newGear.AddComponent<Gear>(); //AddComponenet 함수 반환 값을 미리 선언한 변수에 저장
                    gear.Init(data);
                }
                else{
                    float nextRate = data.damages[level];
                    gear.LevelUp(nextRate);
                }
                // 레벨 값을 올리는 로직을 무기, 장비 case 안쪽으로 이동
                level++;
                break;
            case ItemData.ItemType.Heal:
                // 치료 기능의 음료수 로직은 case 문에서 바로 작성
                GameManager.instance.health = GameManager.instance.maxHealth;
            break;
        }

        // 이후 level 값 하나 더하기
        // 레벨 값을 올리는 로직을 무기, 장비 case 안쪽으로 이동
        //level++;

        // 스크립터블 오브젝트에 작성한 레벨 데이터 개수를 넘기지 않게 로직 추가
        // Canvas > LevelUp > Item 0 ~ 4 > Inspector > Button > Interactable의 체크 표시를 없앨 것이다
        //버튼이 최대레벨로 도달하면 버튼 클릭이 불가능하도록 설정
        if (level == data.damages.Length){
            GetComponent<Button>().interactable = false;
        }
    }
}
