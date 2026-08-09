using System.Security.Cryptography;
using AmusementPark.Application.Interfaces;

namespace AmusementPark.Application.Services
{
    /// <summary>
    /// مسئول تولید و بررسی کد چهار رقمی.
    /// </summary>
    public class OtpService : IOtpService
    {
        public string GenerateCode()
        {
            int code =
                RandomNumberGenerator.GetInt32(
                    1000,
                    10000);

            return code.ToString();
        }

        public bool ValidateCode(
            string enteredCode,
            string generatedCode)
        {
            return !string.IsNullOrWhiteSpace(enteredCode)
                   && enteredCode == generatedCode;
        }
    }
}