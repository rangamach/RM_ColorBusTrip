using System.Collections;
using UnityEngine;

public class SignView : MonoBehaviour
{
    [SerializeField] private SeatSlot[] StandSlot = new SeatSlot[8];

    [Header("Sign Colors")]
    [SerializeField] private MeshRenderer head;
    [SerializeField] private Material yellow;
    [SerializeField] private Material blue;
    [SerializeField] private Material red;
    [SerializeField] private Material white;

    [SerializeField] private ParticleSystem confetti;

    private SeatSlot[] initialStandSlot;
    private Material initialMaterial;

    private Colors color = Colors.White;

    private void Awake()
    {
        initialStandSlot = new SeatSlot[StandSlot.Length];

        for(int i =0;i<StandSlot.Length;i++)
        {
            initialStandSlot[i] = new SeatSlot();
            initialStandSlot[i].SeatTransform = StandSlot[i].SeatTransform;
            initialStandSlot[i].CharacterView = StandSlot[i].CharacterView;
        }
    }
    private void Start()
    {
        PopulateInitial();
        UpdateSignColor();
        ApplySignColor();

        GameService.Instance.EventService.OnRestart.AddListener(OnRestart);
        GameService.Instance.EventService.OnConfetti.AddListener(PlayConfetti);
    }

    private void OnTriggerEnter(Collider other)
    {
        TruckView truck = other.GetComponent<TruckView>();

        if (truck != null)
        {
            HandleTruck(truck);
        }
    }

    // ------------------------------------------------------
    // POPULATE SIGN AT START (ONLY IF PREFAB HAS CHARACTERS)
    // ------------------------------------------------------
    private void PopulateInitial()
    {
        for (int i = 0; i < StandSlot.Length; i++)
        {
            if (StandSlot[i].CharacterView == null)
                continue;

            // Instantiate runtime copy
            CharacterView character = Instantiate(StandSlot[i].CharacterView);

            // Parent to seat
            character.transform.SetParent(StandSlot[i].SeatTransform);

            // Reset local transform
            character.transform.localPosition = Vector3.zero;
            character.transform.localRotation = Quaternion.identity;
            character.transform.localScale = Vector3.one;

            // Replace reference with runtime instance
            StandSlot[i].CharacterView = character;
        }
    }

    // ------------------------------------------------------
    // MAIN TRIGGER LOGIC
    // ------------------------------------------------------

    private void HandleTruck(TruckView truck)
    {
        StartCoroutine(ProcessTruckPassengers(truck));
    }
    private IEnumerator ProcessTruckPassengers(TruckView truck)
    {
        // 1?? Unload wrong-color passengers one by one
        for (int i = 0; i < truck.Seats.Length; i++)
        {
            CharacterView cv = truck.Seats[i].CharacterView;
            if (cv == null) continue;

            if (cv.GetColor() != truck.GetTruckColor())
            {
                int emptyIndex = GetFirstEmptySignSeat();
                if (emptyIndex == -1) yield break; // Sign full

                AssignToSignSeat(cv, emptyIndex);
                StandSlot[emptyIndex].CharacterView = cv;
                truck.Seats[i].CharacterView = null;
                UpdateSignColor();

                yield return new WaitForSeconds(0.1f); // wait 0.5s before next passenger
            }
        }

        // 2?? Load matching passengers one by one
        for (int i = 0; i < StandSlot.Length; i++)
        {
            CharacterView waiting = StandSlot[i].CharacterView;
            if (waiting == null) continue;

            if (waiting.GetColor() == truck.GetTruckColor())
            {
                int emptyTruckSeat = truck.GetNextEmptySeatIndex();
                if (emptyTruckSeat == -1) yield break; // Truck full

                AssignToTruckSeat(waiting, truck, emptyTruckSeat);
                truck.Seats[emptyTruckSeat].CharacterView = waiting;
                StandSlot[i].CharacterView = null;

                if (!truck.fullyCorrect && truck.IsFullWithCorrectColorPassengers())
                {
                    if (truck.fullyCorrectAudio != null)
                    {
                        truck.fullyCorrectAudio.Play();
                    }
                    else
                    {
                        Debug.Log("Fully AS not found.");
                    }
                    truck.fullyCorrect = true;
                    GameService.Instance.EventService.OnWinConfetti.InvokeEvent();
                }

                UpdateSignColor();
                yield return new WaitForSeconds(0.1f); // wait 0.5s before next passenger
            }
        }

        truck.SetNextDestination();
    }

    // ------------------------------------------------------
    // SIGN SEAT / TRUCK SEAT ASSIGNMENT HELPERS
    // ------------------------------------------------------
    private void AssignToSignSeat(CharacterView cv, int index)
    {
        Transform seat = StandSlot[index].SeatTransform;
        cv.MoveToSeat(seat, 20f);
    }

    private void AssignToTruckSeat(CharacterView cv, TruckView truck, int index)
    {
        Transform seat = truck.Seats[index].SeatTransform;
        cv.MoveToSeat(seat, 20f);
    }

    // ------------------------------------------------------
    // GET FIRST EMPTY SIGN SEAT
    // ------------------------------------------------------
    private int GetFirstEmptySignSeat()
    {
        for (int i = 0; i < StandSlot.Length; i++)
        {
            if (StandSlot[i].CharacterView == null)
                return i;
        }
        return -1;
    }

    // ------------------------------------------------------
    // SIGN COLOR LOGIC
    // ------------------------------------------------------

    private void UpdateSignColor()
    {
        Colors newColor = Colors.White;

        // Find the first occupied slot
        for (int i = 0; i < StandSlot.Length; i++)
        {
            if (StandSlot[i].CharacterView != null)
            {
                newColor = StandSlot[i].CharacterView.GetColor();
                break;
            }
        }

        // Only update if different
        if (color != newColor)
        {
            GetComponent<Animator>().SetTrigger("color");
            color = newColor;
            // Don't apply yet — wait for anim event
        }
    }
    public void ChangeColorAnimEvent()
    {
        ApplySignColor();
    }

    private void ApplySignColor()
    {
        switch (color)
        {
            case Colors.White:
                head.material = white; break;
            case Colors.Red:
                head.material = red; break;
            case Colors.Yellow:
                head.material = yellow; break;
            case Colors.Blue:
                head.material = blue; break;
        }
    }

    public void OnRestart()
    {
        // Destroy existing characters
        for (int i = 0; i < StandSlot.Length; i++)
        {
            if (StandSlot[i].CharacterView != null)
            {
                Destroy(StandSlot[i].CharacterView.gameObject);
            }
        }

        // Reset StandSlot array
        StandSlot = new SeatSlot[initialStandSlot.Length];

        for (int i = 0; i < initialStandSlot.Length; i++)
        {
            StandSlot[i] = new SeatSlot();
            StandSlot[i].SeatTransform = initialStandSlot[i].SeatTransform;
            StandSlot[i].CharacterView = initialStandSlot[i].CharacterView;

            if (StandSlot[i].CharacterView != null)
            {
                // Instantiate a fresh runtime copy
                CharacterView newChar = Instantiate(StandSlot[i].CharacterView);

                // Parent to seat
                newChar.transform.SetParent(StandSlot[i].SeatTransform);

                // Reset local transform
                newChar.transform.localPosition = Vector3.zero;
                newChar.transform.localRotation = Quaternion.identity;
                newChar.transform.localScale = Vector3.one;

                // Replace reference with new instance
                StandSlot[i].CharacterView = newChar;
            }
        }

        // Reset sign color
        color = Colors.White; // optional, ensures default
        UpdateSignColor();
    }
    private void PlayConfetti() => confetti.Play();
}