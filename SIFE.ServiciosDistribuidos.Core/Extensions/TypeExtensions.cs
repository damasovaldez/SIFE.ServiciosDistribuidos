namespace SIFE.ServiciosDistribuidos.Core.Extensions
{
    using System;

    public static class TypeExtensions
    {
        public static Int16 ToInt16(this string value)
        {
            Int16 result = 0;

            if (!string.IsNullOrEmpty(value))
                Int16.TryParse(value, out result);

            return result;
        }

        public static Int32 ToInt32(this string value)
        {
            Int32 result = 0;

            if (!string.IsNullOrEmpty(value))
                Int32.TryParse(value, out result);

            return result;
        }

        public static long ToInt64(this string value)
        {
            Int64 result = 0;

            if (!string.IsNullOrEmpty(value))
                Int64.TryParse(value, out result);

            return result;
        }

        public static int ToInt(this object obj)
        {
            int resultado = 0;

            if (obj != null)
            {
                if (!string.IsNullOrEmpty(obj.ToString()) || !string.IsNullOrWhiteSpace(obj.ToString()))
                {
                    int.TryParse(obj.ToString(), out resultado);
                }
            }

            return resultado;
        }

        public static int? ToNullableInt(this string s)
        {
            int i;
            if (int.TryParse(s, out i)) return i;
            return null;
        }

        public static decimal ToDecimal(this object obj)
        {
            decimal resultado = 0;

            if (obj != null)
            {
                if (!string.IsNullOrEmpty(obj.ToString()) || !string.IsNullOrWhiteSpace(obj.ToString()))
                {
                    decimal.TryParse(obj.ToString(), out resultado);
                }
            }

            return resultado;
        }

        public static bool IsNumeric(this string s)
        {
            return float.TryParse(s, out float output);
        }

        public static Boolean ToBoolean(this string str)
        {
            try
            {
                return Convert.ToBoolean(str);
            }
            catch { }
            try
            {
                return Convert.ToBoolean(Convert.ToInt32(str));
            }
            catch { }
            return false;
        }
    }
}
