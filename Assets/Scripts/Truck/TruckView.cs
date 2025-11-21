using System;
using System.Collections;
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
    [SerializeField] private GameplaySO SO;

    [SerializeField] private float checkDistance;
    [SerializeField] private float forwardOffset;
    [SerializeField] private float upOffset;
    [SerializeField] private LayerMask truckLayer;
    [SerializeField] private LayerMask finalLayer;
    [SerializeField] private Transform barrier;
    //[SerializeField] public AudioSource fullyCorrectAudio;
    private bool hasFinalDestination;
    public bool fullyCorrect;
    

    [HideInInspector]
    public NavMeshAgent agent { get; private set; }
    [HideInInspector]
    public int posIndex;

    void Start()
    {
        fullyCorrect = false;
        hasFinalDestination = false;
        posIndex = 0;
        barrier = null;
        agent = GetComponent<NavMeshAgent>();

        PopulateAtStart();
    }
    private void Update()
    {
        AvoidOtherTrucks();
        ReachedFinalDestination();
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
    private void ReachedFinalDestination()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + transform.forward * forwardOffset + Vector3.up * upOffset, transform.forward, out hit, checkDistance, finalLayer))
        {
            if(fullyCorrect)
            {
                if(!hasFinalDestination && barrier == null &&  hit.collider.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Close"))
                {
                    hasFinalDestination = true;
                    barrier = hit.transform;
                    Vector3 final = SO.FinalPosition;
                    StartCoroutine(BarrierAnimation(hit.collider.GetComponent<Animator>()));
                    agent.SetDestination(final);
                    return;
                }
                else if(hit.transform != barrier)
                {
                    this.gameObject.SetActive(false);
                }
            }
            else
            {
                SetNextDestination();
            }
        }
    }
    private IEnumerator BarrierAnimation(Animator anim)
    {
        anim.SetTrigger("openclose");

        yield return new WaitForSeconds(3f);

        anim.SetTrigger("openclose");
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
    public bool IsFullWithCorrectColorPassengers()
    {
        // Loop through all seats
        for (int i = 0; i < Seats.Length; i++)
        {
            // If any seat is empty, return false
            if (Seats[i].CharacterView == null)
                return false;

            // If any passenger color does not match truck color, return false
            if (Seats[i].CharacterView.GetColor() != color)
                return false;
        }

        // All seats filled with correct color passengers
        return true;
    }
    public Colors GetTruckColor() => this.color;

    public void SetNextDestination()
    { 
        if (CanMoveForward())
        {
            int newIndex = posIndex % SO.Positions.Count;
            Vector3 destination = SO.Positions[newIndex];
            posIndex++;
            agent.SetDestination(destination);
        }
    }
}
