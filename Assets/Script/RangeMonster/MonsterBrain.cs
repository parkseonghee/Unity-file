using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

//몬스터의 현재 상태를 정의하는 열거형
public enum MonsterState { Idle, Move, Attack, Hit, Die}

public class MonsterBrain : MonoBehaviour
{
    [Header("Core Modules")]
    //브레인이 통솔할 부하(캡슐)들 연결
    private MonsterStatHandler statHandler;
    private MonsterSensor sensor;          //감지모듈
    private MonsterMovement2D movement;    //이동모듈
    private MonsterCombat combat;          //공격모듈
    //private AnimBridge animBridge;       //애니메이션 브릿지

    private MonsterState currentState = MonsterState.Idle;

    private void Awake()
    {
        statHandler = GetComponent<MonsterStatHandler>();
        sensor = GetComponent<MonsterSensor>();
        movement = GetComponent<MonsterMovement2D>();
        combat = GetComponent<MonsterCombat>();
    }

    private void Start()
    {
        //부하들 초기화 지시
        statHandler.Initialize();
        sensor.Initialize(statHandler);
        movement.Initialize(statHandler, sensor);
        if (combat != null){ combat.Initialize(statHandler); }
        combat.Initialize(statHandler);

        ChangeState(MonsterState.Idle);
    }



    private void Update()
    {
        if (currentState == MonsterState.Die) return;

        //애니메이션 적용 전 임시 코드 0.5초 경직 후 강제로 Idle로 돌려보내기
        if (currentState == MonsterState.Hit)
        {
            //임시 경직 처리: 다른 로직(감지, 이동)을 막고 멈추게 함
            Invoke("RecoverFromHit", 0.5f);
            return; //Update의 아래 로직을 이어서 타지 못하게 여기서 함수 종료
        }
        //1. 플레이어가 [감지 범위] 안에 들어왔는가? (OverlapCircle)
        if (sensor.CheckPlayerInDetectionRange())
        {
            //2. 플레이어가 [공격 사거리]안에도 들어왔는가?
            if (sensor.CheckPlayerInAttackRange())
            {
                //2. 구조물에 가려지지 않고 바로 보이는가? (Raycast2D)
                if (sensor.HasClearLineOfSight())
                {
                    //구조물이 없다면 공격
                    ChangeState(MonsterState.Attack);
                }
                else
                {
                    //구조물이 있다면 이동
                    ChangeState(MonsterState.Move);
                }
            }
            else
            {
                ChangeState(MonsterState.Move); //감지는 했으나 거리가 멀기 때문에 접근한다
            }
        }
        else
        {   //플레이어가 안보인다면 대기
            ChangeState(MonsterState.Idle);
        }

        if (currentState == MonsterState.Attack && combat != null)
        {
            combat.TryAttack(sensor.GetTarget());
        }
    }
    private void RecoverFromHit() { ChangeState(MonsterState.Idle); }
    
    //피격 데미지 계산 및 최종 데미지를 반환
    public void OnHitReceived(HitInfo hit)
    {
        if (currentState == MonsterState.Die) return;   //이미 죽었다면 무시

        //1. 스탯 담당자에게 계산을 위임하고 죽는지 사는지 결과를 받음
        bool isDead = statHandler.TakeDamage(hit.damage);

        //2. 결과에 따라 뇌가 새로운 상태(State)를 결정함
        if (isDead) { ChangeState(MonsterState.Die); }  //죽을 경우
        else { ChangeState(MonsterState.Hit); }         //살 경우 피격판정
    }
    //상태를 변경하는 단일 통로 함수
    public void ChangeState(MonsterState newState)
    {
        if (currentState == MonsterState.Die) return;   //죽으면 무시
        if (currentState == newState) return;           //같은 상태면 무시
        currentState = newState;

        //상태에 따른 행동 지시
        switch (currentState)
        {
            case MonsterState.Idle:
            case MonsterState.Attack:
            case MonsterState.Hit:
                movement.StopMoving();
                break;
            case MonsterState.Move:
                break;
            case MonsterState.Die:
                movement.StopMoving();
                //시체가 길을 막지 않게 콜라이더(물리 충돌)를 끄기
                GetComponent<Collider2D>().enabled = false;
                //애니메이션 추가 전 임시코드로 2초 뒤 오브젝트를 파괴
                Destroy(gameObject, 2f);
                break;
        }
    }
    private void FixedUpdate()
    {
        if (currentState == MonsterState.Move)
        {
            movement.MoveTowardsTarget();
        }
    }
}
