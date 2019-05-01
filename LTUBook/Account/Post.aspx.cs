using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LTUBook.Account
{
    public partial class Post : System.Web.UI.Page
    {
        string currSelectedUser;
        List<string> userIdList = new List<string>();
        SqlConnection db = new SqlConnection("Data Source = (LocalDb)\\MSSQLLocalDB;Initial Catalog=aspnet-LTUBook-20190228033437;Integrated Security=True");

        protected void Page_Load(object sender, EventArgs e)
        {
            db.Open();

            string currUserId = User.Identity.GetUserId();

            SqlCommand cmd = new SqlCommand("SELECT [FriendId] FROM Friends WHERE UserId = '" + currUserId + "';", db);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                userIdList.Add(dr.GetValue(0).ToString());
            }
            dr.Close();

            currSelectedUser = userIdList.ElementAt(0);

            cmd.CommandText = "SELECT [FullName] FROM AspNetUsers WHERE Id in ('" + string.Join("','", userIdList) + "');";
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                UserList.Items.Add(dr.GetValue(0).ToString());
            }
            dr.Close();
            db.Close();
        }

        protected void UserList_SelectionChanged(object sender, EventArgs e)
        {
            int index = UserList.SelectedIndex;
            currSelectedUser = userIdList.ElementAt(index);
        }

        protected void post(object sender, EventArgs e)
        {
            db.Open();

            string currUserId = User.Identity.GetUserId();
            string recUserId = currSelectedUser;
            string postContent = PostContent.Text;
            string insVals = "'" + recUserId + "','" + currUserId + "','" + postContent + "',0,'" + DateTime.Now + "'";

            SqlCommand cmd = new SqlCommand("INSERT INTO Notifications(UserId, CreationUser, Content, FriendReq, DateCreated) VALUES (" + insVals + ");", db);
            int rowsAffected = cmd.ExecuteNonQuery();
            if (rowsAffected != 1)
            {
                throw new Exception("Query to create Post returned " + rowsAffected + " affected rows");
            }
            PostContent.Text = "";
            db.Close();
        }
    }
}