using System;

namespace PaymentSystem.Common.Utilities
{
    public static class TurkishIdentificationNumber
    {
        public static bool IsValid(string turkishIdentificationNumber)
        {
            bool isValid = false;
            if (turkishIdentificationNumber.Length == 11)
            {
                Int64 ATCNO, BTCNO, TcNo;
                long C1, C2, C3, C4, C5, C6, C7, C8, C9, Q1, Q2;

                TcNo = Int64.Parse(turkishIdentificationNumber);

                ATCNO = TcNo / 100;
                BTCNO = TcNo / 100;

                C1 = ATCNO % 10; ATCNO = ATCNO / 10;
                C2 = ATCNO % 10; ATCNO = ATCNO / 10;
                C3 = ATCNO % 10; ATCNO = ATCNO / 10;
                C4 = ATCNO % 10; ATCNO = ATCNO / 10;
                C5 = ATCNO % 10; ATCNO = ATCNO / 10;
                C6 = ATCNO % 10; ATCNO = ATCNO / 10;
                C7 = ATCNO % 10; ATCNO = ATCNO / 10;
                C8 = ATCNO % 10; ATCNO = ATCNO / 10;
                C9 = ATCNO % 10; ATCNO = ATCNO / 10;
                Q1 = ((10 - ((((C1 + C3 + C5 + C7 + C9) * 3) + (C2 + C4 + C6 + C8)) % 10)) % 10);
                Q2 = ((10 - (((((C2 + C4 + C6 + C8) + Q1) * 3) + (C1 + C3 + C5 + C7 + C9)) % 10)) % 10);

                isValid = ((BTCNO * 100) + (Q1 * 10) + Q2 == TcNo);
            }
            return isValid;
        }

        public static string GenerateRandom()
        {
            var rnd = new Random();
            return GenerateRandomId(rnd);
        }

        private static string GenerateRandomId(Random rnd)
        {
            int value = rnd.Next(100_000_000, 1_000_000_000);
            return GenerateIdFromValue(value);
        }

        private static string GenerateIdFromValue(int x)
        {
            int d1 = x / 100_000_000;
            int d2 = (x / 10_000_000) % 10;
            int d3 = (x / 1_000_000) % 10;
            int d4 = (x / 100_000) % 10;
            int d5 = (x / 10_000) % 10;
            int d6 = (x / 1000) % 10;
            int d7 = (x / 100) % 10;
            int d8 = (x / 10) % 10;
            int d9 = x % 10;
            int oddSum = d1 + d3 + d5 + d7 + d9;
            int evenSum = d2 + d4 + d6 + d8;
            int firstChecksum = ((oddSum * 7) - evenSum) % 10;

            if (firstChecksum < 0)
            {
                firstChecksum += 10;
            }
            int secondChecksum = (oddSum + evenSum + firstChecksum) % 10;
            return $"{x}{firstChecksum}{secondChecksum}";
        }
    }
}
