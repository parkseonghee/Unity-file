using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class MonsterSensor : MonoBehaviour
{
    [Header("Sensor Settings(2D)")]
    [Tooltip("자식 오브젝트로 있는 감지 범위의 중심점")]
    [SerializeField] private Transform sensorCenter;

    [Header("Line of Sight")]
    [Tooltip("투사체의 크기에 맞춰 레이저의 두깨(반지름)을 설정하시오")]
    [SerializeField] private float sightThickness = 0.2f;

    [Header("Layer Masks")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask obstacleLayer;


    //감지된 플레이어를 기억해둘 변수
    private Transform currentTarget;

    //감지 범위와 공격 사거리 정보를 가져오기
    private MonsterStatHandler statHandler;
    public void Initialize(MonsterStatHandler stat)
    {
        statHandler = stat;
    }

    //1. 플레이어가 [감지 범위]에 들어왔는가? (어그로 범위)
    public bool CheckPlayerInDetectionRange()
    {
        //핸들러에게 최종 감지 범위를 물어본다
        float currentDetectionRange = statHandler.GetDetectionRange();
        Collider2D hit = Physics2D.OverlapCircle(sensorCenter.position, currentDetectionRange, playerLayer);

        if (hit != null)
        {
            currentTarget = hit.transform;
            return true;
        }
        currentTarget = null;
        return false;
    }

    //2. 플레이어가 [공격 사거리]에 들어왔는가?
    public bool CheckPlayerInAttackRange()
    {
        if (currentTarget == null) return false;

        float currentAttackRange = statHandler.GetAttackRange();
        float distanceToPlayer = Vector2.Distance(sensorCenter.position, currentTarget.position);

        return distanceToPlayer <= currentAttackRange;
    }

    //3. 플레이어한테 레이저를 쐈는데 벽(구조물 오브젝트)가 없는가?
    public bool HasClearLineOfSight()
    {
        if (currentTarget == null) return false;

        //2D이기 때문에 Vector2로 방향과 거리를 계산
        Vector2 startPos = transform.position;
        Vector2 targetPos = currentTarget.position;

        Vector2 directionToPlayer = (targetPos - startPos).normalized;
        float distanceToPlayer = Vector2.Distance(startPos, targetPos);

        ////RaycastHit2D 구조체를 이용하여 부딪힌 결과를 반환받기         ***별로다***
        //RaycastHit2D hitInfo = Physics2D.Raycast(startPos, directionToPlayer, distanceToPlayer, obstacleLayer);
        ////hitInfo.collider가 null이 아니면 구조체 오브젝트에 맞았기 때문에 False이다.
        //return hitInfo.collider == null;

        //파이프를 발사하여 판정 범위의 두께감을 주는걸로 변경
        RaycastHit2D hitInfo = Physics2D.CircleCast(startPos, sightThickness, directionToPlayer, distanceToPlayer, obstacleLayer);
        return hitInfo.collider == null;    //파이프에 걸리는 벽이 없어야 True
    }

    public Transform GetTarget()
    { return currentTarget; }

    //디버깅용 함수
    private void OnDrawGizmosSelected()
    {
        if (sensorCenter == null || statHandler == null) return;

        //1. 감지 범위 그리기(초록색)
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(sensorCenter.position, statHandler.GetDetectionRange());
        //2. 공격 사거리 그리기 (주황색)
        Gizmos.color = new Color(1f, 0.5f, 0f);  //주황색
        Gizmos.DrawWireSphere(sensorCenter.position, statHandler.GetAttackRange());

        //2. 타겟이 있을 때 레이저 그리기
        if (currentTarget != null)
        {
            //시야 확인용 레이저 (빨간색)
            Gizmos.color= Color.red;
            Gizmos.DrawLine(transform.position, currentTarget.position);
        }
    }
}
