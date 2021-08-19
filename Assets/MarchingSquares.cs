using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarchingSquares : MonoBehaviour
{
    
    GameObject borderTrianglesHolder;
    // GameObject borderMeshHolder;
    static Material triMat;
    //    c
    //  C - D
    //d [   ] b
    //  A - B
    //    a
    //    6
    //  2 - 3
    //7 [   ] 5
    //  0 - 1
    //    4
    static int[] a = {4}; //A B
    static int[] b = {5}; //B D
    static int[] c = {6}; //C D
    static int[] d = {7}; //A C
    static int[][] sqrComb = new int[][]{
        new int[]{},
        new int[]{7, 4, 0},
        new int[]{4, 5, 1},
        new int[]{0, 7, 5, 5, 1, 0},
        new int[]{7, 2, 6},
        new int[]{2, 4, 0, 2, 6, 4},
        new int[]{4, 5, 1, 7, 2, 6},
        new int[]{7, 2, 6, 7, 6, 5, 0, 7, 5, 0, 5, 1},
        new int[]{6, 3, 5},
        new int[]{7, 4, 0, 6, 3, 5},
        new int[]{4, 6, 3, 4, 3, 1},
        new int[]{7, 6, 5, 6, 3, 5, 0, 7, 5, 0, 5, 1},
        new int[]{7, 2, 3, 7, 3, 5},
        new int[]{7, 2, 3, 7, 3, 5, 4, 7, 5, 7, 4, 0},
        new int[]{7, 2, 3, 7, 3, 5, 7, 5, 4, 4, 5, 1},
        new int[]{2, 1, 0, 2, 3, 1}
    };
    static int[][] sqrComb1 = new int[][]{
        new int[]{},
        new int[]{6, 4, 0},
        new int[]{4, 5, 1},
        new int[]{0, 6, 5, 5, 1, 0},
        new int[]{6, 2, 7},
        new int[]{2, 4, 0, 2, 7, 4},
        new int[]{4, 5, 1, 6, 2, 7},
        new int[]{6, 2, 7, 6, 7, 5, 0, 6, 5, 0, 5, 1},
        new int[]{7, 3, 5},
        new int[]{6, 4, 0, 7, 3, 5},
        new int[]{4, 7, 3, 4, 3, 1},
        new int[]{6, 7, 5, 7, 3, 5, 0, 6, 5, 0, 5, 1},
        new int[]{6, 2, 3, 6, 3, 5},
        new int[]{6, 2, 3, 6, 3, 5, 4, 6, 5, 6, 4, 0},
        new int[]{6, 2, 3, 6, 3, 5, 6, 5, 4, 4, 5, 1},
        new int[]{2, 1, 0, 2, 3, 1}
    };
    void Start()
    {
        triMat = Resources.Load<Material>("Triangle Material");
    }

    float interpolation(float valueA, float valueB){
        Debug.Log((-valueA)/(valueB - valueA) + " " + valueA + " " + valueB);
        if (valueA >= 0 && valueB >= 0 || valueA <= 0 && valueB <= 0){
            return (0.5f);
        }
        if (valueA > 0){
            valueA *= -1;
        }
        if (valueB < 0){
            valueB *= -1;
        }
        return((0-valueA)/(valueB - valueA));
        
    }

    //3--4
    //[  ]
    //1--2
    public void MarchSquare(GameObject p1, GameObject p2, GameObject p3, GameObject p4){
        Vector3[] lookup = new Vector3[]{
            p1.transform.position,
            p2.transform.position,
            p3.transform.position,
            p4.transform.position,
            ((p2.transform.position - p1.transform.position)) * interpolation(p1.GetComponent<State>().value, p2.GetComponent<State>().value),
            ((p4.transform.position - p2.transform.position)) * interpolation(p2.GetComponent<State>().value, p4.GetComponent<State>().value),
            ((p1.transform.position - p3.transform.position)) * interpolation(p3.GetComponent<State>().value, p1.GetComponent<State>().value),
            ((p3.transform.position - p4.transform.position)) * interpolation(p4.GetComponent<State>().value, p3.GetComponent<State>().value)
        };
        // if config[0+(3*iter) > 3{
        //      lookup[config[0+(3*iter)]] + lookup[config[0+(3*iter)]-4] <- posicao do vertice do triangulo    
        //}
        int[] config = sqrComb1[(p1.GetComponent<State>().isOn == true? 1 : 0) +
                               (p2.GetComponent<State>().isOn == true? 1*2 : 0) +
                               (p3.GetComponent<State>().isOn == true? 1*4 : 0) +
                               (p4.GetComponent<State>().isOn == true? 1*8 : 0)];
        Debug.Log((p1.GetComponent<State>().isOn == true? 1 : 0) +
                               (p2.GetComponent<State>().isOn == true? 1*2 : 0) +
                               (p3.GetComponent<State>().isOn == true? 1*4 : 0) +
                               (p4.GetComponent<State>().isOn == true? 1*8 : 0));
        for (int iter = 0; iter < config.Length/3; iter++){
            Vector3[] edgePoints = new Vector3[]{
                lookup[config[0+(3*iter)]],
                lookup[config[1+(3*iter)]],
                lookup[config[2+(3*iter)]]
            };
            for (int i = 0; i < 3; i++){
                if (config[i+(3*iter)] > 3){
                    edgePoints[i] = lookup[config[i+(3*iter)]] + lookup[config[i+(3*iter)]-4];
                }    
                // if (config[i+(3*iter)] == 7){
                //     edgePoints[i] = (p1.transform.position - p3.transform.position)/2 + p3.transform.position;
                // }

            }
            CreateTriangle(edgePoints[0], edgePoints[1], edgePoints[2], ((p1.GetComponent<State>().isOn == true? 1 : 0) +
                               (p2.GetComponent<State>().isOn == true? 1*2 : 0) +
                               (p3.GetComponent<State>().isOn == true? 1*4 : 0) +
                               (p4.GetComponent<State>().isOn == true? 1*8 : 0)));
        }
    }

    public void CreateBorders(int width, int height, int depth, List<GameObject> pointList){
        borderTrianglesHolder = new GameObject("Border Triangles Holder");

        int z = 0;
        int x = 0;
        int y = 0;
        for (y = 0; y < height-1; y++){
            for (x = 0; x < width-1; x++){
                MarchSquare(pointList[x + (y*width) + (z*width*height)],
                            pointList[(x+1) + (y*width) + (z*width*height)],
                            pointList[x + ((y+1)*width) + (z*width*height)],
                            pointList[(x+1) + ((y+1)*width) + (z*width*height)]);
            }
        }
        z = depth-1;
        for (y = 0; y < height-1; y++){
            for (x = width-1; x > 0; x--){
                MarchSquare(pointList[x + (y*width) + (z*width*height)],
                            pointList[(x-1) + (y*width) + (z*width*height)],
                            pointList[x + ((y+1)*width) + (z*width*height)],
                            pointList[(x-1) + ((y+1)*width) + (z*width*height)]);
            }
        }
        x = 0;
        for (y = 0; y < height-1; y++){
            for (z = depth-1; z > 0; z--){
                MarchSquare(pointList[x + (y*width) + (z*width*height)],
                            pointList[x + (y*width) + ((z-1)*width*height)],
                            pointList[x + ((y+1)*width) + (z*width*height)],
                            pointList[x + ((y+1)*width) + ((z-1)*width*height)]);
            }
        }
        x = width-1;
        for (y = 0; y < height-1; y++){
            for (z = 0; z < depth-1; z++){
                MarchSquare(pointList[x + (y*width) + (z*width*height)],
                            pointList[x + (y*width) + ((z+1)*width*height)],
                            pointList[x + ((y+1)*width) + (z*width*height)],
                            pointList[x + ((y+1)*width) + ((z+1)*width*height)]);
            }
        }
        y = 0;
        for (x = 0; x < width-1; x++){
            for ( z = depth-1; z > 0; z--){
                MarchSquare(pointList[x + (y*width) + (z*width*height)],
                            pointList[(x+1) + (y*width) + (z*width*height)],
                            pointList[x + (y*width) + ((z-1)*width*height)],
                            pointList[(x+1) + (y*width) + ((z-1)*width*height)]);
            }
        }
        y = height-1;
        for (x = 0; x < width-1; x++){
            for (z = 0; z < depth-1; z++){
                MarchSquare(pointList[x + (y*width) + (z*width*height)],
                            pointList[(x+1) + (y*width) + (z*width*height)],
                            pointList[x + (y*width) + ((z+1)*width*height)],
                            pointList[(x+1) + (y*width) + ((z+1)*width*height)]);
            }
        }
    }

    void CreateTriangle(Vector3 edge1, Vector3 edge2, Vector3 edge3, int state){
        GameObject borderMeshHolder = new GameObject("Border Mesh Holder");
        borderTrianglesHolder = GameObject.Find("Border Triangles Holder");
        borderMeshHolder.transform.parent = borderTrianglesHolder.transform;
        
        borderMeshHolder.AddComponent<State>();
        borderMeshHolder.GetComponent<State>().value = state;

        borderMeshHolder.AddComponent<MeshFilter>();
        borderMeshHolder.AddComponent<MeshRenderer>();
        borderMeshHolder.GetComponent<MeshRenderer>().material = triMat;
        Mesh mesh = borderMeshHolder.GetComponent<MeshFilter>().mesh;

        mesh.Clear();
        mesh.vertices = new Vector3[]{
            edge1,
            edge2,
            edge3
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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z)){
            borderTrianglesHolder = GameObject.Find("Border Triangles Holder");
            Destroy(borderTrianglesHolder);
        }
    }
}
