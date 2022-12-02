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
        public const string DBConnection = "Data Source=BUE303D02\\SQLEXPRESS;Initial catalog=TechStudents_DB;User=sa;Password=Senac@2021;";

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

        public static void CreateUsersTable()
        {
            var exist = false;

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

            if (!exist)
            {
                var create = @"CREATE TABLE [USERS](
	                        [ID] [int] NOT NULL,
	                        [SENHA] [varchar](150) NOT NULL,
	                        [BLOCKED] [char](1) NULL,
	                        [LOGINCOUNT] [int] NULL,
	                        [NOMEUSUARIO] [varchar](50) NOT NULL,
	                        [PESSOA] [int] NOT NULL,
	                        [LOGINTYPE] [int] NULL,
	                        [NAME] [varchar](50) NULL,
                                CONSTRAINT [PK_USERS] PRIMARY KEY CLUSTERED 
                                (
	                                [ID] ASC
                                )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
                            ) ON [PRIMARY]
                            GO";
                var alter1 = @"ALTER TABLE [USERS]  WITH CHECK ADD  CONSTRAINT [FK_USERS_PESSOAS] FOREIGN KEY([PESSOA])
                            REFERENCES [PESSOAS] ([ID])
                            GO";
                var alter2 = @"ALTER TABLE [USERS] CHECK CONSTRAINT [FK_USERS_PESSOAS]
                            GO";
                var sequence = @"CREATE SEQUENCE USERS_SEQ START WITH 1 INCREMENT BY 1";

                using (var conn = new SqlConnection(DBConnection))
                {
                    conn.Open();
                    var cmd = new SqlCommand(create, conn);
                    cmd.ExecuteNonQuery();

                    cmd = new SqlCommand(alter1, conn);
                    cmd.ExecuteNonQuery();

                    cmd = new SqlCommand(alter2, conn);
                    cmd.ExecuteNonQuery();

                    cmd = new SqlCommand(sequence, conn);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}