using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof(Rigidbody2D))]
public class MonsterMovement2D : MonoBehaviour
{
    private Rigidbody2D rb;
    private NavMeshAgent agent;
    private MonsterStatHandler statHandler;
    private MonsterSensor sensor;

    //MonsterBrain스크립트가 실행될 때(몬스터 스폰 등) 이 스크립트를 초기화 해주는 함수
    public void Initialize(MonsterStatHandler stat, MonsterSensor snes)
    {
        rb = GetComponent<Rigidbody2D>();
        agent = GetComponent<NavMeshAgent>();
        statHandler = stat;
        sensor = snes;

        //몬스터가 굴러다니지 않게 Z축 회전을 방지하는 코드
        rb.freezeRotation = true;
        //탑뷰 게임이기에 혹시 모를 중력 초기화 코드
        rb.gravityScale = 0f;

        //2D 네비게이션 핵심 세팅
        agent.updateRotation = false;   //몬스터가 3D처럼 뒤집어지는걸 막음뭐
        agent.updateUpAxis = false;     //2D게임에 맞게 축을 고정시킴

        //좌표권한을 혼동시키지 않게 통제권을 False
        agent.updatePosition = false;
    }

    //브레인이 이동하라 명령할 때 호출될 함수
    public void MoveTowardsTarget()
    {
        Transform target = sensor.GetTarget();
        if (target == null) return; //타겟이 없으면 나타날수도 있는 에러를 방지한다.

        //1. 네비게이션 시스템에 목적지(플레이어)를 입력
        agent.SetDestination(target.position);

        //2. 에이전트가 찍어준 '다음으로 꺾어야 할 가장 가까운 점을 가져옴
        Vector2 nextWypoint = agent.steeringTarget;

        //3. 해당 경유지를 향해 방향을 잡고 물리적으로 걷는다
        Vector2 direction = (nextWypoint - (Vector2)transform.position).normalized;
        //이동속도 가져오기 (캡슐화된 스탯에서 가져옴: MonsterStatData)
        float speed = statHandler.GetMoveSpeed();
        //물리적 이동(속도 대입)
        rb.linearVelocity = direction * speed;

        //4. 에이전트의 가상 위치를 몬스터의 실제 물리적 위치와 계속 동기화
        agent.nextPosition = transform.position;
    }

    //브레인이 머추라고 명령할 때 호출될 함수
    public void StopMoving()
    {
        rb.linearVelocity = Vector2.zero;

        //멈출 때는 에이전트의 경로 연산도 멈춰서 최적화
        if (agent.isOnNavMesh)
        {
            agent.ResetPath();
        }
    }
}
