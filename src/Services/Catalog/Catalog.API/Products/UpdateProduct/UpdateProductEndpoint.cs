
namespace Catalog.API.Products.UpdateProduct
{
    public record UpdateProductRequest(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price);
    public record UpdateProductResponse(string Confirmation);

    public class UpdateProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("products/{id}", async (Guid id, UpdateProductRequest product, ISender sender) =>
            {
                var command = product.Adapt<UpdateProductCommand>();
                var result = await sender.Send(command);

                if (result is null) return Results.BadRequest("Product does not exist");
                var response = result.Adapt<UpdateProductResponse>();

                return Results.Ok(response.Confirmation);
            })
                .WithName("UpdateProduct")
                .Produces<string>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Update Product")
                .WithDescription("Updates a product by replacing it with a new version."); ;
        }
    }
}
