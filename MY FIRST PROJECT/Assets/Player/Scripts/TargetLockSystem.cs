using UnityEngine;
using System.Collections.Generic;

public class TargetLockSystem : MonoBehaviour
{
    [Header("Configurações Básicas")]
    public float lockRange = 10f;
    public LayerMask enemyLayer;
    public KeyCode lockKey = KeyCode.Tab;
    public string enemyTag = "Enemy";
    
    [Header("Marcador Visual")]
    [Tooltip("Arraste um prefab personalizado ou deixe vazio para usar o padrão")]
    public GameObject lockOnIndicator;
    public Color defaultIndicatorColor = Color.red;
    public float yOffset = 2f;
    
    private Transform currentTarget;
    private List<Transform> potentialTargets = new List<Transform>();
    private bool isLocked = false;
    private GameObject currentIndicator;

    private void Update()
    {
        HandleTargetLockInput();
        
        if (isLocked && currentTarget != null)
        {
            FaceTarget();
            UpdateIndicatorPosition();
        }
        else if (currentTarget == null && isLocked)
        {
            ClearLock();
        }
    }

    private void HandleTargetLockInput()
    {
        if (Input.GetKeyDown(lockKey))
        {
            if (!isLocked)
            {
                FindAndLockTarget();
            }
            else
            {
                SwitchTarget();
            }
        }
    }

    private void FindAndLockTarget()
    {
        RefreshTargetList();
        
        if (potentialTargets.Count > 0)
        {
            currentTarget = potentialTargets[0];
            isLocked = true;
            CreateLockIndicator();
        }
    }

    private void RefreshTargetList()
    {
        potentialTargets.Clear();
        
        // Encontra todos os inimigos ativos no pool
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, lockRange, enemyLayer);
        
        foreach (Collider enemy in enemiesInRange)
        {
            if (enemy.CompareTag(enemyTag) && enemy.gameObject.activeInHierarchy)
            {
                potentialTargets.Add(enemy.transform);
            }
        }
        
        // Ordena por proximidade
        potentialTargets.Sort((a, b) => 
            Vector3.Distance(transform.position, a.position).CompareTo(
            Vector3.Distance(transform.position, b.position)));
    }

    private void SwitchTarget()
    {
        if (potentialTargets.Count == 0) return;
        
        RefreshTargetList();
        
        if (potentialTargets.Count == 0)
        {
            ClearLock();
            return;
        }
        
        // Se o alvo atual foi destruído/desativado, pega o primeiro da lista
        if (currentTarget == null || !potentialTargets.Contains(currentTarget))
        {
            currentTarget = potentialTargets[0];
        }
        else
        {
            // Encontra o índice do próximo alvo
            int currentIndex = potentialTargets.IndexOf(currentTarget);
            int nextIndex = (currentIndex + 1) % potentialTargets.Count;
            currentTarget = potentialTargets[nextIndex];
        }
        
        CreateLockIndicator();
    }

    private void FaceTarget()
    {
        if (currentTarget == null) return;
        
        Vector3 directionToTarget = currentTarget.position - transform.position;
        directionToTarget.y = 0;
        
        if (directionToTarget != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(directionToTarget);
        }
    }

    private void CreateLockIndicator()
    {
        if (currentIndicator != null)
        {
            Destroy(currentIndicator);
        }

        if (currentTarget == null) return;

        if (lockOnIndicator != null)
        {
            currentIndicator = Instantiate(lockOnIndicator, 
                                       currentTarget.position + Vector3.up * yOffset, 
                                       Quaternion.identity, 
                                       currentTarget);
        }
        else
        {
            CreateDefaultIndicator();
        }
    }

    private void CreateDefaultIndicator()
    {
        currentIndicator = new GameObject("DefaultLockIndicator");
        currentIndicator.transform.SetParent(currentTarget);
        currentIndicator.transform.position = currentTarget.position + Vector3.up * yOffset;
        
        // Cria um quad com material simples para 3D
        var quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        quad.transform.SetParent(currentIndicator.transform);
        quad.transform.localPosition = Vector3.zero;
        quad.transform.localRotation = Quaternion.Euler(90, 0, 0);
        quad.GetComponent<Renderer>().material = new Material(Shader.Find("Unlit/Color")) {
            color = defaultIndicatorColor
        };
        Destroy(quad.GetComponent<Collider>());
        
        // Ajusta escala
        quad.transform.localScale = Vector3.one * 0.5f;
    }

    private void UpdateIndicatorPosition()
    {
        if (currentIndicator != null && currentTarget != null)
        {
            currentIndicator.transform.position = currentTarget.position + Vector3.up * yOffset;
        }
    }

    private void ClearLock()
    {
        if (currentIndicator != null)
        {
            Destroy(currentIndicator);
        }
        
        isLocked = false;
        currentTarget = null;
        potentialTargets.Clear();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, lockRange);
    }
}