using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets._Scripts.Managers
{
    public class GUIManager : MonoBehaviour
    {
        private static GUIManager m_Instance = null;
        public static GUIManager Instance => m_Instance;

        public TextMeshProUGUI MatchCountText;

        private void Awake()
        {
            m_Instance = this;
        }

        private void OnDestroy()
        {
            m_Instance = null;
        }
    }
}
