using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class CustomerBehaviour : MonoBehaviour
{
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] private float delayPeriod = 2f;
    [SerializeField] private float radius = 30f;
    [SerializeField] private Transform marker;
    [SerializeField] private bool delayIsFixed;
    [SerializeField] private Transform[] zones;
    private float currentDelay;
    private bool stopAgent;
    private bool onMarch = true;
    private float waitTime;
    private Vector2 startPosition;
    private NavMeshAgent navMeshAgent;
    private int mood = 1;

    private void Start()
    {
        currentDelay = delayPeriod;
        startPosition = transform.position;
        if (sprite == null) sprite = GetComponent<SpriteRenderer>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
    }

    private void Update()
    {
        if (!stopAgent && !onMarch)
        {
            if (waitTime < Time.timeSinceLevelLoad)
            {
                SetDestination();
            }
        }
        else if (onMarch)
        {
            if (Vector3.Distance(transform.position, navMeshAgent.destination) <= navMeshAgent.stoppingDistance)
            {
                onMarch = false;
                if (!delayIsFixed) currentDelay = Random.Range(0, delayPeriod);
                waitTime = Time.timeSinceLevelLoad + currentDelay;
            }
        }
    }

    /// <summary>
    /// Set the destination point
    /// </summary>
    /// <param name="pos"></param>
    public void SetDestination()
    {
        int index = Random.Range(0, zones.Length);
        Transform zone = zones[index];
        NavMeshModifier navMeshModifier = zone.GetComponent<NavMeshModifier>();
        int areaInt = 0;
        if (navMeshModifier != null) { areaInt = navMeshModifier.area; }
        else
        {
            NavMeshModifierVolume navMeshModifierVolume = zone.GetComponent<NavMeshModifierVolume>();
            if (navMeshModifierVolume != null) { areaInt = navMeshModifierVolume.area; }
        }
        Vector3 pos = GetRandomPositionOnNavmesh(zone.position, areaInt);
        navMeshAgent.destination = pos;
        if (marker != null) marker.position = pos;
        onMarch = true;
    }

    public bool StopAgent { get { return stopAgent; } set { stopAgent = value; } }

    private Vector3 GetRandomPositionOnNavmesh(Vector3 zonePosition, int navMeshArea)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += zonePosition;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        bool navBool = (NavMesh.SamplePosition(randomDirection, out hit, radius, navMeshArea));
        finalPosition = hit.position;
        return finalPosition;
    }


}
