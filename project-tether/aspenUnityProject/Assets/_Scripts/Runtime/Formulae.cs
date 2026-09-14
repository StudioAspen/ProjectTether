using UnityEngine;

public static class Formulae
{
  public static int CalculateStat(Stat stat, Tier tier, int unitLevel)
  {
    switch (stat)
    {
      case Stat.HP:
        return FourDigitStat(tier, unitLevel);
      
      case Stat.EN:
      case Stat.STR:
      case Stat.DEF:
      case Stat.TEC:
      case Stat.RES:        
      case Stat.SPE:
      case Stat.LCK:
      case Stat.PRC:
      case Stat.EVA:
        return ThreeDigitStat(tier, unitLevel);

      default:
        return 1;
    }
  }

  private static int ThreeDigitStat(Tier tier, int unitLevel)
  {
    float power;
    float factor;
    int offset;

    switch (tier)
    {
      case Tier.D:
        power = 1.44f;
        factor = 0.58f;
        offset = 8;
      break;
      
      case Tier.C:
        power = 1.46f;
        factor = 0.60f;
        offset = 10;
      break;

      case Tier.B:
        power = 1.48f;
        factor = 0.62f;
        offset = 12;
      break;
      
      case Tier.A:
        power = 1.50f;
        factor = 0.64f;
        offset = 14;
      break;

      case Tier.S:
        power = 1.52f;
        factor = 0.66f;
        offset = 16;
      break;

      default:
        power = 1;
        factor = 0;
        offset = 0;
      break;
    }

    return Mathf.CeilToInt( Mathf.Pow(factor * unitLevel, power) ) + offset;
  }

  private static int FourDigitStat(Tier tier, int unitLevel)
  {
    float power;
    float factor;
    int offset;

    switch (tier)
    {
      case Tier.D:
        power = 1.44f;
        factor = 2.84f;
        offset = 96;
      break;
      
      case Tier.C:
        power = 1.46f;
        factor = 2.88f;
        offset = 120;
      break;

      case Tier.B:
        power = 1.48f;
        factor = 2.92f;
        offset = 144;
      break;
      
      case Tier.A:
        power = 1.50f;
        factor = 2.96f;
        offset = 168;
      break;

      case Tier.S:
        power = 1.52f;
        factor = 3.00f;
        offset = 192;
      break;

      default:
        power = 1;
        factor = 0;
        offset = 0;
      break;
    }

    return Mathf.CeilToInt( Mathf.Pow(factor * unitLevel, power) ) + offset;
  }
}
