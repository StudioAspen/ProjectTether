using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public float moveSpeed = 5;
    public Transform[] waypoints;
    private int currentWaypoints = 0;
    private EnemyState currentState = EnemyState.Wait;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public float waitDuration = 2f;
    private float waitTimer = 0f;

    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (waypoints == null || waypoints.Length == 0)
        {
            return;
        }
       switch (currentState)
       {
        case EnemyState.Wait:
                // Waiting Behavior goes here:
                waitTimer += Time.deltaTime;
                if (waitTimer >= waitDuration)
                {
                    waitTimer = 0f;
                    currentState = EnemyState.Patrol;
                }
                break;

        case EnemyState.Chase:
        // Chasing Behavior goes here:
            break;

        case EnemyState.Patrol:
                // Chasing Patrol goes here:
                Vector3 targetPosition = waypoints[currentWaypoints].position;
                Vector3 direction = targetPosition - transform.position;
                direction.y = 0f;
                if(direction.magnitude <= 0.1f)
                {
                    currentWaypoints++;

                    if (currentWaypoints >= waypoints.Length)
                    {
                        currentWaypoints = 0;
                    }
                    currentState = EnemyState.Wait;
                }
                else
                {
                    float step = Mathf.Min(moveSpeed * Time.deltaTime, direction.magnitude);
                    controller.Move(direction.normalized * step);
                }
            break;
       }


       
    }

    private enum EnemyState 
    {
        Wait,
        Chase,
        Patrol
    }
}
