using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class GameplayView : MonoBehaviour
{
    private GameplayController gameplayController;
    private ClickInputActions inputAction;

    private void Awake()
    {
        inputAction = new ClickInputActions();
    }

    private void OnEnable()
    {
        inputAction.Click.Click.performed += OnClickPerformed;
        inputAction.Enable();
    }

    private void OnDisable()
    {
        inputAction.Click.Click.performed -= OnClickPerformed;
        inputAction.Disable();
    }

    private void OnClickPerformed(InputAction.CallbackContext ctx)
    {
        Vector2 pos = GetPointerPosition();
        CarClick(pos);
    }

    private Vector2 GetPointerPosition()
    {
        // Mouse first (PC/Laptop)
        if (Mouse.current != null)
            return Mouse.current.position.ReadValue();

        // Touchscreen (Android/iOS)
        if (Touchscreen.current != null)
        {
            var touch = Touchscreen.current.primaryTouch;
            return touch.position.ReadValue();
        }

        return Vector2.zero;
    }

    private void CarClick(Vector2 pos)
    {
        Ray ray = Camera.main.ScreenPointToRay(pos);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            PlayerView player = hit.collider.GetComponent<PlayerView>();
            if (player != null)
            {
                Debug.Log("Clicked on " + hit.collider.name);
                if(player.CanMoveForward() && !player.agent.hasPath)
                {
                    Vector3 destination = gameplayController.GetDestination(player.posIndex);
                    player.posIndex++;
                    player.agent.SetDestination(destination);
                }
            }
            else
                Debug.Log("Clicked on no truck.");
        }
    }

    public void SetController(GameplayController controller)
        => this.gameplayController = controller;
}