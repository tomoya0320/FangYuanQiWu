using UnityEngine;

public class GrayScale : MonoBehaviour
{
    public Shader shader;
    [Range(0, 1.0f)]
    public float GrayFactor;
    private Material material;

    private void OnEnable()
    {
        GrayFactor = 0;
        material = new Material(shader);
    }
    private void Update()
    {
        if (GrayFactor < 1)
        {
            GrayFactor = Mathf.Clamp(GrayFactor + Time.deltaTime * 0.5f, 0, 1.0f);
        }
    }
    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (material != null)
        {
            //设置shader中的_GrayFactor参数
            material.SetFloat("_GrayFactor", GrayFactor);
            Graphics.Blit(src, dest, material);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
    private void OnDisable()
    {
        if (material != null)
        {
            DestroyImmediate(material);
        }
    }
}