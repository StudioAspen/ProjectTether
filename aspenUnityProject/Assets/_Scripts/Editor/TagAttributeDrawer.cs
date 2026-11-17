using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

public sealed class TagStringDrawer : OdinAttributeDrawer<TagAttribute, string>
{
    private readonly GUIContent buttonContent = new GUIContent();

    protected override void Initialize()
    {
        UpdateButtonContent();
    }

    private void UpdateButtonContent()
    {
        buttonContent.text = ValueEntry.SmartValue ?? string.Empty;
    }

    protected override void DrawPropertyLayout(GUIContent label)
    {
        var rect = EditorGUILayout.GetControlRect(label != null);
        rect = label == null ? EditorGUI.IndentedRect(rect) : EditorGUI.PrefixLabel(rect, label);

        if (!EditorGUI.DropdownButton(rect, buttonContent, FocusType.Passive))
            return;

        var selector = new GenericSelector<string>(UnityEditorInternal.InternalEditorUtility.tags);
        selector.SetSelection(ValueEntry.SmartValue);
        selector.SelectionChanged += selection =>
        {
            ValueEntry.Property.Tree.DelayAction(() =>
            {
                ValueEntry.SmartValue = selection.FirstOrDefault();
                UpdateButtonContent();
            });
        };
        selector.ShowInPopup(rect.position);
    }
}

public abstract class TagStringListBaseDrawer<T> : OdinAttributeDrawer<TagAttribute, T>
    where T : IList<string>
{
    private readonly GUIContent buttonContent = new GUIContent();

    protected override void Initialize()
    {
        UpdateButtonContent();
    }

    private void UpdateButtonContent()
    {
        buttonContent.text = buttonContent.tooltip = string.Join(", ", ValueEntry.SmartValue);
    }

    protected override void DrawPropertyLayout(GUIContent label)
    {
        var rect = EditorGUILayout.GetControlRect(label != null);
        rect = label == null ? EditorGUI.IndentedRect(rect) : EditorGUI.PrefixLabel(rect, label);

        if (!EditorGUI.DropdownButton(rect, buttonContent, FocusType.Passive))
            return;

        var selector = new TagSelector(UnityEditorInternal.InternalEditorUtility.tags);
        selector.SetSelection(ValueEntry.SmartValue);
        selector.SelectionChanged += selection =>
        {
            ValueEntry.Property.Tree.DelayAction(() =>
            {
                UpdateValue(selection);
                UpdateButtonContent();
            });
        };
        rect.y += rect.height;
        selector.ShowInPopup(rect.position);
    }

    protected abstract void UpdateValue(IEnumerable<string> selection);
}

[DrawerPriority(1)]
[DontApplyToListElements]
public sealed class TagStringArrayDrawer : TagStringListBaseDrawer<string[]>
{
    protected override void UpdateValue(IEnumerable<string> selection)
    {
        ValueEntry.SmartValue = selection.ToArray();
    }
}

[DrawerPriority(1)]
[DontApplyToListElements]
public sealed class TagStringListDrawer : TagStringListBaseDrawer<List<string>>
{
    protected override void UpdateValue(IEnumerable<string> selection)
    {
        ValueEntry.SmartValue = selection.ToList();
    }
}

public sealed class TagSelector : GenericSelector<string>
{
    private readonly FieldInfo requestCheckboxUpdate;

    public TagSelector(string[] tags) : base(tags)
    {
        CheckboxToggle = true;
        requestCheckboxUpdate = typeof(GenericSelector<string>).GetField(
            "requestCheckboxUpdate",
            BindingFlags.NonPublic | BindingFlags.Instance);
    }

    protected override void DrawSelectionTree()
    {
        base.DrawSelectionTree();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("None"))
        {
            SetSelection(new List<string>());
            RefreshCheckboxes();
            TriggerSelectionChanged();
        }

        if (GUILayout.Button("All"))
        {
            SetSelection(UnityEditorInternal.InternalEditorUtility.tags);
            RefreshCheckboxes();
            TriggerSelectionChanged();
        }
        EditorGUILayout.EndHorizontal();
    }

    private void RefreshCheckboxes()
    {
        requestCheckboxUpdate?.SetValue(this, true);
    }
}
