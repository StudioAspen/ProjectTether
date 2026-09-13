using System;
using Consystently.Essentials;
using Tether.CharacterSystems;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

//TODO: disable move button by listening to unit movement (button just needs to be disabled)
public class CombatUI : MonoBehaviour
{
    [SerializeField] private GameObject playerActionsContainer;
    [SerializeField] private GameObject abilitiesPanel;
    [SerializeField] private Button firstButton; 
    [SerializeField] private GameObject cursor;
    [SerializeField] private InputSystemUIInputModule inputSystemUIInput;
    
    private Button[] abilityButtons;

    public static event Action<CombatActions> PlayerAction;
    public static event Action<CombatActions, int> PlayerSelectiveAction;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        playerActionsContainer.SetActive(false);
        abilitiesPanel.SetActive(false);
        cursor.SetActive(false); 
        CombatManager.battlePhaseChanged += HandleUserActions;
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
        if (abilityButtons == null)
            return;
        foreach (Button button in abilityButtons)
            button?.onClick.RemoveAllListeners();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void HandleUserActions(BattleState bs, UnitController unitController)
    {
        abilitiesPanel.SetActive(false);
        cursor.SetActive(false);
        if (bs is PlayerState)
        {
            playerActionsContainer.SetActive(true);
            Debug.Log(playerActionsContainer.activeSelf);
        }
        else
        {
            playerActionsContainer.SetActive(false);
        }
        inputSystemUIInput.enabled = playerActionsContainer.activeSelf; //painful concurrency bug if this is removed
        int currMove = 0;
        foreach (Transform button in abilitiesPanel.transform)
        {
            currMove++;
            if(currMove > unitController.GetData().Moves.Capacity)
                button.gameObject.SetActive(false);
            else
            {
                button.gameObject.SetActive(true);
                button.GetComponentInChildren<Text>().text = unitController.GetData().Moves[currMove].Name;
            }
        }
    }
    

    private void TrySelection()
    {
        playerActionsContainer.SetActive(false);
        inputSystemUIInput.enabled = false;
        cursor.SetActive(true);
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

    public void OpenAbilitiesMenu()
    {
        abilitiesPanel.SetActive(true);        
        playerActionsContainer.SetActive(false);
    }
    
    
    //for actions that require selection of other things like items, moves, etc.
    public void SendSelectedAction(int action, int move)
    {
        abilitiesPanel.SetActive(false);
        PlayerSelectiveAction?.Invoke((CombatActions)action, move);
    } 
    

}
