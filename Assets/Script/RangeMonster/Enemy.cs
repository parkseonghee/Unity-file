using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float enemyHp = 25;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(enemyHp);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Attack")
        {
            Attack attackScript = collision.gameObject.GetComponent<Attack>();
            enemyHp -= attackScript.attackDamgage;

            Debug.Log("Enemy hit! Current HP: " + enemyHp);
            if (enemyHp <= 0)
            {
                Destroy(gameObject); // Àû Á¦°Å
                Debug.Log("Enemy defeated!");
            }
        }
    }
}
