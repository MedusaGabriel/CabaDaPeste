using UnityEngine;

public class EnemyHit : MonoBehaviour
{
    private EnemyStatus enemyStatus;

    void Start()
    {
        enemyStatus = GetComponent<EnemyStatus>();
    }

    public int CalculateDamage()
    {
        if (enemyStatus != null)
        {
            return Mathf.RoundToInt(enemyStatus.attack);
        }
        return 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            int damage = CalculateDamage();
            // Aqui você pode aplicar o dano se quiser
        }
    }
}