using Assets._Scripts.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Scripts
{
    public class GridController : MonoBehaviour
    {
        private TileBehaviour[,] m_Tiles;
        private float m_TileSize;
        private GridInputController m_InputController;
        private Vector3 m_Offset;

        [SerializeField]
        private int m_Size;
        [SerializeField]
        private TileBehaviour m_TilePrefab;
        [SerializeField]
        private WindowController m_WindowController;


        private void Start()
        {
            if (m_WindowController)
            {
                m_WindowController.WindowResized += OnWindowResized;
            }

            if (m_InputController)
            {
                m_InputController.Clicked += OnClicked;
            }

            CalculateTileSize();
            CreateTiles();
        }

        private void OnDestroy()
        {
            if (m_WindowController)
            {
                m_WindowController.WindowResized -= OnWindowResized;
            }

            if (m_InputController)
            {
                m_InputController.Clicked -= OnClicked;
            }

        }

        private void OnWindowResized()
        {
            //scale tiles depends on screen size.
            CalculateTileSize();

            transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);
            m_Offset = new Vector3(((m_TileSize * m_Size) / -2) + (m_TileSize / 2), ((m_TileSize * m_Size) / -2) + (m_TileSize / 2), 0);

            var scaleSize = Mathf.Max(1, m_TileSize - 1);
            TileBehaviour tile;

            for (int x = 0; x < m_Size; x++)
            {
                for (int y = 0; y < m_Size; y++)
                {
                    tile = m_Tiles[x, y];
                    tile.transform.localScale = new Vector3(scaleSize, scaleSize, 0.1f);
                    tile.transform.localPosition = new Vector3(x * m_TileSize, y * m_TileSize, 0) + m_Offset;
                    tile.Init(x, y, m_TileSize);
                    m_Tiles[x, y] = tile;
                }
            }
        }

        private void CalculateTileSize()
        {
            var min = Mathf.Min(Screen.width, Screen.height);
            m_TileSize = Mathf.FloorToInt(min / m_Size);
        }

        private void CreateTiles()
        {
            transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);
            m_Offset = new Vector3(((m_TileSize * m_Size) / -2) + (m_TileSize / 2) , ((m_TileSize * m_Size) / -2) + (m_TileSize / 2), 0);
            m_Tiles = new TileBehaviour[m_Size, m_Size];
            TileBehaviour tile;
            var scaleSize = Mathf.Max(1, m_TileSize - 1);

            for (int x = 0; x < m_Size; x++)
            {
                for (int y = 0; y < m_Size; y++)
                {
                    tile = Instantiate(m_TilePrefab, transform);
                    tile.transform.localScale = new Vector3(scaleSize, scaleSize, 0.1f);
                    tile.transform.localPosition = new Vector3(x * m_TileSize, y * m_TileSize, 0) + m_Offset;
                    tile.Init(x, y, m_TileSize);
                    m_Tiles[x, y] = tile;
                }
            }
        }

        private void OnClicked(Vector3 wordPos)
        {
            //var pos = wordPos - Offset;
            var pos = wordPos;

            var x = Mathf.RoundToInt(pos.x / m_Size);
            var y = Mathf.RoundToInt(pos.y / m_Size);

            if (m_Tiles.GetLength(0) < x + 1 ||
                m_Tiles.GetLength(1) < y + 1 ||
                x < 0 ||
                y < 0)
            {
                return;
            }

            var tile = m_Tiles[x, y];
            tile.IsAvailable = true;

            CheckMatch(tile);
        }

        private void CheckMatch(TileBehaviour tile)
        {
        }
    }
}
