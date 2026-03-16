using UnityEngine;

[CreateAssetMenu(fileName = "MonsterStatData", menuName = "Scriptable Objects/MonsterStatData")]
public class MonsterStatData : ScriptableObject
{
    [Header("Base Defualt State")]
    public float maxHP;         //최대 체력
    public float attackPower;   //기본 공격력
    public float defense;       //방어력
    public float moveSpeed;     //이동 속도

    [Header("Combat State")]
    [Tooltip("공격 속도 (1.5면 1.5초마다 1번 발사")]
    public float attackCooldown = 1.5f;
    [Tooltip("발사된 투사체가 날아가는 속도")]
    public float projectileSpeed = 10f;


    [Header("Detection & Attack Range(Base)")]
    [Tooltip("모든 몬스터의 공통 기본 감지 범위")]
    public float baseDetectionRange = 10f;
    [Tooltip("모든 몬스터의 공통 기본 공격 사거리")]
    public float baseAttackRange = 5f;
}
