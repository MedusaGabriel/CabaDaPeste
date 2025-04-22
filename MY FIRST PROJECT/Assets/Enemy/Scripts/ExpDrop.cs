using UnityEngine;

public class ExpDrop : MonoBehaviour
{
    public void DropXP(GameObject player)
    {
        PlayerLevelUp playerLevelUp = player.GetComponent<PlayerLevelUp>();
        EnemyStatus enemyStatus = GetComponent<EnemyStatus>();
        if (playerLevelUp != null && enemyStatus != null)
        {
            playerLevelUp.AddXP(enemyStatus.xpDrop);
            Debug.Log($"{gameObject.name} dropou {enemyStatus.xpDrop} XP para {player.name}.");
        }
    }
}