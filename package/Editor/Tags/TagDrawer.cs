using UnityEditor;
using UnityEngine;

namespace Soso.Utils.Editor.Tags
{
    [CustomPropertyDrawer(typeof(TagSelector))]
    public class TagDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.String)
            {
                EditorGUI.LabelField(position, label.text, $"{nameof(TagSelector)} can only be applied to string fields");
                return;
            }
            
            string value = property.stringValue;
            if (string.IsNullOrEmpty(value))
            {
                value = UnityEditorInternal.InternalEditorUtility.tags.FirstOrDefault();
            }
            
            EditorGUI.BeginProperty(position, label, property);
            
            property.stringValue = EditorGUI.TagField(position, label, value);
            
            EditorGUI.EndProperty();
        }
    }
}
