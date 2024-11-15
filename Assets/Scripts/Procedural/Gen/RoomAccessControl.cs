using UnityEngine;

public class RoomAccessControl : MonoBehaviour
{    
    [SerializeField] private AccessCardColor requiredAccessLevel;
    [SerializeField] private bool hasPower = true; // По умолчанию комнаты имеют питание

    private void Awake()
    {
        GameMode.OnInteractGenerator += ActivatePower;
    }

    public AccessCardColor RequiredAccessLevel
    {
        get => requiredAccessLevel;
        set => requiredAccessLevel = value;
    }

    public bool HasPower
    {
        get => hasPower;
        set => hasPower = value;
    }

    public AccessCardColor GetCardColor() 
    {
        return requiredAccessLevel;
    }

    private void ActivatePower() 
    { 
        if (!HasPower) hasPower = true;
    }
}
