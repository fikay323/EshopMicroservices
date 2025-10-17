namespace Catalog.API.Products.GetProductById
{
    public record GetProductByIdRequest(Guid Id);
    public record GetProductByIdResponse(Product Product);

    public class GetProductByIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/{id}", async (Guid id, ISender sender) =>
            {
                var query = new GetProductByIdQuery(id);
                var result = await sender.Send(query);

                if (result.Product is null) return Results.NotFound("Product not found");
                var response = result.Adapt<GetProductByIdResponse>();

                return Results.Ok(response.Product);
            })
                .WithName("GetProductById")
                .Produces<GetProductByIdResponse>()
                .ProducesProblem(StatusCodes.Status404NotFound)
                .WithSummary("Get Product By Id")
                .WithDescription("Retrieves a single product using its unique identifier.");
        }
    }
}
