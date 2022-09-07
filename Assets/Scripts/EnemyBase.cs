using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class EnemyBase : MonoBehaviour
{
    const float pathfindingDelay = 1.0f;

    public float spawnWeight = 1.0f;
    public float minSpawnTime = 3.0f;

    [Space]
    public float pathCheckDistance;

    [Space]
    public float angleScale = 0.05f;
    public UnityEvent[] attackEvents;

    [Space]
    public Transform muzzle;
    public Transform targetPoint;

    [Space]
    public Team[] enemyTeams;

    protected TankMovement movement;

    private GameObject _Target;
    private Health health;
    private Vector2 movePoint;

    private float lastPathfindTime;
    private List<Vector2> path = new List<Vector2>();

    public float SpawnWeight => spawnWeight;
    public float MinSpawnTime => minSpawnTime;
    public Vector2 ShootPoint { get => movement.ShootPoint; set => movement.ShootPoint = value; }
    public Vector2 MuzzleDirection => muzzle.right;

    public GameObject Target
    {
        get
        {
            if (!_Target)
            {
                var enemies = Team.GetOverlapingSet(enemyTeams);
                if (enemies.Count > 0)
                {
                    _Target = enemies.ElementAt(Random.Range(0, enemies.Count)).gameObject;
                }
            }
            return _Target;
        }
        set => _Target = value;
    }

    public static HashSet<EnemyBase> Enemies { get; } = new HashSet<EnemyBase>();

    private void Awake()
    {
        movement = GetComponent<TankMovement>();
    }

    private void OnEnable()
    {
        Enemies.Add(this);

        if (TryGetComponent(out health))
        {
            health.DamageEvent += OnDamage;
        }
    }

    private void LateUpdate()
    {
        if (Time.time > lastPathfindTime + pathfindingDelay)
        {
            Pathfinder.Instance.GetPath(transform.position, movePoint, ref path);
        }

        if (path.Count > 0)
        {
            if ((path.First() - (Vector2)transform.position).sqrMagnitude < pathCheckDistance * pathCheckDistance)
            {
                path.RemoveAt(0);
            }
            MoveTowards(path.First());
        }
        else
        {
            MoveTowards(transform.position);
        }
    }

    private void OnDamage(DamageArgs args)
    {
        Stats.IncrementValue("damage_delt", args.damage);
    }

    private void OnDisable()
    {
        Enemies.Remove(this);

        health.DamageEvent -= OnDamage;

        Stats.IncrementValue("enemies_killed", 1.0f);
    }

    protected virtual void Update()
    {
        if (Target)
        {
            ShootPoint = Target.transform.position;
            targetPoint.position = Target.transform.position;
        }
    }

    public void Attack(int index)
    {
        if (index < 0 || index >= attackEvents.Length) return;

        attackEvents[index].Invoke();
    }

    public void MoveTowards(Vector2 point)
    {
        MoveInDirection(point - (Vector2)transform.position);
    }

    public void MoveInDirection(Vector2 direction)
    {
        if (direction.sqrMagnitude < 0.01f * 0.01f)
        {
            movement.ThrottleInput = 0.0f;
            movement.TurnInput = 0.0f;
            return;
        }

        direction.Normalize();

        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float currentAngle = movement.transform.eulerAngles.z;
        float deltaAngle = Mathf.DeltaAngle(targetAngle, currentAngle);

        float dot = Vector2.Dot(direction, movement.transform.right);

        movement.ThrottleInput = dot;
        movement.TurnInput = Mathf.Clamp(deltaAngle * angleScale, -1.0f, 1.0f);
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        for (int i = 0; i < path.Count - 1; i++)
        {
            Gizmos.DrawLine(path[i], path[i + 1]);
        }

        Gizmos.color = Color.white;
    }
}
