using UnityEngine;
using UnityEngine.AI;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private float checkDistance;
    [SerializeField] private float sphereRadius;
    [SerializeField] private float forwardOffset;
    [SerializeField] private float upOffset;
    [SerializeField] private LayerMask truckLayer;

    [HideInInspector]
    public NavMeshAgent agent { get; private set; }
    [HideInInspector]
    public int posIndex;

    void Start()
    { 
        posIndex = 0;
        agent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        AvoidOtherTrucks();
    }

    public bool CanMoveForward()
    {
        Vector3 origin = transform.position + transform.forward * forwardOffset + Vector3.up * upOffset;
        Collider[] hits = Physics.OverlapSphere(origin,checkDistance,truckLayer);

        foreach(var hit in hits)
        {
            Vector3 toOther = (hit.transform.position - transform.position).normalized;

            float dot = Vector3.Dot(transform.forward,toOther);

            if(dot > 0)
            {
                Debug.Log("Blocked by: " + hit.name);
                return false;
            }
        }
        return true;
    }

    //public bool CanMoveForward()
    //{
    //    Vector3 start = transform.position + transform.forward * forwardOffset + Vector3.up * upOffset;
    //    Vector3 direction = transform.forward;

    //    if (Physics.Raycast(start, direction, out RaycastHit hit, checkDistance, truckLayer))
    //    {
    //        Debug.Log("Blocked by: " + hit.collider.name);
    //        return false;
    //    }
    //    return true;
    //}
    private void AvoidOtherTrucks()
    {
        if (agent.hasPath && agent.remainingDistance > agent.stoppingDistance)
        {
            if (!CanMoveForward())
            {
                agent.isStopped = true;
            }
            else
            {
                agent.isStopped = false;
            }
        }
        else
        {
            agent.isStopped = true;
        }
    }
    private void OnDrawGizmos()
    {
        Vector3 origin = transform.position + transform.forward * forwardOffset + Vector3.up * upOffset;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(origin, checkDistance);

        // Draw forward line for reference
        Gizmos.color = Color.red;
        Gizmos.DrawLine(origin, origin + transform.forward * checkDistance);
    }
}
