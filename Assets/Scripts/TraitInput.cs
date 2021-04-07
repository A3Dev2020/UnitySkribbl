using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class TraitInput
    {
        public float X;
        public float Y;

        public Vector3 GetVector()
        {
            return (new Vector3(X, Y, -0.1f));
        }
    }

    public class StartEndInput
    {
        public bool Start;
        public float X;
        public float Y;
    }
}
