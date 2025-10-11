using UnityEngine;

namespace SerapKeremGameKit._Tile
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] private Vector2Int _gridPosition;
        public Vector2Int GridPosition { get { return _gridPosition; } private set { _gridPosition = value; } }

        public void Initialize(Vector2Int gridPos)
        {
            _gridPosition = gridPos;
        }
    }
}


