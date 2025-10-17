namespace Catalog.API.Products.DeleteProduct
{
    public class DeleteProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("products/{id}", async (Guid id, ISender sender) =>
            {
                var command = new DeleteProductCommand(id);
                await sender.Send(command);

                return Results.Ok();
            });
        }
    }
}
