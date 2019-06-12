using System.Collections;
using UnityEngine;
using UnityEngine.AI;


public class EnemyAI : MonoBehaviour
{
    Transform player;
    NavMeshAgent agent;
    public Transform[] wayPoints;
    public Transform RayOrigin;
    Vector3[] wayPointsPos = new Vector3[3];
    Animator fsm;
    Vector3 ppos = new Vector3(1, 1, 0);
    float shootFreq = 1f;
    int currentWayPointIndex;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < wayPoints.Length; i++)
        {
            wayPointsPos[i] = wayPoints[i].position;
        }
        fsm = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(wayPointsPos[0]);

        StartCoroutine("CheckPlayer");
    }

    IEnumerator CheckPlayer()
    {
        CheckWayPoint();
        CheckVisibility();
        CheckDistance();
        yield return new WaitForSeconds(0.1f);
        yield return CheckPlayer();
    }

    private void CheckWayPoint()
    {
        float distaceFromWaypoint = Vector3.Distance(transform.position, wayPointsPos[currentWayPointIndex]);
        fsm.SetFloat("distanceFromWaypoint", distaceFromWaypoint);
        
    }

    private void CheckDistance()
    {
        float distance = (player.position - transform.position).magnitude;

        fsm.SetFloat("distance", distance);
    }

    public void SetLookRotation()
    {
        Vector3 dir = (player.position - transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Lerp(transform.rotation,rotation,0.2f);
    }

    private void CheckVisibility()
    {
        float maxDistance = 20;
        Vector3 direction = (player.position - transform.position).normalized;

        if (Physics.Raycast(RayOrigin.position, direction, out RaycastHit info, maxDistance)) 
        {
            if (info.transform.CompareTag("Player"))
            {
                fsm.SetBool("isVisible", true);
            }
            else
                fsm.SetBool("isVisible", false);
        }
        else
            fsm.SetBool("isVisible", false);
    }

    public void SetNextWayPoint()
    {
        if (currentWayPointIndex == 0)
            currentWayPointIndex = 1;
        else if (currentWayPointIndex == 1)
            currentWayPointIndex = 2;
        else
            currentWayPointIndex = 0;

        agent.SetDestination(wayPointsPos[currentWayPointIndex]);

    }

    public void Shoot()
    {
        GetComponent<ShootBehaviour>().Shoot(shootFreq);
    }

    public void Chase()
    {
        agent.SetDestination(player.position - ppos);
    }
    public void Patrol()
    {
        Debug.Log("Patrolling");
    }
}
