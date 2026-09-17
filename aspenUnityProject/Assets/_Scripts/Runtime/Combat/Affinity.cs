using System;

public enum Affinity
{
  Neutral = 0,
  Weak = 1,
  Tolerant = 2,
  Immune = 3,
  Absorbent = 4
}

public static class AffinityExtensions
{
  public static float Multiplier(this Affinity affinity) => affinity switch
  {
    Affinity.Neutral => 1.0f,
    Affinity.Weak => 2.0f,
    Affinity.Tolerant => 0.5f,
    Affinity.Immune => 0.0f,
    Affinity.Absorbent => -1.0f,
    _ => throw new ArgumentOutOfRangeException(nameof(affinity), $"Unexpected affinity value: {affinity}"),
  };
}