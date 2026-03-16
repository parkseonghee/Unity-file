using Unity.VisualScripting;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;


    public Player playerScript;
    public Transform player;
    public float distance = 1.0f; // 무기의 위치를 플레이어로부터 얼마나 떨어뜨릴지 결정하는 변수

    private Camera _Camera;

    public float attackDamgage = 10;
    public float attackSpeed = 1.0f;

    private float lastAttackTime = 0f;
    private Collider2D attackCollider;
    public KeyCode actionKey = KeyCode.Mouse0;
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        attackCollider = GetComponent<Collider2D>();
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        _Camera = Camera.main;
    }

    void Update()
    {

        HandleAttackInput();

        UpdateWeaponTransform();
    }

    void HandleAttackInput()
    {
        float attackDelay = 1f / attackSpeed;

        if(Input.GetKey(actionKey) && Time.time >= lastAttackTime + attackDelay && transform.rotation.z < 0)
        {
            if(spriteRenderer != null)
            {
                spriteRenderer.enabled = true; // 공격 애니메이션이 시작될 때 스프라이트 렌더러 활성화
                
            }
            animator.speed = attackSpeed; // 애니메이션 속도 조절
            animator.SetTrigger("atk");
            lastAttackTime = Time.time;
        }

        if (Input.GetKey(actionKey) && Time.time >= lastAttackTime + attackDelay && transform.rotation.z > 0)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = true; // 공격 애니메이션이 시작될 때 스프라이트 렌더러 활성화

            }
            animator.speed = attackSpeed; // 애니메이션 속도 조절
            animator.SetTrigger("atk2");
            lastAttackTime = Time.time;

        }


    }

    void UpdateWeaponTransform()
    {
        // 1. 마우스 위치 및 각도 계산
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
        Vector2 direction = (mousePos - player.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 2. 부모(Pivot)의 위치와 회전 설정
        transform.position = player.position + (Vector3)direction * distance;
        float offset = -90f;
        transform.rotation = Quaternion.Euler(0, 0, angle + offset);

        // 3. 좌우 반전 로직 (자식의 spriteRenderer를 제어하게 됨)
        bool isLeft = direction.x < 0;
        if (spriteRenderer != null) spriteRenderer.flipX = isLeft;

        if (playerScript != null)
        {
            playerScript.spriteRenderer2.flipX = isLeft;
        }
    }

    public void EnableAttackCollider()
    {
        if(attackCollider)
        {
            attackCollider.enabled = true; // 공격 콜라이더 활성화
        }
    }

    public void DisableAttackCollider()
    {
        if (attackCollider)
        {
            attackCollider.enabled = false; // 공격 콜라이더 비활성화
        }
    }




}
