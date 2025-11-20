using UnityEngine;
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
                Debug.Log("Clicked non-truck object");
                return;
            }

            if (truck.agent == null)
            {
                //Debug.LogError("Truck NavMeshAgent missing: " + truck.name);
                return;
            }

            if (gameplayController == null)
            {
                Debug.LogError("GameplayController missing");
                return;
            }

            if (truck.CanMoveForward() && !truck.agent.hasPath)
            {
                Vector3 destination = gameplayController.GetDestination(truck.posIndex);
                truck.posIndex++;
                truck.agent.SetDestination(destination);
            }
        }
    }

    public void SetController(GameplayController controller)
    {
        gameplayController = controller;
    }
}