﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSystem.Common.Utilities
{
    public static class StringUtilities
    {
        public static string GenerateKey(int length = 13)
        {
            StringBuilder builder = new StringBuilder();
            Enumerable
                .Range(65, 26).Select(e => ((char)e).ToString())
                .Concat(Enumerable.Range(97, 26).Select(e => ((char)e).ToString()))
                .Concat(Enumerable.Range(0, 10).Select(e => e.ToString()))
                .OrderBy(e => Guid.NewGuid())
                .Take(length)
                .ToList().ForEach(e => builder.Append(e));
            return builder.ToString();
        }

        public static string GenerateVerison(string name = null, string separator = "_")
        {
            return $"{DateTime.Now.ToString("yyyyMMddHHmmssf")}{separator}{name}";
        }
    }
}
