using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Collection",
                 menuName = "Collection / Reports", order = 55)]

public class SOCollections: ScriptableObject
{
    [SerializeField] private CollectibleType _typeColect;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private List<string> _tagList;

    /// <summary>
    /// Три варианта avail:
    /// f - недоступно
    /// t - доступно
    /// n - новое, недавно получено, но не просмотренно
    /// </summary>
    /// <param name="avail"></param>
    /// <returns></returns>
    public List<string> GetTagList(string avail)
    {
        List<string> list = new List<string>();
        foreach (string tag in _tagList)
        {
            if (LocalizationManager.CheackAvail(tag,avail))
                list.Add(tag);
        }
        return list;
    }

    public GameObject GetPrefab() => _prefab;

    public CollectibleType GetTypeColl() => _typeColect;


}