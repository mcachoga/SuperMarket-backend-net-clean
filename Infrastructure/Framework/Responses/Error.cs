﻿namespace SuperMarket.Infrastructure.Framework.Responses
{
    public class Error
    {
        public List<string> ErrorMessages { get; set; }

        public string FriendlyErrorMessage { get; set; }
    }
}