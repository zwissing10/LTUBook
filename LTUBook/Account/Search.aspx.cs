using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using LTUBook.Models;
using Microsoft.AspNet.Identity;

namespace LTUBook.Account
{
    public partial class Search : System.Web.UI.Page
    {
        SqlConnection db;
        protected void Page_Load(object sender, EventArgs e)
        {
            if(IsPostBack)
            {
                userSearch(sender, e);
            }
        }

        protected void userSearch(object sender, EventArgs e)
        {
            db = new SqlConnection("Data Source = (LocalDb)\\MSSQLLocalDB;Initial Catalog=aspnet-LTUBook-20190228033437;Integrated Security=True");
            db.Open();

            TableHeaderRow header = new TableHeaderRow();
            header.Cells.Add(new TableHeaderCell { CssClass = "text-center", Text = "User Results", ColumnSpan = 2});
            SearchTable.Rows.Add(header);

            List<string> friends = new List<string>();
            SqlCommand cmd = new SqlCommand("SELECT [FriendId] FROM Friends WHERE UserId = '" + User.Identity.GetUserId() + "';", db);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                friends.Add(dr.GetValue(0).ToString());
            }
            dr.Close();

            string searchName = SearchBox.Text;
            cmd.CommandText = "SELECT * FROM AspNetUsers WHERE FullName LIKE '%" + searchName + "%';";
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                string queriedName = dr.GetValue(15).ToString();
                if (queriedName != null)
                { 
                    TableRow row = new TableRow();
                    if (friends.Contains(dr.GetValue(0).ToString()))
                    {
                        Button button = new Button { Text = "Already Friends!", CssClass = "btn btn-default disabled" };
                        Button button2 = new Button { Text = "View Page", CssClass = "btn btn-default", PostBackUrl = "~/Account/UserPage?id=" + dr.GetValue(0).ToString() };
                        TableCell tc = new TableCell { HorizontalAlign = HorizontalAlign.Right };
                        tc.Controls.Add(button);
                        tc.Controls.Add(button2);
                        row.Cells.Add(new TableCell { Text = queriedName, VerticalAlign = VerticalAlign.Middle });
                        row.Cells.Add(tc);
                        SearchTable.Rows.Add(row);
                    }
                    else
                    {
                        if (dr.GetValue(0).ToString().CompareTo(User.Identity.GetUserId()) != 0)
                        {
                            Button button = new Button { Text = "Send Friend Request", CssClass = "btn btn-default", ID = dr.GetValue(0).ToString() };
                            Button button2 = new Button { Text = "View Page", CssClass = "btn btn-default", PostBackUrl = "~/Account/UserPage?id=" + dr.GetValue(0).ToString() };
                            button.Click += SendReq_Click;
                            TableCell tc = new TableCell { HorizontalAlign = HorizontalAlign.Right };
                            tc.Controls.Add(button);
                            tc.Controls.Add(button2);
                            row.Cells.Add(new TableCell { Text = queriedName, VerticalAlign = VerticalAlign.Middle });
                            row.Cells.Add(tc);
                            SearchTable.Rows.Add(row);
                        }
                    }
                }
            }
            dr.Close();
            db.Close();
        }

        protected void SendReq_Click(object sender, EventArgs e)
        {
            db = new SqlConnection("Data Source = (LocalDb)\\MSSQLLocalDB;Initial Catalog=aspnet-LTUBook-20190228033437;Integrated Security=True");
            db.Open();

            Button button = (Button)sender;
            string recUserId = button.ID;
            string senderId = User.Identity.GetUserId();
            string insVals = "'" + recUserId + "','" + senderId + "','',1,'" + DateTime.Now.ToString() + "'";

            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) as [NotificationCount] FROM Notifications WHERE UserId = '" + recUserId + "' AND CreationUser = '" + senderId + "' AND FriendReq = 1;", db);
            SqlDataReader dr = cmd.ExecuteReader();
            while(dr.Read())
            {
                if (dr.GetValue(0).ToString().CompareTo("0") != 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Friend Request Already Sent!')", true);
                    return;
                }
            }
            dr.Close();

            cmd.CommandText = "INSERT INTO Notifications(UserId, CreationUser, Content, FriendReq, DateCreated) VALUES (" + insVals + ");";
            int rowsAffected = cmd.ExecuteNonQuery();
            if(rowsAffected != 1)
            {
                throw new Exception("Query to create FR returned " + rowsAffected + " affected rows");
            }

            button.Enabled = false;
        }

        /*protected void ViewPage_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            string[] splitStr = button.ID.ToString().Split('_');
            string userPageId = splitStr[0];

            
        }*/
    }
}