using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;

[CustomEditor(typeof(GradientTexture))]
public class GradientTextureEditor : Editor 
{
   public override void OnInspectorGUI() 
   {
      EditorGUI.BeginChangeCheck();
      SerializedProperty gradient = serializedObject.FindProperty("gradient");
      SerializedProperty wrapMode = serializedObject.FindProperty("wrapMode");
      SerializedProperty filterMode = serializedObject.FindProperty("filterMode");
      SerializedProperty resolution = serializedObject.FindProperty("resolution");
      SerializedProperty useSharedMaterial = serializedObject.FindProperty("useSharedMaterial");
      SerializedProperty anisoLevel = serializedObject.FindProperty("anisoLevel");
      
      GradientTexture ct = target as GradientTexture;
      Renderer r = ct.GetComponent<Renderer>();
      if (r == null)
      {
         EditorGUILayout.BeginHorizontal();
         EditorGUILayout.PrefixLabel("global name");
         ct.globalName = EditorGUILayout.TextField(ct.globalName);
         EditorGUILayout.EndHorizontal();
      }
      else
      {
         Material mat = r.sharedMaterial;
         Shader s = mat.shader;
         int count = ShaderUtil.GetPropertyCount(s);
         List<string> textureNames = new List<string>();
         for (int i = 0; i < count; ++i)
         {
            if (ShaderUtil.GetPropertyType(s, i) == ShaderUtil.ShaderPropertyType.TexEnv)
            {
               textureNames.Add(ShaderUtil.GetPropertyName(s, i));
            }
         }
         int index = textureNames.IndexOf(ct.propertyName);
         int newIdx = EditorGUILayout.Popup(index, textureNames.ToArray());
         if (newIdx != index)
         {
            ct.propertyName = textureNames[newIdx];
         }
      }
      EditorGUILayout.PropertyField(gradient);
      EditorGUILayout.PropertyField(wrapMode);
      EditorGUILayout.PropertyField(filterMode);
      EditorGUILayout.PropertyField(resolution);
      if (r != null)
      {
         EditorGUILayout.PropertyField(useSharedMaterial);
      }
      EditorGUILayout.PropertyField(anisoLevel);
      if (EditorGUI.EndChangeCheck())
      {
         serializedObject.ApplyModifiedProperties();
         ct.Refresh();
      }
      
      if (GUILayout.Button("Save Texture"))
      {
         string path = EditorUtility.SaveFilePanel("Save Texture", Application.dataPath, "curve", "png");
         if (!string.IsNullOrEmpty(path))
         {
            Texture2D tex = ct.Generate(false);
            byte[] bytes = tex.EncodeToPNG();
            File.WriteAllBytes(path, bytes);
            DestroyImmediate(tex);
            AssetImporter ai = AssetImporter.GetAtPath(path);
            if (ai != null)
            {
               TextureImporter ti = ai as TextureImporter;
               ti.anisoLevel = ct.anisoLevel;
               ti.linearTexture = true;
               ti.generateMipsInLinearSpace = true;
               ti.filterMode = ct.filterMode;
               ti.wrapMode = ct.wrapMode;
               ti.textureFormat = TextureImporterFormat.ARGB32;
               ti.SaveAndReimport();
            }
         }
      }
   }
}
