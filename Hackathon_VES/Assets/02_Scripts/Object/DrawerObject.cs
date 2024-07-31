using System.Collections;
using UnityEngine;

public class DrawerObject : MonoBehaviour, IInteractable
{
    private bool isOpen = false;
    public float slideDistance = 1f; // �����̵��� �Ÿ�
    public float slideSpeed = 2f; // �����̵� �ӵ�
    private Vector3 closedPosition;
    private Vector3 openPosition;
    public bool isXopen = false;

    void Start()
    {
        closedPosition = transform.position;
        if(isXopen)
        {
            openPosition = closedPosition + transform.right * slideDistance;
        }
        else
        {
            openPosition = closedPosition + transform.forward * slideDistance;
        }
    }

    public void Interact()
    {
        if (!isOpen)
        {
            StartCoroutine(SlideDrawer(openPosition));
        }
        else
        {
            StartCoroutine(SlideDrawer(closedPosition));
        }
        isOpen = !isOpen;
    }

    private IEnumerator SlideDrawer(Vector3 targetPosition)
    {
        float t = 0;
        Vector3 startPosition = transform.position;

        while (t < 1)
        {
            t += Time.deltaTime * slideSpeed;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }
    }
}