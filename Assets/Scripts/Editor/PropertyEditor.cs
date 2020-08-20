using UnityEditor;

[CustomEditor(typeof(Property))]
public class PropertyEditor : Editor {
  private SerializedObject targetObject;
  private SerializedProperty m_index;
  private SerializedProperty m_sideType;
  private SerializedProperty m_buffManager;

  void OnEnable() {
    targetObject = new SerializedObject(base.target);
    m_index = targetObject.FindProperty("index");
    m_sideType = targetObject.FindProperty("sideType");
    m_buffManager = targetObject.FindProperty("buffManager");
  }
  public override void OnInspectorGUI() {
    targetObject.Update();
    EditorGUILayout.PropertyField(m_sideType);
    EditorGUILayout.PropertyField(m_buffManager);
    if (m_sideType.intValue == (int)SideType.Monster) {
      EditorGUILayout.PropertyField(m_index);
    }
    //serializedObject.ApplyModifiedProperties();
    targetObject.ApplyModifiedProperties();
  }
}