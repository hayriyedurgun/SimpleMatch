﻿using Assets._Scripts.Utils;
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
        private Vector3 m_Offset;

        [SerializeField]
        private int m_Size;
        [SerializeField]
        private TileBehaviour m_TilePrefab;
        [SerializeField]
        private WindowController m_WindowController;
        [SerializeField]
        private GridInputController m_InputController;

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

            m_Offset = new Vector3(((m_TileSize * m_Size) / -2) + (m_TileSize / 2), ((m_TileSize * m_Size) / -2) + (m_TileSize / 2), 0);

            var scaleSize = m_TileSize * 0.95f;

            TileBehaviour tile;

            for (int x = 0; x < m_Size; x++)
            {
                for (int y = 0; y < m_Size; y++)
                {
                    tile = m_Tiles[x, y];
                    tile.transform.localScale = new Vector3(scaleSize, scaleSize, 0.1f);
                    tile.transform.localPosition = new Vector3(x * m_TileSize, y * m_TileSize, 0) + m_Offset;
                }
            }
        }

        private void CalculateTileSize()
        {
            if (!Camera.main) return;
            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Vector3.zero);
            Physics.Raycast(ray, out hit, int.MaxValue, LayerHelper.Or(Layer.HitPlane));
            var bottomLeft = hit.point;

            ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width, Screen.height, 0));
            Physics.Raycast(ray, out hit, int.MaxValue, LayerHelper.Or(Layer.HitPlane));
            var upperRight = hit.point;

            var diff = upperRight - bottomLeft;
            var min = Mathf.Min(diff.x, diff.y);
            m_TileSize = min / m_Size;
        }

        private void CreateTiles()
        {
            m_Offset = new Vector3(((m_TileSize * m_Size) / -2) + (m_TileSize / 2), ((m_TileSize * m_Size) / -2) + (m_TileSize / 2), 0);
            m_Tiles = new TileBehaviour[m_Size, m_Size];
            TileBehaviour tile;
            var scaleSize = m_TileSize * .95f;

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
            var pos = wordPos - m_Offset;

            var x = Mathf.RoundToInt((pos.x / m_TileSize));
            var y = Mathf.RoundToInt((pos.y / m_TileSize));

            if (m_Tiles.GetLength(0) < x + 1 ||
                m_Tiles.GetLength(1) < y + 1 ||
                x < 0 ||
                y < 0)
            {
                return;
            }

            var tile = m_Tiles[x, y];
            tile.IsAvailable = false;

            CheckMatch(tile);
        }

        private void CheckMatch(TileBehaviour tile)
        {
            var x = tile.PosX;
            var y = tile.PosY;

            var leftmost = Mathf.Max(0, x - 2);
            var rightMost = Mathf.Min(m_Tiles.GetLength(0), x + 2);

            var topMost = Mathf.Min(m_Tiles.GetLength(1), y + 2);
            var bottomMost = Mathf.Max(0, y - 2);

            var matchFound = false;

            if (leftmost >= 0)
            {
                for (int i = leftmost; i <= rightMost; i++)
                {
                    if (i + 2 < m_Tiles.GetLength(0) &&
                        !m_Tiles[i, y].IsAvailable &&
                        !m_Tiles[i + 1, y].IsAvailable &&
                        !m_Tiles[i + 2, y].IsAvailable)
                    {
                        m_Tiles[i, y].IsAvailable = true;
                        m_Tiles[i + 1, y].IsAvailable = true;
                        m_Tiles[i + 2, y].IsAvailable = true;
                        matchFound = true;

                        break;
                    }
                }
            }

            if (bottomMost >= 0 &&
                !matchFound)
            {
                for (int i = bottomMost; i <= topMost; i++)
                {
                    if (i + 2 < m_Tiles.GetLength(1) &&
                        !m_Tiles[x, i].IsAvailable &&
                        !m_Tiles[x, i + 1].IsAvailable &&
                        !m_Tiles[x, i + 2].IsAvailable)
                    {
                        m_Tiles[x, i].IsAvailable = true;
                        m_Tiles[x, i + 1].IsAvailable = true;
                        m_Tiles[x, i + 2].IsAvailable = true;
                        break;
                    }
                }
            }
        }
    }
}
