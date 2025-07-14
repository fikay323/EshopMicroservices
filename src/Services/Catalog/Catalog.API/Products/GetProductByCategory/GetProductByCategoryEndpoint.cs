
using Catalog.API.Products.GetProductById;

namespace Catalog.API.Products.GetProductByCategory
{
    public record GetProductByCategoryRequest(string category) : IQuery<GetProductByCategoryResult>;
    public record GetProductByCategoryResponse(IEnumerable<Product> Products);
    public class GetProductByCategoryHandlerEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("products/category/{category}", async (string category, ISender sender) =>
            {
                var query = new GetProductByCategoryQuery(category);
                var result = await sender.Send(query);
                var response = result.Adapt<GetProductByCategoryResponse>();

                return Results.Ok(response.Products);
            });
        }
    }
}
