using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PDF_merger
{
    public static class NaturalSortComparer
    {
        public static int Compare(string x, string y)
        {
            if (ReferenceEquals(x, y))
                return 0;

            if (x == null)
                return -1;

            if (y == null)
                return 1;

            var partsX = Regex.Matches(x, @"\d+|\D+").Cast<Match>().Select(m => m.Value).ToList();
            var partsY = Regex.Matches(y, @"\d+|\D+").Cast<Match>().Select(m => m.Value).ToList();

            int count = Math.Min(partsX.Count, partsY.Count);

            for (int i = 0; i < count; i++)
            {
                string a = partsX[i];
                string b = partsY[i];

                bool aNum = long.TryParse(a, NumberStyles.Integer, CultureInfo.InvariantCulture, out long aVal);
                bool bNum = long.TryParse(b, NumberStyles.Integer, CultureInfo.InvariantCulture, out long bVal);

                int result;

                if (aNum && bNum)
                    result = aVal.CompareTo(bVal);
                else
                    result = string.Compare(a, b, StringComparison.CurrentCultureIgnoreCase);

                if (result != 0)
                    return result;
            }

            return partsX.Count.CompareTo(partsY.Count);
        }
    }
}
