using UnityEngine;

public class Gameboy : MonoBehaviour {

    // Based on https://medium.com/@cyrilltoboe/code-first-game-boy-post-processing-shader-for-unity-ef140252fd7d
    // Requires Identity shader

    public Camera cam;
    public Material gameboyMaterial;
    public Material identityMaterial;

    private int height = 144;
    private int width;

    private RenderTexture _downscaledRenderTexture;

    private void OnEnable()
    {
        int width = Mathf.RoundToInt(cam.aspect * height);
        _downscaledRenderTexture = new RenderTexture(width, height, 16);
        _downscaledRenderTexture.filterMode = FilterMode.Point;
    }

    private void OnDisable()
    {
        Destroy(_downscaledRenderTexture);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, _downscaledRenderTexture, gameboyMaterial); // Render image without effects
        Graphics.Blit(_downscaledRenderTexture, destination, identityMaterial); // Then copy that into a fullscreen image
    }

}
