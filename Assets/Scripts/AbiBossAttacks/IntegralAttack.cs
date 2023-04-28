using System;
using System.Collections.Generic;
using AbiBossAttacks.Functions;
using DefaultNamespace;
using TMPro;
using UnityEngine;

namespace AbiBossAttacks
{
    [CreateAssetMenu(menuName = "AbiBoss Attacks/Integral Attack")]
    public class IntegralAttack : AbiBossAttack
    {
        public Material meshMat;
        public Material lineMat;
        public Function function;

        private GameObject _gameObject;
        private Mesh _mesh;
        private MeshRenderer _meshRenderer;
        private LineRenderer _lineRenderer;

        private State _state;

        protected override void UseImpl(GameObject boss)
        {
            Debug.Log("Use Integral Attack");
            var bossPosition = boss.transform.position;
            InitGameObject(bossPosition + Vector3.forward);
            _state = State.ANNOUNCING;

            var text = new GameObject("Function Announcement Text")
            {
                transform =
                {
                    position = bossPosition
                }
            };
            var tmp = text.AddComponent<TextMeshProUGUI>();
            text.transform.SetParent(GlobalCanvas.CanvasGameObject.transform, false);
            tmp.fontSize = 120;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.fontStyle = FontStyles.Bold;
            tmp.text = "∫ " + function.textRepresentation;
            text.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 10000f);
            text.transform.localScale = new Vector3(1, 1, 1);
            text.transform.localPosition = new Vector3(0, 400, -10);

            var result = PlotGraph(_gameObject.transform.position + Vector3.back * .1f, 10, .1f, function.Apply);
            
            _mesh.vertices = result.Vertices;
            _mesh.triangles = result.Triangles;
            
            _lineRenderer.positionCount = result.Points.Length;
            _lineRenderer.SetPositions(result.Points);
        }

        private void InitGameObject(Vector3 position)
        {
            _gameObject = new GameObject("IntegralAttack")
            {
                transform =
                {
                    position = position
                }
            };
            _lineRenderer = _gameObject.AddComponent<LineRenderer>();
            var meshFilter = _gameObject.AddComponent<MeshFilter>();
            _meshRenderer = _gameObject.AddComponent<MeshRenderer>();
            _mesh = new Mesh();
            meshFilter.mesh = _mesh;

            _lineRenderer.startWidth = 0.1f;
            _lineRenderer.endWidth = 0.1f;
            _lineRenderer.numCornerVertices = 3;
        }

        public override void Update()
        {
            _lineRenderer.material = lineMat;
            _meshRenderer.material = meshMat;
        }

        private static GraphResult PlotGraph(Vector3 origin, int radius, float step, Func<float, float> function)
        {
            var vertices = new List<Vector3>
            {
                new(-radius, -radius, 0), // 0 - bottom left
                new(radius, -radius, 0) // 1 - bottom right
            };
            var triangles = new List<int>();
            var points = new List<Vector3>();

            for (float x = -radius; x <= radius; x += step)
            {
                var y = function.Invoke(x);
                if (y < -radius) y = -radius;
                if (y > radius) y = radius;
                var localPoint = new Vector3(x, y, 0);
                var newVertex = vertices.Count;
                vertices.Add(localPoint);
                if (x < 0)
                {
                    var newTriangle = new[] { 0, newVertex, newVertex + 1 };
                    triangles.AddRange(newTriangle);
                }

                if (MathUtils.CompareAlmostEqual(x, 0f, 0.01f))
                {
                    var newTriangle = new[] { 0, newVertex, 1 };
                    triangles.AddRange(newTriangle);
                }

                if (x > 0)
                {
                    var newTriangle = new[] { 1, newVertex - 1, newVertex };
                    triangles.AddRange(newTriangle);
                }

                var worldPoint = localPoint + origin;
                points.Add(worldPoint);
            }

            return new GraphResult()
            {
                Points = points.ToArray(),
                Vertices = vertices.ToArray(),
                Triangles = triangles.ToArray()
            };
        }

    }

    internal enum State
    {
        ANNOUNCING,
        DRAWING
    }
    
    internal struct GraphResult
    {
        public Vector3[] Points;
        public Vector3[] Vertices;
        public int[] Triangles;
    }
}