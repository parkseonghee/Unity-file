using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMoveing : MonoBehaviour
{
    public RectTransform[] buttons;
    public Player playerScript;
    public Attack attackScript;
    public float upDistance = 100f;
    public float speed = 10f;
    public TextMeshProUGUI statsText;
    public TextMeshProUGUI statsText2;
    public TextMeshProUGUI statsText3;

    private Vector2[] startPos;
    private bool isUp = false;
    private int randomHp;
    private int randomHp2;
    private int randomHp3;
    private int randomDamdage;
    private int randomDamdage2;
    private int randomDamdage3;
    private int randomSpeed;
    private int randomSpeed2;
    private int randomSpeed3;

    //핵심 추가: 현재 뽑힌 보상의 종류를 기억할 변수 (0 = HP, 1 = Attack Damage, 2 = Attack Speed)
    private int currentRewardType = -1;
    private int currentRewardType2 = -1;
    private int currentRewardType3 = -1;

    void Start()
    {
        startPos = new Vector2[buttons.Length];
        for (int i = 0; i < buttons.Length; i++)
            startPos[i] = buttons[i].anchoredPosition;


    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            isUp = true;

            // 랜덤으로 0 또는 1을 뽑아서 변수에 저장합니다.
            currentRewardType = Random.Range(0, 3);
            currentRewardType2 = Random.Range(2, 5);
            currentRewardType3 = Random.Range(4, 7);
            if (currentRewardType == 0)
            {
                randomHp = Random.Range(1, 3);
                statsText.text = "HP : " + randomHp.ToString();
               
            }
            else if (currentRewardType == 1)
            {
                randomDamdage = Random.Range(1, 3);
                statsText.text = "Attack : " + randomDamdage.ToString();
                
            }
            else if (currentRewardType == 2)
            {
                randomSpeed = Random.Range(1, 3);
                statsText.text = "Speed : " + randomSpeed.ToString();
               
            }


            if (currentRewardType2 == 2)
            {
                randomHp2 = Random.Range(1, 3);
                statsText2.text = "HP : " + randomHp2.ToString();
                
            }
            else if (currentRewardType2 == 3)
            {
                randomDamdage2 = Random.Range(1, 3);
                statsText2.text = "Attack : " + randomDamdage2.ToString();
               
            }
            else if (currentRewardType2 == 4)
            {
                randomSpeed2 = Random.Range(1, 3);
                statsText2.text = "Speed : " + randomSpeed2.ToString();
                
            }


            if (currentRewardType3 == 4)
            {
                randomHp3 = Random.Range(1, 3);
                statsText3.text = "HP : " + randomHp3.ToString();
                
            }
            else if (currentRewardType3 == 5)
            {
                randomDamdage3 = Random.Range(1, 3);
                statsText3.text = "Attack : " + randomDamdage3.ToString();
                
            }
            else if (currentRewardType3 == 6)
            {
                randomSpeed3 = Random.Range(1, 3);
                statsText3.text = "Speed : " + randomSpeed3.ToString();
                
            }
        }

        for (int i = 0; i < buttons.Length; i++)
        {
            Vector2 targetPos = isUp ? startPos[i] + new Vector2(0, upDistance) : startPos[i];
            buttons[i].anchoredPosition = Vector2.Lerp(buttons[i].anchoredPosition, targetPos, Time.deltaTime * speed);
        }
    }

    public void ReturnToOrigin()
    {
        isUp = false;

        // 핵심 수정: 저장해둔 보상 타입에 따라 하나만 올려줍니다.
        if (currentRewardType == 0)
        {
            playerScript.maxHp += randomHp;
            
        }
        else if (currentRewardType == 1)
        {
            attackScript.attackDamgage += randomDamdage;
            
        }
        else if (currentRewardType == 2)
        {

            attackScript.attackSpeed += randomSpeed;
            
        }

        // 보상을 받고 나면 타입을 다시 -1로 초기화 (중복 수령이나 버그 방지)
        currentRewardType = -1;
    }

    public void ReturnToOrigin2()
    {
        isUp = false;

        // 핵심 수정: 저장해둔 보상 타입에 따라 하나만 올려줍니다.
        if (currentRewardType2 == 2)
        {
            playerScript.maxHp += randomHp2;
            
        }
        else if (currentRewardType2 == 3)
        {
            attackScript.attackDamgage += randomDamdage2;
            
        }
        else if (currentRewardType2 == 4)
        {

            attackScript.attackSpeed += randomSpeed2;
            
        }
        // 보상을 받고 나면 타입을 다시 -1로 초기화 (중복 수령이나 버그 방지)
        currentRewardType2 = -1;
    }

    public void ReturnToOrigin3()
    {
        isUp = false;

        // 핵심 수정: 저장해둔 보상 타입에 따라 하나만 올려줍니다.
        if (currentRewardType3 == 4)
        {
            playerScript.maxHp += randomHp3;
            
        }
        else if (currentRewardType3 == 5)
        {
            attackScript.attackDamgage += randomDamdage3;
            
        }
        else if (currentRewardType3 == 6)
        {

            attackScript.attackSpeed += randomSpeed3;
            
        }

        // 보상을 받고 나면 타입을 다시 -1로 초기화 (중복 수령이나 버그 방지)
        currentRewardType3 = -1;
    }
}