using System;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public class SeatSlot
{
    public Transform SeatTransform;
    public CharacterView CharacterView;
}

public class TruckView : MonoBehaviour
{
    [SerializeField] public SeatSlot[] Seats = new SeatSlot[16];
    [SerializeField] private Colors color;

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

        PopulateAtStart();
    }
    private void Update()
    {
        AvoidOtherTrucks();
    }

    public bool CanMoveForward()
    {
        Vector3 origin = transform.position + transform.forward * forwardOffset + Vector3.up * upOffset;
        Collider[] hits = Physics.OverlapSphere(origin, checkDistance, truckLayer);

        foreach (var hit in hits)
        {
            if (hit.gameObject.name == transform.name) continue;

            Vector3 toOther = (hit.transform.position - transform.position).normalized;

            float dot = Vector3.Dot(transform.forward, toOther);

            if (dot > 0)
            {
                return false;
            }
        }
        return true;
    }
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
    private void PopulateAtStart()
    {
        for (int i = 0; i < Seats.Length; i++)
        {
            if (Seats[i].CharacterView != null)
            {
                CharacterView character = Instantiate(Seats[i].CharacterView, Seats[i].SeatTransform);

                character.transform.localPosition = Vector3.zero;
                character.transform.localScale = Vector3.one;
                character.transform.rotation = Quaternion.identity;

                Seats[i].CharacterView = character;
            }
        }
    }
    public int GetNextEmptySeatIndex()
    {
        for (int i = 0; i < Seats.Length; i++)
        {
            if (Seats[i].CharacterView == null)
            {
                return i;
            }
        }
        return -1;
    }
    public bool CheckIfPassengersCanLeave()
    {
        for (int i = Seats.Length - 1; i >= 0; i--)
        {
            if (Seats[i].CharacterView && Seats[i].CharacterView.GetColor() != color)
            {
                return true;
            }
        }
        return false;
    }
    public Colors GetTruckColor() => this.color;
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
