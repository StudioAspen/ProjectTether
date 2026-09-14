using UnityEngine;
using TMPro;

public class TestUI : MonoBehaviour
{
    [SerializeField] GameObject _playerPrefab;
    [SerializeField] TextMeshProUGUI _playerText;
    [SerializeField] TextMeshProUGUI _classText;

    UnitDataSO _stats;
    CombatClassSystem _combatClassSystem;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _stats = _playerPrefab.GetComponent<Character>().UnitData as UnitDataSO;
        _combatClassSystem = _playerPrefab.GetComponent<CombatClassSystem>();

        _playerText.text = $"Health: {_stats.Health}\nEnergy: {_stats.Energy}\nStrength: {_stats.Strength}\nDefense: {_stats.Defense}\nTech: {_stats.Tech}\nResistance: {_stats.Resistance}\nSpeed: {_stats.Speed}\nPrecision: {_stats.Precision}\nEvasion: {_stats.Evasion}\nLuck: {_stats.Luck}";

    }

    private void Update()
    {
        string text = _combatClassSystem.SelectedClass == null ? "None" : _combatClassSystem.SelectedClass.ClassType.ToString();
        _classText.text = $"Class: {text}";
    }

    public void ForceAddClass()
    {
        _combatClassSystem.AddClass(CombatClassType.Breacher);
    }

    public void ForceSwitchclass()
    {
        _combatClassSystem.SwitchClass(CombatClassType.Breacher);
    }
}
