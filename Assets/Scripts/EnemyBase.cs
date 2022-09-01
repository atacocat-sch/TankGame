using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] float spawnWeight = 1.0f;
    [SerializeField] float minSpawnTime = 3.0f;

    [Space]
    [SerializeField] float angleScale = 0.05f;  
    [SerializeField] UnityEvent[] attackEvents;

    protected TankMovement movement;

    private GameObject _Target;

    public float SpawnWeight => spawnWeight;
    public float MinSpawnTime => minSpawnTime;
    public Vector2 ShootPoint { get => movement.ShootPoint; set => movement.ShootPoint = value; }

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

    protected virtual void Update ()
    {
        if (Target)
        {
            ShootPoint = Target.transform.position;
        }
    }

    public void Attack (int index)
    {
        if (index < 0 || index >= attackEvents.Length) return;

        attackEvents[index].Invoke();
    }

    public void MoveTowards (Vector2 point)
    {
        MoveInDirection(point - (Vector2)transform.position);

    }

    public void MoveInDirection (Vector2 direction)
    {
        direction.Normalize();

        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float currentAngle = movement.transform.eulerAngles.z;
        float deltaAngle = Mathf.DeltaAngle(targetAngle, currentAngle);

        float dot = Vector2.Dot(direction, movement.transform.right);

        movement.ThrottleInput = dot;
        movement.TurnInput = Mathf.Clamp(deltaAngle * angleScale, -1.0f, 1.0f);
    }
}
