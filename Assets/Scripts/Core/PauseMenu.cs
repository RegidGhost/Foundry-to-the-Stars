using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private bool _isOpen;
    
    // Стили
    private GUIStyle _titleStyle;
    private GUIStyle _buttonStyle;
    private GUIStyle _boxStyle;
    private bool _stylesInitialized;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            _isOpen = !_isOpen;
    }

    private void InitStyles()
    {
        // Стиль заголовка
        _titleStyle = new GUIStyle(GUI.skin.label);
        _titleStyle.fontSize = 28;
        _titleStyle.fontStyle = FontStyle.Bold;
        _titleStyle.alignment = TextAnchor.MiddleCenter;
        _titleStyle.normal.textColor = Color.white;
        _titleStyle.richText = true;

        // Стиль кнопок
        _buttonStyle = new GUIStyle(GUI.skin.button);
        _buttonStyle.fontSize = 18;
        _buttonStyle.normal.textColor = Color.white;
        _buttonStyle.hover.textColor = Color.cyan; // Цвет при наведении
        _buttonStyle.fontStyle = FontStyle.Bold;
        
        // Создаем текстуры для кнопок
        Texture2D normalTex = MakeTex(2, 2, new Color(0.2f, 0.4f, 0.8f, 0.9f)); // Синий
        Texture2D hoverTex = MakeTex(2, 2, new Color(0.3f, 0.5f, 0.9f, 1f));   // Светло-синий
        Texture2D activeTex = MakeTex(2, 2, new Color(0.1f, 0.3f, 0.7f, 1f));  // Темно-синий
        
        _buttonStyle.normal.background = normalTex;
        _buttonStyle.hover.background = hoverTex;
        _buttonStyle.active.background = activeTex;

        // Стиль окна (фона)
        _boxStyle = new GUIStyle(GUI.skin.box);
        _boxStyle.normal.background = MakeTex(2, 2, new Color(0.1f, 0.1f, 0.1f, 0.95f)); // Темно-серый

        _stylesInitialized = true;
    }

    private void OnGUI()
    {
        if (!_isOpen) return;
        if (!_stylesInitialized) InitStyles();

        // Затемняем весь экран
        GUI.color = new Color(0, 0, 0, 0.7f);
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
        GUI.color = Color.white;

        // Размеры меню
        float w = 350f;
        float h = 320f;
        Rect windowRect = new Rect((Screen.width - w) / 2, (Screen.height - h) / 2, w, h);

        // Рисуем фон меню
        GUI.Box(windowRect, "", _boxStyle);

        GUILayout.BeginArea(new Rect(windowRect.x + 20, windowRect.y + 20, w - 40, h - 40));

        // Заголовок
        GUILayout.Label("ПАУЗА", _titleStyle);
        GUILayout.Space(20);

        // Кнопки
        if (GUILayout.Button("СОХРАНИТЬ", _buttonStyle, GUILayout.Height(45))) 
        {
            SaveSystem.Save();
        }
        
        GUILayout.Space(10);
        if (GUILayout.Button("ЗАГРУЗИТЬ", _buttonStyle, GUILayout.Height(45))) 
        {
            SaveSystem.Load();
        }
        
        GUILayout.Space(10);
        if (GUILayout.Button("ПРОДОЛЖИТЬ", _buttonStyle, GUILayout.Height(45))) 
        {
            _isOpen = false;
        }
        
        GUILayout.Space(10);
        if (GUILayout.Button("ВЫЙТИ ИЗ ИГРЫ", _buttonStyle, GUILayout.Height(45))) 
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

        GUILayout.EndArea();
    }

    // Вспомогательный метод для генерации цветных текстур
    private Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; ++i) pix[i] = col;
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }
}