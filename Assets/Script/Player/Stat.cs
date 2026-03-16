using UnityEngine;
using TMPro;
using JetBrains.Annotations;
public class Stat : MonoBehaviour
{
    public RectTransform uiElement;   // 움직일 UI 이미지
    public Vector2 targetPosition;    // 이동할 목표 위치
    public float speed = 10f;         // 이동 속도 (숫자가 클수록 빠름)
    public TextMeshProUGUI statsText;
    public Player playerScript;
    public Attack attackScript;

    private Vector2 originalPosition; // 처음 위치를 기억할 공간
    private bool isMoved = false;     // 현재 목표 위치로 가 있는지 확인하는 스위치

    void Start()
    {
        // 게임이 시작될 때(Start), UI의 원래 위치를 기억해 둡니다.
        originalPosition = uiElement.anchoredPosition;

    }

    void Update()
    {
        // Tab 키를 누를 때마다 스위치의 상태를 반대로 바꿉니다 (true <-> false)
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isMoved = !isMoved;
        }
        statsText.text = "HP : " + playerScript.maxHp + "\nAttack : " + attackScript.attackDamgage +"\n AttackSpeed :   " + attackScript.attackSpeed;

        // 스위치가 켜져 있으면 목표 위치로, 꺼져 있으면 원래 위치를 목적지로 정합니다.
        Vector2 currentTarget = isMoved ? targetPosition : originalPosition;

        // 정해진 목적지를 향해 매 프레임 부드럽게 이동합니다.
        uiElement.anchoredPosition = Vector2.Lerp(uiElement.anchoredPosition, currentTarget, Time.deltaTime * speed);
    }
}