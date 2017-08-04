using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TryEnums
{
    class Program
    {
        public static AttackType SetFlag(AttackType a, AttackType b)
        {
            return a | b;
        }

        public static AttackType UnsetFlag(AttackType a, AttackType b)
        {
            return a & (~b);
        }

        // Works with "None" as well
        public static bool HasFlag(AttackType a, AttackType b)
        {
            return (a & b) == b;
        }

        public static AttackType ToogleFlag(AttackType a, AttackType b)
        {
            return a ^ b;
        }


        static void Main(string[] args)
        {

            var t3 = (MyEnum[])Enum.GetValues(typeof(MyEnum));

            foreach (var myEnum in t3)
            {
                Console.WriteLine((byte)myEnum);
            }

            var attack = AttackType.Melee | AttackType.Fire;
            attack &= AttackType.Fire;
            Console.WriteLine(attack);

            var attack2 = AttackType.Melee | AttackType.Fire;
            attack2 &= AttackType.Ice;
            Console.WriteLine(attack2);

            var attack3 = AttackType.Melee | AttackType.Fire;
            attack3 ^= AttackType.Fire;
            Console.WriteLine(attack3);

            var attack4 = AttackType.Melee | AttackType.Fire;
            attack4 ^= AttackType.Ice;
            attack4 ^= AttackType.Ice;
            Console.WriteLine(attack4);

            var attack5 = AttackType.Melee | AttackType.Fire;
            attack5 &= ~AttackType.Melee;
            Console.WriteLine(attack5);
        }
    }
}
