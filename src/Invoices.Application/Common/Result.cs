using System;
using System.Collections.Generic;
using System.Linq;

namespace Invoices.Application.Common;

public readonly struct Result<T>
{
    private readonly bool _isSuccess;
    private readonly T _entity;
    private readonly ValidationErrors _validationErrors;

    public Result(T entity)
    {
        _isSuccess = true;
        _entity = entity;
        _validationErrors = default;
    }

    public Result(ValidationErrors errors)
    {
        _isSuccess = false;
        _entity = default;
        _validationErrors = errors;
    }

    public TResult Match<TResult>(Func<T, TResult> success, Func<ValidationErrors, TResult> failure)
    {
        return _isSuccess ? success(_entity) : failure(_validationErrors);
    }

    public static implicit operator Result<T>(T entity) => new(entity);
    public static implicit operator Result<T>(ValidationErrors errors) => new(errors);
}

public class ValidationErrors : List<string>
{
}