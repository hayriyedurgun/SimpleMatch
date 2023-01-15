using Assets._Scripts;
using Assets._Scripts.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Scripts
{
    public class GridInputController : MonoBehaviour
    {
        private RaycastHit m_Hit;

        public Action<Vector3> Clicked;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                var result = Physics.Raycast(ray, out m_Hit, int.MaxValue, LayerHelper.Or(Layer.Tile));
                if (result)
                {
                    OnClicked();
                }
            }
        }

        private void OnClicked()
        {
            Clicked?.Invoke(m_Hit.point);
        }
    }
}
