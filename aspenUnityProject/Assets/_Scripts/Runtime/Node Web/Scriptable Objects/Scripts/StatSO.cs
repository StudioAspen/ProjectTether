using UnityEngine;

public class StatSO : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; } = "Stat Name";
    public void SetName(string name) => Name = name;

    [Header("Stats")]

    [field: SerializeField] public int Health { get; private set; } = 0;
    public void SetHealth(int health) => Health = health;
    [field: SerializeField] public int Strength { get; private set; } = 0;
    public void SetStrength(int strength) => Strength = strength;
    [field: SerializeField] public int Tech { get; private set; } = 0;
    public void SetTech(int tech) => Tech = tech;
    [field: SerializeField] public int Defense { get; private set; } = 0;
    public void SetDefense(int defense) => Defense = defense;
    [field: SerializeField] public int Resistance { get; private set; } = 0;
    public void SetResistance(int resistance) => Resistance = resistance;
    [field: SerializeField] public int Speed { get; private set; } = 0;
    public void SetSpeed(int speed) => Speed = speed;
    [field: SerializeField] public int Luck { get; private set; } = 0;
    public void SetLuck(int luck) => Luck = luck;

}
