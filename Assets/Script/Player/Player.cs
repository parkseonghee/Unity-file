using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("이동 설정")]
    public float moveSpeed = 5f;
    public SpriteRenderer spriteRenderer2;

    [Header("대쉬 설정")]
    public float dashSpeed = 50f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;


    [Header("무적 설정")]
    public float invincibilityDuration = 1.0f; // 무적 지속 시간
    private bool isInvincible = false;         // 현재 무적 상태인지 여부
    private SpriteRenderer spriteRenderer;     // 깜빡임 효과를 위한 변수


    private Transform tr;            // Transform을 매번 찾지 않도록 저장
    private Vector3 moveInput;       // 프레임당 한 번만 계산할 입력 벡터
    private Vector3 dashDirection;   // 대쉬 방향

    private float dashTimer;         // 0보다 크면 대쉬 중
    private float nextDashTime;      // 다음 대쉬가 가능한 시간

    [SerializeField]
    private Slider hpbar;

    public float maxHp = 25;
    public float curHp = 25;

    private Rigidbody2D rb;

    public TextMeshProUGUI HpText;
    public int hp = 0;



    // ... (기존 Update 문 내부) ...

    void Awake()
    {
        // 1. Transform 캐싱 (유니티 최적화의 기본)
        tr = transform;
        spriteRenderer2 = GetComponent<SpriteRenderer>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        hpbar.value = (float)curHp / (float)maxHp;
    }

    void Update()  // 물리 연산을 위해 FixedUpdate 사용 speed와 빨라지면 벽이 뚫리는 현상 때매 수정
    {
        // 1. 입력은 반드시 Update에서 받아야 씹히지 않습니다.
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize();

        // 2. 대쉬 입력 체크 (Update에서 변수에 저장)
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1)) && Time.time >= nextDashTime)
        {
            StartDash();
        }

        // 3. UI 및 기타 업데이트
        HandleHp();
        HpText.text = curHp.ToString() + "/" + maxHp.ToString();
    }

    void FixedUpdate()
    {
        // 4. 물리 이동 연산은 FixedUpdate에서 처리
        if (dashTimer > 0f)
        {
            DashMove();
        }
        else
        {
            NormalMove();
        }
    }

    private void NormalMove()
    {
        // sqrMagnitude 연산이 (x==0 && y==0)보다 미세하게 더 빠르고 안전합니다.
        if (moveInput.sqrMagnitude > 0)
        {
            // position += 보다 Translate가 최적화 및 이동 처리에 더 적합합니다.
            tr.Translate(moveInput * moveSpeed * Time.deltaTime);
        }
    }

    private void StartDash()
    {
        dashTimer = dashDuration;
        nextDashTime = Time.time + dashCooldown;
        isInvincible = true; // 대쉬 시작 시 무적 상태 활성화

        if (moveInput.sqrMagnitude == 0)
        {
            dashDirection = new Vector3(spriteRenderer2.flipX ? -1f : 1f, 0f, 0f);
        }
        else
        {
            dashDirection = moveInput;
        }
    }

    private void DashMove()
    {
        rb.linearVelocity = dashDirection * dashSpeed; // Translate 대신 속도 제어
        dashTimer -= Time.deltaTime;

        if (dashTimer <= 0)
        {
            rb.linearVelocity = Vector2.zero; // 정지
            isInvincible = false;
        }
    }

    private void HandleHp()
    {
        hpbar.value = (float)curHp / (float)maxHp;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isInvincible) return;
        
        // 나중에 몬스터 공격력을 가져와서 대미지를 계산할 때 사용할 수 있도록 주석 처리된 코드입니다.
/*        if (collision.gameObject.CompareTag("Enemy"))
        {
            Attack attackScript = collision.gameObject.GetComponent<Attack>();

            if (attackScript != null)
            {
                TakeDamage(attackScript.attackDamgage);
            }
               
        }*/

        if(collision.gameObject.CompareTag("Enemy"))
        {
          TakeDamage(10);
        }
    }

    public void TakeDamage(float damage)
    {
        curHp -= damage;
        
        if (curHp < 0) curHp = 0;

        // 대미지를 입은 후 무적 코루틴 시작
        StartCoroutine(BecomeInvincible());
    }

    // 무적 상태 코루틴
    private IEnumerator BecomeInvincible()
    {
        isInvincible = true;

        // 무적 시간 동안 깜빡이는 연출 (선택 사항)
        float elapsed = 0;
        while (elapsed < invincibilityDuration)
        {
            // 알파값을 조절해 깜빡임 표현
            spriteRenderer.color = new Color(1, 1, 1, 0.5f);
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = new Color(1, 1, 1, 1f);
            yield return new WaitForSeconds(0.1f);

            elapsed += 0.2f;
        }

        spriteRenderer.color = new Color(1, 1, 1, 1f); // 원래 색상으로 복구
        isInvincible = false;
    }


}