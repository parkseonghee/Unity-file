using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    //밑 4줄 연결만 해두고 각각 구현
    public StartManager startManager;
    public UIManager uiManager;
    public SpawnManager spawnManager;
    public WaveManager waveManager;

    public GameState currentState;


    private void Awake()
    {

        if (instance != null && instance != this) // 게임매니저 중복생성 됐는지 체크
        {
            Destroy(gameObject); //게임매니저가 중복생성 됐다면 삭제
            return; //리턴으로 실행멈춰주고
        }

        instance = this; // 게임매니저 변수저장해서 다른 코드에서 접근성 추가
        DontDestroyOnLoad(gameObject); //씬이 바뀌어도 게임매니저 삭제안함
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public enum GameState
    {

    }

    public void StartGame()
    {

    }

    public void NextStage()
    {

    }
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void GameOver()
    {

    }


}
