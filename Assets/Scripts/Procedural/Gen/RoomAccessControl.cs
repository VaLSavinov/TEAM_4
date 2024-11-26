using UnityEngine;

public class RoomAccessControl : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] private AccessCardColor requiredAccessLevel;
    [SerializeField] private bool hasPower = true; // �� ��������� ������� ����� �������

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
        if (!HasPower)
        {
            hasPower = true;

            // ����� ������ ���������� ���������� ������
            if (gridManager != null)
            {
                gridManager.UpdateDoorMaterials();
            }
            else
            {
                Debug.LogWarning("GridManager �� �������� ��� RoomAccessControl.");
            }
        }
    }
    public void Initialize(GridManager gridManager)
{
    this.gridManager = gridManager;
}
}
