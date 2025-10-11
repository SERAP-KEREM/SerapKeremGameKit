using SerapKeremGameKit._Logging;
using SerapKeremGameKit._Singletons;
using UnityEditor;
using UnityEngine;
using Array2DEditor;
using TriInspector;

namespace SerapKeremGameKit._Tile
{
    public sealed class TileManager : MonoSingleton<TileManager>
    {
        [Header("Grid Settings")]
        [SerializeField] private GameObject _parentObject;
        [SerializeField, Range(0.1f, 100f)] private float _distance = 1f;
        [Title("Grid Settings"), PropertyOrder(2)]
        [SerializeField] private Array2DInt _tileSizeArray; // Drives grid size/content via Array2DEditor

        // Prefab is taken from TileSpawner; no need to duplicate here

#if UNITY_EDITOR
        [Button("Build Grid (Editor)")]
        [ContextMenu("Editor/Build Grid")]
        [DisableInPlayMode]
        private void EditorBuildGrid()
        {
            if (Application.isPlaying) return;
            // Find spawner also in edit mode (Awake not called yet)
            TileSpawner spawner = TileSpawner.IsInitialized
                ? TileSpawner.Instance
                : UnityEngine.Object.FindFirstObjectByType<TileSpawner>(FindObjectsInactive.Include);
            if (spawner == null)
            {
                TraceLogger.LogError("TileSpawner is missing in the scene.", this);
                return;
            }

            if (_tileSizeArray == null)
            {
                TraceLogger.LogError("Array2DInt reference is missing.", this);
                return;
            }

            Transform parent = _parentObject != null ? _parentObject.transform : transform;

            ClearEditor();

            var cells = _tileSizeArray.GetCells(); // int[,]
            int width = cells.GetLength(0);
            int height = cells.GetLength(1);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int v = cells[x, y];
                    if (v <= 0) continue; // 0 => empty, >0 => place tile

                    Vector3 pos = new Vector3(x * _distance, y * _distance, 0f) + parent.position;
                    Tile tile = spawner.SpawnTileInEditor(pos, parent);
                    if (tile != null)
                    {
                        tile.Initialize(new Vector2Int(x, y));
                        tile.name = $"Tile [{x},{y}]";
                        Undo.RegisterCreatedObjectUndo(tile.gameObject, "Create Tile");
                        EditorUtility.SetDirty(tile);
                    }
                }
            }
        }

        [Button("Clear Grid (Editor)")]
        [ContextMenu("Editor/Clear Grid")]
        [DisableInPlayMode]
        private void ClearEditor()
        {
            if (Application.isPlaying) return;
            Transform root = _parentObject == null ? transform : _parentObject.transform;
            for (int i = root.childCount - 1; i >= 0; i--)
            {
                GameObject go = root.GetChild(i).gameObject;
                Undo.DestroyObjectImmediate(go);
            }
            EditorUtility.SetDirty(gameObject);
        }
#endif
    }
}


