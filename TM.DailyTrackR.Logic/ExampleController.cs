namespace TM.DailyTrackR.Logic
{
    using System.Data;
    using System.Data.SqlClient;

  public sealed class ExampleController
  {
    string connectionString = @"Server=.\TM_DAILY_TRACKR;Database=TRACKR_DATA;Integrated Security=true;";

    public int GetDataExample()
    {
       string procedureName = "tm.GetAllProjectTypes";

            using (SqlConnection connection = new SqlConnection(connectionString))

      try
      {
                using (SqlCommand command = new SqlCommand(procedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();

                        //command.Parameters.AddWithValue("@param1", value1);
                        //command.Parameters.AddWithValue("@param1", value2);
                         // megcsinálni, hogy mukodjon a beszurasos Procedura

                    SqlDataReader reader = command.ExecuteReader();
                Dictionary<int, string> result = new Dictionary<int, string>();
                    while (reader.Read())
                    {
                        result.Add((int)reader["project_type_id"], (string)reader["project_type_description"]);
                    }
                }
      }
      catch (Exception ex)
      {
        Console.WriteLine("An error occurred: " + ex.Message);
      }

      return 0;
    }

        public int InsertProjectType()
        {
            string procedureName = "tm.InsertNewProjectType";
            string value1 = "TestingProjectTypeInsertion";


            using (SqlConnection connection = new SqlConnection(connectionString))

                try
                {
                    using (SqlCommand command = new SqlCommand(procedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();

                        command.Parameters.AddWithValue("@ProjectTypeDescription", value1);
                        //command.Parameters.AddWithValue("@param1", value2);
                        // megcsinálni, hogy mukodjon a beszurasos Procedura

                        command.ExecuteNonQuery();
                        Console.WriteLine("Insertion successful.");


                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }

            return 0;
        }

        public int InsertNewActivity()
        {
            string procedureName = "tm.InsertNewActivity";

          
            int projectTypeId = 6;
            int activityTypeId = 1;
            string description = "TestingProjectTypeInsertion Activity 1";
            int statusId = 1;
            string userName = "User Z";
            DateTime creationDate = new DateTime(2024, 6, 17);

            using (SqlConnection connection = new SqlConnection(connectionString))

                try
                {
                using (SqlCommand command = new SqlCommand(procedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();

                    command.Parameters.AddWithValue("@ProjectTypeId", projectTypeId);
                    command.Parameters.AddWithValue("@ActivityTypeId", activityTypeId);
                    command.Parameters.AddWithValue("@Description", description);
                    command.Parameters.AddWithValue("@StatusId", statusId);
                    command.Parameters.AddWithValue("@UserName", userName);
                    command.Parameters.AddWithValue("@CreationDate", creationDate);

                    command.ExecuteNonQuery();

                    Console.WriteLine("Insertion successful.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }

            return 0;
        }

    }
}
