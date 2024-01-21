using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// MonoBehaviour : 게임 로직 구성에 필요한 것들을 가진 클래스
// MonoBehaviour 안에 게임 로직을 작성할 모든 함수, 속성 포함
public class Player : MonoBehaviour
{
    // [변수의 타입] [변수의 이름];
    // 이름은 데이터가 지닌 의미를 파악할 수 있도록 짓기
    // public : 다른 스크립트에게 '공개한다'라고 선언하는 키워드
    // 값이 잘 들어오고 있는지 확인하기 위해 public 입력
    public Vector2 inputVec;
    // 속도를 편리하게 관리할 수 있도록 float 변수 추가
    public float speed; //속도 관리 변수
    // 플레이어 스크립트에서 검색 클래스 타입 변수 선언 및 초기화
    public Scanner scanner;
    // 플레이어에서 손 스크립트를 담을 배열변수 선언 및 초기화
    public Hand[] hands;
    // 플레이어 스크립트에 여러 애니메이터 컨트롤러를 저장할 배열 변수 선언
    public RuntimeAnimatorController[] animCon; //여러 애니메이터 컨트롤러를 저장할 배열 변수 

    // 게임 오브젝트의 Rigidbody 2D를 저장할 변수 선언
    // 선언부
    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;

    // 시작할 때 한 번만 실행되는 생명주기 Awake에서 초기화 진행
    void Awake()
    {
        // GetComponent<T> : 해당 오브젝트에서 컴포넌트를 가져오는 함수
        // 함수이므로 <컴포넌트 타입>()이 필요
        // 컴포넌트 Rigidbody2D가 rigid 안에 들어간다
        rigid = GetComponent<Rigidbody2D>();
        // SpriteRenderer 변수 초기화 하기
        spriter = GetComponent<SpriteRenderer>();
        // Animator 변수 초기화 하기
        anim = GetComponent<Animator>();
        // 직접 만든 스크립트도 컴포넌트로 동일하게 취급해요.
        scanner = GetComponent<Scanner>();
        // 비활성화된 오브젝트는 제외된 상태이므로 true를 넣어준다
        // 인자 값 true를 넣으면 비활성화 된 오브젝트도 OK!
        hands = GetComponentsInChildren<Hand>(true); //인자값에 true를 넣으면 비활성화된 오브젝트도 인식한다. 
    }

    // OnEnable 함수 추가 후, 애니메이터 변경 로직 추가
    void OnEnable()
    {
        // 평소 많이 활용한 클라스들의 속성들도 이런 방식으로 작성되어 있다.
        speed *= Character.Speed;
        anim.runtimeAnimatorController = animCon[GameManager.instance.playerId];
    }

    // void Update()
    // {
    //      if(!GameManager.instance.isLive)
    //          return;
    //     inputVec.x = Input.GetAxisRaw("Horizontal");
    //     inputVec.y = Input.GetAxisRaw("Vertical");
    //     // GetAxisRaw로 더욱 명확한 컨트롤 가능 딱딱 숫자가 떨어지는 컨트롤
    //     // GetAxis로 하면 자동 보정됨, 좀 미끄러짐
    // }
    
