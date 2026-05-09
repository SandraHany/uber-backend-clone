using System;
using System.Collections.Generic;
using System.Text;

namespace Uber.Sharedkernel.Errors;

public sealed class ValidationError : Error
{
   
    public ValidationError(Error[] errors) 
        : base("Validation.General", "One or more validation errors occured.")
    { 
       
    }
}
