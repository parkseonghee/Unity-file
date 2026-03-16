using UnityEngine;

public class StartManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        //Scene바꾸기 정확한 스테이지명 기입
        GameManager.instance.LoadScene("Stage1");
    }

}
