using UnityEditor;
using UnityEngine;

public class HorizontalAttribute : PropertyAttribute
{
    public int maxElementsPerLine;

    public HorizontalAttribute(int maxElementsPerLine = 2)
    {
        this.maxElementsPerLine = Mathf.Max(1, maxElementsPerLine);
    }
}


[CustomPropertyDrawer(typeof(HorizontalAttribute))]
public class HorizontalDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        HorizontalAttribute horizontal = (HorizontalAttribute)attribute;

        EditorGUI.BeginProperty(position, label, property);

        if (property.propertyType != SerializedPropertyType.Generic)
        {
            EditorGUI.LabelField(position, label.text, "Use [Horizontal] only with structs/classes.");
            return;
        }

        SerializedProperty iterator = property.Copy();
        SerializedProperty end = iterator.GetEndProperty();

        // Label for the entire line
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        float totalWidth = position.width;
        float spacing = 5f;

        int visibleFields = 0;
        // Count fields
        for (SerializedProperty child = iterator.Copy(); child.NextVisible(true) && !SerializedProperty.EqualContents(child, end);)
        {
            visibleFields++;
        }

        int perLine = Mathf.Min(horizontal.maxElementsPerLine, visibleFields);
        float widthPerField = (totalWidth - spacing * (perLine - 1)) / perLine;

        int i = 0;
        for (SerializedProperty child = property.Copy(); child.NextVisible(true) && !SerializedProperty.EqualContents(child, end); i++)
        {
            if (i >= perLine) break; // Only draw up to max per line

            Rect fieldRect = new Rect(
                position.x + i * (widthPerField + spacing),
                position.y,
                widthPerField,
                position.height
            );

            EditorGUI.PropertyField(fieldRect, child, GUIContent.none);
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight;
    }
}
