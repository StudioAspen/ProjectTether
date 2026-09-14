using System;
using UnityEngine;

public class ExperienceSystem : MonoBehaviour, IInitializer
{
  [Header("Level")]
  [Range(1, 99)]
  [SerializeField] protected int _level;
  public int Level => _level;
  public int ExpToNextLevel { get; private set; }
  public int TotalExpGained { get; private set; }


  public event Action<int> OnLevelUp;

  public void Intialize(UnitDataSO unitData)
  {
    _level = unitData.Level;

    ExpToNextLevel = unitData.ExPtsToNextLevel;
    TotalExpGained = unitData.TotalExPtsGained;
  }

  public void ResetSystem()
  {
    _level = 1;
    CalcExpToNextLevel();
    TotalExpGained = 0;
  }

  public void AddExperience(int expGained)
  {
    if(_level == 99)
    {
      ExpToNextLevel = 0;
      return;
    }
    else if (expGained <= ExpToNextLevel)
    {
      TotalExpGained += expGained;
      ExpToNextLevel -= expGained;
    }
    else if(expGained >= ExpToNextLevel)
    {
      expGained -= ExpToNextLevel;
      TotalExpGained += ExpToNextLevel;
      ExpToNextLevel = 0;
      LevelUp(expGained);
    }
  }

  public void LevelUp(int remainingExp = 0)
  {
    if(_level < 99)
    {
      _level += 1;
      CalcExpToNextLevel();
      OnLevelUp?.Invoke(_level);
    }

    if(remainingExp > 0 && _level < 99)
      AddExperience(remainingExp);
  }

  public void CalcExpToNextLevel()
  {
    if(_level == 99)
      return;

    // Calculations here
  }
}
