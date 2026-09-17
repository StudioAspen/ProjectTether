using System;
public enum Tier
{
  S = 0,
  A = 1,
  B = 2,
  C = 3,
  D = 4
}

//robbing Fire Emblem's leveling system for now. f value * 100 = chance the stat is raised on level up
//probably use plus-minus 5-10 variance (random)
public static class TierExtensions
{
  public static float GetWeight(this Tier tier) => tier switch
  {
    Tier.S => 0.6f, 
    Tier.A => 0.5f,
    Tier.B => 0.4f,
    Tier.C => 0.3f,
    Tier.D => 0.2f,
    _ => throw new ArgumentOutOfRangeException(nameof(tier), $"Unexpected tier value: {tier}"), 
  };
}