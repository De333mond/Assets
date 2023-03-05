using System;
using System.Collections.Generic;
using PlayerInventory.Scriptable;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[Serializable]
public class WeightDrop
{
    
    [Range(0,10)]
    [SerializeField] private int _randomDropAmount;
    [SerializeField] private WeightedItem[] randomDropList;
    [SerializeField] private Item[] constantDropList;

    private int[] _weights;
    private int _totalWeight;

    private void InitTotalWeight()
    {
        _weights = new int[randomDropList.Length];

        var i = 0;

        foreach (var item in randomDropList)
        {
            _totalWeight += item.weight;
            _weights[i++] = _totalWeight;
        }
    }

    public Item[] GetDropSet()
    {
        InitTotalWeight();
        var dropSet = new List<Item>();

        for (var i = 0; i < _randomDropAmount; i++)
        {
            var rand = Random.Range(0, _totalWeight);
            var j = 0;
            while (_weights[j] < rand)
                j++;
            
            if (randomDropList[j].item is null)
                continue;
            
            dropSet.Add(randomDropList[j].item);
        }

        foreach (var item in constantDropList)
        {
            dropSet.Add(item);
        }

        return dropSet.ToArray();
    }
}