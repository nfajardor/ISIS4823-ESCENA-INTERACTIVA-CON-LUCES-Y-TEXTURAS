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

    /* Defines the current environment (starts at 0) */
    int current;

    /* Time since the last scene change */
    float now;

    /* Defines the camera */
    Camera cam;

    void Start()
    {
        current = 0;
        now = 0;
        church = new Church();
        field = new Field();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            if (current != CHURCH_ID)
            {
                now = Time.time;
                DestroyCurrentEnvironment();
                church.CreateSceneElements();
                current = CHURCH_ID;
            }
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            if (current != FIELD_ID)
            {
                now = Time.time;
                DestroyCurrentEnvironment();
                field.CreateSceneElements();
                current = FIELD_ID;
            }
        }
        else if (Input.GetKey(KeyCode.Alpha0))
        {
            if (current != 0)
            {
                now = Time.time;
                DestroyCurrentEnvironment();
                current = 0;
            }
        }

        if (current != 0)
        {
            StartEnvironmentAnimation();
            if (current == FIELD_ID)
            {
                if (Input.GetKey(KeyCode.A))
                {
                    field.ChangeLightColor(1);
                }
                else if (Input.GetKey(KeyCode.B))
                {
                    field.ChangeLightColor(2);
                }
                else if (Input.GetKey(KeyCode.C))
                {
                    field.ChangeLightColor(3);
                }
            }
        }
    }

    /* Starts the animations of the current environment */
    void StartEnvironmentAnimation()
    {
        if (current == CHURCH_ID)
        {
            church.ChurchAnimation (now);
        }
        else if (current == FIELD_ID)
        {
            field.FieldAnimation (now);
        }
    }

    /* Eliminates elements of the current environment */
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
    }

    //-----------------------------------------------------
    //Environment classes
    //-----------------------------------------------------
    /* Class for Church*/
    public class Church
    {
        /* Main camera */
        Camera cam;

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

        GameObject[] cross;

        GameObject[] altar;

        GameObject music;

        /* List of the Animation Curves */
        AnimationCurve fire1x;

        AnimationCurve fire1y;

        AnimationCurve fire1z;

        AnimationCurve fire2x;

        AnimationCurve fire2y;

        AnimationCurve fire2z;

        AnimationCurve camerax;

        AnimationCurve cameray;

        AnimationCurve cameraz;

        /* Times for each of the animationCurves */
        float t0;

        float t1;

        float t2;

        float t3;

        float t4;

        float t5;

        float t6;

        float t7;

        /* Creates all the scene elements */
        public void CreateSceneElements()
        {
            floor = CreateFloor();
            walls = CreateWalls();
            roof = CreateRoof();
            well = CreateWell();
            lava = CreateLava();
            fire = CreateFire();
            cross = CreateCross();
            altar = CreateAltar();
            AdjustCamera();
            CreateAnimationCurves();
            music = CreateMusic();
        }

        /* Creates the music */
        GameObject CreateMusic()
        {
            GameObject go = new GameObject("");
            AudioSource source = go.AddComponent<AudioSource>();
            source.clip = Resources.Load<AudioClip>("Church/music");
            source.Play();
            return go;
        }

        /* Eliminates all the scene elements */
        public void EliminateSceneElements()
        {   
            Destroy(music);
            Destroy (floor);
            for (int i = 0; i < 4; i++)
            {
                Destroy(walls[i]);
                Destroy(well[i]);
            }
            Destroy (roof);
            Destroy (lava);
            for (int i = 0; i < fire.Length; i++)
            {
                Destroy(fire[i]);
            }
            for (int i = 0; i < 2; i++)
            {
                Destroy(cross[i]);
                Destroy(altar[i]);
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
        /* Generate animation curves */
        void CreateAnimationCurves()
        {
            t0 = 0;
            t1 = 2 * 1.5f;
            t2 = 2 * 2.5f;
            t3 = 2 * 4;
            t4 = 2 * 5;
            t5 = 2 * 6.5f;
            t6 = 2 * 7;
            t7 = 2 * 7.5f;
            fire1x =
                new AnimationCurve(new Keyframe(t0, 0),
                    new Keyframe(t1, 0),
                    new Keyframe(t2, 25),
                    new Keyframe(t3, 25),
                    new Keyframe(t4, 0),
                    new Keyframe(t5, 0),
                    new Keyframe(t6, 0),
                    new Keyframe(t7, 7));
            fire1y =
                new AnimationCurve(new Keyframe(t0, -10),
                    new Keyframe(t1, 15),
                    new Keyframe(t2, 35),
                    new Keyframe(t3, 35),
                    new Keyframe(t4, 35),
                    new Keyframe(t5, 35),
                    new Keyframe(t6, 20),
                    new Keyframe(t7, 5.5f));
            fire1z =
                new AnimationCurve(new Keyframe(t0, 15),
                    new Keyframe(t1, 15),
                    new Keyframe(t2, 0),
                    new Keyframe(t3, -40),
                    new Keyframe(t4, -25),
                    new Keyframe(t5, 20),
                    new Keyframe(t6, 32.5f),
                    new Keyframe(t7, 32.5f));
            fire2x =
                new AnimationCurve(new Keyframe(t0, 0),
                    new Keyframe(t1, 0),
                    new Keyframe(t2, -25),
                    new Keyframe(t3, -25),
                    new Keyframe(t4, 0),
                    new Keyframe(t5, 0),
                    new Keyframe(t6, 0),
                    new Keyframe(t7, -7));
            fire2y =
                new AnimationCurve(new Keyframe(t0, -10),
                    new Keyframe(t1, 15),
                    new Keyframe(t2, 35),
                    new Keyframe(t3, 35),
                    new Keyframe(t4, 35),
                    new Keyframe(t5, 35),
                    new Keyframe(t6, 20),
                    new Keyframe(t7, 5.5f));
            fire2z =
                new AnimationCurve(new Keyframe(t0, 15),
                    new Keyframe(t1, 15),
                    new Keyframe(t2, 0),
                    new Keyframe(t3, -40),
                    new Keyframe(t4, -25),
                    new Keyframe(t5, 20),
                    new Keyframe(t6, 32.5f),
                    new Keyframe(t7, 32.5f));
            camerax =
                new AnimationCurve(new Keyframe(t0, 45),
                    new Keyframe(t1, 0),
                    new Keyframe(t2, -45),
                    new Keyframe(t3, -45),
                    new Keyframe(t4, -45),
                    new Keyframe(t5, -45),
                    new Keyframe(t6, 0),
                    new Keyframe(t7, 30));
            cameray =
                new AnimationCurve(new Keyframe(t0, 0),
                    new Keyframe(t1, 0),
                    new Keyframe((t1 + t2 - 0.5f) / 2, -45),
                    new Keyframe(t2, -90),
                    new Keyframe(t3, -150),
                    new Keyframe(t4, -210),
                    new Keyframe(t5, -360),
                    new Keyframe(t6, -360),
                    new Keyframe(t7, -360));
            cameraz =
                new AnimationCurve(new Keyframe(t0, 0),
                    new Keyframe(t1, 0),
                    new Keyframe(t2, 0),
                    new Keyframe(t3, 0),
                    new Keyframe(t4, 0),
                    new Keyframe(t5, 0),
                    new Keyframe(t6, 0),
                    new Keyframe(t7, 0));
        }

        /* Adjusts the camera to its initial position */
        void AdjustCamera()
        {
            cam = Camera.main;
            cam.transform.position = new Vector3(0, 15, 0);
            Quaternion target = Quaternion.Euler(45, 0, 0);
            cam.transform.rotation =
                Quaternion.Slerp(cam.transform.rotation, target, 1);
        }

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

        /* Creates the fire lights */
        GameObject[] CreateFire()
        {
            GameObject[] gos = new GameObject[2];
            for (int i = 0; i < gos.Length; i++)
            {
                gos[i] =
                    (GameObject)
                    Instantiate((GameObject) Resources.Load("Church/fire"));
                gos[i].transform.position = new Vector3(0, -10, 15);
                Light l = gos[i].AddComponent<Light>();
                l.color = RGBToPercent(new Vector3(83, 78, 255));
                l.type = LightType.Point;
                l.range = 15;
                l.intensity = 10;
            }
            return gos;
        }

        /* Creates the cross */
        GameObject[] CreateCross()
        {
            GameObject hor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject ver = GameObject.CreatePrimitive(PrimitiveType.Cube);

            hor.transform.localScale = new Vector3(15, 5, 5);
            ver.transform.localScale = new Vector3(5, 20, 5);

            ver.transform.position += new Vector3(0, 20, 50);
            hor.transform.position += new Vector3(0, 22.5f, 50.001f);

            hor.GetComponent<Renderer>().material =
                (Material) Resources.Load("Church/crossh");
            ver.GetComponent<Renderer>().material =
                (Material) Resources.Load("Church/crossv");

            ver
                .GetComponent<Renderer>()
                .material
                .SetTextureScale("_MainTex", new Vector2(0.5f, 3.36f));
            ver
                .GetComponent<Renderer>()
                .material
                .SetTextureOffset("_MainTex", new Vector2(0.44f, 0.15555f));

            return new GameObject[] { hor, ver };
        }

        /* Creates the altar */
        GameObject[] CreateAltar()
        {
            GameObject body = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject top = GameObject.CreatePrimitive(PrimitiveType.Cube);

            body.transform.localScale = new Vector3(15, 4, 3);
            top.transform.localScale = new Vector3(20, 1, 5);

            body.transform.position += new Vector3(0, 2, 32.5f);
            top.transform.position += new Vector3(0, 4, 32.5f);

            body.GetComponent<Renderer>().material =
                (Material) Resources.Load("Church/bMarble");
            top.GetComponent<Renderer>().material =
                (Material) Resources.Load("Church/bMarble");
            return new GameObject[] { body, top };
        }

        /* Animation */
        public void ChurchAnimation(float tsc)
        {
            float now = Time.time;
            float localEnvTime = now - tsc;
            fire[0].transform.position =
                new Vector3(fire1x.Evaluate(localEnvTime),
                    fire1y.Evaluate(localEnvTime),
                    fire1z.Evaluate(localEnvTime));
            fire[1].transform.position =
                new Vector3(fire2x.Evaluate(localEnvTime),
                    fire2y.Evaluate(localEnvTime),
                    fire2z.Evaluate(localEnvTime));
            Quaternion target =
                Quaternion
                    .Euler(camerax.Evaluate(localEnvTime),
                    cameray.Evaluate(localEnvTime),
                    cameraz.Evaluate(localEnvTime));
            cam.transform.rotation =
                Quaternion
                    .Slerp(cam.transform.rotation,
                    target,
                    CalculateSmoothness(localEnvTime));
        }

        float CalculateSmoothness(float letime)
        {
            float ret = 0;
            if (letime >= t0 && letime < t1)
            {
                ret = 1 - (t1 - letime) / 2;
            }
            else if (letime >= t1 && letime < t2)
            {
                ret = 1 - (t2 - letime) / 2;
            }
            else if (letime >= t2 && letime < t3)
            {
                ret = 1 - (t3 - letime) / 2;
            }
            else if (letime >= t3 && letime < t4)
            {
                ret = 1 - (t4 - letime) / 2;
            }
            else if (letime >= t4 && letime < t5)
            {
                ret = 1 - (t5 - letime) / 2;
            }
            else if (letime >= t5 && letime < t6)
            {
                ret = 1 - (t6 - letime) / 2;
            }
            else if (letime >= t6 && letime < t7)
            {
                ret = 1 - (t7 - letime) / 2;
            }
            if (ret > 1)
            {
                ret = 1;
            }
            return ret;
        }
    }

    /* Class for Field */
    public class Field
    {
        /* The main camera */
        Camera cam;

        /* Origin of the camera */
        Vector3 camorg;

        public Field()
        {
        }

        //Game Objects
        GameObject[] planes;

        GameObject lght;

        GameObject lght2;

        GameObject lantern;

        GameObject music;

        /* List of the Animation Curves */
        AnimationCurve cx;

        AnimationCurve cy;

        AnimationCurve cz;

        AnimationCurve crx;

        AnimationCurve cry;

        AnimationCurve crz;

        /* Times for each of the animationCurves */
        float t0;

        float t1;

        float t2;

        float t3;

        float t4;

        public void CreateSceneElements()
        {
            AdjustCamera();
            planes = CreatePlanes();
            lght = CreateLight();
            lght2 = CreateLight2();
            lantern = CreateLantern();
            CreateAnimationCurves();
            music = CreateMusic();
        }

        /* Creates the music */
        GameObject CreateMusic()
        {
            GameObject go = new GameObject("");
            AudioSource source = go.AddComponent<AudioSource>();
            source.clip = Resources.Load<AudioClip>("Field/music");
            source.Play();
            return go;
        }

        public void EliminateSceneElements()
        {
            for (int i = 0; i < planes.Length; i++)
            {
                Destroy(planes[i]);
            }
            Destroy (lght);
            Destroy (lght2);
            Destroy (lantern);
            Destroy(music);
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

        //Individual Methods
        /* Creates the terrain */
        GameObject CreatePlane()
        {
            GameObject go =
                new GameObject("Terrain",
                    typeof (MeshFilter),
                    typeof (MeshRenderer));
            Vector3[] vert;
            int[] tri;
            Mesh msh = new Mesh();
            int w = 200;
            int h = 200;

            go.GetComponent<MeshFilter>().mesh = msh;
            go.GetComponent<Renderer>().material.color =
                RGBToPercent(new Vector3(20, 105, 105));
            vert = new Vector3[(w + 1) * (h + 1)];
            int c = 0;
            for (int i = 0; i <= h; i++)
            {
                for (int j = 0; j <= w; j++)
                {
                    float y = Mathf.PerlinNoise(i * 0.2f, j * 0.25f) * 4f;
                    vert[c] = new Vector3(j, y, i);
                    c++;
                }
            }

            tri = new int[6 * w * h];
            int v = 0;
            int t = 0;
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    tri[t] = v;
                    tri[t + 1] = v + w + 1;
                    tri[t + 2] = v + 1;
                    tri[t + 3] = v + 1;
                    tri[t + 4] = v + w + 1;
                    tri[t + 5] = v + w + 2;
                    v++;
                    t += 6;
                }
                v++;
            }
            msh.Clear();
            msh.vertices = vert;
            msh.triangles = tri;
            msh.RecalculateNormals();
            return go;
        }

        /* Creates the whole terrain */
        GameObject[] CreatePlanes()
        {
            planes = new GameObject[9];
            for (int i = 0; i < planes.Length; i++)
            {
                planes[i] = CreatePlane();
            }
            int d = 195;
            planes[1].transform.position += new Vector3(d, 0, 0);
            planes[2].transform.position += new Vector3(-d, 0, 0);
            planes[3].transform.position += new Vector3(0, 0, d);
            planes[4].transform.position += new Vector3(0, 0, -d);
            planes[5].transform.position += new Vector3(d, 0, d);
            planes[6].transform.position += new Vector3(d, 0, -d);
            planes[7].transform.position += new Vector3(-d, 0, d);
            planes[8].transform.position += new Vector3(-d, 0, -d);

            return planes;
        }

        /* Creates a light source */
        GameObject CreateLight()
        {
            GameObject go = new GameObject("");
            Light l = go.AddComponent<Light>();
            l.color = RGBToPercent(new Vector3(250, 54, 2));
            l.type = LightType.Directional;

            Quaternion target = Quaternion.Euler(0, 0, 180);
            go.transform.rotation =
                Quaternion.Slerp(cam.transform.rotation, target, 1);
            l.intensity = 5;
            return go;
        }

        GameObject CreateLight2()
        {
            GameObject go = new GameObject("");
            Light l = go.AddComponent<Light>();
            l.color = RGBToPercent(new Vector3(25, 2, 255));
            l.type = LightType.Directional;
            Quaternion target = Quaternion.Euler(90, 0, 0);
            go.transform.rotation =
                Quaternion.Slerp(cam.transform.rotation, target, 1);
            l.intensity = 0.5f;
            return go;
        }

        /* Creates the lantern */
        GameObject CreateLantern()
        {
            GameObject go = new GameObject("");
            Light l = go.AddComponent<Light>();
            l.color = RGBToPercent(new Vector3(250, 243, 100));
            l.type = LightType.Spot;
            l.range = 150;
            l.intensity = 15;
            l.spotAngle = 25;
            go.transform.position = cam.transform.position;
            go.transform.rotation = cam.transform.rotation;
            go.transform.parent = cam.transform;
            return go;
        }

        /* Generate animation curves */
        void CreateAnimationCurves()
        {
            t0 = 0;
            t1 = 15;
            t2 = 30;
            t3 = 45;
            t4 = 60;
            float p = Mathf.PI;
            float r = 200;
            cx =
                new AnimationCurve(new Keyframe(t0, -150),
                    new Keyframe((t0 + t1) / 2, 100 + r * Mathf.Cos(3 * p / 4)),
                    new Keyframe(t1, 100),
                    new Keyframe((t1 + t2) / 2, 100 + r * Mathf.Cos(p / 4)),
                    new Keyframe(t2, 350),
                    new Keyframe((t2 + t3) / 2, 100 + r * Mathf.Cos(7 * p / 4)),
                    new Keyframe(t3, 100),
                    new Keyframe((t3 + t4) / 2, 100 + r * Mathf.Cos(5 * p / 4)),
                    new Keyframe(t4, -150));
            cz =
                new AnimationCurve(new Keyframe(t0, 100),
                    new Keyframe((t0 + t1) / 2, 100 + r * Mathf.Sin(3 * p / 4)),
                    new Keyframe(t1, 350),
                    new Keyframe((t1 + t2) / 2, 100 + r * Mathf.Sin(p / 4)),
                    new Keyframe(t2, 100),
                    new Keyframe((t2 + t3) / 2, 100 + r * Mathf.Sin(7 * p / 4)),
                    new Keyframe(t3, -150),
                    new Keyframe((t3 + t4) / 2, 100 + r * Mathf.Sin(5 * p / 4)),
                    new Keyframe(t4, 100));
            cry =
                new AnimationCurve(new Keyframe(t0, 0),
                    new Keyframe(t1, 90),
                    new Keyframe(t2, 180),
                    new Keyframe(t3, 270),
                    new Keyframe(t4, 360));
            cx.postWrapMode = WrapMode.Loop;
            cz.postWrapMode = WrapMode.Loop;
            cry.postWrapMode = WrapMode.Loop;
        }

        /* Adjusts the camera to its initial position */
        void AdjustCamera()
        {
            cam = Camera.main;
            camorg = new Vector3(-150, 20, 100);
            cam.transform.position = camorg;
            Quaternion target = Quaternion.Euler(45, 0, 0);
            cam.transform.rotation =
                Quaternion.Slerp(cam.transform.rotation, target, 1);
        }

        /* Animation */
        public void FieldAnimation(float tsc)
        {
            float now = Time.time;
            float localEnvTime = now - tsc;

            cam.transform.position =
                new Vector3(cx.Evaluate(localEnvTime),
                    20,
                    cz.Evaluate(localEnvTime));
            Quaternion target =
                Quaternion.Euler(45, cry.Evaluate(localEnvTime), 0);
            cam.transform.rotation =
                Quaternion
                    .Slerp(cam.transform.rotation,
                    target,
                    CalculateSmoothness(localEnvTime));
        }

        float CalculateSmoothness(float letime)
        {
            float ret = 0;
            if (letime >= t0 && letime < t1)
            {
                ret = 1 - (t1 - letime) / 10;
            }
            else if (letime >= t1 && letime < t2)
            {
                ret = 1 - (t2 - letime) / 10;
            }
            else if (letime >= t2 && letime < t3)
            {
                ret = 1 - (t3 - letime) / 10;
            }
            else if (letime >= t3 && letime < t4)
            {
                ret = 1 - (t4 - letime) / 10;
            }
            if (ret > 1)
            {
                ret = 1;
            }
            return ret;
        }

        /* Change color of the lantern */
        public void ChangeLightColor(int color)
        {
            Light l = lantern.GetComponent<Light>();
            if (color == 1)
            {
                l.color = RGBToPercent(new Vector3(250, 243, 100));
                l.intensity = 15;
                l.spotAngle = 25;
            }
            else if (color == 2)
            {
                l.color = RGBToPercent(new Vector3(250, 250, 250));
                l.intensity = 20;
                l.spotAngle = 45;
            }
            else if (color == 3)
            {
                l.color = RGBToPercent(new Vector3(255, 255, 255));
                l.intensity = 25;
                l.spotAngle = 60;
            }
        }
    }
}