    //FixedUpdate : 물리 연산 프레임마다 호출되는 생명주기 함수
    void FixedUpdate()
    {
        if(!GameManager.instance.isLive)
            return;

        // 이동 방식 3가지
        /*
        // 1. 힘을 준다 = AddForce
        // inputVec으로 방향과 크기를 준다
        rigid.AddForce(inputVec);

        // 2. 속도 제어 = Velocity(물리적인 속도를 의미)
        rigid.velocity = inputVec;
        */

        // 플레이해보고 방향키를 누르면 Player가 너무 빨리 움직이는 것을 볼 수 있다
        // 컴퓨터마다 다르게 설정되어있는 프레임 마다 이동하는 속도도 달라질 수도 있다
        // 다른 프레임 환경에도 이동거리는 같아야 한다
        // 임시로 값을 저장해 둘 Vector2 변수 추가
        // normalized : 벡터 값의 크기가 1이 되도록 좌표가 수정된 값
        // 피타고라스 정리 이론으로 대각선은 루트 2
        // normalized 이론을 쓰지 않으면 대각선으로 훨씬 빠르게 움직이게 된다
        // fixedDeltaTime : 물리 프레임 하나가 소비한 시간
        // Time.deltaTime : Update() 함수에서 사용
        // Time.fixedDeltaTime : FixedUpdate() 함수에서 사용
        // 계산된 변수 nextVec을 MovePosition에 사용

        // 이미 Player Input에서 normalized를 사용하고 있기 때문에 normalized를 사용하지 않아도 된다
        //Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;

        // 3. 위치 이동
        // rigid.position : 현재 위치
        // 다른 프레임에도 이동 거리는 같아야 한다
        // normalized : 벡터 값의 크기가 1이 되도록 좌표가 수정된 값 
        // 만약 normalized를 쓰지 않으면 사실상 대각선을 갈 때 루트 2만큼 가므로 
        // 더 빠르게 이동한다. 
        // fixedDeltaTime : 물리 프레임 하나가 소비한 시간
        // MovePosition은 위치 이동이라 inputVec에 현재 위치(rigid.position)도 더해주어야 한다
        //rigid.MovePosition(rigid.position + inputVec);
        rigid.MovePosition(rigid.position + nextVec);
    }

    // OnMove 함수는 자동완성이 되지 않는다
    // InputValue 타입의 매개변수 작성
    void OnMove(InputValue value)
    {
        // Get<T> : 프로필에서 설정한 컨트롤 타입 T 값을 가져오는 함수
        inputVec = value.Get<Vector2>();
    }

    // LateUpdate : 프레임이 종료 되기 전 실행되는 생명주기 함수
    void LateUpdate()
    {
        if(!GameManager.instance.isLive)
            return;

        // 애니메이터에서 설정한 파라메터 타입과 동일한 함수 작성
        // SetFloat 첫 번째 인자 : 파라메터 이름
        // SetFloat 두 번째 인자 : 반영할 float 값
        // magnitude : 벡터의 순수한 크기 값
        anim.SetFloat("Speed", inputVec.magnitude); //magnitude : 백터의 순수한 크기만 주는 값

        // if : 조건이 true일 때, 자신의 코드를 실행하는 키워드
        // != : '왼쪽과 오른쪽이 서로 다릅니까?' 의미의 비교 연산자
        if (inputVec.x != 0){
            // if 문 안에 flipX 속성 바꾸기
            //spriter.flipX = true;

            // 비교 연산자의 결과를 바로 넣을 수 있다
            spriter.flipX = inputVec.x < 0; //비교연산자 
        }
    }

    //플레이어 피격 시 함수 설정    
    // 플레이어 스크립트에 OnCollisionEnter2D 이벤트 함수 작성
    void OnCollisionStay2D(Collision2D collision)
    {
        if(!GameManager.instance.isLive)
            return;

        // Time.deltaTime을 활용하여 적절한 피격 데미지 계산
        GameManager.instance.health -= Time.deltaTime * 10;
        //프레임마다 적용되어 너무 빠르게 피격피해가 나지 않도록 Time.deltaTime 활용

        // 생명력이 0보다 작다를 if 조건으로 작성
        if (GameManager.instance.health < 0)
        {
            // childCount : 자식 오브젝트의 개수
            //묘비 애니메이션을 보여줄 instance를 제외하고 비활성화
            for (int index = 2; index < transform.childCount; index++)
            {
                // GetChild : 주어진 인덱스의 자식 오브젝트를 반환하는 함수
                transform.GetChild(index).gameObject.SetActive(false);
            }

            // 애니메이서 SetTrigger 함수로 죽음 애니메이션 실행
            anim.SetTrigger("Dead");
            // 게임오버 함수를 플레이어 스크립트에의 사망 부분에서 호출하도록 작성
            GameManager.instance.GameOver();
        }
    }
}
