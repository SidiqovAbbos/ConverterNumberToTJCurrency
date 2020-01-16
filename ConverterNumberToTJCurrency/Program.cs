using System;
using System.Linq;

namespace ConverterNumberToTJCurrency
{
    class Program
    {
        static void Main(string[] args)
        {
            string str = Price.ConvertToTJ(105.50);  // Price.ConvertToTJ(105.50, true);
            Console.WriteLine(str);
            Console.ReadKey();
        }
    }

    static class Price
    {
        static string[] yaki = { "", "як", "ду", "се", "чор", "панҷ", "шаш", "ҳафт", "ҳашт", "нуҳ" };
        static string[] daxi1 = { "", "ёздаҳ", "дувоздаҳ", "сенздаҳ", "чордаҳ", "понздаҳ", "шонздаҳ", "ҳабдаҳ", "ҳаждаҳ", "нуздаҳ" };
        static string[] daxi2 = { "", "даҳ", "бист", "си", "чил", "панҷоҳ", "шаст", "ҳафтод", "ҳаштод", "навад" };

        public static string ConvertToTJ(double money, bool typeDiram = false)
        {
            int asos = (int)money;
            int diram = (int)((money % 1) * 100);

            string asosStr = $"{TranslateNumbers(asos)} сомони";


            if (diram != 0)
            {
                if (typeDiram)
                    asosStr += $"ю {TranslateNumbers(diram)} дирам";
                else
                    asosStr += $"ю {diram} дирам";
            }


            return $"{asosStr}";
        }

        private static string TranslateNumbers(int asos)
        {
            string result = string.Empty;
            int length = asos.ToString().Length;

            if (length == 1)
            {
                result += GetYak(asos);
            }
            else if (length == 2)
            {
                result += GetDax(asos);
            }
            else if (length == 3)
            {
                result += GetSad(asos);
            }
            else if (length == 4 || length == 5 || length == 6)
            {
                result += GetXazor(asos);
            }
            else
            {
                result += "миллион";
            }

            return result;
        }

        static string GetYak(int yak)
            => yaki[yak];

        static string GetDax(int dax)
        {
            if (dax == 0) return string.Empty;

            if (dax.ToString().Length == 1)
                return GetYak(dax);

            int end = int.Parse(dax.ToString().Substring(1, 1));
            int first = dax / 10;

            if (end == 0)
                return daxi2[first];

            if (first == 1)
            {
                return daxi1[end];
            }

            return $"{daxi2[first]}{GetPostfix(daxi2[first].Last())} {GetYak(end)}";
        }

        static string GetSad(int sad)
        {
            if (sad == 0) return string.Empty;

            if (sad.ToString().Length == 1)
                return GetYak(sad);

            if (sad.ToString().Length == 2)
                return GetDax(sad);

            int first = sad / 100;
            int dax = int.Parse(sad.ToString().Substring(1, 2));

            if (dax == 0)
            {
                if (first == 1)
                    return "сад";
                else
                    return $"{yaki[first]} сад";
            }

            return $"{yaki[first]} саду {GetDax(dax)}";
        }
        static string GetXazor(int xazor)
        {
            if (xazor == 0) return string.Empty;

            int first = xazor / 1000;
            int sad = int.Parse(xazor.ToString().Substring(xazor.ToString().Length - 3, 3));

            if (first == 1 && sad == 0)
                return "ҳазор";

            if (first.ToString().Length == 1)
            {
                return $"{yaki[first]} ҳазор{GetPostfix(sad)} {GetSad(sad)}";
            }
            else if (first.ToString().Length == 2)
            {
                return $"{GetDax(first)} ҳазор{GetPostfix(sad)} {GetSad(sad)}";
            }
            else
            {
                return $"{GetSad(first)} ҳазор{GetPostfix(sad)} {GetSad(sad)}";
            }
        }

        static char GetPostfix(char ch) =>
            ch == 'и' ? 'ю' : 'у';

        static string GetPostfix(int i) => i == 0 ? "" : "у";

    }
}
