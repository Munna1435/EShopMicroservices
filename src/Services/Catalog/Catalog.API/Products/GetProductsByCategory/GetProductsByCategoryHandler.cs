
namespace Catalog.API.Products.GetProductsByCategory
{
    public record GetProductsByCategoryQeury(string Category) : IQuery<GetProductsByCategoryResult>;
    public record GetProductsByCategoryResult(IEnumerable<Product> Products);

    internal class GetProductsByCategoryQueryHandler(IDocumentSession session)
        : IQueryHandler<GetProductsByCategoryQeury, GetProductsByCategoryResult>
    {
        public async Task<GetProductsByCategoryResult> Handle(GetProductsByCategoryQeury query, CancellationToken cancellationToken)
        {
            var products = await session.Query<Product>()
                                        .Where(p => p.Category.Contains(query.Category))
                                        .ToListAsync();

            return new GetProductsByCategoryResult(products);
        }
    }
}
