namespace AmusementPark.Application.Interfaces
{
    /// <summary>
    /// قرارداد مربوط به تولید و بررسی OTP.
    /// </summary>
    public interface IOtpService
    {
        string GenerateCode();

        bool ValidateCode(
            string enteredCode,
            string generatedCode);
    }
}