using System;
using System.Collections.Generic;
using Scriptable;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[Serializable]
public class WeightDrop
{
    [Range(0,10)]
    [SerializeField] private int _dropAmount;
    [SerializeField] private WeightedItem[] droplist;

    private int[] _weights;
    private int _totalWeight;

    private void InitTotalWeight()
    {
        _weights = new int[droplist.Length];

        var i = 0;

        foreach (var item in droplist)
        {
            _totalWeight += item.weight;
            _weights[i++] = _totalWeight;
        }
    }

    public Item[] GetDropSet()
    {
        InitTotalWeight();
        var dropSet = new List<Item>();

        for (var i = 0; i < _dropAmount; i++)
        {
            var rand = Random.Range(0, _totalWeight);
            var j = 0;
            while (_weights[j] < rand)
                j++;
            
            if (droplist[j].item is null)
                continue;
            
            dropSet.Add(droplist[j].item);
        }

        return dropSet.ToArray();
    }
}