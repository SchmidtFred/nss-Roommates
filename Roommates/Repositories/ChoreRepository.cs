using Microsoft.Data.SqlClient;
using Roommates.Models;
using System.Collections.Generic;

namespace Roommates.Repositories
{
    public class ChoreRepository : BaseRepository
    {
        public ChoreRepository(string connectionString) : base(connectionString) { }

        public List<Chore> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name FROM Chore";

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Chore> chores = new List<Chore>();

                        while (reader.Read())
                        {
                            int idValue = reader.GetInt32(reader.GetOrdinal("Id"));
                            string nameValue = reader.GetString(reader.GetOrdinal("Name"));

                            Chore chore = new Chore
                            {
                                Id = idValue,
                                Name = nameValue
                            };

                            chores.Add(chore);
                        }
                        return chores;
                    }
                }
            }
        }

        public Chore GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Name FROM Chore WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Chore chore = null;

                        if (reader.Read())
                        {
                            chore = new Chore
                            {
                                Id = id,
                                Name = reader.GetString(reader.GetOrdinal("Name"))
                            };
                        }
                        return chore;
                    }
                }
            }
        }

        public void Insert(Chore chore)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Chore (Name)
                                                OUTPUT INSERTED.Id
                                                VALUES (@name)";
                    cmd.Parameters.AddWithValue("@name", chore.Name);
                    int id = (int)cmd.ExecuteScalar();

                    chore.Id = id;
                }
            }
        }

        public List<Chore> GetUnassignedChores()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT c.Name, c.Id 
                                        FROM Chore c 
                                        LEFT JOIN RoommateChore rc ON rc.ChoreId = c.Id 
                                        LEFT JOIN Roommate r ON rc.RoommateId = r.Id
                                        WHERE rc.Id IS NULL";
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Chore> chores = new List<Chore>();

                        while (reader.Read())
                        {
                            string nameValue = reader.GetString(reader.GetOrdinal("Name"));
                            int idValue = reader.GetInt32(reader.GetOrdinal("Id"));

                            Chore chore = new Chore
                            {
                                Id = idValue,
                                Name = nameValue
                            };

                            chores.Add(chore);
                        }

                        return chores;
                    }
                }
            }
        }

        public void AssignChore(int roommateId, int choreId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO RoommateChore (ChoreId, RoommateId)
                                                VALUES (@choreId, @roommateId)";
                    cmd.Parameters.AddWithValue("@choreId", choreId);
                    cmd.Parameters.AddWithValue("@roommateId", roommateId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(Chore chore)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Chore
                                         SET Name = @name
                                         WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@name", chore.Name);
                    cmd.Parameters.AddWithValue("@id", chore.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Room WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
