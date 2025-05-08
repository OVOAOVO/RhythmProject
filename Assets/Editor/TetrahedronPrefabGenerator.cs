using UnityEngine;
using UnityEditor;
using System.IO;


// 这个脚本用于创建一个四面体的预制体，并将其保存到指定路径
// 你可以在 Unity 编辑器中运行这个脚本来生成预制体
// 注意：这个脚本目前没有用，因为3棱锥显示很差的原因
public class TetrahedronPrefabGenerator
{
    [MenuItem("Tools/Create Enemy Tetrahedron Prefab")]
    public static void CreateTetrahedron()
    {
        // 创建 GameObject
        GameObject tetra = new GameObject("Tetrahedron");

        MeshFilter mf = tetra.AddComponent<MeshFilter>();
        MeshRenderer mr = tetra.AddComponent<MeshRenderer>();

        // 设置 Mesh
        Mesh tetrahedronMesh = CreateTetrahedronMesh();

        // 检查 Mesh 是否有效
        if (tetrahedronMesh == null)
        {
            Debug.LogError("Mesh generation failed!");
            return;
        }

        // 将 Mesh 保存为 Asset
        string meshPath = "Assets/Mesh/TetrahedronMesh.asset";
        if (!AssetDatabase.IsValidFolder("Assets/Prefab/Enemy"))
        {
            Directory.CreateDirectory("Assets/Prefab/Enemy");
        }

        AssetDatabase.CreateAsset(tetrahedronMesh, meshPath);
        AssetDatabase.SaveAssets();

        // 赋值 Mesh
        mf.sharedMesh = AssetDatabase.LoadAssetAtPath<Mesh>(meshPath);

        // 创建材质
        Material mat = new Material(Shader.Find("Standard"));
        mat.color = Color.cyan;
        mr.sharedMaterial = mat;

        // 保存为 Prefab
        string folderPath = "Assets/Prefab/Enemy";
        string prefabPath = folderPath + "/Tetrahedron.prefab";

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        PrefabUtility.SaveAsPrefabAsset(tetra, prefabPath);
        Object.DestroyImmediate(tetra);

        Debug.Log("✅ Created Prefab at " + prefabPath);
    }

    static Mesh CreateTetrahedronMesh()
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[]
        {
            new Vector3(0, 0, 0),
            new Vector3(1, 0, 0),
            new Vector3(0.5f, 0, Mathf.Sqrt(0.75f)),
            new Vector3(0.5f, 1, Mathf.Sqrt(0.75f) / 3f)
        };

        int[] triangles = new int[]
        {
            0, 2, 1,
            0, 1, 3,
            1, 2, 3,
            2, 0, 3
        };

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        Debug.Log("Mesh generated: " + mesh); // 打印 Mesh 信息

        return mesh;
    }
}
