using Basket.API.Data;
using Basket.API.Models;
using BuildingBlocks.CQRS;
using Discount.Grpc;
using FluentValidation;
using JasperFx.Events.Daemon;

namespace Basket.API.Basket.StoreBasket
{
    public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;
    public record StoreBasketResult(string UserName);

    public class StoreBasketCommmandValidator : AbstractValidator<StoreBasketCommand>
    {
        public StoreBasketCommmandValidator()
        {
            RuleFor(x => x.Cart).NotNull().WithMessage("Cart can not be null");
            RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("UserName is required").When(x => x.Cart is not null);
        }
    }

    public class StoreBasketCommandHandler(IBasketRepository repository, DiscountProtoService.DiscountProtoServiceClient discountProto) : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
        {
            await DeductDiscountAsync(command.Cart, cancellationToken);

            ShoppingCart cart = command.Cart;
            var basket = await repository.StoreBasket(cart, cancellationToken);
            return new StoreBasketResult(basket.UserName);
        }

        private async Task DeductDiscountAsync(ShoppingCart cart, CancellationToken cancellationToken)
        {
            //communicate with grpc to apply discount calculation
            foreach (var item in cart.Items)
            {
                var coupon = await discountProto.GetDiscountAsync(new GetDiscountRequest { ProductName = item.ProductName }, cancellationToken: cancellationToken);
                item.Price -= coupon.Amount;
            }

        }
    }
}
