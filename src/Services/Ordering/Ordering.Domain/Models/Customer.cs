using Ordering.Domain.Abstractions;
using Ordering.Domain.ValueObjects;

namespace Ordering.Domain.Models
{
    public class Customer : Entity<CustomerId>
    {
        public string Name { get; private set; } = default!;
        public string Email { get; private set; } = default!;

        public static Customer Create(CustomerId id, string Name, string Email) {
            ArgumentException.ThrowIfNullOrWhiteSpace(Name);
            ArgumentException.ThrowIfNullOrWhiteSpace(Email);

            return new Customer
            {
                Id = id,
                Name = Name,
                Email = Email
            };
        }
    }
}
