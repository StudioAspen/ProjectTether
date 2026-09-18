using System;
using Consystently.Essentials;
using Tether.CharacterSystems;
using TileSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

//TODO: disable move button by subscribing to unit movement (button just needs to be disabled)
//CAN probably refactor because I was not sure how Unity UI works when making this. 
public class CombatUI : MonoBehaviour
{
    
    [SerializeField] private GameObject playerActionsContainer;
    [SerializeField] private GameObject abilitiesPanel;
    [SerializeField] private Button firstButton; 
    [SerializeField] private GameObject cursor;

    [SerializeField] private InputSystemUIInputModule uiInputModule;
    private InputAction cancelSubmenu;
    
    private Button[] abilityButtons;

    public static event Action<CombatActions> PlayerAction;
    public static event Action<CombatActions, int> PlayerSelectiveAction;

    private void Awake()
    {
        if (uiInputModule != null)
            cancelSubmenu = uiInputModule.cancel.action; 
    }


    void OnEnable()
    {
        playerActionsContainer.SetActive(false);
        abilitiesPanel.SetActive(false);
        cursor.SetActive(false); 
        CombatManager.battlePhaseChanged += HandleUserActions;
        cancelSubmenu.performed += OnCloseSubmenu;
        CombatManager.examinedTile += ExamineTile;
        CombatManager.exitedExamine += ExitExamine;
        CombatManager.examinedUnit += ExamineUnit;
        if (abilityButtons == null || abilityButtons.Length == 0)
        {
           abilityButtons = abilitiesPanel.GetComponentsInChildren<Button>();
           for (int ability = 0; ability < abilityButtons.Length; ability++)
           {
               int i = ability;
               abilityButtons[ability].onClick.RemoveAllListeners();
               abilityButtons[ability].onClick.AddListener(() => SendSelectedAction(3, i));
           }

        }
   
    }


    void OnDisable()
    {
        CombatManager.battlePhaseChanged -= HandleUserActions;
        CombatManager.examinedTile -= ExamineTile;
        CombatManager.exitedExamine -= ExitExamine;
        if (abilityButtons == null)
            return;
        foreach (Button button in abilityButtons)
            button?.onClick.RemoveAllListeners();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void HandleUserActions(BattlePhase bs, UnitController unitController)
    {
        abilitiesPanel.SetActive(false);
        cursor.SetActive(false);
        if (bs is PlayerPhase)
            playerActionsContainer.SetActive(true);
        else
        {
            playerActionsContainer.SetActive(false);
        }
        uiInputModule.enabled = playerActionsContainer.activeSelf; //painful concurrency bug if this is removed
        int currMove = 0;
        foreach (Transform button in abilitiesPanel.transform)
        {
            if(currMove > unitController.GetData().Moves.Count-1)
                button.gameObject.SetActive(false);
            else
            {
                button.gameObject.SetActive(true);
                button.GetComponentInChildren<TextMeshProUGUI>().text = unitController.GetData().Moves[currMove].Name;
            }
            currMove++;
        }
    }
    

    private void TrySelection()
    {
        playerActionsContainer.SetActive(false);
        uiInputModule.enabled = false;
        cursor.SetActive(true);
    }

    private void ExamineTile(TileController tc)
    {
        cursor.SetActive(false);
        //do more stuff related to ui
    }

    private void ExitExamine()
    {
        cursor.SetActive(true);
    }

    //does nothing for now but will do stuff related to ui later
    private void ExamineUnit(UnitController uc)
    {
       Debug.Log(uc.GetData().Name); 
    }

    //buttons on the combat panel will use this function
    //convert to use enum later 
    //for player actions that do not require selection of items, moves, etc. 
    public void SendAction(int action)
    {
        CombatActions pAction = (CombatActions)action; 
        if(pAction != CombatActions.Defend)
            TrySelection();
        PlayerAction?.Invoke(pAction);
        Debug.Log(pAction);
    }

    //convert into openSubmenu for both inventory and abilities in the future 
    private void OpenAbilitiesMenu()
    {
        abilitiesPanel.SetActive(true);        
        playerActionsContainer.SetActive(false);
    }

    private void CloseAbilitiesMenu()
    {
       abilitiesPanel.SetActive(false);
       playerActionsContainer.SetActive(true);
    }
    
    //Band-Aid QoL fix  
    private void OnCloseSubmenu(InputAction.CallbackContext context)
    {
       if(abilitiesPanel.activeSelf) 
           CloseAbilitiesMenu(); 
    }

    
    //for actions that require selection of other things like items, moves, etc.
    public void SendSelectedAction(int action, int move)
    {
        abilitiesPanel.SetActive(false);
        TrySelection();
        PlayerSelectiveAction?.Invoke((CombatActions)action, move);
    } 
    

}
