using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using System;

#if UNITY_EDITOR
using UnityEditor.Rendering.PostProcessing;
#endif

[Serializable]
[PostProcess(typeof(PixelateImageEffectRenderer), PostProcessEvent.BeforeStack, "Custom/PixelateImageEffect")]
public class PixelateImageEffect : PostProcessEffectSettings
{   
    [Range(1, 50)]
	public IntParameter tileSize = new IntParameter() { value = 10 };
}

public sealed class PixelateImageEffectRenderer : PostProcessEffectRenderer<PixelateImageEffect>
{
    private Shader shader = null;

    public override void Render(PostProcessRenderContext iContext)
    {
        if (shader == null)
        {
            shader = Shader.Find("Hidden/PixelateImageEffect");
        }
        if(shader != null)
        {
            var sheet = iContext.propertySheets.Get(shader);

            sheet.properties.SetFloat("_TileSize", settings.tileSize);

            iContext.command.BlitFullscreenTriangle(iContext.source, iContext.destination, sheet, 0);
        }
    }

    public override void Release()
    {
        base.Release();

        shader = null;
    }
}

#if UNITY_EDITOR
[PostProcessEditor(typeof(PixelateImageEffect))]
public class PixelateImageEffectEditor : PostProcessEffectEditor<PixelateImageEffect>
{
    SerializedParameterOverride tileSize;

    public override void OnEnable()
    {
        base.OnEnable();

        tileSize = FindParameterOverride(x => x.tileSize);
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        PropertyField(tileSize);
    }
}
#endif