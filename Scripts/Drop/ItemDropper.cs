using System;
using Unity.Mathematics;
using UnityEngine;


public class ItemDropper : MonoBehaviour
{
    [SerializeField] private Vector3 dropPositionOffset;
    [SerializeField] private GameObject _collectablePrefab;
    [SerializeField] private Transform _droppedItemsParent;
    [Space]
    [SerializeField] private WeightDrop _DropSet;

    private float _dropRange = 2f;
    
    public void DropItems()
    {
        foreach (var item in _DropSet.GetDropSet())
        {
            var position = transform.position + Vector3.right * UnityEngine.Random.Range(0, _dropRange);
            var droppedObject = Instantiate(_collectablePrefab, position + dropPositionOffset, quaternion.identity, _droppedItemsParent);
            droppedObject.GetComponentInChildren<Collectable>().Item = item;
        }    
    }


}
