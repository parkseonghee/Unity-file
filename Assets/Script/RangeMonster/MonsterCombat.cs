using UnityEngine;

public class MonsterCombat : MonoBehaviour
{
    [Header("Combat Settings")]
    [Tooltip("투사체가 생성될 위치(몬스터 앞 부분에 빈 오브젝트 생성 후 연결")]
    [SerializeField] private Transform firePoint;
    [Tooltip("날아갈 투사체 프리팹")]
    [SerializeField] private GameObject projectilePrefab;

    private MonsterStatHandler statHandler;
    private float lastAttackTime = 0f;

    //Brain이 Start()에서 호출해줄 초기화 함수
    public void Initialize(MonsterStatHandler stat)
    {
        statHandler = stat;
    }
    
    //Brain이 "공격 상태"이면 매 프레임 호출할 함수
    public void TryAttack(Transform Target)
    {
        if (Target == null) return;

        //굳이?
        if (Time.time >= lastAttackTime + statHandler.GetAttackCooldown())
        {
            FireProjectile(Target);
            lastAttackTime = Time.time; //발사 성공 후 마지막 발사 시간 갱신!
        }
    }

    private void FireProjectile(Transform target)
    {
        //1. 타겟을 향한 방향 벡터 계산
        Vector2 direction = (target.position - firePoint.position).normalized;
        //2. 투사체 생성 (총구 위치에 생성)
        GameObject projObj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        //3. 생성된 투사체의 Projectile 스크립트를 가져와서 스탯(데미지, 속도, 방향)을 주입
        Projectile projectile = projObj.GetComponent<Projectile>();
        if (projectile != null)
        {
            projectile.Initialize(
                statHandler.GetAttackPower(),
                statHandler.GetProjectileSpeed(),
                direction);
        }
    }
}
