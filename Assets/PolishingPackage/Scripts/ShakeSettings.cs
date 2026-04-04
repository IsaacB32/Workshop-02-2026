using System;

namespace Polishing.Shakes
{
    [Serializable]
    struct ShakeSettings
    {
        public float amplitude;
        public bool randomizeX;
        public bool randomizeY;
        public bool randomizeZ;
    }
}
