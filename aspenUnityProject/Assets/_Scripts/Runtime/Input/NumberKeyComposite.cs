using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;

#if UNITY_EDITOR
[UnityEditor.InitializeOnLoad]
#endif
/// <summary>
/// Maps keyboard keys 1, 2, 3, and 4 to integer values 0, 1, 2, and 3.
/// If multiple keys are held, the lowest numbered held key takes priority.
/// </summary>
public class NumberKeyComposite : InputBindingComposite<int>
{
    static NumberKeyComposite()
    {
        InputSystem.RegisterBindingComposite<NumberKeyComposite>("NumberKey");
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InitializeAtRuntime()
    {
        // Invoking this method triggers the static constructor and registration.
    }

    [InputControl(layout = "Button")]
    public int one;

    [InputControl(layout = "Button")]
    public int two;

    [InputControl(layout = "Button")]
    public int three;

    [InputControl(layout = "Button")]
    public int four;

    public override int ReadValue(ref InputBindingCompositeContext context)
    {
        if (context.ReadValue<float>(one) > 0.5f)
            return 0;

        if (context.ReadValue<float>(two) > 0.5f)
            return 1;

        if (context.ReadValue<float>(three) > 0.5f)
            return 2;

        if (context.ReadValue<float>(four) > 0.5f)
            return 3;

        return 0;
    }

    // Key 1 produces the value 0, so report actuation separately from the
    // composite value. This lets the Value action recognize key 1 as pressed.
    public override float EvaluateMagnitude(ref InputBindingCompositeContext context)
    {
        return Mathf.Max(
            context.EvaluateMagnitude(one),
            context.EvaluateMagnitude(two),
            context.EvaluateMagnitude(three),
            context.EvaluateMagnitude(four));
    }
}
