using System.Collections.Generic;
using Consystently.Essentials;
using Consystently.Essentials.Math;
using Tether.CharacterSystems;
using TileSystem;
using UnityEngine;

public class RangeDisplay : MonoBehaviour
{
    [SerializeField] GameObject rangeDisplayObject;
    private List<GameObject> rangeObjects = new List<GameObject>();
    private List<GameObject> activeObjects = new List<GameObject>();
    private const int ArbitraryYOffset = 10;
     

    public void Initialize(TileController[] tileControllers)
    {
        foreach (TileController tileController in tileControllers)
        {
            GameObject newTempObject = Instantiate(rangeDisplayObject, Vector3.zero, Quaternion.identity, transform);
            newTempObject.transform.position = new Vector3(tileController.Position().x, tileController.Position().y+ArbitraryYOffset, tileController.Position().z);
            rangeObjects.Add(newTempObject);
            newTempObject.SetActive(false);
        }
    }
    
    
    public void DisplayMoveRange(Vector3Int from, TileController[] tileControllers, int range)
    {
        for (int ro = 0; ro < rangeObjects.Count; ro++)
        {
            if (tileControllers[ro].IsMoveable(from, range))
            {
                rangeObjects[ro].SetActive(true);
                activeObjects.Add(rangeObjects[ro]);
            }
        }
    }

    public void DisplayAttackRange(TileController[] tileControllers, UnitController cc)
    {
        for (int ro = 0; ro < rangeObjects.Count; ro++)
        {
            if (cc.AttackReachable(tileControllers[ro]))
            {
                rangeObjects[ro].SetActive(true);
                activeObjects.Add(rangeObjects[ro]);
            }
        }       
    }

    public void DisplayAbilityRange(TileController[] tileControllers, UnitController cc, AbilitySO ability)
    {
        for (int ro = 0; ro < rangeObjects.Count; ro++)
        {
            if (cc.AbilityReachable(ability, tileControllers[ro]))
            {
                rangeObjects[ro].SetActive(true);
                activeObjects.Add(rangeObjects[ro]);
            }
        }       

    }
    
    public void HideRange()
    {
        foreach (GameObject obj in activeObjects)
            obj.SetActive(false);
        activeObjects.Clear();
    }

}
