using Common.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Common.ValueObjects
{
    public class Money : ValueObject
    {
        
        public Money(decimal value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(ProductResources.price_value_error);
            else
                Value = value;
        }
        public decimal Value { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
