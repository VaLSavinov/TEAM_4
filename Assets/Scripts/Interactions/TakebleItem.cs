using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TakeblItem : MonoBehaviour, IInteractable
{  
    [Header("Item Settings")]
    [SerializeField] private ItemType _itemType;
    [SerializeField] private CollectibleType _typeCollect;

    [Header("Lsits")]
    [SerializeField] private List<string> _tags;
    [SerializeField] private List<AudioClip> _sounds;
    [SerializeField] private List<Image> _images;

    private int _index;

    public ItemType GetItemType() { return _itemType; }

    public CollectibleType GetCardColor() { return _typeCollect; }

    public void Interact()
    {
        Destroy(gameObject);
    }

    public bool Interact(ref GameObject interactingOject)
    {
        return true;
    }
}