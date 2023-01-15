using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets._Scripts
{
    public class TileBehaviour : MonoBehaviour
    {
        [SerializeField]
        private TextMeshPro m_Text;

        public int PosX;
        public int PosY;
        public float Size;

        private bool m_IsAvailable;
        public bool IsAvailable
        {
            get => m_IsAvailable;
            set
            {
                m_IsAvailable = value;
                m_Text.SetText(m_IsAvailable ? "" : "X");
            }
        }

        public void Init(int posX, int posY, float size)
        {
            PosX = posX;
            PosY = posY;
            Size = size;

            IsAvailable = true;
        }
    }
}
