using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {

    public float lookRadius = 10f;
    public List<Transform> waypoints = new List<Transform>();

    public float aggroSpeed = 3.5f; // How fast to move when chasing the player, mostly
    public float patrolSpeed = 2.0f;

    public int maxHealth = 50;
    private int currentHealth;

    private Transform playerTransform;
    private NavMeshAgent agent;

    private bool movingToWaypoint = true; // Are we currently moving towards our next waypoint? TODO: should this just be "patrolling"?
    private int waypointIndex = 0;
    private float reachProximitySqr = 0.01f; // Sqr distance at which a waypoint is considered "reached"

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerTransform = PlayerManager.instance.player.transform;
        agent.speed = patrolSpeed;
        agent.SetDestination(waypoints[waypointIndex].position);

        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (CanSeePlayer())
        {
            movingToWaypoint = false;
            Aggro();
            agent.SetDestination(playerTransform.position);

            // if within attack radius, do Attack()
        }
        else // Can't see player, proceed to waypoint
        {
            // If we aren't already headed to the waypoint...
            if (!movingToWaypoint)
            {
                Deaggro();
                movingToWaypoint = true;
                agent.SetDestination(waypoints[waypointIndex].position);
            }
            // Check to see if we're close enough to the next waypoint to consider it reached
            Vector3 offset = waypoints[waypointIndex].position - transform.position;
            float sqrLen = offset.sqrMagnitude;
            if (sqrLen < reachProximitySqr)
            {
                // Reached the waypoint
                waypointIndex += 1;
                if (waypointIndex >= waypoints.Count)
                {
                    waypointIndex = 0;
                }
                agent.SetDestination(waypoints[waypointIndex].position);
            }
        }
    }

    #region Damage

    public void TakeHit(int damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    #endregion

    private void Aggro()
    {
        agent.speed = aggroSpeed;
    }

    private void Deaggro()
    {
        agent.speed = patrolSpeed;
    }

    private bool CanSeePlayer()
    {
        // TODO: Frustrum planes or TestPlanesAABB and NavMeshAgent.Raycast
        // or https://answers.unity.com/questions/41759/creating-enemy-field-of-vision.html
        if (Vector3.Distance(transform.position,playerTransform.position) <= lookRadius)
        {
            return true;
        }
        return false;
    }

    private void FaceTarget()
    {
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }


}
