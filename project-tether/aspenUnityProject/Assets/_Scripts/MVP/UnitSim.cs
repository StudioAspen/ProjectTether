using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UnitSim : MonoBehaviour
{
  public UnitDataSO Data;
  public UnitPieceSlot Slot { get; private set; }
  public UnitPiece Piece { get; private set; }

  [Space(5)]
  [SerializeField] private Image _portrait;
  [SerializeField] private TextMeshProUGUI _name;
  [SerializeField] private TextMeshProUGUI _class;
  [SerializeField] private TextMeshProUGUI _level;

  public Button Button { get; private set; }

  private void Awake()
  {
    Button ??= GetComponent<Button>();

    Slot ??= GetComponentInChildren<UnitPieceSlot>();
    Slot?.SetSim(this);

    Piece ??= GetComponentInChildren<UnitPiece>();
    Piece?.SetSim(this);

    UpdateDetails(Data);
  }

  // private void Start()
  // {
  //   Activate(Button.interactable);
  // }

  public void UpdateDetails (UnitDataSO unitData)
  {
    Data ??= unitData;

    if (Data = unitData)
    {
      if(Data.Portrait)
        _portrait.sprite = Data.Portrait;

      _name.text = Data.Name;

      if(_level)
        _level.text = $"Lv.{Data.Level}";
    }

    Button.interactable = Data;
    if (Button.interactable && Data)
    {
      Button.onClick.AddListener(() => {
        SimDetails.Instance.DisplayUnitDetails(Data);
        SimDetails.Instance.OnLevelUpdated += UpdateDetails;
      });
    }
    else
      Button.onClick.RemoveAllListeners();
  }

  public void ClearDetails()
  {
    Activate(false);
    Data = null;
    Button.interactable = Data;

    if(_portrait)
      _portrait.sprite = null;

    _name.text = string.Empty;
    
    if(_level)
      _level.text = string.Empty;
  }

  public void ReturnPiece() => Slot?.ReturnPiece();

  public void RemoveEnemy() {
    if(Data.GetType() == typeof(EnemyUnitSO))
      EnemySelect.Instance.RemoveEnemy(this);
  }

  public void Activate(bool value)
  {
    if(value == true)
      BattleSimManager.Instance?.ActivateSim(this);
    else
      BattleSimManager.Instance?.DeactivateSim(this);
  }
}
