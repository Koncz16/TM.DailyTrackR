using System.Windows;

namespace TM.DailyTrackR.Logic
{
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using TM.DailyTrackR.DataType.Enums;
    using TM.DailyTrackR.DataType.Models;

    public sealed class ExampleController
  {
    string connectionString = @"Server=.\TM_DAILY_TRACKR;Database=TRACKR_DATA;Integrated Security=true;";

        string userName = "User A";
    public int GetDataExample()
    {
       string procedureName = "tm.GetAllProjectTypes";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
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
            }
      return 0;
    }


  
        public int InsertProjectType()
        {
            string procedureName = "tm.InsertNewProjectType";
            string value1 = "TestingProjectTypeInsertion";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
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
                    return -1;
                }
            }
            return 0;
        }

        public int InsertNewActivity(int projectTypeId, int activityTypeId, string description, int statusId, DateTime creationDate)
        {
            string procedureName = "tm.InsertNewActivity";


            //int projectTypeId = 6;
            //int activityTypeId = 1;
            //string description = "TestingProjectTypeInsertion Activity 1";
            //int statusId = 1;
            //string userName = "User Z";
            //DateTime creationDate = new DateTime(2024, 6, 17);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
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
                        command.Parameters.AddWithValue("@UserName", this.userName);
                        command.Parameters.AddWithValue("@CreationDate", creationDate);

                        command.ExecuteNonQuery();

                        Console.WriteLine("Insertion successful.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                    return -1;
                }

            }
            return 0;

        }

        public int UpdateActivity(ActivityModel activity)
        {
            string updateProcedure = "tm.UpdateActivityById";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    using (SqlCommand command = new SqlCommand(updateProcedure, connection))
                    {

                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();
                        ProjectTypeEnum projectType = (ProjectTypeEnum)Enum.Parse(typeof(ProjectTypeEnum), activity.ProjectTypeDescription);
                        int projectTypeInt = (int)projectType;
                        command.Parameters.AddWithValue("@description", activity.Description);
                        command.Parameters.AddWithValue("@status_id", (int)activity.StatusId);
                        command.Parameters.AddWithValue("@project_type_id", projectType);
                        command.Parameters.AddWithValue("@activity_type_id", (int)activity.ActivityTypeId);
                        command.Parameters.AddWithValue("@id", activity.Id);

                        command.ExecuteNonQuery();

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                return 0;
            }
        }


        public int DeleteActivity(int id)
        {
            string deleteProcedure = "tm.DeleteActivityById";
            int rowsAffected = 0;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    using (SqlCommand command = new SqlCommand(deleteProcedure, con))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        con.Open();
                        command.Parameters.AddWithValue("@id", id);

                      command.ExecuteNonQuery();
                  

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return -1;
                }
            }


            return 0;
        }



        public List<ActivityModel> GetUserActivities(string username, DateTime date)
        {
            string getDailyTaskProcedure = "tm.GetActivitiesForUserBySpecificDate";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    using (SqlCommand command = new SqlCommand(getDailyTaskProcedure, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();
                        command.Parameters.AddWithValue("@UserName", username);
                        command.Parameters.AddWithValue("@SpecificDate", date);
                 
                        SqlDataReader reader = command.ExecuteReader();
                        Console.WriteLine("Request Succefull");

                        List<ActivityModel> activities = new();
                        while (reader.Read())
                        {
                            Console.WriteLine(reader[0]);
                            ActivityModel activity = new ActivityModel
                            {
                                Id = (int)reader["id"],
                                Description = (string)reader["activity_description"],
                                ProjectTypeDescription = (string)reader["project_type_description"],
                                UserName = this.userName,
                                ActivityTypeId = (TaskTypeEnum)(int)reader["activity_type_id"],
                                StatusId = (StatusEnum)(int)reader["status_id"]
                            };
                            activities.Add(activity);
                        }
                        Console.WriteLine(activities.Count);

                        return activities;
                       
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                    return new List<ActivityModel>();
                }
            }

        }
        public List<ActivityModel> GetActivitiesBetweenDates(DateTime startdate, DateTime endDate)
        {
            string getActivitiesByDateRange = "tm.GetActivitiesByDateRange";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    using (SqlCommand command = new SqlCommand(getActivitiesByDateRange, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();
                        command.Parameters.AddWithValue("@StartDate", startdate);
                        command.Parameters.AddWithValue("@EndDate", endDate);
                        command.ExecuteNonQuery();
                        SqlDataReader reader = command.ExecuteReader();
                        List<ActivityModel> activitiesByDateRange = new();
                        while (reader.Read())
                        {
                            ActivityModel activity = new ActivityModel
                            {
                                Id = (int)reader["id"],
                                Description = (string)reader["activity_description"],
                                ProjectTypeDescription = (string)reader["project_type_description"],
                                ActivityTypeId = (TaskTypeEnum)(int)reader["activity_type_id"],
                                StatusId = (StatusEnum)(int)reader["status_id"]
                            };
                            activitiesByDateRange.Add(activity);
                        }
                        return activitiesByDateRange;
                    };
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                    return new List<ActivityModel>();
                }
            }
        }

        public List<ActivityModel> GetAllActivities(DateTime date)
        {
            string getDailyTaskForAllProcedure = "tm.GetAllActivitiesByDate";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    using (SqlCommand command = new SqlCommand(getDailyTaskForAllProcedure, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();
                        command.Parameters.AddWithValue("@SpecificDate", date);
                 

                        SqlDataReader reader = command.ExecuteReader();
                        List<ActivityModel> activities = new();
                        while (reader.Read())
                        {
                            ActivityModel activity = new ActivityModel
                            {
                                Id = (int)reader["id"],
                                Description = (string)reader["activity_description"],
                                ProjectTypeDescription = (string)reader["project_type_description"],
                                UserName = (string)reader["username"],
                                ActivityTypeId = (TaskTypeEnum)(int)reader["activity_type_id"],
                                StatusId = (StatusEnum)(int)reader["status_id"]
                            };
                            activities.Add(activity);
                        }
                        Console.WriteLine(activities.Count);
                        return activities;
                        
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                    return new List<ActivityModel>();
                }
            };

        }


        public List<ActivityModel> GetUserAllActivities(string username)
        {
            string getDailyTaskProcedure = "tm.GetAllActivitiesForUser";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    using (SqlCommand command = new SqlCommand(getDailyTaskProcedure, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();
                        command.Parameters.AddWithValue("@UserName", username  );

                        SqlDataReader reader = command.ExecuteReader();
                        Console.WriteLine("Request Succefull");

                        List<ActivityModel> activities = new();
                        while (reader.Read())
                        {
                            Console.WriteLine(reader[0]);
                            ActivityModel activity = new ActivityModel
                            {
                                Id = (int)reader["id"],
                                Description = (string)reader["activity_description"],
                                ProjectTypeDescription = (string)reader["project_type_description"],
                                UserName = username,
                                ActivityTypeId = (TaskTypeEnum)(int)reader["activity_type_id"],
                                StatusId = (StatusEnum)(int)reader["status_id"]
                            };
                            activities.Add(activity);
                        }
                        Console.WriteLine(activities.Count);

                        return activities;

                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                    return new List<ActivityModel>();
                }
            }

        }

        public UserModel GetUserByName(string username)
        {
            string getDailyTaskProcedure = "tm.FindUserByName"; 

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    using (SqlCommand command = new SqlCommand(getDailyTaskProcedure, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@UserName", username);

                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read()) 
                        {
                            UserModel userModel = new UserModel
                            {
                                Id = (int)reader["id"],
                                Name = (string)reader["name"],
                                Password = (string)reader["password"],
                                RoleId = (RoleEnum)(int)reader["role_id"]
                            };

                            return userModel;
                        }
                        else
                        {
                            return new UserModel();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                    return new UserModel(); 
                }
            }
        }


    }
}
