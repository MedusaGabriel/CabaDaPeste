using UnityEngine;

public class EnemyHit : MonoBehaviour
{
    public int minDamage = 10;
    public int maxDamage = 20;
     private EnemyStatus enemyStatus;

    public int CalculateDamage()
    {
        int baseDamage = Random.Range(minDamage, maxDamage);
        if (enemyStatus != null)
        {
            baseDamage += Mathf.RoundToInt(enemyStatus.attack);
        }
        return baseDamage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            int damage = CalculateDamage();
        }
    }
}
