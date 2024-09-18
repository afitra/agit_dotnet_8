namespace agit.Api.Helpers;

public class Helper
{
    public static bool Hel_convert_string_to_bool(string value)
    {
        if (bool.TryParse(value, out var result))
            return result;

        return false;
    }
}
