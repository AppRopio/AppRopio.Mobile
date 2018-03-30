using System;

namespace AppRopio.Beacons.iOS.Models
{
    public enum BeaconType
    {
        Unknown = 0,
        Eddystone, // 10 bytes namespace + 6 bytes instance = 16 byte ID
        EddystoneEID // 8 byte ID
    }
}

