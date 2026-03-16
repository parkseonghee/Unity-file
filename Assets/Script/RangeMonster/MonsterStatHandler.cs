using UnityEngine;

public class MonsterStatHandler : MonoBehaviour
{
    [SerializeField] private MonsterStatData BaseStat;  //원본 데이터 연결

    [Header("Local Modifiers (개별 보정치)")]
    [Tooltip("감지 범위 배율 (1.0 = 기본값, 1.5 = 50% 증가)")]
    [SerializeField] private float detectionMultiplier = 1.0f;
    [Tooltip("공격 사거리 배율 (1.0 = 기본값)")]
    [SerializeField] private float attackRangeMultiplier = 1.0f;

    // 실시간으로 변하는 개별 데이터
         
    public MonsterStatData statData;
    public float currentHP;
    public float FinalDamage = 0;

    public void Initialize()
    {
        if (statData == null)
        {
            Debug.LogError("statData가 연결되지 않았습니다.", this);
            return;
        }
        currentHP = BaseStat.maxHP;
    }


    public bool TakeDamage(float incomingDamage)
    {
        
        //2. 체력이 0이하라면 사망 처리 결과 보고
        if (currentHP <= 0f)
        {
            currentHP = 0f;
            return true;    //죽음
        }
        return false;       //살음
    }
    void OnTriggerEnter2D(Collider2D collision)
    {

        //1. 실제 들어갈 데미지 계산 (방어력 적용 공식 예시)
        //데미지가 방어력보다 낮아도 최소1은 들어가게 처리
        //차후 방어력이 데미지 경감%으로 되게 변경 
        //float finalDamage = Mathf.Max(incomingDamage - BaseStat.defense, 1f);
        if (collision.gameObject.tag == "Attack")
        {
            //Attack attackScript = collision.gameObject.GetComponent<Attack>();
            Attack atk = collision.GetComponent<Attack>();

            FinalDamage = atk.attackDamgage - statData.defense;
            FinalDamage = Mathf.Max(FinalDamage, 1);
            currentHP -= FinalDamage; 

            Debug.Log("Enemy hit! Current HP: " + currentHP);
            if (currentHP <= 0)
            {
                Destroy(gameObject); // 적 제거
                Debug.Log("Enemy defeated!");
            }
        }
    }

    //외부(Brain, UI)에서 현재  체력 및 공격력을 물어볼 때 대답해주는 함수들
    public float GetCurrentHP() { return currentHP; }
    public float GetAttackPower() { return BaseStat.attackPower; }
    public float GetDefense() {  return BaseStat.defense; }
    public float GetMoveSpeed() { return BaseStat.moveSpeed; }
    public float GetAttackCooldown() { return BaseStat.attackCooldown; }
    public float GetProjectileSpeed() { return BaseStat.projectileSpeed; }

    //기본값에서 개별적인 배율을 곱해 '최종 범위'를 반환
    public float GetDetectionRange() { return BaseStat.baseDetectionRange * detectionMultiplier; }
    public float GetAttackRange() { return BaseStat.baseAttackRange * attackRangeMultiplier; }

}
