using System.Collections.Generic;

namespace GPWebpayNet.Sdk
{
    /// <summary>
    /// Helper class with PR and SR codes values.
    /// </summary>
    public static class ErrorCodesValues
    {
        private static readonly IReadOnlyDictionary<uint, string> PRCodes = new Dictionary<uint, string>()
        {
            {0, "OK" },
            {1, "Field too long." },
            {2, "Field too short." },
            {3, "Incorrect content of field." },
            {4, "Field is null." },
            {5, "Missing required field." },
            {11, "Unknown merchant." },
            {14, "Duplicate order number." },
            {15, "Object not found." },
            {17, "Amount to deposit exceeds approved amount." },
            {18, "Total sum of credited amounts exceeded deposited amount." },
            {20, "Object not in valid state for operation." },
            {25, "Operation not allowed for user." },
            {26, "Technical problem in connection to authorization center." },
            {27, "Incorrect order type." },
            {28, "Declined in 3D." },
            {30, "Declined in AC." },
            {31, "Wrong digest." },
            {35, "Session expired." },
            {50, "The cardholder canceled the payment." },
            {200, "Additional info request." },
            {1000, "Technical problem." },
        };

        private static readonly IReadOnlyDictionary<uint, string> SRCodes1_5_15_20 = new Dictionary<uint, string>()
        {
            {0, string.Empty },
            {1, "ORDERNUMBER" },
            {2, "MERCHANTNUMBER" },
            {6, "AMOUNT" },
            {7, "CURRENCY" },
            {8, "DEPOSITFLAG" },
            {10, "MERORDERNUM" },
            {11, "CREDITNUMBER" },
            {12, "OPERATION" },
            {18, "BATCH" },
            {22, "ORDER" },
            {24, "URL" },
            {25, "MD" },
            {26, "DESC" },
            {34, "DIGEST" },
        };

        private static readonly IReadOnlyDictionary<uint, string> SRCodes28 = new Dictionary<uint, string>()
        {
            {0, string.Empty },
            {3000, "Declined in 3D. Cardholder not authenticated in 3D." },
            {3001, "Authenticated." },
            {3002, "Not Authenticated in 3D. Issuer or Cardholder not participating in 3D." },
            {3004, "Not Authenticated in 3D. Issuer not participating or Cardholder not enrolled." },
            {3005, "Declined in 3D. Technical problem during Cardholder authentication." },
            {3006, "Declined in 3D. Technical problem during Cardholder authentication." },
            {3007, "Declined in 3D. Acquirer technical problem. Contact the merchant." },
            {3008, "Declined in 3D. Unsupported card product." },
        };

        private static readonly IReadOnlyDictionary<uint, string> SRCodes30 = new Dictionary<uint, string>()
        {
            {0, string.Empty },
            {1001, "Declined in AC, Card blocked." },
            {1002, "Declined in AC, Declined." },
            {1003, "Declined in AC, Card problem." },
            {1004, "Declined in AC, Technical problem in authorization process." },
            {1005, "Declined in AC, Account problem." },
        };

        /// <summary>
        /// Gets value of PR code
        /// </summary>
        /// <param name="prCode">id of the PR code</param>
        /// <returns>PR code value</returns>
        public static string GetPRCode(uint prCode)
        {
            if (PRCodes.TryGetValue(prCode, out string value))
            {
                return value;
            }

            return $"Unknown value for PR code {prCode}";
        }

        /// <summary>
        /// Gets value of SR code
        /// </summary>
        /// <param name="prCode">id of the PR code</param>
        /// <param name="srCode">id of the PR code</param>
        /// <returns>SR code value</returns>
        public static string GetSRCode(uint prCode, uint srCode)
        {
            IReadOnlyDictionary<uint, string> srCodes;

            switch (prCode)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 15:
                case 20:
                    srCodes = SRCodes1_5_15_20;
                    break;
                case 28:
                    srCodes = SRCodes28;
                    break;
                case 30:
                    srCodes = SRCodes30;
                    break;
                default:
                    srCodes = new Dictionary<uint, string>();
                    break;
            }

            if (srCodes.TryGetValue(srCode, out string value))
            {
                return value;
            }

            return $"Unknown value for PR code {prCode} and SR code {srCode}.";
        }
    }
}
