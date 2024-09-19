using agit.Api.Connection;
using agit.Api.Interface;
using agit.Api.Master;
using agit.Api.Models;
using Dapper;

namespace agit.Api.Repossitory;

public class Rep_production : IN_rep_production
{
    protected string _source;
    protected string _type_log;
    private readonly Basic_database _connection;
    private readonly Basic_logger _basic_logger;
    private readonly string _table;

    public Rep_production(Basic_database db_context, Basic_logger basic_logger)
    {
        _source = GetType().Name;
        _type_log = "SQL Query";
        _connection = db_context ?? throw new ArgumentNullException(nameof(db_context));
        _basic_logger = basic_logger ?? throw new ArgumentNullException(nameof(basic_logger));
        _table = "master.dbo.production";
    }

    public async Task<bool> Rep_production_insert(Mod_base_production modProduction)
    {
        var query =
            $"INSERT INTO {_table} (senin, selasa, rabo, kamis, jumat, sabtu, minggu, remap_senin, remap_selasa, remap_rabo, remap_kamis, remap_jumat, remap_sabtu, remap_minggu) " +
            $"VALUES ('{modProduction.Senin}', '{modProduction.Selasa}', '{modProduction.Rabo}', '{modProduction.Kamis}', " +
            $"'{modProduction.Jumat}', '{modProduction.Sabtu}', '{modProduction.Minggu}', '{modProduction.Remap_senin}', " +
            $"'{modProduction.Remap_selasa}', '{modProduction.Remap_rabo}', '{modProduction.Remap_kamis}', " +
            $"'{modProduction.Remap_jumat}', '{modProduction.Remap_sabtu}', '{modProduction.Remap_minggu}');";

        _basic_logger.Create_sql_log(_source, _type_log, query);
        var result = await _connection.DB.ExecuteAsync(query);
        return result > 0;
    }

    public async Task<Mod_base_production[]> Rep_production_get_all()
    {
        var query = $"SELECT * FROM {_table};";
        _basic_logger.Create_sql_log(_source, _type_log, query);
        var result = await _connection.DB.QueryAsync<Mod_base_production>(query);

        return result.ToArray();
    }
}
