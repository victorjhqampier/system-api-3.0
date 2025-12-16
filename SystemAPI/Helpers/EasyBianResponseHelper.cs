using Application.Internals.Adapters;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using SystemAPI.Models.Internals;

namespace SystemAPI.Helpers;

public static class EasyBianResponseHelper
{
    // Cache para nombres de campos de respuesta para evitar recalcular
    private static readonly ConcurrentDictionary<Type, string> _responseFieldNameCache = new();
    
    // Constantes para evitar allocaciones repetidas
    private const string ErrorsKey = "errors";
    private const string DefaultErrorCode = "1099";
    private const string DefaultErrorMessage = "No es un problema de tu lado. Estamos experimentando dificultades técnicas";
    private const string GeneralField = "General";
    private const string InSeparator = " in ";
    private const string AdapterSuffix = "Adapter";
    private const string ResponseSuffix = "Response";
    
    // Reutilizar array para errores simples
    private static readonly BianErrorInternalModel[] _singleErrorArray = new BianErrorInternalModel[1];
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Dictionary<string, object?> SuccessResponse(object data, string responseFieldName)
    {
        return new Dictionary<string, object?>(capacity: 1)
        {
            [responseFieldName] = data
        };
    }

    public static Dictionary<string, object?> WarningResponse(IReadOnlyCollection<ValidationResultAdapter> validationErrors)
    {
        var errorCount = validationErrors.Count;
        var errors = new BianErrorInternalModel[errorCount];
        var index = 0;
        
        foreach (var error in validationErrors)
        {
            var fieldName = string.IsNullOrEmpty(error.Field) ? GeneralField : error.Field;
            errors[index++] = new BianErrorInternalModel
            {
                Status_code = error.Code,
                Message = string.Concat(error.Message, InSeparator, fieldName)
            };
        }

        return new Dictionary<string, object?>(capacity: 1)
        {
            [ErrorsKey] = errors
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Dictionary<string, object?> ErrorResponse(string errorCode = DefaultErrorCode, string message = DefaultErrorMessage)
    {
        // Reutilizar array estático para errores únicos
        _singleErrorArray[0] = new BianErrorInternalModel
        {
            Status_code = errorCode,
            Message = message
        };

        return new Dictionary<string, object?>(capacity: 1)
        {
            [ErrorsKey] = _singleErrorArray
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Dictionary<string, object?> EasySuccessRespond<T>(T data)
    {
        var responseFieldName = GetCachedResponseFieldName<T>();
        return SuccessResponse(data!, responseFieldName);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Dictionary<string, object?> EasyValidationErrorRespond<T>(IReadOnlyCollection<ValidationResultAdapter> validationErrors)
    {
        return WarningResponse(validationErrors);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Dictionary<string, object?> EasyErrorRespond<T>(string errorCode = DefaultErrorCode, string message = DefaultErrorMessage)
    {
        return ErrorResponse(errorCode, message);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string GetCachedResponseFieldName<T>()
    {
        return _responseFieldNameCache.GetOrAdd(typeof(T), GenerateResponseFieldName);
    }

    private static string GenerateResponseFieldName(Type type)
    {
        var typeName = type.Name;
        
        // Si el tipo termina en "Adapter", lo removemos
        if (typeName.EndsWith(AdapterSuffix))
        {
            return string.Concat(typeName.AsSpan(0, typeName.Length - AdapterSuffix.Length), ResponseSuffix);
        }
        
        // Simplemente agregar "Response" - ASP.NET Core maneja camelCase automáticamente
        return string.Concat(typeName, ResponseSuffix);
    }
}