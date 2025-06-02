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
    public float targetIndicatorOffsetY = 2f;
    private GameObject _currentIndicator;

    void Start() => _animator = GetComponent<Animator>();

    void Update()
    {
        if (Input.GetKeyDown(lockKey))
        {
            if (!_isTargeting) LockOnNearestEnemy();
            else SwitchTarget();
        }

        if (Input.GetKeyDown(unlockKey)) ClearLock();

        if (_isTargeting && _currentTarget != null)
        {
            var rb = GetComponent<Rigidbody>();
            if (rb == null || rb.linearVelocity.sqrMagnitude < 0.01f)
                FaceTarget();
        }

        _animator?.SetBool("IsTarget", _isTargeting);

        UpdateTargetIndicator();

        if (_isTargeting)
            GetComponent<PlayerInputS.PlayerInputSystem>()?.SprintInput(false);
    }

    private void RefreshEnemiesInRange()
    {
        _enemiesInRange.Clear();
        foreach (var c in Physics.OverlapSphere(transform.position, lockRange))
            if (c.CompareTag("Enemy") || c.tag.StartsWith("Enemy"))
                _enemiesInRange.Add(c.transform);

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
        _currentTarget = index == -1
            ? _enemiesInRange[0]
            : _enemiesInRange[(index + 1) % _enemiesInRange.Count];
        CreateTargetIndicator();
    }

    private void FaceTarget()
    {
        if (_currentTarget == null) return;
        Vector3 dir = _currentTarget.position - transform.position;
        dir.y = 0f;
        if (dir != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir);
            var rb = GetComponent<Rigidbody>();
            if (rb != null)
                rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, Time.deltaTime * 10f));
            else
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 10f);
        }
    }
    private void ClearLock()
    {
        _currentTarget = null;
        _isTargeting = false;
        if (_currentIndicator != null)
        {
            Destroy(_currentIndicator);
            _currentIndicator = null;
        }
    }

    private Vector3 GetIndicatorPosition() =>
        _currentTarget.position + Vector3.up * targetIndicatorOffsetY;

    private void CreateTargetIndicator()
    {
        if (_currentIndicator != null)
            Destroy(_currentIndicator);

        if (!_isTargeting || _currentTarget == null || targetIndicatorPrefab == null) return;

        _currentIndicator = Instantiate(targetIndicatorPrefab, GetIndicatorPosition(), Quaternion.identity);
        _currentIndicator.transform.localScale = Vector3.one * 0.5f;

        var rend = _currentIndicator.GetComponent<Renderer>();
        if (rend != null)
            rend.material.color = Color.magenta;
    }

    private void UpdateTargetIndicator()
    {
        if (_currentIndicator == null || _currentTarget == null) return;

        _currentIndicator.transform.position = GetIndicatorPosition();
        _currentIndicator.transform.Rotate(Vector3.up, 100f * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, lockRange);
    }
}