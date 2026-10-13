using _Scripts.Runtime.Combat;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.Runtime.Characters.Classes.Scripts
{
    [CreateAssetMenu(fileName = "CombatClass", menuName = "Scriptable Objects/Misc/Combat Class")]
    public class CombatClassSO : ScriptableObject
    {
    
        //TODO: add stat modifier later 
        [SerializeField] private Element[] defaultAttackElements;
        public Element[] DefaultAttackElements => defaultAttackElements;

        [FormerlySerializedAs("className")] [SerializeField] private string _className;
        public string ClassName => _className;

        //0 = cannot attack anything, 1 = 1 ring around unit, 3 = effectively global range for 19 tiles
        [FormerlySerializedAs("defaultAttackRange")] [SerializeField, Range(0,3)]
        private int _defaultAttackRange;

        public int DefaultAttackRange => _defaultAttackRange; 
    
        [FormerlySerializedAs("classDescription")] [SerializeField] private string _classDescription;
        public string ClassDescription => _classDescription;
        
        
        [Tooltip("stats raised by overclock. 0=hp, 1=en, 2=str, 3=def, 4=tec, 5=res, 6=spe, 7=luck, 8=prc, 9=eva")]
        [SerializeField] private int[] _overclockedStats;
        public int[] OverclockedStats => _overclockedStats; 
    
    }
}
