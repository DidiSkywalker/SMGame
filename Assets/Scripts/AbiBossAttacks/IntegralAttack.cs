using System;
using System.Collections.Generic;
using System.Linq;
using AbiBossAttacks.Functions;
using TMPro;
using UnityEngine;
using Utils;

namespace AbiBossAttacks
{
    [CreateAssetMenu(menuName = "AbiBoss Attacks/Integral Attack")]
    public class IntegralAttack : AbiBossAttack
    {
        public Material meshMat;
        public Material lineMat;
        public Function function;
        public float meshRadius;
        public float step;

        private GameObject _gameObject;
        private Mesh _mesh;
        private MeshRenderer _meshRenderer;
        private LineRenderer _graphLineRenderer;
        private LineRenderer _yAxisLineRenderer;
        private TMPBundle _tmpBundle;

        private MeshWrapper _meshWrapper;
        private long _startedAt;
        private State _state;

        // ReSharper disable Unity.PerformanceAnalysis
        protected override void UseImpl(GameObject boss)
        {
            Debug.Log("Use Integral Attack");
            _startedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            Debug.Log($"started at {_startedAt}");
            var position = ZLevelHelper.Between(boss.transform.position, ZLevelHelper.Background, ZLevelHelper.Player);
            InitGameObject(position);
            _state = State.ANNOUNCING;

            _tmpBundle = TMPHelper.CreateTextObject("Function Announcement Text", new TextOptions()
            {
                Position = new Vector3(0, 400, ZLevelHelper.Foreground),
                FontSize = 120,
                FontStyle = FontStyles.Bold,
                Text = "∫ " + function.textRepresentation
            });
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void InitGameObject(Vector3 position)
        {
            _gameObject = new GameObject("IntegralAttack")
            {
                transform =
                {
                    position = position
                }
            };
            _graphLineRenderer = _gameObject.AddComponent<LineRenderer>();
            var meshFilter = _gameObject.AddComponent<MeshFilter>();
            _meshRenderer = _gameObject.AddComponent<MeshRenderer>();
            _mesh = new Mesh();
            meshFilter.mesh = _mesh;

            _graphLineRenderer.startWidth = 0.1f;
            _graphLineRenderer.endWidth = 0.1f;
            _graphLineRenderer.numCornerVertices = 3;
        }

        private void DrawAxis()
        {
            _yAxisLineRenderer = _gameObject.AddComponent<LineRenderer>();
            _graphLineRenderer.startWidth = 0.1f;
            _graphLineRenderer.endWidth = 0.1f;

            var points = new List<Vector3>();
            for (int i = 0; i < 10; i++)
            {
                
            }
            
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public override void Update()
        {
            _graphLineRenderer.material = lineMat;
            _meshRenderer.material = meshMat;
            if (_startedAt > 0 && _startedAt + 1000 < DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() )
            {
                Debug.Log($"Current time: {DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}");
                _startedAt = -1;
                DrawGraph();
            }

            if (_startedAt < 0)
            {
                if (_meshWrapper.Points[^1].x < meshRadius && Time.frameCount % 5 == 0)
                {
                    DrawGraphSection(_meshWrapper);
                }
            }
        }

        public override void Destroy()
        {
            Destroy(_gameObject);
            Destroy(_tmpBundle.GameObject);
        }

        private MeshWrapper DrawGraph()
        {
            Debug.Log("DrawGraph");
            _meshWrapper = new MeshWrapper()
            {
                Points = new List<Vector3>(),
                Vertices = new List<Vector3>(),
                Triangles = new List<int>()
            };
            var x = -meshRadius;
            var y = function.Apply(x);
            var localPoint = new Vector3(x, y, 0);
            _meshWrapper.Vertices.Add(localPoint);
            _meshWrapper.Vertices.Add(new Vector3(x, -meshRadius, 0));
            _meshWrapper.Points.Add(localPoint + _gameObject.transform.position + Vector3.back * .1f);

            return _meshWrapper;
        }
        
        private void DrawGraphSection(MeshWrapper meshWrapper)
        {
            var x = meshWrapper.Points[^1].x + step;
            var y = function.Apply(x);
            var localPoint = new Vector3(x, y, 0);
            var newVertex = meshWrapper.Vertices.Count;
            meshWrapper.Vertices.Add(localPoint);
            meshWrapper.Vertices.Add(new Vector3(x, -meshRadius, 0));
            meshWrapper.Triangles.AddRange(new []{ newVertex - 2, newVertex, newVertex - 1});
            meshWrapper.Triangles.AddRange(new []{ newVertex - 1, newVertex, newVertex + 1});
            meshWrapper.Points.Add(localPoint + _gameObject.transform.position + Vector3.back * .1f);
            UpdateMeshAndLine(meshWrapper);
        }

        private void UpdateMeshAndLine(MeshWrapper result)
        {
            _mesh.vertices = result.Vertices.ToArray();
            _mesh.triangles = result.Triangles.ToArray();
            
            _graphLineRenderer.positionCount = result.Points.Count;
            _graphLineRenderer.SetPositions(result.Points.ToArray());
        }

        private static MeshWrapper PlotGraph(Vector3 origin, int radius, float step, Func<float, float> function)
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

            return new MeshWrapper()
            {
                Points = points,
                Vertices = vertices,
                Triangles = triangles
            };
        }

    }

    internal enum State
    {
        ANNOUNCING,
        DRAWING
    }
    
    internal struct MeshWrapper
    {
        public List<Vector3> Points;
        public List<Vector3> Vertices;
        public List<int> Triangles;
    }
}