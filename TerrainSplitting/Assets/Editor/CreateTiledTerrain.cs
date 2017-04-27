using UnityEngine;
using UnityEditor;
using System.IO;

public class CreateTiledTerrain : EditorWindow {

    private static EditorWindow window;

    private static Vector2 tileAmount = Vector2.one;

    private float width = 1000;
    private float lenght = 1000;
    private float height = 600;

    private int heightmapResoltion = 513;
    private int detailResolution = 1024;
    private int detailResolutionPerPatch = 8;
    private int controlTextureResolution = 512;
    private int baseTextureReolution = 1024;

    private string path = string.Empty;

    private enum Alphabet {
        None, A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z
    }
    private Alphabet alphabet = Alphabet.None;



    [MenuItem("Terrain/Create Tiled Terrain")]
    public static void CreateWindow() {
        window = EditorWindow.GetWindow(typeof(CreateTiledTerrain));
        window.titleContent.text = "Tiled Terrain";
        window.minSize = new Vector2(500f, 700f);
    }

    private void OnGUI() {
        EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(false));
        tileAmount = EditorGUILayout.Vector2Field("Amount", tileAmount);
        EditorGUILayout.EndHorizontal();

        width = EditorGUILayout.FloatField("Terrain Width", width);
        lenght = EditorGUILayout.FloatField("Terrain Lenght", lenght);
        height = EditorGUILayout.FloatField("Terrain Height", height);

        EditorGUILayout.Space();

        heightmapResoltion = EditorGUILayout.IntField("Heightmap Resoltion", heightmapResoltion);
        heightmapResoltion = Mathf.ClosestPowerOfTwo(heightmapResoltion) + 1;
        heightmapResoltion = Mathf.Clamp(heightmapResoltion, 33, 4097);

        detailResolution = EditorGUILayout.IntField("Detail Resolution", detailResolution);
        detailResolution = Mathf.ClosestPowerOfTwo(detailResolution);
        detailResolution = Mathf.Clamp(detailResolution, 0, 4096);

        detailResolutionPerPatch = EditorGUILayout.IntField("Detail Resolution Per Patch", detailResolutionPerPatch);
        detailResolutionPerPatch = Mathf.ClosestPowerOfTwo(detailResolutionPerPatch);
        detailResolutionPerPatch = Mathf.Clamp(detailResolutionPerPatch, 8, 128);

        controlTextureResolution = EditorGUILayout.IntField("Control Texture Resolution", controlTextureResolution);
        controlTextureResolution = Mathf.ClosestPowerOfTwo(controlTextureResolution);
        controlTextureResolution = Mathf.Clamp(controlTextureResolution, 16, 1024);

        baseTextureReolution = EditorGUILayout.IntField("Base Texture Reolution", baseTextureReolution);
        baseTextureReolution = Mathf.ClosestPowerOfTwo(baseTextureReolution);
        baseTextureReolution = Mathf.Clamp(baseTextureReolution, 16, 2048);

        EditorGUILayout.Space();
        GUILayout.Label("Path were to save TerrainDate:");
        path = EditorGUILayout.TextField("Assets/", path);

        if(GUILayout.Button("Create")) {
            ValidatePath();
            CreateTerrain();

            path = string.Empty;
        }
    }

    private void ValidatePath() {
        if(path == string.Empty) path = "TiledTerrain/TerrainData/";

        string pathToCheck = Application.dataPath + "/" + path;
        if(Directory.Exists(pathToCheck) == false) {
            Directory.CreateDirectory(pathToCheck);
        }
    }

    private void CreateTerrain() {
        GameObject parent = (GameObject) Instantiate(new GameObject("Terrain"));
        parent.transform.position = new Vector3(0, 0, 0);


        for(int x = 1; x <= tileAmount.x; x++) {
            for(int y = 1; y <= tileAmount.y; y++) {

                TerrainData terrainData = new TerrainData();

                alphabet = (Alphabet) x;
                string name = alphabet + "-" + y;

                terrainData.size = new Vector3(width / 16f,
                                                height,
                                                lenght / 16f);

                terrainData.baseMapResolution = baseTextureReolution;
                terrainData.heightmapResolution = heightmapResoltion;
                terrainData.alphamapResolution = controlTextureResolution;
                terrainData.SetDetailResolution(detailResolution, detailResolutionPerPatch);

                terrainData.name = name;
                GameObject terrain = (GameObject) Terrain.CreateTerrainGameObject(terrainData);

                terrain.name = name;
                terrain.transform.parent = parent.transform;
                terrain.transform.position = new Vector3(lenght * (x - 1), 0, width * (y - 1));

                AssetDatabase.CreateAsset(terrainData, "Assets/" + path + name + ".asset");


            }
        }



    }

}