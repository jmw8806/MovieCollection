using System;

namespace DataObjects
{
    public static class UtilityHelpers
    {
        public static int generate_random_id(int count)
        {
            int num = 0;
            var random = new Random();
            num = random.Next(0, count) + 10000;
            return num;
        }

    }
}
