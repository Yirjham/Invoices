using System.Collections.Generic;
using System.Linq;

namespace Invoices.Application.Common;

public class Result<T>
{
    public Result()
    {
        
    }

    public Result(List<string> errors)
    {
        ValidationErrors = errors;
    }
    public T Entity { get; set; }
    public List<string> ValidationErrors { get; } = new();
    public bool IsValid => !ValidationErrors.Any();

}