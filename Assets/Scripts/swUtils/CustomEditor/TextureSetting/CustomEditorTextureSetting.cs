#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CustomEditorTextureSetting : EditorWindow
{
    private List<TextureData> textureList = new List<TextureData>();
    private Vector2 scrollPosition;
    private Rect dropArea;

    [System.Serializable]
    private class TextureData
    {
        public Texture2D texture;
        public TextureImporterType textureType = TextureImporterType.Sprite;
        public SpriteImportMode spriteMode = SpriteImportMode.Single;
        public int calculatedPPU;
        public bool isSelected;
    }

    [MenuItem("SwEditor/Texture Setting")]
    public static void ShowWindow()
    {
        GetWindow<CustomEditorTextureSetting>("Texture Setting");
    }

    private void OnGUI()
    {
        DrawTitle();
        DrawDragAndDropArea();
        DrawTextureList();
        DrawButtons();

        // 드래그 앤 드롭 처리
        HandleDragAndDrop();
    }

    private void DrawTitle()
    {
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Texture Manager", EditorStyles.boldLabel);
        EditorGUILayout.Space(5);
    }

    private void DrawDragAndDropArea()
    {
        // 드래그 앤 드롭 영역 설정
        Event evt = Event.current;
        Rect dropRect = GUILayoutUtility.GetRect(0.0f, 50.0f, GUILayout.ExpandWidth(true));
        dropArea = dropRect;

        GUI.Box(dropRect, "Drag and Drop Textures Here", EditorStyles.helpBox);
    }

    private void DrawTextureList()
    {
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Texture List", EditorStyles.boldLabel);

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        for (int i = 0; i < textureList.Count; i++)
        {
            EditorGUILayout.BeginVertical("box");

            var textureData = textureList[i];

            // 체크박스
            EditorGUILayout.BeginHorizontal();
            textureData.isSelected = EditorGUILayout.Toggle(textureData.isSelected, GUILayout.Width(20));

            // 텍스처 정보 표시
            if (textureData.texture != null)
            {
                EditorGUILayout.LabelField($"{textureData.texture.name} ({textureData.texture.width}x{textureData.texture.height})");
            }
            EditorGUILayout.EndHorizontal();

            if (textureData.texture != null)
            {
                EditorGUI.indentLevel++;

                // Texture Type
                textureData.textureType = (TextureImporterType)EditorGUILayout.EnumPopup("Texture Type", textureData.textureType);

                // Sprite Mode (Texture Type이 Sprite일 때만)
                if (textureData.textureType == TextureImporterType.Sprite)
                {
                    textureData.spriteMode = (SpriteImportMode)EditorGUILayout.EnumPopup("Sprite Mode", textureData.spriteMode);
                }

                // PPU 표시 (자동 계산된 값)
                EditorGUILayout.LabelField($"Calculated PPU: {textureData.calculatedPPU}");

                EditorGUI.indentLevel--;
            }

            // 제거 버튼
            if (GUILayout.Button("Remove", GUILayout.Width(60)))
            {
                textureList.RemoveAt(i);
                i--;
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(5);
        }

        EditorGUILayout.EndScrollView();
    }

    private void DrawButtons()
    {
        EditorGUILayout.Space(10);

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Apply Settings"))
        {
            ApplySettingsToSelected();
        }

        if (GUILayout.Button("Select All"))
        {
            SelectAll(true);
        }

        if (GUILayout.Button("Deselect All"))
        {
            SelectAll(false);
        }

        if (GUILayout.Button("Clear List"))
        {
            textureList.Clear();
        }

        if(GUILayout.Button("Mode Single"))
        {
            ChangeSpriteModeSingle();
        }

        if(GUILayout.Button("Mode Multiple"))
        {
            ChangeSpriteModeMultiple();
        }

        EditorGUILayout.EndHorizontal();
    }

    private void HandleDragAndDrop()
    {
        Event evt = Event.current;
        switch (evt.type)
        {
            case EventType.DragUpdated:
            case EventType.DragPerform:
                if (!dropArea.Contains(evt.mousePosition))
                    return;

                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                if (evt.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();

                    foreach (Object draggedObject in DragAndDrop.objectReferences)
                    {
                        if (draggedObject is Texture2D texture)
                        {
                            AddTexture(texture);
                        }
                    }
                }
                Event.current.Use();
                break;
        }
    }

    private void AddTexture(Texture2D texture)
    {
        // 중복 체크
        if (textureList.Exists(t => t.texture == texture))
            return;

        var textureData = new TextureData
        {
            texture = texture,
            calculatedPPU = CalculatePPU(texture),
            isSelected = true
        };

        textureList.Add(textureData);
    }

    private int CalculatePPU(Texture2D texture)
    {
        // PPU 계산 로직
        string path = AssetDatabase.GetAssetPath(texture);
        TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;

        if (importer != null)
        {
            // 텍스처 크기를 기준으로 PPU 계산
            return texture.width; // 기본적으로 너비 값을 사용
        }

        return 100; // 기본값
    }

    private void ApplySettingsToSelected()
    {
        foreach (var textureData in textureList)
        {
            if (!textureData.isSelected || textureData.texture == null)
                continue;

            string path = AssetDatabase.GetAssetPath(textureData.texture);
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;

            if (importer != null)
            {
                importer.textureType = textureData.textureType;

                if (textureData.textureType == TextureImporterType.Sprite)
                {
                    importer.spriteImportMode = textureData.spriteMode;
                    importer.spritePixelsPerUnit = textureData.calculatedPPU;
                }

                importer.filterMode = FilterMode.Point;
                importer.textureCompression = TextureImporterCompression.Uncompressed;

                importer.SaveAndReimport();
            }
        }

        Debug.Log("Settings applied to selected textures");
    }

    private void SelectAll(bool select)
    {
        foreach (var textureData in textureList)
        {
            textureData.isSelected = select;
        }
    }

    private void ChangeSpriteModeSingle()
    {
        foreach (var textureData in textureList)
        {
            textureData.spriteMode = SpriteImportMode.Single;
        }
    }

    private void ChangeSpriteModeMultiple()
    {
        foreach (var textureData in textureList)
        {
            textureData.spriteMode = SpriteImportMode.Multiple;
        }
    }
}
#endif // UNITY_EDITOR