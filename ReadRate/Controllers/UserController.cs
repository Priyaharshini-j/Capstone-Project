using Microsoft.AspNetCore.Mvc;
using ReadRate.Models;
using System.Data;
using System.Data.SqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ReadRate.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        SqlConnection conn;
        private readonly IConfiguration _configuration;
        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost, Route("[action]", Name = "Login")]
        public UserModel Login (UserLogin user)
        {
            UserModel userModel = new UserModel();
            userModel.result = new Models.Results();
            try
            {
                if(user != null && !string.IsNullOrWhiteSpace(user.UserEmail) && !string.IsNullOrWhiteSpace(user.Password))
                {
                    conn = new SqlConnection(_configuration["ConnectionStrings:SqlConn"]);
                    using (conn)
                    {
                        SqlCommand cmd = new SqlCommand("ValidateLogin", conn);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserEmail", user.UserEmail);
                        cmd.Parameters.AddWithValue("@UserPassword", user.Password);
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            userModel.UserName = dt.Rows[0]["UserName"].ToString();
                            userModel.UserId =(int) dt.Rows[0]["UserId"];
                            userModel.Password = dt.Rows[0]["Password"].ToString();
                            userModel.SecurityQn = dt.Rows[0]["SecurityQn"].ToString();
                            userModel.SecurityAns = dt.Rows[0]["SecurityAns"].ToString();

                            userModel.result.result = true;
                            userModel.result.message = "success";
                        }
                        else
                        {
                            userModel.result.result = false;
                            userModel.result.message = "Invalid user";
                        }
                    }
                }
                else
                {
                    userModel.result.result = false;
                    userModel.result.message = "Please enter username and password";
                }
            }
            catch (Exception ex)
            {
                userModel.result.result = false;
                userModel.result.message = "Please enter username and password";
                ex.Message.ToString();
            }
            return userModel;
        }
        [HttpPost, Route("[action]",Name = "SignUp")]
        public Models.Results SignUp(UserModel userModel)
        {
            Models.Results result = new Models.Results();
            try
            {                  
                conn = new SqlConnection(_configuration["ConnectionStrings:SqlConn"]);
                using (conn)
                {
                    SqlCommand cmd = new SqlCommand("CreateUser", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserName", userModel.UserName);
                    cmd.Parameters.AddWithValue("@UserEmail", userModel.UserEmail);
                    cmd.Parameters.AddWithValue("@Password", userModel.Password);
                    cmd.Parameters.AddWithValue("@SecurityQn", userModel.SecurityQn);
                    cmd.Parameters.AddWithValue("@SecurityAns", userModel.SecurityAns);                       
                    try 
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        result.result = true;
                        result.message = "User Account Has been created successfully";
                       
                    }
                    catch(SqlException ex)
                    {
                        result.result = false;
                        result.message = ex.Message;
                        Console.WriteLine(ex.Message);
                    }
                }
                conn.Close();
            }
            catch(Exception ex)
            {
                result.result = false;
                result.message = "Error creating the Profile. Try Again Later...";
                Console.WriteLine(ex.Message);
            }
            return result;
        }

        [HttpPut , Route("[action]", Name = "EditProfile")]
        public Models.Results UpdateProfile(UserModel userModel)
        {
            Models.Results result = new Models.Results();
            try
            {
                if (userModel.UserName != null && userModel.UserEmail != null && userModel.Password != null && userModel.SecurityAns != null && userModel.SecurityQn != null)
                {

                    conn = new SqlConnection(_configuration["ConnectionStrings:SqlConn"]);
                    using (conn)
                    {
                        SqlCommand cmd = new SqlCommand("UpdateUser", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserId", userModel.UserId);
                        cmd.Parameters.AddWithValue("@Password", userModel.Password);
                        cmd.Parameters.AddWithValue("@SecurityQn", userModel.SecurityQn);
                        cmd.Parameters.AddWithValue("@SecurityAns", userModel.SecurityAns);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            result.result = true;
                            result.message = "User Account Has been Updated successfully";

                        }
                        else
                        {
                            result.result = false;
                            result.message = "Error in Updating the profile... Try Again";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.result = false;
                result.message = "Error Updating the Profile. Try Again Later...";
                ex.Message.ToString();
            }
            return result;
        }

        [HttpDelete, Route("[action]", Name = "DeleteProfile")]
        public Models.Results DeleteProfile(int UserId)
        {
            Models.Results result = new Models.Results();
            try
            {
                conn = new SqlConnection(_configuration["ConnectionStrings:SqlConn"]);
                using (conn)
                {
                    SqlCommand cmd = new SqlCommand("UpdateUser", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", UserId);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        result.result = true;
                        result.message = "User Account Has been Updated successfully";

                    }
                    else
                    {
                        result.result = false;
                        result.message = "Error in Updating the profile... Try Again";
                    }

                }
            }   
            catch (SqlException ex)
            {
                result.result = false;
                result.message = ex.Message;
                ex.Message.ToString();
            }
            return result;
        }

    }
}
