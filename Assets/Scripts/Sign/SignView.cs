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

    private Colors color = Colors.White;

    private void Start()
    {
        PopulateInitial();
        UpdateSignColor();
    }

    private void OnTriggerEnter(Collider other)
    {
        TruckView truck = other.GetComponent<TruckView>();

        if (truck != null)
        {
            Debug.Log("Truck entered sign trigger.");
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

            CharacterView original = StandSlot[i].CharacterView;

            // instantiate copy
            CharacterView character = Instantiate(original);
            AssignToSignSeat(character, i);

            // clean prefab reference
            StandSlot[i].CharacterView = character;
        }
    }

    // ------------------------------------------------------
    // MAIN TRIGGER LOGIC
    // ------------------------------------------------------
    private void HandleTruck(TruckView truck)
    {
        UnloadWrongColorPassengers(truck);
        LoadPassengersToTruck(truck);

        UpdateSignColor();
    }

    // ------------------------------------------------------
    // UNLOAD WRONG-COLOR PASSENGERS FROM TRUCK ? SIGN
    // ------------------------------------------------------
    private void UnloadWrongColorPassengers(TruckView truck)
    {
        for (int i = 0; i < truck.Seats.Length; i++)
        {
            CharacterView cv = truck.Seats[i].CharacterView;
            if (cv == null) continue;

            // Only remove mismatched passengers
            if (cv.GetColor() == truck.GetTruckColor())
                continue;

            int emptyIndex = GetFirstEmptySignSeat();
            if (emptyIndex == -1) return; // Sign is full

            // Move passenger to sign
            AssignToSignSeat(cv, emptyIndex);

            StandSlot[emptyIndex].CharacterView = cv;
            truck.Seats[i].CharacterView = null;
        }
    }

    // ------------------------------------------------------
    // LOAD MATCHING PASSENGERS FROM SIGN ? TRUCK
    // ------------------------------------------------------
    private void LoadPassengersToTruck(TruckView truck)
    {
        for (int i = 0; i < StandSlot.Length; i++)
        {
            CharacterView waiting = StandSlot[i].CharacterView;
            if (waiting == null) continue;

            // Only load passengers matching truck color
            if (waiting.GetColor() != truck.GetTruckColor())
                continue;

            int emptyTruckSeat = truck.GetNextEmptySeatIndex();
            if (emptyTruckSeat == -1)
                return; // Truck full

            // Move passenger to truck
            AssignToTruckSeat(waiting, truck, emptyTruckSeat);

            truck.Seats[emptyTruckSeat].CharacterView = waiting;
            StandSlot[i].CharacterView = null;
        }
    }

    // ------------------------------------------------------
    // SIGN SEAT / TRUCK SEAT ASSIGNMENT HELPERS
    // ------------------------------------------------------
    private void AssignToSignSeat(CharacterView cv, int index)
    {
        cv.transform.SetParent(StandSlot[index].SeatTransform);
        cv.transform.localPosition = Vector3.zero;
        cv.transform.localRotation = Quaternion.Euler(0, 90, 0);
        cv.transform.localScale = Vector3.one;
    }

    private void AssignToTruckSeat(CharacterView cv, TruckView truck, int index)
    {
        cv.transform.SetParent(truck.Seats[index].SeatTransform);
        cv.transform.localPosition = Vector3.zero;
        cv.transform.localRotation = Quaternion.identity;
        cv.transform.localScale = Vector3.one;
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

        for (int i = 0; i < StandSlot.Length; i++)
        {
            if (StandSlot[i].CharacterView != null)
            {
                newColor = StandSlot[i].CharacterView.GetColor();
                break;
            }
        }

        color = newColor;
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
}