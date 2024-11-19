using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReportCollectebel : MonoBehaviour, IInteractable
{

    protected CollectibleType _collectibleType;
    protected string _tag;

    public CollectibleType CollectibleType 
    {
        get { return _collectibleType; }
        set { _collectibleType = value; }
    }

    public string Tag
    {
        get { return _tag; }
        set { _tag = value; }
    }

    public void Interact()
    {

    }

    public bool Interact(ref GameObject interactingOject)
    {
        return true;
    }
}
