using UnityEngine;
using System.Collections;

public class GradientTexture : MonoBehaviour 
{
   public Gradient gradient = new Gradient();
   public TextureWrapMode wrapMode = TextureWrapMode.Clamp;
   public FilterMode filterMode = FilterMode.Bilinear;
   [Range(1, 16)]
   public int anisoLevel = 1;
   
   public enum Resolution
   {
      k16  = 16,
      k32  = 32,
      k64  = 64,
      k128 = 128,
      k256 = 256,
      k512 = 512
   }
   
   public Resolution resolution = Resolution.k256;
   public string     propertyName;
   public bool       useSharedMaterial = false;
   public string     globalName;
   
   private Texture2D texture;
   
   void Start()
   {
      Refresh();
   }
   
   public Texture2D Generate(bool makeNoLongerReadable = false)
   {
      Texture2D tex = new Texture2D((int)resolution, 1, TextureFormat.ARGB32, false, true);
      tex.wrapMode = wrapMode;
      tex.filterMode = filterMode;
      tex.anisoLevel = anisoLevel;
      
      // this method does GC allocation; we could do it pixel by pixel and avoid that, but that would
      // cross the C#->C bridge once per pixel, so this seems like a better way to go for most uses.
      Color[] colors = new Color[(int)resolution];
      float div = (float)(int)resolution;
      for (int i = 0; i < (int)resolution; ++i)
      {
         float t = (float)i/div;
         colors[i] = gradient.Evaluate(t);
      }
      tex.SetPixels(colors);
      tex.Apply(false, makeNoLongerReadable);
      
      return tex;
   }
   
   public void Refresh()
   {
      if (texture != null)
      {
         DestroyImmediate(texture);
      }
      texture = Generate();
      
      if (!string.IsNullOrEmpty(propertyName))
      {
         Renderer r = GetComponent<Renderer>();
         if (r != null)
         {
            Material mat;
            if (useSharedMaterial || !Application.isPlaying)
            {
               mat = r.sharedMaterial;
            }
            else
            {
               mat = r.material;
            }
            if (mat.HasProperty(propertyName))
            {
               mat.SetTexture(propertyName, texture);
            }
         }
      }
      else if (!string.IsNullOrEmpty(globalName))
      {
         Shader.SetGlobalTexture(globalName, texture);
      }
   }
   
   void OnDestroy()
   {
      if (texture != null)
      {
         DestroyImmediate(texture);
      }
   }
}
