using System.Data;
using agit.Api.Master;
using Microsoft.Data.SqlClient;

namespace agit.Api.Connection;

public class Basic_database
{
    protected readonly Basic_logger _basic_logger;
    protected string _typeLog;
    protected string _source;
    protected string _connection_string;
    protected string _connect_message;
    protected string _reconnect_message;
    public SqlConnection DB;

    public Basic_database(Basic_logger basic_logger)
    {
        _basic_logger = basic_logger;
        _typeLog = "Connection DB";
        _source = GetType().Name;
        _connection_string =
            Basic_configuration.Get_variable_global("DB_URI"); // Atur sesuai connection string untuk SQL Server
        _connect_message = "DB Connection successfully";
        _reconnect_message = "DB Connection re-open";
        set_connection();
    }

    private void set_connection()
    {
        try
        {
            if (DB == null)
            {
                DB = new SqlConnection(_connection_string);
                DB.Open();
                _basic_logger.Custom_Specific_Log(_source, _typeLog, _connect_message);
            }
            else if (DB.State == ConnectionState.Closed)
            {
                DB.Open();
                _basic_logger.Custom_Specific_Log(_source, _typeLog, _reconnect_message);
            }
        }
        catch (Exception ex)
        {
            _basic_logger.Debug(_source, $"{_typeLog}  -=> " + ex.Message);
        }
    }
}
