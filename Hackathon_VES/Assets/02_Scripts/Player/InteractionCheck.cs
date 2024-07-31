using UnityEngine;
using UnityEngine.UI;

public class InteractionController : MonoBehaviour
{
    public float interactionDistance = 3.0f;
    public Camera playerCamera;
    public Text interactionText;
    public PlayerStateInfo playerStateInfo;

    private IEffectable currentEffectable;
    private IInteractable currentInteractable;
    public bool isDisabled;
    void Update()
    {
        DetectInteractable();
        HandleInteraction();
    }

    void DetectInteractable()
    {
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            IEffectable effectable = hit.collider.GetComponent<IEffectable>();
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                currentInteractable = interactable;
                if(isDisabled)
                {
                    interactionText.color = Color.red;
                }
                interactionText.gameObject.SetActive(true);
                interactionText.text = "E를 눌러 상호작용";
            }
            else
            {
                currentInteractable = null;
                interactionText.gameObject.SetActive(false);
            }

            if (effectable != null)
            {
                currentEffectable = effectable;
            }
            else
            {
                currentEffectable = null;
            }
        }
        else
        {
            currentInteractable = null;
            currentEffectable = null;
            interactionText.gameObject.SetActive(false);
        }
    }

    void HandleInteraction()
    {
        if (currentEffectable != null && Input.GetKeyDown(KeyCode.E))
        {
            currentEffectable.EffectToPlayer(playerStateInfo);
        }

        if (currentInteractable != null && Input.GetKeyDown(KeyCode.E))
        {
            currentInteractable.Interact();
        }
    }
}