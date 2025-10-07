namespace PP2.Helpers
{
    public static class BinaryHelper
    {
        public static string PadToByte(string s) => s.PadLeft(8, '0');

        private static (string, string) AlignSameLength(string a, string b)
        {
            int n = Math.Max(a.Length, b.Length);
            return (a.PadLeft(n, '0'), b.PadLeft(n, '0'));
        }

        public static string BinaryAnd(string a, string b)
        {
            (a, b) = AlignSameLength(a, b);
            return TrimLeadingZeros(new string(a.Zip(b, (x, y) => (x == '1' && y == '1') ? '1' : '0').ToArray()));
        }

        public static string BinaryOr(string a, string b)
        {
            (a, b) = AlignSameLength(a, b);
            return TrimLeadingZeros(new string(a.Zip(b, (x, y) => (x == '1' || y == '1') ? '1' : '0').ToArray()));
        }

        public static string BinaryXor(string a, string b)
        {
            (a, b) = AlignSameLength(a, b);
            return TrimLeadingZeros(new string(a.Zip(b, (x, y) => (x != y) ? '1' : '0').ToArray()));
        }

        public static string BinarySum(string a, string b) =>
            Convert.ToString(Convert.ToInt32(a, 2) + Convert.ToInt32(b, 2), 2);

        public static string BinaryMul(string a, string b) =>
            Convert.ToString(Convert.ToInt32(a, 2) * Convert.ToInt32(b, 2), 2);

        public static string ToDecimal(string bin) => Convert.ToInt32(bin, 2).ToString();
        public static string ToOctal(string bin) => Convert.ToString(Convert.ToInt32(bin, 2), 8);
        public static string ToHex(string bin) => Convert.ToString(Convert.ToInt32(bin, 2), 16).ToUpper();

        public static string TrimLeadingZeros(string s)
        {
            if (string.IsNullOrEmpty(s)) return "0";
            var idx = s.IndexOf('1');
            return idx == -1 ? "0" : s.Substring(idx);
        }
    }
}