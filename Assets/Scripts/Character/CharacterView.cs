using System.Collections;
using UnityEngine;

public class CharacterView : MonoBehaviour
{

    //[SerializeField] private AudioSource audio;
    [SerializeField] private Colors Color;
    public Colors GetColor() => this.Color;

    public void MoveToSeat(Transform seat, float speed = 15f)
    {
        StopAllCoroutines();
        StartCoroutine(MoveToSeatRoutine(seat, speed));
    }

    private IEnumerator MoveToSeatRoutine(Transform seat, float speed)
    {
        //audio.Play();
        
        // --- Capture WORLD target BEFORE parenting ---
        Vector3 targetPos = seat.position;
        Quaternion targetRot = seat.rotation;
        Vector3 targetScale = Vector3.one;

        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;
        Vector3 startScale = transform.localScale;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * speed;
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            transform.rotation = Quaternion.Slerp(startRot, targetRot, t);
            transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            yield return null;
        }

        // --- Parent AFTER movement ---
        transform.SetParent(seat);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }
}
