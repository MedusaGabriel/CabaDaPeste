using UnityEngine;

[DefaultExecutionOrder(-100)]
public class EnemyAudioGlobalManager : MonoBehaviour
{
    public static EnemyAudioGlobalManager Instance;

    [Header("Limites Globais de Áudio")]
    [Tooltip("Máximo de sons de ataque de inimigos tocando ao mesmo tempo.")]
    public int maxAttackSounds = 2;

    [Header("Cooldown Global (opcional)")]
    [Tooltip("Tempo mínimo entre sons de ataque globais (segundos).")]
    public float globalAttackCooldown = 0f;

    private int currentAttackSounds = 0;
    private float lastAttackSoundTime = -10f;

    void Awake()
    {
        Instance = this;
    }

    public bool TryPlayAttackSound()
    {
        if (currentAttackSounds < maxAttackSounds && Time.time >= lastAttackSoundTime + globalAttackCooldown)
        {
            currentAttackSounds++;
            lastAttackSoundTime = Time.time;
            return true;
        }
        return false;
    }

    public void OnAttackSoundEnd()
    {
        currentAttackSounds = Mathf.Max(0, currentAttackSounds - 1);
    }
}