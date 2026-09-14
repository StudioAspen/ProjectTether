using UnityEngine;

public abstract class CombatClass
{
    CombatClassType _classType;
    public CombatClassType ClassType => _classType;

    public virtual void AddClassStatBuff(UnitDataSO stats) { }
    public virtual void RemoveClassStatBuff(UnitDataSO stats) { }

    protected void SeClassType(CombatClassType classType) => _classType = classType;
}