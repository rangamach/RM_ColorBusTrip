using UnityEngine;

public class FinalDestination : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<TruckView>())
        {
            other.gameObject.SetActive(false);
        }
    }
}
