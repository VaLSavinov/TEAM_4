using System;
using UnityEngine;

public class RoomAccessControl : MonoBehaviour
{
    [SerializeField] private string _tagNameRoom;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private AccessCardColor requiredAccessLevel;
    [SerializeField] private bool hasPower = true; // �� ��������� ������� ����� �������

    public event Action NoPower;

    private void Awake()
    {
        GameMode.Events.OnInteractGenerator += ActivatePower;
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

    public void NoNav() 
    {
        NoPower?.Invoke();
    }
    public string GetTagNameRoom()
    {
        return _tagNameRoom;
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

   
    public void Initialize(GridManager gridManager)
{
    this.gridManager = gridManager;
}
}
