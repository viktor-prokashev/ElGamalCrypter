using System;
using System.Collections.Generic;


namespace ELGamal_C_Sharp_
{

    static class PrimalityTester
    {

        /// <summary>
        /// Maximum number of iterations for the Miller-Rabin test.
        /// </summary>
        private const int MaxMillerRabinIterations = 3;

        // Constants
        private static readonly BigInteger Zero;
        private static readonly BigInteger One;
        private static readonly BigInteger Two;
        private static readonly BigInteger Three;

        private static readonly long[] SmallPrimes;
        private static readonly List<BigInteger> SmallPrimesList;


        static PrimalityTester()
        {
            Zero = new BigInteger();
            One = new BigInteger(1);
            Two = new BigInteger(2);
            Three = new BigInteger(3);

            SmallPrimes = new long[] {2, 3, 5, 7, 11, 13, 17, 19, 23, 29,
                                      31, 37, 41, 43, 47, 53, 59, 61, 67, 71,
                                      73, 79, 83, 89, 97, 101, 103, 107, 109, 113,
                                      127, 131, 137, 139, 149, 151, 157, 163, 167, 173,
                                      179, 181, 191, 193, 197, 199, 211, 223, 227, 229,
                                      233, 239, 241, 251, 257, 263, 269, 271, 277, 281,
                                      283, 293, 307, 311, 313, 317, 331, 337, 347, 349,
                                      353, 359, 367, 373, 379, 383, 389, 397, 401, 409,
                                      419, 421, 431, 433, 439, 443, 449, 457, 461, 463,
                                      467, 479, 487, 491, 499, 503, 509, 521, 523, 541,
                                      547, 557, 563, 569, 571, 577, 587, 593, 599, 601,
                                      607, 613, 617, 619, 631, 641, 643, 647, 653, 659,
                                      661, 673, 677, 683, 691, 701, 709, 719, 727, 733,
                                      739, 743, 751, 757, 761, 769, 773, 787, 797, 809,
                                      811, 821, 823, 827, 829, 839, 853, 857, 859, 863,
                                      877, 881, 883, 887, 907, 911, 919, 929, 937, 941,
                                      947, 953, 967, 971, 977, 983, 991, 997};

            SmallPrimesList = new List<BigInteger>(SmallPrimes.Length);
            for (int i = 0; i < SmallPrimes.Length; i++)
                SmallPrimesList.Add(new BigInteger(SmallPrimes[i]));
        }


        /// <summary>
        /// Tests whether the number provided is a prime.
        /// </summary>
        /// <param name="number">The number to be tested for primality</param>
        /// <returns>True, if the number given is a prime, false, otherwise</returns>
        public static bool IsPrime(BigInteger number)
        {
            if (IsSmallPrime(number) == true)
                return true;
            else
                return IsPrimeByTrialDivision(number) || IsPrimeMillerRabinTest(number);//&&
        }


        private static bool IsSmallPrime(BigInteger number)
        {
            foreach (BigInteger aPrime in SmallPrimesList)
                if (number == aPrime)
                    return true;

            return false;
        }


        public static bool IsPrimeByTrialDivision(BigInteger number)
        {
            foreach (BigInteger aPrime in SmallPrimesList)
                if ((number != aPrime) && (number % aPrime == Zero))
                    return false;

            return true;
        }


        /// <summary>
        /// Determines whether the given BigInteger number is probably prime, with a probability
        /// of at least 1/(4^MaxMillerRabinIterations), using the Miller-Rabin primality test.
        /// </summary>
        private static bool IsPrimeMillerRabinTest(BigInteger number)
        {
            BigInteger s = new BigInteger();
            BigInteger t = number - One;
            BigInteger b = new BigInteger(2);
            BigInteger nmin1 = number - One;
            BigInteger r, j, smin1;

            if (number == One)
                return false;
            else if (number == Two)
                return true;
            else if (number == Three)
                return true;

            while (t % Two == Zero)
            {
                t /= Two;
                s++;
            }

            smin1 = s - One;

            for (int i = 0; i < MaxMillerRabinIterations; i++)
            {
                r = BigInteger.ModularExponentiation(b, t, number);

                if ((r != One) && (r != nmin1))
                {
                    j = new BigInteger(One);
                    while ((j <= smin1) && (r != nmin1))
                    {
                        r = (r * r) % number;
                        if (r == One)
                            return false;
                        j++;
                    }
                    if (r != nmin1)
                        return false;
                }

                if (b == Two)
                    b++;
                else
                    b += Two;
            }

            return true;
        }

    }

}
