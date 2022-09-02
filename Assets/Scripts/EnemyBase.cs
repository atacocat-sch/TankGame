using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] float spawnWeight = 1.0f;
    [SerializeField] float minSpawnTime = 3.0f;

    [Space]
    [SerializeField] float obstacleAvoidanceDistance;
    [SerializeField] LayerMask obstacleAvoidanceMask;
    [SerializeField] float obstacleAvoidanceTime;

    [Space]
    [SerializeField] float angleScale = 0.05f;
    [SerializeField] UnityEvent[] attackEvents;

    [Space]
    [SerializeField] Transform muzzle;
    [SerializeField] Transform targetPoint;

    protected TankMovement movement;
    float movementOverrideTime;

    private GameObject _Target;

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
                _Target = GameObject.FindGameObjectWithTag("Player");
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
    }

    private void OnDisable()
    {
        Enemies.Remove(this);
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
        if (movementOverrideTime > 0.0f)
        {
            direction = -transform.position;
            movementOverrideTime -= Time.deltaTime;
        }
        else
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, obstacleAvoidanceDistance, obstacleAvoidanceMask);
            if (hit)
            {
                movementOverrideTime = obstacleAvoidanceTime;
            }
        }

        direction.Normalize();

        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float currentAngle = movement.transform.eulerAngles.z;
        float deltaAngle = Mathf.DeltaAngle(targetAngle, currentAngle);

        float dot = Vector2.Dot(direction, movement.transform.right);

        movement.ThrottleInput = dot;
        movement.TurnInput = Mathf.Clamp(deltaAngle * angleScale, -1.0f, 1.0f);
    }
}
