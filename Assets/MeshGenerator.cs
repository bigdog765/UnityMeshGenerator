using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using DataStructures.ViliWonka.KDTree;
using Random = UnityEngine.Random;


public class MeshGenerator : MonoBehaviour
{
    private Mesh mesh;
    public GameObject pointPrefab; // The prefab of the sphere
    
    private const int VERTICES = 300;
    private int[] triangles = new int[VERTICES*3];
    private KDTree tree;
    private KDQuery query;
    private float maxSpace = 1f;
    
    private Vector3[] points = new Vector3[VERTICES];
    // Start is called before the first frame update
    void Start()
    {
        
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        generatePoints();
        updateMesh();
    }

    void generatePoints()
    {
        for(int i = 0; i < VERTICES; i++) {
            float[] rCoords = getRandomCoords();
            points[i] = new Vector3(rCoords[0],rCoords[1],rCoords[2]);
            Instantiate(pointPrefab, points[i], Quaternion.identity);
        }
    }

    float[] getRandomCoords()
    {
        //make sure these random numbers distance formula is within a certain range(0.7-0.9)
        while (true)
        {
            
            float x = Mathf.Round(Random.Range(-.9f, .9f) * 100f) / 100f;
            float y = Mathf.Round(Random.Range(-.9f, .9f) * 100f) / 100f;
            float z = Mathf.Round(Random.Range(-.9f, .9f) * 100f) / 100f;
            float answer = Mathf.Sqrt(x * x + y * y + z * z);
            if (answer >= 0.75f && answer <= .8f)
            {
                float[] ansArray = { x, y, z };
                return ansArray;
            }
        }
    }
    void updateMesh()
    {
        mesh.Clear();
        mesh.vertices = points;
        for (int i = 0; i < VERTICES*3; i+=3)
        {
            triangles[i] = i/3;
            triangles[i + 1] = GetClosestNeighbor(points, points[i/3],false);
            triangles[i + 2] = GetClosestNeighbor(points, points[i/3],true);
            //Check if mesh if facing outwards(away from origin)
            Vector3 normal = calcNormal(points[triangles[i]], points[triangles[i+1]], points[triangles[i+2]]);
            Vector3 centroid = (points[triangles[i]] + points[triangles[i + 1]] + points[triangles[i + 2]]) / 3f;
            //if centroid vector & normal vector are facing away from each other = mesh needs to be flipped
            if (Vector3.Dot(normal, centroid) < 0)
            {
                Debug.Log("Flipping mesh");
                //swap position of vertex 1 & 3
                (triangles[i], triangles[i + 2]) = (triangles[i + 2], triangles[i]);
            }
            mesh.triangles = triangles;
        }
    }

    int GetClosestNeighbor(Vector3[] points,Vector3 measuredPoint, bool second)
    {
        tree= new KDTree(points, 32);
        query = new KDQuery();
        List<int> results = new List<int>();
        // closest point query
        query.KNearest(tree, measuredPoint,3, results);

        if (second)
            return results[1];
        return results[0];
    }

    Vector3 calcNormal(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        Vector3 v1 = p2 - p1;
        Vector3 v2 = p3 - p1;
        return Vector3.Cross(v1, v2).normalized;
    }
}
