using UnityEngine;

[ExecuteAlways]
public class GridRenderer : MonoBehaviour
{
    [SerializeField] private int width = 64;
    [SerializeField] private int height = 64;
    [SerializeField] private Color lineColor = new Color(1f, 1f, 1f, 0.15f);
    [SerializeField] private bool showInGameView = true;
    [SerializeField] private bool showInSceneView = true;

    private Material _mat;

    private void OnEnable() => CreateMaterial();
    private void OnDisable()
    {
        if (_mat == null) return;
        if (Application.isPlaying) Destroy(_mat); else DestroyImmediate(_mat);
        _mat = null;
    }

    private void CreateMaterial()
    {
        if (_mat != null) return;
        var shader = Shader.Find("Hidden/Internal-Colored");
        if (shader == null) { Debug.LogError("[GridRenderer] Shader not found"); return; }
        _mat = new Material(shader) { hideFlags = HideFlags.HideAndDontSave };
        _mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        _mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        _mat.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
        _mat.SetInt("_ZWrite", 0);
        _mat.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);
    }

    private void OnRenderObject()
    {
        if (_mat == null || Camera.current == null) return;
        bool isScene = Camera.current.cameraType == CameraType.SceneView;
        if (isScene && !showInSceneView) return;
        if (!isScene && !showInGameView) return;

        _mat.SetPass(0);
        GL.PushMatrix();
        GL.Begin(GL.LINES);
        GL.Color(lineColor);
        for (int x = 0; x <= width; x++) { GL.Vertex3(x, 0, 0); GL.Vertex3(x, height, 0); }
        for (int y = 0; y <= height; y++) { GL.Vertex3(0, y, 0); GL.Vertex3(width, y, 0); }
        GL.End();
        GL.PopMatrix();
    }
}