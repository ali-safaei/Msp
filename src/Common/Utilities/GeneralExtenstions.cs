using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace Common.Utilities
{
    public static class GeneralExtenstions
    {
        public static bool HasValue(this IEnumerable<object> data)
        {
            if (data == null)
                return false;
            return data.Any();
        }
        public static bool HasValue(this IList<object> data)
        {
            if (data == null)
                return false;
            return data.Any();
        }

    }
}
