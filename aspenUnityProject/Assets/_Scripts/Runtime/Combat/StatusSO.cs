using UnityEngine;

[CreateAssetMenu(fileName = "StatusSO", menuName = "Scriptable Objects/StatusSO")]
public class StatusSO : ScriptableObject
{
    [SerializeField] private string _statusId; 
    public string StatusId => _statusId;
    
    [SerializeField] private string _statusName;
    public string StatusName => _statusName;

    [SerializeField] private string _statusDescription;
    public string StatusDescription => _statusDescription;

    [SerializeField] private float _damageTakenModifier;
    public float DamageTakenModifier => _damageTakenModifier;
    
    [SerializeField] private int _damageTakenPerTurn;
    public int  DamageTakenPerTurn => _damageTakenPerTurn;

    //-1 for infinite duration 
    [SerializeField] private int _duration;
    public int Duration => _duration;

    [SerializeField] private bool _isStun;
    public bool IsStun => _isStun;
        
    //ARRAY here containing other status scriptable objects if combining statuses is desired 
}
