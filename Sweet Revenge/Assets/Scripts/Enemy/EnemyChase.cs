using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;


public class EnemyChase : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private PlayerLife player;
    private NavMeshAgent navMeshAgent;

    private void Start()
    {
        target=FindObjectOfType<Player>().transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
    }

    private void Update()
    {
        if (!player.isDead)
        {
            navMeshAgent.SetDestination(target.position);

            Vector2 direction = target.transform.position - transform.position;
            direction.Normalize();
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(Vector3.forward * angle);
        }
        else navMeshAgent.SetDestination(new Vector3(0,0,0));

        //Quaternion targetRotation = Quaternion.LookRotation(transform.forward, -target.position);
        //Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        //rb.SetRotation(rotation);
    }


}
