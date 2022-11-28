using System.Data.SqlClient;
using TechStudents.Utils.DataBase;

namespace TechStudents.Utils.Entities
{
    public enum LoginType
    {
        Adm = 10,
        Teacher = 20,
        Student = 30,
    }

    public enum LoginStatus
    {
        Sucess = 10,
        InvalidPassword = 20,
        Blocked = 30,
    }

    public class User
    {
        private const int MAX_LOGIN_COUNT = 3;

        public bool Blocked { get; set; }

        public int ID { get; set; } = -1;
        public int PersonID { get; set; } = -1;
        public int LoginCount { get; set; }

        public string Name { get; set; }
        public string NickName { get; set; }
        public string Password { get; set; }

        public LoginType LoginType { get; set; }

        public User() { }
        public User(int id) 
        {
            using (var conn = new SqlConnection(DBInfo.DBConnection))
            {
                var cmd = new SqlCommand($"SELECT ID, NAME, NOMEUSUARIO, SENHA, LOGINTYPE, LOGINCOUNT, BLOCKED, PESSOA FROM USERS WHERE ID = {id}", conn);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ID = reader.GetInt32(0);
                        Name = reader.GetString(1);
                        NickName = reader.GetString(2);
                        Password = reader.GetString(3);
                        LoginType = (LoginType)reader.GetInt32(4);
                        LoginCount = reader.GetInt32(5);
                        Blocked = reader.GetString(6) == "X";
                        PersonID = reader.GetInt32(7);
                    }
                }
            }
        }
        public User(string nomeUsuario)
        {
            NickName = nomeUsuario;

            using (var conn = new SqlConnection(DBInfo.DBConnection))
            {
                var cmd = new SqlCommand($"SELECT U.ID, U.SENHA, P.PERFIL, P.NOME,  U.PESSOA, U.BLOCKED, U.LOGINCOUNT FROM USERS U INNER JOIN PESSOAS P ON P.ID = U.PESSOA WHERE NOMEUSUARIO = '{NickName}'", conn);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ID = reader.GetInt32(0);
                        Password = reader.GetString(1);
                        LoginType = (LoginType)reader.GetInt32(2);
                        Name = reader.GetString(3);
                        PersonID = reader.GetInt32(4);
                        Blocked = reader.GetString(5) == "X";
                        LoginCount = reader.GetInt32(6);
                    }
                }
            }
        }

        public LoginStatus ValidarSenha(string senha)
        {
            if (Blocked)
            {
                return LoginStatus.Blocked;
            }

            if (GenerateHash(senha).ToUpper() == Password.ToUpper())
            {
                return LoginStatus.Sucess;
            }
            else
            {
                LoginCount++;
                var isBlocked = LoginCount > MAX_LOGIN_COUNT;

                using (var conn = new SqlConnection(DBInfo.DBConnection))
                {
                    var cmd = new SqlCommand($"UPDATE USERS SET LOGINCOUNT = {LoginCount}{(isBlocked ? ", BLOCKED = 'X'" : string.Empty)} WHERE ID = {ID};", conn);
                    conn.Open();

                    if (cmd.ExecuteNonQuery() < 1)
                    {
                        throw new Exception("O usuário não existe na base de dados!");
                    }
                }

                return isBlocked ? LoginStatus.Blocked : LoginStatus.InvalidPassword;
            }
        }

        public void Create()
        {
            using (var conn = new SqlConnection(DBInfo.DBConnection))
            {
                var cmd = new SqlCommand($"INSERT INTO USERS (ID, NAME, NOMEUSUARIO, LOGINTYPE, SENHA, BLOCKED, LOGINCOUNT, PESSOA) VALUES (NEXT VALUE FOR USERS_SEQ, '{Name}', '{NickName}', {LoginType.GetHashCode()}, '{GenerateHash(Password)}', '{(Blocked ? "X" : ".")}', {LoginCount}, {PersonID})", conn);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Update()
        {
            using (var conn = new SqlConnection(DBInfo.DBConnection))
            {
                var querry = $"UPDATE USERS SET NAME = '{Name}', NOMEUSUARIO = '{NickName}', LOGINTYPE = {LoginType.GetHashCode()}, LOGINCOUNT = {LoginCount}, BLOCKED = '{(Blocked ? "X" : ".")}'{(Password != "00000000" ? ($", SENHA = '{GenerateHash(Password)}'") : string.Empty)} WHERE ID = {ID}";
                var cmd = new SqlCommand(querry, conn);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete()
        {
            using (var conn = new SqlConnection(DBInfo.DBConnection))
            {
                var cmd = new SqlCommand($"DELETE FROM USERS WHERE ID = {ID}", conn);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public static List<User> QuerryAll()
        {
            var list = new List<User>();

            using (var conn = new SqlConnection(DBInfo.DBConnection))
            {
                var cmd = new SqlCommand($"SELECT ID, SENHA, BLOCKED, LOGINCOUNT, NOMEUSUARIO, LOGINTYPE, NAME FROM USERS", conn);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var id = reader.GetInt32(0);
                        list.Add(new User() {
                            ID = id,
                            Password = reader.GetString(1),
                            Blocked = reader.GetString(2) == "X",
                            LoginCount = reader.GetInt32(3),
                            NickName = reader.GetString(4),
                            LoginType = (LoginType)reader.GetInt32(5),
                            Name = reader.GetString(6),
                        });
                    }
                }
            }

            return list;
        }

        public static string GenerateHash(string text)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(text);

            using (var hash = System.Security.Cryptography.SHA512.Create())
            {
                var hashedInputBytes = hash.ComputeHash(bytes);

                // Convert to text
                // StringBuilder Capacity is 128, because 512 bits / 8 bits in byte * 2 symbols for byte 
                var hashedInputStringBuilder = new System.Text.StringBuilder(128);
                foreach (var b in hashedInputBytes)
                {
                    hashedInputStringBuilder.Append(b.ToString("X2"));
                }

                return hashedInputStringBuilder.ToString();
            }
        }
    }
}