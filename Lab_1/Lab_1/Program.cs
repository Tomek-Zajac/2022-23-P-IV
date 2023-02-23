using System.Configuration;
using System.Data;
using System.Data.SqlClient;

var connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
var con = new SqlConnection(connectionString);
    
var cmd = CommandType.Text;
var commandName = "Select * from Employee";
var table = ExecuteSelectCommand(con, cmd, commandName);
DisplayTableRows(table);
Console.ReadKey();
Console.WriteLine("----------------------------------------------");

commandName = @"Select * from Employee where BusinessEntityID > @id and
    JobTitle = @job";
var id = 150;
var jobTitle = "Buyer";
var paramList = new SqlParameter[2];
var param = new SqlParameter("@id", id);
var param2 = new SqlParameter("job", jobTitle);
paramList[0] = param;
paramList[1] = param2;
table = ExecuteSelectCommand(con, cmd, commandName, paramList);

DisplayTableRows(table);
Console.ReadKey();

static void DisplayTableRows(DataTable dataTable)
{
    foreach (DataRow item in dataTable.Rows)
    {
        Console.WriteLine("Id: {0}, Job title: {1}, Płeć: {2}",
        item["BusinessEntityID"],
        item["JobTitle"].ToString(),
        item["Gender"].ToString());
    }
}

static DataTable ExecuteSelectCommand(
    SqlConnection con,
    CommandType cmdType,
    string commandName,
    SqlParameter[] param = default)
{
    var cmd = new SqlCommand();
    DataTable table = new DataTable();

    if (con.State == ConnectionState.Closed)
        con.Open();

    cmd = con.CreateCommand();
    cmd.CommandType = cmdType;
    cmd.CommandText = commandName;

    if(param != null)
        cmd.Parameters.AddRange(param);

    try
    {
        SqlDataAdapter da = new SqlDataAdapter();
        using (da = new SqlDataAdapter(cmd))
            da.Fill(table);
    }
    finally
    {
        cmd.Dispose();
        cmd = null;
        con.Close();
    }
    return table;
}