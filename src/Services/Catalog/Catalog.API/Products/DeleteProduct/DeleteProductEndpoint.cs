
namespace Catalog.API.Products.DeleteProduct
{
    public record DeleteProductRequest(Guid Id) : ICommand<DeleteProductResult>;
    public record DeleteProductResponse();
    public class DeleteProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("products/{id}", async (Guid id, ISender sender) =>
            {
                var command = new DeleteProductCommand(id);
                var result = await sender.Send(command);

                return Results.Ok();
            });
        }
    }
}
