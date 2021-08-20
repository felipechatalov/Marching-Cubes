using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleGridDebug : MonoBehaviour
{
    GameObject DebugTrianglesHolder;
    GameObject Debugging;

    List<GameObject> debugPointList = new List<GameObject>();

    public GameObject pointPF;

    public Material triMat;

    static int width = 2;
    static int height = 2;
    // static int depth = 2;

    int currState;

    bool debugSquareActive = false;

    static int[] a = {0, 2}; // A C
    static int[] b = {0, 1}; // A B
    static int[] c = {2, 3}; // C D
    static int[] d = {1, 3}; // B D
    static int[] e = {0, 4}; // A E
    static int[] f = {1, 5}; // B F
    static int[] g = {4, 6}; // E G
    static int[] h = {5, 7}; // F H
    static int[] i = {6, 7}; // G H
    static int[] j = {4, 5}; // E F
    static int[] k = {2, 6}; // C G
    static int[] l = {3, 7}; // D H
    

    GameObject FindEdgePoint(int baseX, int baseY, int baseZ, int edgeNum){
        // when using this for debugSquare function, you need to set
        // width, height and depth to 2 to work properly!!!!!!!!!!!!!!!
        int zz = 0;
        int yy = 0;
        int xx = 0;
        if (edgeNum >= 4){
            zz = 1;
            edgeNum -= 4;
        }
        if (edgeNum >= 2){
            yy = 1;
            edgeNum -= 2;
        }
        if (edgeNum >= 1){
            xx = 1;
            edgeNum -= 1;
        }
        GameObject a = debugPointList[(baseX + xx) + ((baseY + yy) * width) + ((baseZ + zz) * width * height)];
        return a;
    }

    float VertexPositionInterpolated(float valueB, float valueA){
        // float value =  (0 - valueA) / (valueB - valueA); // interpolation
        // float value =  (valueA + valueB) / 2; //interpolation 2
        float value = 0.5f;
        return value;
    }

    void MarchDebug(int x, int y, int z, int state){
        int[][] triConfig = GetComponent<HandleGrid>().triangulationList[state];
        DebugTrianglesHolder = new GameObject("Debug Triangles holder");
        // Debug.Log(triConfig.Length);
        // Debug.Log(state);
        for (int iter = 0; iter < triConfig.Length/3; iter++){
            GameObject DebugTriangleHolder = new GameObject("Debug Mesh Holder");
            DebugTriangleHolder.transform.parent = DebugTrianglesHolder.transform;
            DebugTriangleHolder.AddComponent<MeshFilter>();
            DebugTriangleHolder.AddComponent<MeshRenderer>();
            DebugTriangleHolder.GetComponent<MeshRenderer>().material = triMat;
            Mesh mesh = DebugTriangleHolder.GetComponent<MeshFilter>().mesh;

            mesh.Clear();
            mesh.vertices = new Vector3[]{
                (FindEdgePoint(x, y, z, triConfig[0+(3*iter)][0]).transform.position + 
                ((FindEdgePoint(x, y, z, triConfig[0+(3*iter)][1]).transform.position - 
                FindEdgePoint(x, y, z, triConfig[0+(3*iter)][0]).transform.position) *
                (float)VertexPositionInterpolated(FindEdgePoint(x, y, z, triConfig[0+(3*iter)][0]).GetComponent<State>().value, FindEdgePoint(x, y, z, triConfig[0+(3*iter)][1]).GetComponent<State>().value))),

                (FindEdgePoint(x, y, z, triConfig[1+(3*iter)][0]).transform.position + 
                ((FindEdgePoint(x, y, z, triConfig[1+(3*iter)][1]).transform.position - 
                FindEdgePoint(x, y, z, triConfig[1+(3*iter)][0]).transform.position) *
                (float)VertexPositionInterpolated(FindEdgePoint(x, y, z, triConfig[1+(3*iter)][0]).GetComponent<State>().value, FindEdgePoint(x, y, z, triConfig[1+(3*iter)][1]).GetComponent<State>().value))),
                
                (FindEdgePoint(x, y, z, triConfig[2+(3*iter)][0]).transform.position + 
                ((FindEdgePoint(x, y, z, triConfig[2+(3*iter)][1]).transform.position - 
                FindEdgePoint(x, y, z, triConfig[2+(3*iter)][0]).transform.position) *
                (float)VertexPositionInterpolated(FindEdgePoint(x, y, z, triConfig[2+(3*iter)][0]).GetComponent<State>().value, FindEdgePoint(x, y, z, triConfig[2+(3*iter)][1]).GetComponent<State>().value))),
 
            };
            mesh.uv = new Vector2[]{
                new Vector2(0, 0),
                new Vector2(0, 1),
                new Vector2(1, 1)
            };
            mesh.triangles = new int[]{
                0, 1, 2
            };
            mesh.RecalculateNormals();
        }
    }

    // void CreateGridDebug(){
    //     int states = 0;
    //     GameObject debugPoints = new GameObject("DebugPoints");
    //     for (int j = 0; j < 16; j++){
    //         for (int i = 0; i < 16; i++){
    //             int toCalulate = states;
    //             float xdisp = (float)0.1*i;
    //             float ydisp = (float)0.1*j;
    //             for (int z = 1; z > -1; z--){
    //                 for (int y = 1; y > -1; y--){
    //                     for (int x = 1; x > -1; x--){
    //                         GameObject p = Instantiate(pointPF, new Vector3((float)((i+xdisp)+x), (float)((j+ydisp)+y), z), Quaternion.identity);
    //                         pointList.Add(p);
    //                         p.transform.parent = debugPoints.transform;
    //                         // states >= Mathf.Pow(2, x+(y*2)+(z*4)) ? p.GetComponent<State>().isOn = true : p.GetComponent<State>().isOn = false;
    //                         if (toCalulate >= Mathf.Pow(2, x+(y*2)+(z*4))){
    //                             p.GetComponent<State>().isOn = true;
    //                             toCalulate -= (int)Mathf.Pow(2, x+(y*2)+(z*4));
    //                         }else{
    //                             p.GetComponent<State>().isOn = false;
    //                         }
    //                         // p.GetComponent<State>().isOn == true ? p.GetComponent<State>().SetColor(new Color(1f, 1f, 1f, 1f)) : p.GetComponent<State>().SetColor(new Color(0f, 0f, 0f, 1f));
    //                         if (p.GetComponent<State>().isOn == true){
    //                             p.GetComponent<State>().SetColor(new Color(1f, 1f, 1f, 1f));
    //                         }else{
    //                             p.GetComponent<State>().SetColor(new Color(0f, 0f, 0f, 1f));
    //                         }
    //                     }
    //                 }
    //             }
    //             // MarchDebug(i+xdisp, j+ydisp, 0, states); 
    //             MarchDebug(i, j, 0, states); 
    //             states++;
    //         }
    //     }
    // }

    // void DebugSquare(){
    //     Debugging = new GameObject("Debugging");
    //     GameObject DebugTrianglesHolder = new GameObject("Debug Triangle Holder");
    //     int[][] vertices = new int[][]{
            
    //     };
    //     for (int z = 0; z < 2; z++){
    //         for (int y = 0; y < 2; y++){
    //             for (int x = 0; x < 2; x++){
    //                 GameObject p = Instantiate(pointPF, new Vector3(x, y, z), Quaternion.identity);
    //                 p.transform.parent = Debugging.transform;
    //             }   
    //         }
    //     }
    //     Vector3 pivotPos = new Vector3(0f, 0f, 0f);
    //     for (int iter = 0; iter < vertices.Length/3; iter++){
    //         GameObject DebugtriangleHolder = new GameObject("Debug Mesh Holder");
    //         DebugTrianglesHolder.transform.parent = Debugging.transform;
    //         DebugtriangleHolder.transform.parent = DebugTrianglesHolder.transform;
    //         DebugtriangleHolder.AddComponent<MeshFilter>();
    //         DebugtriangleHolder.AddComponent<MeshRenderer>();
    //         DebugtriangleHolder.GetComponent<MeshRenderer>().material = triMat;
    //         Mesh mesh = DebugtriangleHolder.GetComponent<MeshFilter>().mesh;

    //         mesh.Clear();
    //         mesh.vertices = new Vector3[]{
    //             new Vector3(pivotPos.x + (vertices[0+(3*iter)][0] * scale),
    //                         pivotPos.y + (vertices[0+(3*iter)][1] * scale),
    //                         pivotPos.z + (vertices[0+(3*iter)][2] * scale)),
    //             new Vector3(pivotPos.x + (vertices[1+(3*iter)][0] * scale),
    //                         pivotPos.y + (vertices[1+(3*iter)][1] * scale),
    //                         pivotPos.z + (vertices[1+(3*iter)][2] * scale)),
    //             new Vector3(pivotPos.x + (vertices[2+(3*iter)][0] * scale),
    //                         pivotPos.y + (vertices[2+(3*iter)][1] * scale),
    //                         pivotPos.z + (vertices[2+(3*iter)][2] * scale)),
    //         };
    //         mesh.uv = new Vector2[]{
    //             new Vector2(0, 0),
    //             new Vector2(0, 1),
    //             new Vector2(1, 1)
    //         };
    //         mesh.triangles = new int[]{
    //             0, 1, 2
    //         };
    //         mesh.RecalculateNormals();
    //     }
    // }
    void debugSquares(){
        if (Input.GetKeyDown(KeyCode.LeftArrow)){
            if (currState <= 255 && currState > 0){
                currState -= 1;
            }
            recalculateDebugSquare(currState);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow)){
            if (currState < 255 && currState >= 0){
                currState += 1;
            }
            recalculateDebugSquare(currState);
        }
    }
    void recalculateDebugSquare(int currState){
        Debug.Log(currState);
        Destroy(DebugTrianglesHolder);
        int toCalc = currState;
        for (int z = 1; z > -1; z--){
            for (int y = 1; y > -1; y--){
                for (int x = 1; x > -1; x--){
                    // GameObject p = Instantiate(pointPF, new Vector3(x, y, z), Quaternion.identity);
                    // p.transform.parent = Debugging.transform;
                    // Debug.Log(x + (y*2) + (z*4));
                    GameObject p = debugPointList[x + (y*2) + (z*4)];
                    if (toCalc >= Mathf.Pow(2, x+(y*2)+(z*4))){
                        p.GetComponent<State>().isOn = true;
                        p.GetComponent<State>().SetColor(new Color(1f, 1f, 1f, 1f));
                        toCalc -= (int)Mathf.Pow(2, x+(y*2)+(z*4));
                    }else{
                        p.GetComponent<State>().isOn = false;
                        p.GetComponent<State>().SetColor(new Color(0f, 0f, 0f, 1f));
                    }
                }   
            }
        }
        MarchDebug(0, 0, 0, currState);
    }
    void CreateDebugBase(){
        currState = 0;
        Debugging = new GameObject("Debugging");
        for (int z = 0; z < 2; z++){
            for (int y = 0; y < 2; y++){
                for (int x = 0; x < 2; x++){
                    GameObject p = Instantiate(pointPF, new Vector3(x, y, z), Quaternion.identity);
                    p.transform.parent = Debugging.transform;
                    p.GetComponent<State>().SetColor(new Color(0f, 0f, 0f, 1f));
                    debugPointList.Add(p);
                }   
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)){
            // debugSquareActive == true? debugSquareActive = false : debugSquareActive = true;
            if (debugSquareActive == true){
                debugSquareActive = false;
            }else debugSquareActive = true;
            CreateDebugBase();
        }
        if (debugSquareActive == true){
            debugSquares();
        }
        if (Input.GetKeyDown(KeyCode.Z)){
            Destroy(Debugging);
        }
    }
}
