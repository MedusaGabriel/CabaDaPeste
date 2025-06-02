using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    [Header("Vida")]
    public float maxHealth = 50f;

    [Header("Dano")]
    public float attack = 10f;
    public float attackCooldown = 1.5f;
    public float attackRange = 2f;
    public float attackAngle = 90f;

    [Header("Velocidade")]
    public float speed = 3f;
    public float angularSpeed = 120f;
    public float acceleration = 10f;

    [Header("Others")]
    public float height = 2f;
    public float width = 1f;

}
