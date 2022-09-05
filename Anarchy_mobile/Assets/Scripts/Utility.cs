using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AnarchyUtility
{ 
    public class Utility
    {
        // SetActive Functions
        #region
        public void SetActive(Image image, bool b)
        {
            image.gameObject.SetActive(b);
        }

        public void SetActive(Text text, bool b)
        {
            text.gameObject.SetActive(b);
        }

        public void SetActive(Button button, bool b)
        {
            button.gameObject.SetActive(b);
        }

        public void SetActive(GameObject go, bool b)
        {
            go.SetActive(b);
        }

        public void SetActive(ParticleSystem ps, bool b)
        {
            ps.gameObject.SetActive(b);
        }
        #endregion

        public bool CheckCost(int cost, int money)
        {
            if (cost > money)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int CalculateCost(int cost, int money)
        {
            return money - cost;
        }

        public void SetManager(ref UIManager UI, ref CentralProcessor CP, ref VariableManager VM)
        {
            UI = CentralProcessor.Instance.uIManager;
            CP = CentralProcessor.Instance;
            VM = VariableManager.Instance;
        }

        public void SetManager(ref UIManager UI, ref CentralProcessor CP)
        {
            UI = CentralProcessor.Instance.uIManager;
            CP = CentralProcessor.Instance;
        }
    }
}