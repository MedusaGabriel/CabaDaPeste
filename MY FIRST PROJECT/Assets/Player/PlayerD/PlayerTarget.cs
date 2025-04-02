using UnityEngine;
using System.Collections.Generic;

public class PlayerTarget : MonoBehaviour
{
    [Header("Configurações Básicas")]
    public float lockRange = 10f;
    public KeyCode lockKey = KeyCode.Tab;
    public KeyCode unlockKey = KeyCode.Q;

    private List<Transform> _enemiesInRange = new List<Transform>();
    private Transform _currentTarget;
    private bool _isTargeting;

    public bool IsTargeting => _isTargeting;

    [Header("Referência para Animator")]
    private Animator _animator;

    [Header("Indicador de Alvo")]
    public GameObject targetIndicatorPrefab;
    private GameObject _currentIndicator;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(lockKey))
        {
            if (!_isTargeting)
            {
                LockOnNearestEnemy();
            }
            else
            {
                SwitchTarget();
            }
        }

        if (Input.GetKeyDown(unlockKey))
        {
            ClearLock();
        }

        if (_isTargeting && _currentTarget != null)
        {
            FaceTarget();
        }

        if (_animator != null)
        {
            _animator.SetFloat("Speed", _isTargeting ? -1f : 1f);
        }

        UpdateTargetIndicator();
    }

    private void RefreshEnemiesInRange()
    {
        _enemiesInRange.Clear();
        Collider[] hits = Physics.OverlapSphere(transform.position, lockRange);

        foreach (Collider c in hits)
        {
            if (c.gameObject.tag.StartsWith("Enemy"))
            {
                _enemiesInRange.Add(c.transform);
            }
        }

        _enemiesInRange.Sort((a, b) =>
            Vector3.Distance(transform.position, a.position)
            .CompareTo(Vector3.Distance(transform.position, b.position))
        );
    }

    private void LockOnNearestEnemy()
    {
        RefreshEnemiesInRange();
        if (_enemiesInRange.Count > 0)
        {
            _currentTarget = _enemiesInRange[0];
            _isTargeting = true;
            CreateTargetIndicator();
        }
    }

    private void SwitchTarget()
    {
        RefreshEnemiesInRange();
        if (_enemiesInRange.Count == 0)
        {
            ClearLock();
            return;
        }

        int index = _enemiesInRange.IndexOf(_currentTarget);
        if (index == -1)
        {
            _currentTarget = _enemiesInRange[0];
        }
        else
        {
            int nextIndex = (index + 1) % _enemiesInRange.Count;
            _currentTarget = _enemiesInRange[nextIndex];
        }
        CreateTargetIndicator();
    }

    private void FaceTarget()
    {
        if (_currentTarget == null) return;
        Vector3 dir = _currentTarget.position - transform.position;
        dir.y = 0f;
        if (dir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(dir);
        }
    }


    private void ClearLock()
    {
        _currentTarget = null;
        _enemiesInRange.Clear();
        _isTargeting = false;
        if (_currentIndicator != null)
        {
            Destroy(_currentIndicator);
            _currentIndicator = null;
        }
    }

    private void CreateTargetIndicator()
    {
        if (_currentIndicator != null)
        {
            Destroy(_currentIndicator);
        }

        if (!_isTargeting || _currentTarget == null || targetIndicatorPrefab == null) return;

        Vector3 spawnPos = _currentTarget.position + Vector3.up * 2f;
        _currentIndicator = Instantiate(targetIndicatorPrefab, spawnPos, Quaternion.identity);
        _currentIndicator.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        Renderer rend = _currentIndicator.GetComponent<Renderer>();
        if (rend != null)
        {
            rend.material.color = Color.magenta; 
        }
    }

    private void UpdateTargetIndicator()
    {
        if (_currentIndicator == null || _currentTarget == null) return;

        _currentIndicator.transform.position = _currentTarget.position + Vector3.up * 2f;

        _currentIndicator.transform.Rotate(Vector3.up, 100f * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, lockRange);
    }
}