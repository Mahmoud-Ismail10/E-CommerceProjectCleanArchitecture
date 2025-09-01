namespace E_Commerce.Core.Features.Payments.Queries.Responses
{
    public record PaymobCallbackResponse
    (
        bool IsSuccess,
        string HtmlContent
    );
}
