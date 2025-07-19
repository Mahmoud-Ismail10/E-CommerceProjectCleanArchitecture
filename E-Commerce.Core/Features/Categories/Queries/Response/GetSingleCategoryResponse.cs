namespace E_Commerce.Core.Features.Categories.Queries.Response
{
    public record GetSingleCategoryResponse(Guid Id, string Name, string? Description);
}
