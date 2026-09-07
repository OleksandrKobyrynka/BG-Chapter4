using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class EditorWindowDemo : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    [MenuItem("Window/UI Toolkit/EditorWindowDemo")]
    public static void ShowExample()
    {
        EditorWindowDemo wnd = GetWindow<EditorWindowDemo>();
        wnd.titleContent = new GUIContent("EditorWindowDemo");
    }

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;

        VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
        root.Add(labelFromUXML);

        VisualElement label = new Label("C# created");
        root.Add(label);

        VisualElement image = new Image();
        image.style.width = 100;
        image.style.height = 100;
        image.style.alignSelf = Align.Center;
        image.style.backgroundColor = Color.red;
        root.Add(image);
    }
}
