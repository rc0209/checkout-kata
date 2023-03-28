namespace checkout_kata_contracts
{
    using System;

    public class MissingDataException : Exception
    {
        public MissingDataException(string missingKey) : base($"Product with Sku '{missingKey}' not found")
        {

        }
    }
}