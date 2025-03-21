using UnityEngine;

public class EnemyHit : MonoBehaviour
{
    public float damagePercentage = 0.1f;  // Dano será 10% da vida atual do jogador

    // Função para obter o dano que o inimigo vai causar ao jogador
    public int CalculateDamage(int playerCurrentHealth)
    {
        return Mathf.CeilToInt(playerCurrentHealth * damagePercentage);  // Calcula 10% da vida atual do jogador
    }
}
