﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Seamster_Products.Models
{
    public class MyAzureFunctionResponse
    {
        /*
         * ErrorCode is 0 for success
         * ErrorCode is 100 for failure of business of validation (Catch) 
         * ErroCode is 200 for validation failure or bad data
         * ErrorCode is 500 for Unknown Technical Error
         */
        public int ErrorCode { get; set; }

        public string Message { get; set; }
    }
}
