using UnityEngine;

public class EnemyHit : MonoBehaviour
{
    public int minDamage = 10;
    public int maxDamage = 20;

    public int CalculateDamage()
    {
        int damage = Random.Range(minDamage, maxDamage);
        // Debug.Log($"[EnemyHit] Dano calculado: {damage}");
        return damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            int damage = CalculateDamage();
            // Debug.Log($"[EnemyHit] Colisão com o player detectada. Dano causado: {damage}");

            // Aqui você pode aplicar o dano no script do player, tipo:
            // other.GetComponent<PlayerHealth>()?.TakeDamage(damage);
        }
    }
}
