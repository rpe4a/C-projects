using System;

namespace TryEnums
{
    [Flags]
    enum AttackType
    {
        None = 0, //0 - 00000000
        Melee = 1 << 0, //1 - 00000001
        Fire = 1 << 1, //2 - 00000010
        Ice = 1 << 2, //4 - 00000100
        Poison = 1 << 3, //8 - 00001000
        Water = 1 << 4, //16 - 00010000

        MeleeAndFire = Melee | Fire, //00000011
        MeleeAndIce = Melee | Ice, //00000101

    }
}