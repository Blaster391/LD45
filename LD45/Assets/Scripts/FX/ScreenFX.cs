using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFX : MonoBehaviour
{
    Material blackAndWhiteMaterial;

    Material pointMultiColourMaterial;
    Material passThroughMaterial;
    // Start is called before the first frame update
    void Start()
    {
        blackAndWhiteMaterial = new Material(Shader.Find("Hidden/BlackAndWhiteShader"));
        pointMultiColourMaterial = new Material(Shader.Find("Hidden/PointMultiColourShader"));
        passThroughMaterial = new Material(Shader.Find("Hidden/PassThroughShader"));
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        RenderTexture original;

     //   Graphics.Blit(source, destination, blackAndWhiteMaterial);

        Vector2 point = new Vector2(0.5f,0.5f);
        float radius = 0.1f;
        pointMultiColourMaterial.SetFloat("Radius", radius);
        pointMultiColourMaterial.SetVector("Point", point);

        Graphics.Blit(source, destination, passThroughMaterial);
        Graphics.SetRenderTarget(source);
        Graphics.Blit(destination, source, passThroughMaterial);
        Graphics.SetRenderTarget(destination);
        Graphics.Blit(source, destination, passThroughMaterial);

      //  Graphics.Blit(source, destination, pointMultiColourMaterial);
    }
}
