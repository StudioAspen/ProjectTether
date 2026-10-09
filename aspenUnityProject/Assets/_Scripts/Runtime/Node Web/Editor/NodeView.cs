using _Scripts.Runtime.Combat;
using Codice.CM.Client.Differences;
using Slinky.NodeWeb;
using Unity.VectorGraphics;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class NodeView : UnityEditor.Experimental.GraphView.Node
{
    public BaseNodeSO DataReference;
    string _shortGUID;


    // Future Chnage: Rewrite the switch statement to check the type itself as looking at a string is inefficent
    public NodeView(BaseNodeSO data)
    {
        _shortGUID = data.GUID.ToString().Substring(31);

        this.SetPosition(new Rect(data.EditorPosition.x, data.EditorPosition.y, 100, 150));
        this.DataReference = data;

        // How the Ports look
        GeneratePorts();
        // How the NodeViews look
        RedrawUI();
    }

    void GeneratePorts()
    {
        // Output Port Logic
        Port outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
        outputPort.portName = "Unlocks";
        this.outputContainer.Add(outputPort);

        // Input Port Logic
        Port inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
        inputPort.portName = "Requires";
        this.inputContainer.Add(inputPort);

        this.RefreshExpandedState();
        this.RefreshPorts();
    }

    public void RedrawUI()
    {
        if (DataReference is AbilityNodeSO) AbilityNodeView();
        if (DataReference is SkillNodeSO) SkillNodeView();
        if (DataReference is StatNodeSO) StatNodeView();
    }

    void AbilityNodeView()
    {
        AbilityNodeSO abilityNode = DataReference as AbilityNodeSO;
        AbilitySO ability = abilityNode.Ability;

        CreateNameTextField<AbilityNodeSO>(abilityNode);
        CreateUnlockToggle<AbilityNodeSO>(node: abilityNode, unlockColor: Color.red, lockColor: Color.darkRed);

        this.RefreshExpandedState();
    }

    void StatNodeView()
    {
        StatNodeSO statNode = DataReference as StatNodeSO;
        StatSO stat = statNode.Stat;

        CreateNameTextField<StatNodeSO>(statNode);
        CreateUnlockToggle<StatNodeSO>(node: statNode, unlockColor: Color.lightGreen, lockColor: Color.darkGreen);

        this.RefreshExpandedState();
    }

    void SkillNodeView()
    {
        SkillNodeSO skillNode = DataReference as SkillNodeSO;
        SkillSO skill = skillNode.Skill;

        CreateNameTextField<SkillNodeSO>(skillNode);
        CreateUnlockToggle<SkillNodeSO>(node: skillNode, unlockColor: Color.lightYellow, lockColor: Color.yellow);

        this.RefreshExpandedState();
    }

    void CreateNameTextField<NodeType>(NodeType node) where NodeType : BaseNodeSO
    {
        TextField nameField = new TextField("Name");

        switch (node)
        {
            case AbilityNodeSO abilityNode:
                AbilitySO ability = abilityNode.Ability;

                nameField.value = ability.Name;

                nameField.RegisterValueChangedCallback(evt =>
                {
                    ability.SetName(evt.newValue);
                    this.title = $"{evt.newValue}_{_shortGUID}";

                    EditorUtility.SetDirty(ability);
                    EditorUtility.SetDirty(abilityNode);
                });
                break;

            case StatNodeSO statNode:
                StatSO stat = statNode.Stat;

                nameField.value = stat.Name;

                nameField.RegisterValueChangedCallback(evt =>
                {
                    stat.SetName(evt.newValue);
                    this.title = $"{evt.newValue}_{_shortGUID}";

                    EditorUtility.SetDirty(stat);
                    EditorUtility.SetDirty(statNode);
                });
                break;

            case SkillNodeSO skillNode:
                SkillSO skill = skillNode.Skill;

                nameField.value = skill.Name;

                nameField.RegisterValueChangedCallback(evt =>
                {
                    skill.SetName(evt.newValue);
                    this.title = $"{evt.newValue}_{_shortGUID}";

                    EditorUtility.SetDirty(skill);
                    EditorUtility.SetDirty(skillNode);
                });
                break;
        }

        this.extensionContainer.Add(nameField);
    }

    void CreateUnlockToggle<NodeType>(NodeType node, Color unlockColor, Color lockColor) where NodeType : BaseNodeSO
    {
        Toggle unlockedToogle = new Toggle("Is Unlocked") { value = false };

        this.style.backgroundColor = lockColor;

        unlockedToogle.RegisterValueChangedCallback(evt =>
        {
            this.style.backgroundColor = node.IsUnlocked ? unlockColor : lockColor;
            DataReference.SetIsUnlocked(evt.newValue);

            switch (node)
            {
                case AbilityNodeSO abilityNode:
                    AbilitySO ability = abilityNode.Ability;

                    EditorUtility.SetDirty(ability);
                    EditorUtility.SetDirty(abilityNode);
                    break;

                case StatNodeSO statNode:
                    StatSO stat = statNode.Stat;

                    EditorUtility.SetDirty(stat);
                    EditorUtility.SetDirty(statNode);

                    break;

                case SkillNodeSO skillNode:
                    SkillSO skill = skillNode.Skill;

                    EditorUtility.SetDirty(skill);
                    EditorUtility.SetDirty(skillNode);
                    break;
            }
        });

        this.extensionContainer.Add(unlockedToogle);
    }


    public override void SetPosition(Rect newPos)
    {
        base.SetPosition(newPos);

        if (this.DataReference != null)
        {
            this.DataReference.SetEditorPosition(new Vector2(newPos.xMin, newPos.yMin));
            EditorUtility.SetDirty(this.DataReference);
        }
    }

    public override void OnSelected()
    {
        base.OnSelected();

        if (this.DataReference != null)
        {
            Selection.activeObject = this.DataReference;
        }
    }
}
