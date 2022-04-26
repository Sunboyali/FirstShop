using System;
using System.Collections.Generic;
using System.Text;

namespace Baya.Core.Classes
{
   public class CodeGenerators
    {
        public static string ActiveCode()
        {
            Random random = new Random();
            string activeCode = random.Next(100000, 999990).ToString();
            return activeCode;
        }

        public static string FactorCode()
        {
            Random random = new Random();
            string factorCode = random.Next(10000000, 99999990).ToString();
            return factorCode;
        }



        public static string FileCode()
        {
            return Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10).ToString();
        }

        public static string ProductCode()
        {
            Random random = new Random();
            string factorCode = random.Next(10000000, 99999990).ToString();
            return factorCode;
        }
    }
}
