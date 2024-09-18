namespace agit.Api.Master;

public class Basic_message
{
    // SUCCESS
    public static readonly string Message_general_success = "Data already send";
    public static readonly string Message_login_success = "Login Succesfully";
    public static readonly string Message_data_register = "Data on Process Registered";
    public static readonly string Message_data_process = "Add Data on Process";

    // INFO

    // ERROR
    public static readonly string Message_error_general = "Internal Server Error";
    public static readonly string Message_error_already_exist = "Data Already Exist";
    public static readonly string Message_error_login = "Invalid username or password";
    public static readonly string Message_error_auth = "Invalid credentials";
    public static readonly string Message_error_data_not_found = "Data not found for the specified entity";
    public static readonly string Message_error_validate_form = "Request data invalid";
}
