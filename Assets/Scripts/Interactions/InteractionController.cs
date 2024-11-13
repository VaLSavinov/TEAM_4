using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    [SerializeField] private float _interactionDistance;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }
    }

    private void TryInteract()
    {
        RaycastHit hit;

        // ��������� Raycast ������ �� ������
        if (Physics.Raycast(transform.position, transform.forward, out hit, _interactionDistance))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                interactable.Interact(); // �������� ����� Interact � �������
            }
        }
    }
}
