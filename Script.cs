using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script : MonoBehaviour
{
    /* Constants for each environment */
    public const int CHURCH_ID = 1, FIELD_ID = 2, DISCO_ID = 3, NONE = 0;

    /* Creates an element for each environment */
    Church church;

    Field field;

    Disco disco;

    /* Defines the current environment (starts at 0) */
    int current;

    void Start()
    {
        current = 0;
        church = new Church();
        field = new Field();
        disco = new Disco();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            if (current != CHURCH_ID)
            {
                DestroyCurrentEnvironment();
                church.CreateSceneElements();
                current = CHURCH_ID;
            }
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            if (current != FIELD_ID)
            {
                DestroyCurrentEnvironment();
                field.CreateSceneElements();
                current = FIELD_ID;
            }
        }
        else if (Input.GetKey(KeyCode.Alpha3))
        {
            if (current != DISCO_ID)
            {
                DestroyCurrentEnvironment();
                disco.CreateSceneElements();
                current = DISCO_ID;
            }
        }
        else if (Input.GetKey(KeyCode.Alpha0))
        {
            if (current != 0)
            {
                DestroyCurrentEnvironment();
                current = 0;
            }
        }
    }

    void DestroyCurrentEnvironment()
    {
        if (current == CHURCH_ID)
        {
            church.EliminateSceneElements();
        }
        else if (current == FIELD_ID)
        {
            field.EliminateSceneElements();
        }
        else if (current == DISCO_ID)
        {
            disco.EliminateSceneElements();
        }
    }

    //-----------------------------------------------------
    //Environment classes
    //-----------------------------------------------------
    /* Class for Church*/
    public class Church
    {
        /* Church constructor */
        public Church()
        {
        }

        /* List of GameObjects used */
        GameObject floor;

        GameObject[] walls;

        GameObject roof;

        GameObject[] well;

        GameObject lava;

        GameObject[] fire;

        /* Creates all the scene elements */
        public void CreateSceneElements()
        {
            floor = CreateFloor();
            walls = CreateWalls();

            roof = CreateRoof();
            well = CreateWell();
            lava = CreateLava();
            fire = CreateFire();
        }

        /* Eliminates all the scene elements */
        public void EliminateSceneElements()
        {
            Destroy (floor);
            for (int i = 0; i < 4; i++)
            {
                Destroy(walls[i]);
                Destroy(well[i]);
            }

            Destroy (roof);
            Destroy (lava);
            for(int i = 0; i < fire.Length; i++){
                Destroy (fire[i]);
            }
        }

        GameObject
        DrawMesh(
            Vector3[] vert,
            int[] tris,
            Vector2[] uvs,
            Vector4[] tangs,
            string name
        )
        {
            GameObject go =
                new GameObject(name,
                    typeof (MeshFilter),
                    typeof (MeshRenderer));
            Mesh msh = new Mesh();
            go.GetComponent<MeshFilter>().mesh = msh;
            msh.Clear();
            msh.RecalculateNormals();
            msh.vertices = vert;
            msh.triangles = tris;
            msh.uv = uvs;
            msh.tangents = tangs;
            return go;
        }

        Color RGBToPercent(Vector3 colores)
        {
            Vector3 p =
                new Vector3((float)(colores.x / 255),
                    (float)(colores.y / 255),
                    (float)(colores.z / 255));
            return new Color(p.x, p.y, p.z);
        }

        //-----------------------------------------------------
        //Individual methods
        //-----------------------------------------------------
        /* Creates the floor */
        GameObject CreateFloor()
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.transform.localScale = new Vector3(60f, 1, 100f);
            go.transform.position -= new Vector3(0, 0.5f, 0);
            go.GetComponent<Renderer>().material =
                (Material) Resources.Load("Church/floor");
            return go;
        }

        /* Creates the walls */
        GameObject[] CreateWalls()
        {
            GameObject[] gos = new GameObject[4];
            for (int i = 0; i < 2; i++)
            {
                gos[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
                gos[i].transform.localScale = new Vector3(60, 40, 1);
                gos[i].transform.position +=
                    new Vector3(0, 20, 50 * (-1 + 2 * (i % 2)));
                gos[i].GetComponent<Renderer>().material =
                    (Material) Resources.Load("Church/wallShort");
                gos[i]
                    .GetComponent<Renderer>()
                    .material
                    .SetTextureScale("_MainTex", new Vector2(5, 4));
            }
            for (int i = 2; i < 4; i++)
            {
                gos[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
                gos[i].transform.localScale = new Vector3(1, 40, 100);
                gos[i].transform.position +=
                    new Vector3(30 * (-1 + 2 * (i % 2)), 20, 0);
                gos[i].GetComponent<Renderer>().material =
                    (Material) Resources.Load("Church/wallLong");
                gos[i]
                    .GetComponent<Renderer>()
                    .material
                    .SetTextureScale("_MainTex", new Vector2(5, 2));
            }

            return gos;
        }

        /* Creates the roof */
        GameObject CreateRoof()
        {
            GameObject go = CreateFloor();
            go.transform.position += new Vector3(0, 40, 0);
            go.GetComponent<Renderer>().material =
                (Material) Resources.Load("Church/roof");
            return go;
        }

        /* Creates the well */
        GameObject[] CreateWell()
        {
            GameObject[] gos = new GameObject[4];
            for (int i = 0; i < 2; i++)
            {
                gos[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
                gos[i].transform.localScale = new Vector3(17, 2, 2);
                gos[i].transform.position +=
                    new Vector3(0, 0, 7.5f * (-1 + 2 * (i % 2)) + 15);
                gos[i].GetComponent<Renderer>().material =
                    (Material) Resources.Load("Church/well");
                gos[i]
                    .GetComponent<Renderer>()
                    .material
                    .SetTextureScale("_MainTex", new Vector2(10, 1.5f));
            }
            for (int i = 2; i < 4; i++)
            {
                gos[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
                gos[i].transform.localScale = new Vector3(2, 2, 13);
                gos[i].transform.position +=
                    new Vector3(7.5f * (-1 + 2 * (i % 2)), 0, 15);
                gos[i].GetComponent<Renderer>().material =
                    (Material) Resources.Load("Church/well");
                gos[i]
                    .GetComponent<Renderer>()
                    .material
                    .SetTextureScale("_MainTex", new Vector2(2, 5));
            }
            return gos;
        }

        /* Creates the lava */
        GameObject CreateLava()
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject lava = new GameObject("Lava");
            lava.transform.SetParent(go.transform);
            go.transform.localScale = new Vector3(14, 1.5f, 14);
            go.transform.position += new Vector3(0, 0, 15);
            go.GetComponent<Renderer>().material =
                (Material) Resources.Load("Church/lava");
            lava.transform.position += new Vector3(0, 1, 0);
            Light lght = lava.AddComponent<Light>();
            lght.color = RGBToPercent(new Vector3(255, 89, 23));
            lght.type = LightType.Point;
            lght.range = 50;
            lght.intensity = 30;

            return go;
        }

        GameObject[] CreateFire()
        {
            GameObject[] gos = new GameObject[2];
            for (int i = 0; i < gos.Length; i++)
            {
                gos[i] =
                    (GameObject)
                    Instantiate((GameObject) Resources.Load("Church/fire"));
                    gos[i].transform.position = new Vector3(0,-10,15);
            }
            return gos;
        }
    }

    /* Class for Field */
    public class Field
    {
        GameObject a;

        public Field()
        {
        }

        public void CreateSceneElements()
        {
            a = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            a.GetComponent<Renderer>().material.color = Color.green;
            a.transform.position = new Vector3(0, 3, 0);
        }

        public void EliminateSceneElements()
        {
            Destroy (a);
        }
    }

    /* Class for Disco */
    public class Disco
    {
        GameObject a;

        public Disco()
        {
        }

        public void CreateSceneElements()
        {
            a = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            a.GetComponent<Renderer>().material.color = Color.blue;
            a.transform.position = new Vector3(0, 0, 3);
        }

        public void EliminateSceneElements()
        {
            Destroy (a);
        }
    }
}
