using SystemAPI.Models.Internals;

namespace SystemAPI.Helpers;

internal static class EasyResponseHelper
{
    public static ResponseInternalModel EasyInternalErrorRespond(string errorCode, string message = "Error general interno")
    {
        return new ResponseInternalModel()
        {
            StatusCode = 500,
            Errors = new List<FieldErrorInternalModel>()
            {
                new FieldErrorInternalModel
                {
                    StatusCode = errorCode,
                    Message = message
                }
            }
        };
    }
    public static ResponseInternalModel EasyListErrorRespond(List<FieldErrorInternalModel> errorList, int nStatusCode = 400)
    {
        return new ResponseInternalModel()
        {
            StatusCode = nStatusCode,
            Errors = errorList
        };
    }
    public static ResponseInternalModel EasyEmptyRespond(int statusCode = 204)
    {
        return new ResponseInternalModel()
        {
            StatusCode = statusCode
        };
    }
    public static ResponseInternalModel EasySuccessRespond(dynamic dataResponse)
    {
        return new ResponseInternalModel()
        {
            StatusCode = 200,
            Response = dataResponse
        };
    }

    public static T EasySuccessRespond<T>(dynamic dataResponse) where T : ResponseInternalModel, new()
    {
        var result = new T();
        result.StatusCode = 200;

        var newProp = typeof(T).GetProperties()
            .Where(prop => prop.Name != "statusCode" && prop.Name != "errors")
            .FirstOrDefault();

        if (newProp != null) newProp.SetValue(result, dataResponse);

        return result;
    }
}
