
using UnityEditor;
[CustomEditor(typeof(PlayerInputController))]
public class InputManagerEditor : Editor
{

    bool showKeybindSettings = false;
    bool showPrimaryMouseSettings = false;
    bool showSecondaryMouseSettings = false;
    bool showInteractionSettings = false;
    bool showOtherSettings = false;
    bool showReloadEvent = false;

    bool showFlashlightEvents = false;
    public override void OnInspectorGUI()
    {

        var reloadKeyProperty = serializedObject.FindProperty("reloadKey");
        var changeToolKeyProperty = serializedObject.FindProperty("changeToolKey");
        var interactionKeyProperty = serializedObject.FindProperty("InteractionKey");
        var flashlightkey = serializedObject.FindProperty("flashlightKey");
        var primaryMouseKeyProperty = serializedObject.FindProperty("primaryMouseKey");
        var secondaryMouseKeyProperty = serializedObject.FindProperty("secondaryMouseKey");
       

        showKeybindSettings = EditorGUILayout.Foldout(showKeybindSettings, "Keybind Settings");

        if (showKeybindSettings)
        {
            EditorGUILayout.PropertyField(reloadKeyProperty);
            EditorGUILayout.PropertyField(changeToolKeyProperty);
            EditorGUILayout.PropertyField(interactionKeyProperty);
            EditorGUILayout.PropertyField(flashlightkey);
            EditorGUILayout.PropertyField(primaryMouseKeyProperty);
            EditorGUILayout.PropertyField(secondaryMouseKeyProperty);

        }
        showPrimaryMouseSettings = EditorGUILayout.Foldout(showPrimaryMouseSettings, "Primary Mouse Events");
        var primaryMouseDownProperty = serializedObject.FindProperty("PrimaryMouseKeyEvent");
        var primaryMouseUpProperty = serializedObject.FindProperty("PrimaryMouseKeyUpEvent");
        var primaryMouseHoldCompleteProperty = serializedObject.FindProperty("OnPrimaryHoldComplete");
        var primaryHoldProperty = serializedObject.FindProperty("OnPrimaryHold");

        if (showPrimaryMouseSettings)
        {
            EditorGUILayout.PropertyField(primaryMouseDownProperty);
            EditorGUILayout.PropertyField(primaryMouseUpProperty);
            EditorGUILayout.PropertyField(primaryMouseHoldCompleteProperty);
            EditorGUILayout.PropertyField(primaryHoldProperty);
        }
        showSecondaryMouseSettings = EditorGUILayout.Foldout(showSecondaryMouseSettings, "Secondary Mouse Events");
        var secondaryMouseDownProperty = serializedObject.FindProperty("SecondaryMouseKeyEvent");
        var secondaryMouseUpProperty = serializedObject.FindProperty("SecondaryMousekeyUpEvent");
        var secondaryMouseHoldComplete = serializedObject.FindProperty("OnSecondaryHoldComplete");
        var secondaryHoldProperty = serializedObject.FindProperty("OnSecondaryHold");
        if (showSecondaryMouseSettings)
        {
            EditorGUILayout.PropertyField(secondaryMouseDownProperty);
            EditorGUILayout.PropertyField(secondaryMouseUpProperty);
            EditorGUILayout.PropertyField(secondaryMouseHoldComplete);
            EditorGUILayout.PropertyField(secondaryHoldProperty);
        }

        showInteractionSettings = EditorGUILayout.Foldout(showInteractionSettings, "Interaction Events");

        var interactDownProperty = serializedObject.FindProperty("OnInteractKeyEvent");
        var interactEarlyReleaseProperty = serializedObject.FindProperty("OnInteractEarlyRelease");
        var interactHoldComplete = serializedObject.FindProperty("OnInteractHoldComplete");
        var interactHoldProperty = serializedObject.FindProperty("OnInteractHold");
        if (showInteractionSettings)
        {
            EditorGUILayout.PropertyField(interactDownProperty);
            EditorGUILayout.PropertyField(interactEarlyReleaseProperty);
            EditorGUILayout.PropertyField(interactHoldComplete);
            EditorGUILayout.PropertyField(interactHoldProperty);
        }
        showReloadEvent = EditorGUILayout.Foldout(showReloadEvent, "Reload Events");
        var reloadDownProperty = serializedObject.FindProperty("OnWeaponReloadEvent");
        if (showReloadEvent)
        {
            EditorGUILayout.PropertyField(reloadDownProperty);
        }
        showFlashlightEvents = EditorGUILayout.Foldout(showFlashlightEvents, "Flashlight Events");
        var flashlightDownProperty = serializedObject.FindProperty("FlashlightKeyEvent");
        var flashlightUpProperty = serializedObject.FindProperty("FlashlightKeyUpEvent");
        if (showFlashlightEvents)
        {
            EditorGUILayout.PropertyField(flashlightDownProperty);
            EditorGUILayout.PropertyField(flashlightUpProperty);
        }

        showOtherSettings = EditorGUILayout.Foldout(showOtherSettings, "Other Settings");
        var interactionTimeProperty = serializedObject.FindProperty("interactionTime");
        if (showOtherSettings)
        {
            EditorGUILayout.PropertyField(interactionTimeProperty);
        }

        serializedObject.ApplyModifiedProperties();
    }
}