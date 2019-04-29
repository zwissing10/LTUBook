using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using System.Data.SqlClient;

namespace LTUBook.Account
{
    public partial class Page : System.Web.UI.Page
    {
        SqlConnection db;
        protected void Page_Load(object sender, EventArgs e)
        {
            string userPageID;
            bool loggedUser = false;
            if (Request.QueryString["id"] == "0" || !Request.QueryString.AllKeys.Contains("id"))
            {
                userPageID = User.Identity.GetUserId();
                loggedUser = true;
            }
            else
            {
                userPageID = Request.QueryString["id"].ToString();
                loggedUser = false;
            }

            db = new SqlConnection("Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog=aspnet-LTUBook-20190228033437;Integrated Security=True");
            db.Open();
            
            generateNotifTable(userPageID, loggedUser);
        }

        protected void generateNotifTable(string id, bool loggedUser)
        {
            TableHeaderRow header = new TableHeaderRow();
            header.Cells.Add(new TableHeaderCell { CssClass = "text-center", Text = "Notifications" });
            header.Cells.Add(new TableHeaderCell { CssClass = "text-center", Text = "Date Posted" });
            NotifTable.Rows.Add(header);

            SqlCommand cmd = new SqlCommand("SELECT TOP (100) [Id],[FullName],[UserId],[CreationUserId],[Content],[FriendReq],[DateCreated] FROM Notifications, AspNetUsers WHERE UserId = '" + id + "' ORDER BY [DateCreated] DESC;", db);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                string fullName = dr.GetValue(1).ToString();
                if (fullName != null)
                {
                    userLabel.Text = fullName + "'s Page";
                }
                string createdUser = dr.GetValue(3).ToString();
                string NotifBody = dr.GetValue(4).ToString();
                string isFriendReq = dr.GetValue(5).ToString();
                string dateCreated = dr.GetValue(6).ToString();

                TableRow row = new TableRow();

                if (loggedUser)
                {
                    if (isFriendReq.CompareTo("0") == 0)
                    {
                        row.Cells.Add(new TableCell { Text = createdUser + " posted to your page: " + NotifBody });
                    }
                    else
                    {
                        row.Cells.Add(new TableCell
                        {
                            Text = createdUser + " added you as a friend! <asp:Button runat = \"server\" OnClick = \"AddFriend_Click\" Text = \"Add Friend\" CssClass = \"btn btn-default\" /> " +
                            "<asp:Button runat = \"server\" OnClick = \"DeleteRequest_Click\" Text = \"Delete Request\" CssClass = \"btn btn-default\" />"
                        });
                    }
                }
                else
                {
                    if (isFriendReq.CompareTo("0") == 0)
                    {
                        row.Cells.Add(new TableCell { Text = createdUser + " posted to" + userLabel.Text.ToString() + "'s page: " + NotifBody });
                    }
                }
                row.Cells.Add(new TableCell { Text = dateCreated });
                NotifTable.Rows.Add(row);
            }
            db.Close();
        }
    }
}