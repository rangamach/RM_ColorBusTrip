using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameplayView : MonoBehaviour
{
    private GameplayController gameplayController;
    private TruckView[] trucks;
    private Image Tint;
    private bool confettiPlayed;
    private int trucksinactive;
    private ClickInputActions inputAction;

    private void Awake()
    {
        inputAction = new ClickInputActions();
    }

    private void OnEnable()
    {
        inputAction.Click.Click.performed += OnClickPerformed;
        inputAction.Enable();

        confettiPlayed = false;
        GameService.Instance.EventService.OnWinConfetti.AddListener(WinConfetti);
    }

    private void OnDisable()
    {
        inputAction.Click.Click.performed -= OnClickPerformed;
        inputAction.Disable();

        GameService.Instance.EventService.OnWinConfetti.RemoveListener(WinConfetti);
    }

    private void OnClickPerformed(InputAction.CallbackContext ctx)
    {
        Vector2 pos = GetPointerPosition();
        HandleTruckClick(pos);
    }

    private Vector2 GetPointerPosition()
    {
        if (Mouse.current != null) return Mouse.current.position.ReadValue();
        if (Touchscreen.current != null)
        {
            var touch = Touchscreen.current.primaryTouch;
            return touch.position.ReadValue();
        }
        return Vector2.zero;
    }

    private void HandleTruckClick(Vector2 pos)
    {
        Ray ray = Camera.main.ScreenPointToRay(pos);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            TruckView truck = hit.collider.GetComponent<TruckView>();
            if (truck == null)
            {
                return;
            }
            else
            {
                if (truck.posIndex == 0 && !Tint.gameObject.activeInHierarchy)
                {
                    truck.SetNextDestination();
                }
            }
        }
    }

    private void WinConfetti()
    {
        if (!confettiPlayed)
        {
            foreach (TruckView truck in trucks)
            {
                if (!truck.fullyCorrect)
                {
                    return;
                }
            }

            confettiPlayed = true;
            Debug.Log("Confetti...");
            //Play confetti here...
        }
        else
        {
            trucksinactive = 0;
            foreach (TruckView truck in trucks)
            {
                if (!truck.gameObject.activeInHierarchy)
                {
                    trucksinactive++;
                }
            }
            if (trucksinactive == trucks.Length - 1)
            {
                Debug.Log("Win UI...");
                GameService.Instance.UI.AddCoins();
                //Show win UI here...
            }
        }
    }
    private IEnumerator WaitsForTruckToDisable()
    {
        yield return new WaitForSeconds(0.1f);


    }

    public void SetController(GameplayController controller)
    {
        gameplayController = controller;
    }
    public void SetTint(Image tint) => this.Tint = tint;
    public void SetTruckArray(TruckView[] trucks) => this.trucks = trucks;
}