using System.Data.SqlClient;

namespace TechStudents.Utils.DataBase
{
    internal class DBInfo
    {
        /// <summary>
        /// Data Source: Origem de dados -> Servidor
        /// Initial Catalog: Base de dados que apontamos
        /// User: Usuário de login no banco
        /// Password: Senha de login no banco
        /// </summary>
        public const string DBConnection = @"Data Source=BNU-NT005159;Initial catalog=TechStudents_DB;User=sa;Password=benner;Trusted_Connection=True;";
        //public const string DBConnection = @"Data Source=BUE205D017\SQLEXPRESS;Initial catalog=TechStudents_DB;User=sa;Password=Senac@2021;Trusted_Connection=True;";

        public static void TestarBanco()
        {
            using (var conn = new SqlConnection(DBConnection))
            {
                var cmd = new SqlCommand("SELECT * FROM ALUNOS", conn);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader[1]}");
                    }
                }
            }

            Console.ReadLine();
        }
    }
}