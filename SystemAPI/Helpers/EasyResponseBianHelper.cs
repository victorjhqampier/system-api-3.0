using Application.Internals.Adapters;
using System.Collections.Generic;
using SystemAPI.Models.Internals;

namespace SystemAPI.Helpers;

internal static class EasyResponseBianHelper
{
    public static T EasyWarningRespond<T>(IReadOnlyCollection<ValidationResultAdapter> validationValues) where T : BianResponseModel, new()
    {
        return new T()
        {
            errors = validationValues.Select(e => new BianErrorInternalModel
                {
                    Status_code = e.Code,
                    Message = e.Message
                })
            .ToList()
        };
    }

    public static BianResponseModel EasyErrorRespond(string errorCode = "1099", string Message = "No es un problema de tu lado. Estamos experimentando dificultades técnicas")
    {
        return new BianResponseModel()
        {
            errors = new List<BianErrorInternalModel>{
                new BianErrorInternalModel
                {
                    Status_code = errorCode.ToString(),
                    Message = Message
                }
            }
        };
    }

    public static T EasySuccessRespond<T>(dynamic dataResponse) where T : BianResponseModel, new()
    {
        var result = new T
        {
            //statusCode = 200
        };

        var newProp = typeof(T).GetProperties()
            .Where(prop => prop.Name != "statusCode" && prop.Name != "errors")
            .FirstOrDefault();

        if (newProp != null) newProp.SetValue(result, dataResponse);

        return result;
    }
}
