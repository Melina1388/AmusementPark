using AmusementPark.Application.DTOs;

namespace AmusementPark.Application.Interfaces
{
    /// <summary>
    /// قرارداد مربوط به عملیات پرداخت.
    /// </summary>
    public interface IPaymentService
    {
        PaymentResultDto ProcessPayment(
            PaymentRequestDto request);
    }
}