using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class SecurityBehaviour : MonoBehaviour
{
    [SerializeField] private float delayPeriod = 2f;
    [SerializeField] private float checkDistance = 2f;
    private NavMeshAgent navMeshAgent;
    private GameObject newTarget;
    private GameObject previousTarget;
    private float waitTime;
    private float distance;
    private bool isChecking;
    private CustomerBehaviour customerBehaviour;



    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }


    private void Update()
    {

        if (!isChecking)
        {
            OnMarch();
        }
        else
        {
            CheckingTime();
        }

        if (newTarget != null)
        {
            distance = Vector3.Distance(transform.position, newTarget.transform.position);
            if (distance < checkDistance && !isChecking)
            {
                GotTheTarget();
            }
        }
    }

    /// <summary>
    /// return closest customer if he/she is not the previous one
    /// </summary>
    /// <returns></returns>
    private GameObject FindNewTarget()
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
            if (curDistance < distance && previousTarget != go)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }

    private GameObject[] GetAllCustomers()
    {
        GameObject[] allCustomers = GameObject.FindGameObjectsWithTag("Civilian");

        return allCustomers;
    }

    private void OnMarch()
    {
        if (newTarget == null)
        {
            newTarget = FindNewTarget();
        }
        else
        {
            navMeshAgent.destination = newTarget.transform.position;
        }
    }

    private void GotTheTarget()
    {
        customerBehaviour = newTarget.GetComponent<CustomerBehaviour>();
        waitTime = delayPeriod;
        if (customerBehaviour != null) customerBehaviour.StopAgent = true;
        previousTarget = newTarget;
        isChecking = true;
    }

    /// <summary>
    /// Period for checking the customer
    /// </summary>
    private void CheckingTime()
    {
        if (waitTime > 0)
        {
            waitTime -= Time.deltaTime;
        }
        else
        {
            ResetChecking();
        }
    }

    /// <summary>
    /// Set all values of checking process to default
    /// </summary>
    private void ResetChecking()
    {
        newTarget = null;
        isChecking = false;
        waitTime = 0;
        if (customerBehaviour != null)
        {
            customerBehaviour.StopAgent = false;
            customerBehaviour = null;
        }
    }
}
