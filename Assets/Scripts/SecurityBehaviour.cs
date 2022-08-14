using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SecurityBehaviour : MonoBehaviour
{
    [SerializeField] private float delayPeriod = 2f;
    private NavMeshAgent navMeshAgent;
    private GameObject newTarget;
    private GameObject previousTarget;
    private float waitTime;



    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }


    private void Update()
    {
        
    }
    
    private void FindNewTarget()
    {
        newTarget = null;
        GameObject[] gos = GetAllCustomers();
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
    }

    private void OnMarch()
    {
        navMeshAgent.destination = newTarget.transform.position;
    }

    private GameObject[] GetAllCustomers()
    {
        GameObject[] allCustomers = GameObject.FindGameObjectsWithTag("Civilian");

        return allCustomers;
    }
}
