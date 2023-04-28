using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Utils;

public class MeshTest : MonoBehaviour
{
    public Material meshMat;
    public Material lineMat;
    private MeshRenderer _meshRenderer;
    private LineRenderer _lineRenderer;
    
    private void Start()
    {
        _lineRenderer = gameObject.AddComponent<LineRenderer>();
        var meshFilter = gameObject.AddComponent<MeshFilter>();
        _meshRenderer = gameObject.AddComponent<MeshRenderer>();
        var mesh = new Mesh();
        meshFilter.mesh = mesh;

        _lineRenderer.startWidth = 0.1f;
        _lineRenderer.endWidth = 0.1f;
        _lineRenderer.numCornerVertices = 3;
        
        var result = PlotGraph(transform.position + Vector3.back, 10, .1f, XSquared);
        
        mesh.vertices = result.vertices;
        mesh.triangles = result.triangles;
        
        _lineRenderer.positionCount = result.points.Length;
        _lineRenderer.SetPositions(result.points);
    }

    private static float XSquared(float x)
    {
        // return .5f * x * x;
        return Mathf.Sin(x);
    }

    private void Update()
    {
        _lineRenderer.material = lineMat;
        _meshRenderer.material = meshMat;
    }

    private static GraphResult PlotGraph(Vector3 origin, int radius, float step, Func<float, float> function)
    {
        var vertices = new List<Vector3>();
        vertices.Add(new Vector3(-radius, -radius, 0)); // 0 - bottom left
        vertices.Add(new Vector3(radius, -radius, 0)); // 1 - bottom right
        var triangles = new List<int>();
        var points = new List<Vector3>();

        for (float x = -radius; x <= radius; x += step)
        {
            Debug.Log(x);
            var y = function.Invoke(x);
            if (y < -radius) y = -radius;
            if (y > radius) y = radius;
            var localPoint = new Vector3(x, y, 0);
            var newVertex = vertices.Count;
            Debug.Log($"Added vertex #{newVertex}");
            vertices.Add(localPoint);
            if (x < 0)
            {
                var newTriangle = new[] { 0, newVertex, newVertex + 1 };
                Debug.Log($"New triangle: {newTriangle.ToCommaSeparatedString()}");
                triangles.AddRange(newTriangle);
            }

            if (MathUtils.CompareAlmostEqual(x, 0f, 0.01f))
            {
                Debug.Log("ZERO");
                var newTriangle = new[] { 0, newVertex, 1 };
                Debug.Log($"New triangle: {newTriangle.ToCommaSeparatedString()}");
                triangles.AddRange(newTriangle);
            }

            if (x > 0)
            {
                var newTriangle = new[] { 1, newVertex - 1, newVertex };
                Debug.Log($"New triangle: {newTriangle.ToCommaSeparatedString()}");
                triangles.AddRange(newTriangle);
            }

            var worldPoint = localPoint + origin;
            points.Add(worldPoint);
        }

        return new GraphResult()
        {
            points = points.ToArray(),
            vertices = vertices.ToArray(),
            triangles = triangles.ToArray()
        };
    }

    class GraphResult
    {
        public Vector3[] points;
        public Vector3[] vertices;
        public int[] triangles;
    }
}