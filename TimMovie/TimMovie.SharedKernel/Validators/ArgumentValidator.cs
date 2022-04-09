namespace TimMovie.SharedKernel.Validators;

public static class ArgumentValidator
{
    public static void CheckOnNull(object obj, string paramName)
    {
        if (obj is null)
        {
            throw new ArgumentNullException(paramName);
        }
    }
}