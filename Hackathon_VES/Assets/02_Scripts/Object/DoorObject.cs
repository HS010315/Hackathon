using System.Collections;
using UnityEngine;

public class DoorObject : MonoBehaviour, IInteractable
{
    private bool isOpen = false;
    public float rotationAngle = 90f;
    public float rotationSpeed = 5f;
    private Rigidbody rb;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // Rigidbody를 물리적으로 움직이지 않게 설정
        closedRotation = transform.rotation;
        openRotation = closedRotation * Quaternion.Euler(0, rotationAngle, 0);
    }

    public void Interact()
    {
        if (!isOpen)
        {
            StartCoroutine(RotateDoor(openRotation));
        }
        else
        {
            StartCoroutine(RotateDoor(closedRotation));
        }
        isOpen = !isOpen;
    }

    private IEnumerator RotateDoor(Quaternion targetRotation)
    {
        float t = 0;
        Quaternion startRotation = transform.rotation;

        while (t < 1)
        {
            t += Time.deltaTime * rotationSpeed;
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            yield return null;
        }
    }
}