using UnityEngine;

//misc directory is temp name? 
[CreateAssetMenu(fileName = "NewEncounter", menuName = "Scriptable Objects/Misc/Encounter")]
public class EncounterSO : ScriptableObject
{
    [System.Serializable]
    public class Tile
    {
        [SerializeField] private UnitDataSO[] entities = new UnitDataSO[4];

        public UnitDataSO this[int index]
        {
            get => entities[index];
            set => entities[index] = value;
        }

        public int Length => entities.Length;
        
        public void Validate()
        {
            if (entities == null || entities.Length != 4)
            {
               System.Array.Resize(ref entities, 4); 
            }
        }
    }

    [Header("Tiles")]
    [SerializeField] private Tile[] battleField = new Tile[19];

    void OnValidate()
    {
        if (battleField == null || battleField.Length != 19)
        {
            System.Array.Resize(ref battleField, 19);
        }

        for (int i = 0; i < battleField.Length; i++)
        {
            if(battleField[i] == null)
                battleField[i] = new Tile();
            
            battleField[i].Validate();
        }
    }
}