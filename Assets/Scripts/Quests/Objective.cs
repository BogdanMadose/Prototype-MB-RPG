using System;
using UnityEngine;

[Serializable]
public abstract class Objective
{
    [Tooltip("Total amount required to complete objective")]
    [SerializeField] private int ammount;
    [Tooltip("Type of quest")]
    [SerializeField] private string type;
    private int _currentAmmount;

    public int Ammount => ammount;
    public string Type => type;
    public int CurrentAmmount { get => _currentAmmount; set => _currentAmmount = value; }
    public bool IsComplete => CurrentAmmount >= Ammount;
}

