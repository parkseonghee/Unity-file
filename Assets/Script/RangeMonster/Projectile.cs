using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Projectile : MonoBehaviour
{
    private Rigidbody2D rb;
    private float damage;   //투사체가 갖고 있는 데미지

    //몬스터의 전투 모듈(MonsterCombat.cs)이 투사체를 생성하면, 이 함수로 정보를 주입함
    public void Initialize(float damageAmount, float speed, Vector2 direction)
    {
        rb = GetComponent<Rigidbody2D>();
        damage = damageAmount;

        //1. 투사체는 물리적 중력의 영향을 받으면 안된다.
        rb.gravityScale = 0f;
        //2. 전달받은 방향과 속도로 날아가도록 물리 엔진에 속도 대입
        rb.linearVelocity = direction.normalized * speed;
        //3. 5초 뒤에도 아무것도 못 맞추면 메모리 낭비 방지를 위한 파괴조치
        Destroy(gameObject, 5f);
    }

    //플레이어에게 투사체가 닿았는지 감지하는 함수
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //1. 플레이어에게 닿았는가?
        if (collision.CompareTag("Player"))
        {
            //TODO: 나중에 Player의 StatHandler를 가져와서 데미지를 입히는 로직 추가 예정
            Debug.Log($"플레이어 적중! {damage}데미지!");
            Destroy(gameObject);    //일단 부딪히면 투사체 파괴
        }
        //2. 구조물(장애물)에 닿았는가?
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            Debug.Log("벽에 막혀서 투사체가 파괴됨!");
            Destroy(gameObject);
        }
        //3. (확장성) 패시브 스킬(예: 방패막기)에 닿았는가?
        //else if (collision.CompareTag("Shield"))
        //{
        //    Debug.Log("방패에 막힘!");
        //    Destroy(gameObject);
        //}
    }
}
