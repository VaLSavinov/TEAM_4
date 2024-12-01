using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacmentPoint : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject _interactObject;
    [SerializeField] private ItemType _itemType;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip genActivationSound;

    private IInteractable _interactable;

    private bool _isInteract = false;

    private void Awake()
    {
        _interactable = _interactObject.GetComponent<IInteractable>();
    }

    private IEnumerator ShowText()
    {
        yield return new WaitForSeconds(15);
        GameMode.PlayerUI.DeactivatePanel();
    }
    
    public void Interact()
    {
        if (_isInteract) { return; }
        else 
        {
            GameMode.PlayerUI.DeactivatePanel();
            GameMode.PlayerUI.ShowText("UI.Request",true);
            StartCoroutine(ShowText());
        }
    }

    public bool Interact(ref GameObject interactingOject)
    {      
        PickableItem pickable;
        if (interactingOject.TryGetComponent<PickableItem>(out pickable) && _itemType == pickable.GetItemType())
        {
            StartCoroutine(MoveToGenerator(interactingOject));

            if (audioSource != null && genActivationSound != null)
            {
                audioSource.PlayOneShot(genActivationSound);
            }
            else
            {
                Debug.LogWarning("AudioSource ��� ���� ��������� �� ��������� ��� PlacementPoint.");
            }

            interactingOject = null;
            _isInteract = true;
            return true;
            /*interactingOject.transform.position = transform.position;
            interactingOject.transform.rotation = transform.rotation;
            interactingOject.transform.SetParent(transform);
            interactingOject.transform.tag = "Untagged";
            _interactable.Interact();
            interactingOject = null;
            _isInteract = true;
            return true;*/
        }
        else
        {
            GameMode.PlayerUI.DeactivatePanel();
            GameMode.PlayerUI.ShowText("UI.Request", true);
            StartCoroutine(ShowText());
        }
        return false;
    }
    private IEnumerator MoveToGenerator(GameObject item)
    {
        Vector3 startPosition = item.transform.position; // ��������� �������
        Quaternion startRotation = item.transform.rotation; // ��������� �������

        Vector3 targetPosition = transform.position; // ������� ������� (������� ����������)
        Quaternion targetRotation = transform.rotation; // ������� �������

        float duration = 1f; // ������������ ��������
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // ������� ����������� � ��������
            item.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            item.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime / duration);

            elapsedTime += Time.deltaTime; // ����������� �����
            yield return null; // ���� ���������� �����
        }
        // ������� (������� ������ ����� ����������� ������)
        Vector3 costScale = new Vector3(0.5f, 0.5f, 0.5f);

        item.transform.SetParent(null);
        item.transform.localScale = costScale;

        // ���������, ��� ������ ����� �� ������� ������� � �������
        item.transform.position = targetPosition;
        item.transform.rotation = targetRotation;

        // ������������� �������� ��� �������
        item.transform.SetParent(transform);
        item.tag = "Untagged"; // ���������� ���
        _interactable.Interact(); // ��������������� � ��������
    }
}
