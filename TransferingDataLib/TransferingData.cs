using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransferingDataLib
{
    [Serializable]
    public struct GameObject
    {
        public float X;
        public float Y;

        public float VelosityX;
        public float VelosityY;

        public float CurrentSpeed;

        public float angle;
    }

    [Serializable]
    public class TransferingData : EventArgs
    {
        public List<GameObject> PlayerParts;

        public TransferingData()
        {
            PlayerParts = new List<GameObject>();
        }
    }
}
