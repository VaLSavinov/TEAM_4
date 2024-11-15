using UnityEngine;

public class PickableItem : MonoBehaviour
{
    public enum ItemType
    {
        AccessCard,
        PortableBattery
    }

    public enum AccessCardColor
    {
        None, // »спользуетс€ дл€ объектов, не €вл€ющихс€ картой доступа
        Red,
        Green,
        Blue
    }

    [Header("Item Settings")]
    public ItemType itemType;
    public AccessCardColor cardColor = AccessCardColor.None;
}