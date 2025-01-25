// <copyright file="PaymentFailedException.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Interfaces.Exceptions
{
    public class PaymentFailedException : Exception
    {
        public PaymentFailedException()
            : base()
        {
        }

        public PaymentFailedException(string message)
            : base(message)
        {
        }

        public PaymentFailedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
