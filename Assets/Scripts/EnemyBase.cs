using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float acceleration;

    [Space]
    [SerializeField] EnemyBehaviour[] behaviours;

    [Space]
    [SerializeField] UnityEvent attackEvent;
    [SerializeField] Transform facingContainer;

    protected new Rigidbody2D rigidbody;

    private int behaviourIndex;
    private float clock = 0.0f;

    private GameObject _Target;

    public GameObject Target 
    { 
        get
        {
            if (!_Target)
            {
                _Target = FindObjectOfType<TankMovement>().gameObject;
            }
            return _Target;
        }
        set => _Target = value; 
    }

    public Vector2 MoveDirection { get; set; }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        MoveDirection = Vector2.zero;

        Behave();

        ApplyMovement();

        clock += Time.deltaTime;
    }

    protected virtual void ApplyMovement()
    {
        Vector2 target = MoveDirection.normalized * moveSpeed;
        Vector2 current = rigidbody.velocity;

        Vector2 force = Vector2.ClampMagnitude(target - current, moveSpeed) * acceleration;
        rigidbody.velocity += force * Time.deltaTime;


        facingContainer.right = MoveDirection;
    }

    protected virtual void Behave()
    {
        if (clock > behaviours[behaviourIndex].duration)
        {
            clock = 0.0f;
            behaviourIndex = (behaviourIndex + 1) % behaviours.Length;

            switch (behaviours[behaviourIndex].type)
            {
                case EnemyBehaviour.BehaviourType.Attack:
                    attackEvent?.Invoke();
                    break;
                default:
                    break;
            }
        }

        switch (behaviours[behaviourIndex].type)
        {
            case EnemyBehaviour.BehaviourType.Move:
                MoveDirection = Target ? (Target.transform.position - transform.position) : Vector3.zero;
                break;
            default:
                break;
        }
    }

    [System.Serializable]
    public class EnemyBehaviour
    {
        public enum BehaviourType
        {
            Wait,
            Move,
            Attack
        }

        public BehaviourType type;
        public float duration;
    }

}
