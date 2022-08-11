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
    private int areaInt;
    private Transform zone;
    private Bounds bounds;
    private float xBound;
    private float yBound;
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
        zone = zones[index];
        NavMeshModifier navMeshModifier = zone.GetComponent<NavMeshModifier>();
        areaInt = 0;
        Vector3 pos = Vector3.zero;
        if (navMeshModifier != null)
        {
            areaInt = navMeshModifier.area;
        }
        else
        {
            NavMeshModifierVolume navMeshModifierVolume = zone.GetComponent<NavMeshModifierVolume>();
            if (navMeshModifierVolume != null) { areaInt = navMeshModifierVolume.area; }
        }

        Renderer renderer = zone.GetComponent<Renderer>();
        if (renderer != null)
        {
            bounds = renderer.bounds;
            pos = new Vector3(Random.Range(bounds.min.x, bounds.max.x), zone.position.y, Random.Range(bounds.min.z, bounds.max.z));
        }
        else
        {
            pos = GetRandomPositionOnNavmesh(zone.position);
        }
        marker.position = pos;
        navMeshAgent.destination = pos;
        onMarch = true;
    }

    public bool StopAgent { get { return stopAgent; } set { stopAgent = value; } }

    private Vector3 GetRandomPositionOnNavmesh(Vector3 zonePosition)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += zonePosition;
        Debug.DrawLine(transform.position, randomDirection);
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        bool navBool = (NavMesh.SamplePosition(randomDirection, out hit, radius, NavMesh.AllAreas));
        if (!navBool) Debug.Log("noon!!!");
        finalPosition = hit.position;
        return finalPosition;
    }


}
