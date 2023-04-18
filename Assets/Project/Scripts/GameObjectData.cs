using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ObjectDatas")]
public class GameObjectData : ScriptableObject
{
    [SerializeField] private ObjectType type;
    [SerializeField] private Color color;

    public ObjectType GetObjectType => type;

    public Color GetObjectColor => color;

}
