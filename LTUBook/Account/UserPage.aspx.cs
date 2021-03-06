﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using System.Data.SqlClient;

namespace LTUBook.Account
{
    public partial class UserPage : Page
    {
        SqlConnection db;
        string userPageID;
        bool loggedUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            loggedUser = false;
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

            SqlCommand cmd = new SqlCommand("SELECT [FullName] FROM AspNetUsers WHERE Id = '" + userPageID + "';", db);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                string fullName = dr.GetValue(0).ToString();
                if (fullName != null)
                {
                    userLabel.Text = fullName + "'s Page";
                }
            }
            dr.Close();
            db.Close();

            generateNotifTable(userPageID, loggedUser);
            GenerateFriendList(userPageID, loggedUser, sender, e);
        }

        protected void generateNotifTable(string id, bool loggedUser)
        {
            TableHeaderRow header = new TableHeaderRow();
            header.Cells.Add(new TableHeaderCell { CssClass = "text-center", Text = "Notifications", ColumnSpan=2 });
            header.Cells.Add(new TableHeaderCell { CssClass = "text-center", Text = "Date Posted" });
            NotifTable.Rows.Add(header);

            db = new SqlConnection("Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog=aspnet-LTUBook-20190228033437;Integrated Security=True");
            db.Open();
            SqlCommand cmd = new SqlCommand("SELECT [UserId], [CreationUser], [FullName] as [CreatedName], [Content], [FriendReq], [DateCreated] FROM Notifications n JOIN AspNetUsers u ON n.CreationUser = u.Id WHERE UserId = '" + id + "' ORDER BY DateCreated DESC;", db);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                string createdUserId = dr.GetValue(1).ToString();
                string createdUser = dr.GetValue(2).ToString();
                string NotifBody = dr.GetValue(3).ToString();
                string isFriendReq = dr.GetValue(4).ToString();
                string dateCreated = dr.GetValue(5).ToString();

                TableRow row = new TableRow();

                if (loggedUser)
                {
                    if (isFriendReq.CompareTo("0") == 0)
                    {
                        row.Cells.Add(new TableCell { Text = createdUser + " posted to your page: " + NotifBody, ColumnSpan = 2 });
                    }
                    else
                    {
                        Button button = new Button { Text = "Add Friend", CssClass = "btn btn-default", ID = createdUserId + "_add" };
                        button.Click += AddFriend_Click;
                        Button button2 = new Button { Text = "Delete Request", CssClass = "btn btn-default", ID = createdUserId + "_delete" };
                        button2.Click += DeleteRequest_Click;
                        TableCell tc = new TableCell { HorizontalAlign = HorizontalAlign.Right };
                        tc.Controls.Add(button);
                        tc.Controls.Add(button2);
                        row.Cells.Add(new TableCell { Text = createdUser + " added you as a friend!", VerticalAlign = VerticalAlign.Middle });
                        row.Cells.Add(tc);
                    }
                    row.Cells.Add(new TableCell { Text = dateCreated });
                }
                else
                {
                    if (isFriendReq.CompareTo("0") == 0)
                    {
                        row.Cells.Add(new TableCell { Text = createdUser + " posted to" + userLabel.Text.ToString() + "'s page: " + NotifBody, ColumnSpan = 2 });
                        row.Cells.Add(new TableCell { Text = dateCreated });
                    }
                }
                NotifTable.Rows.Add(row);
            }
            db.Close();
        }

        protected void GenerateFriendList(string id, bool user, object sender, EventArgs e)
        {
            TableHeaderRow header = new TableHeaderRow();
            header.Cells.Add(new TableHeaderCell { CssClass = "text-center", Text = "Friends" });

            db = new SqlConnection("Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog=aspnet-LTUBook-20190228033437;Integrated Security=True");
            db.Open();

            List<string> nonLoggedFriends = new List<string>(); 
            if (!loggedUser)
            {
                header.Cells.Add(new TableHeaderCell { CssClass = "text-center", Text = "Add Friend" });
                SqlCommand cmd2 = new SqlCommand("SELECT [FriendId] FROM Friends WHERE UserId = '" + User.Identity.GetUserId() + "';", db);
                SqlDataReader dr2 = cmd2.ExecuteReader();
                //FIX WHERE CURRENTLY LOGGED IN USER SHOWS UP
                while (dr2.Read())
                {
                    nonLoggedFriends.Add(dr2.GetValue(0).ToString());
                }
                dr2.Close();
            }
            else
            {
                header.Cells.Add(new TableHeaderCell { CssClass = "text-center", Text = "Delete" });
            }
            FriendTable.Rows.Add(header);

            SqlCommand cmd = new SqlCommand("SELECT [FullName], [FriendId] FROM AspNetUsers n JOIN Friends f ON n.Id = f.FriendId WHERE UserId = '" + id + "';", db);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                string friendName = dr.GetValue(0).ToString();
                string friendId = dr.GetValue(1).ToString();

                TableRow row = new TableRow();

                if (loggedUser)
                {
                    row.Cells.Add(new TableCell { Text = friendName });
                    Button button3 = new Button { Text = "Delete Friend", CssClass = "btn btn-default", ID = friendId + "_deletefriend" };
                    button3.Click += DeleteFriend_Click;
                    TableCell tc2 = new TableCell { HorizontalAlign = HorizontalAlign.Right };
                    tc2.Controls.Add(button3);
                    row.Cells.Add(tc2);
                }
                else
                {
                    if (friendId == User.Identity.GetUserId())
                    {
                    }
                    else if (!nonLoggedFriends.Contains(friendId))
                    {
                        row.Cells.Add(new TableCell { Text = friendName });
                        Button button3 = new Button { Text = "Add Friend", CssClass = "btn btn-default", ID = friendId + "_sendReq" };
                        button3.Click += SendReq_Click;
                        TableCell tc2 = new TableCell { HorizontalAlign = HorizontalAlign.Right };
                        tc2.Controls.Add(button3);
                        row.Cells.Add(tc2);
                    }
                    else
                    {
                        row.Cells.Add(new TableCell { Text = friendName });
                        Button button3 = new Button { Text = "Already Friends!", CssClass = "btn btn-default disabled" };
                        TableCell tc2 = new TableCell { HorizontalAlign = HorizontalAlign.Right };
                        tc2.Controls.Add(button3);
                        row.Cells.Add(tc2);
                    }
                }
                FriendTable.Rows.Add(row);
            }
            db.Close();
        }

        protected void AddFriend_Click(object sender, EventArgs e)
        {
            db.Open();

            Button button = (Button)sender;
            string friendId = button.ID;
            string userId = User.Identity.GetUserId();

            string[] splitStr = friendId.Split('_');
            friendId = splitStr[0];

            //Add entries into friends table for both users
            SqlCommand cmd = new SqlCommand("INSERT INTO Friends(UserId, FriendId) VALUES ('" + userId + "','" + friendId + "');", db);
            int rowsAffected = cmd.ExecuteNonQuery();
            if (rowsAffected != 1)
            {
                throw new Exception("Query to create friend row returned " + rowsAffected + " affected rows");
            }
            cmd.CommandText = "INSERT INTO Friends(UserId, FriendId) VALUES ('" + friendId + "','" + userId + "');";
            rowsAffected = cmd.ExecuteNonQuery();
            if (rowsAffected != 1)
            {
                throw new Exception("Query to create friend row returned " + rowsAffected + " affected rows");
            }

            //Delete request from notifications
            cmd.CommandText = "DELETE FROM Notifications WHERE UserId = '" + userId + "' AND CreationUser = '" + friendId + "' AND FriendReq = 1";
            rowsAffected = cmd.ExecuteNonQuery();
            if (rowsAffected != 1)
            {
                throw new Exception("Query to delete FR returned " + rowsAffected + " affected rows");
            }
            Server.TransferRequest(Request.Url.AbsolutePath, false);
            db.Close();
        }

        protected void DeleteRequest_Click(object sender, EventArgs e)
        {
            db.Open();

            Button button = (Button)sender;
            string senderId = button.ID;
            string userId = User.Identity.GetUserId();

            string[] splitStr = senderId.Split('_');
            senderId = splitStr[0];

            SqlCommand cmd = new SqlCommand("DELETE FROM Notifications WHERE UserId = '" + userId + "' AND CreationUser = '" + senderId + "' AND FriendReq = 1", db);
            int rowsAffected = cmd.ExecuteNonQuery();
            if (rowsAffected != 1)
            {
                throw new Exception("Query to delete FR returned " + rowsAffected + " affected rows");
            }
            Server.TransferRequest(Request.Url.AbsolutePath, false);
            db.Close();
        }

        protected void DeleteFriend_Click(object sender, EventArgs e)
        {
            db.Open();

            Button button = (Button)sender;
            string senderId = button.ID;
            string userId = User.Identity.GetUserId();

            string[] splitStr = senderId.Split('_');
            senderId = splitStr[0];

            SqlCommand cmd = new SqlCommand("DELETE FROM Friends WHERE (FriendId = '" + senderId + "' AND UserId = '" + userId + "') OR (UserId = '" + senderId + "' AND FriendId = '" + userId + "')", db);
            int rowsAffected = cmd.ExecuteNonQuery();
            if (rowsAffected != 2)
            {
                throw new Exception("Query to delete Friend returned " + rowsAffected + " affected rows");
            }
            Server.TransferRequest(Request.Url.AbsolutePath, false);
            db.Close();
        }

        protected void SendReq_Click(object sender, EventArgs e)
        {
            db = new SqlConnection("Data Source = (LocalDb)\\MSSQLLocalDB;Initial Catalog=aspnet-LTUBook-20190228033437;Integrated Security=True");
            db.Open();

            Button button = (Button)sender;
            string[] splitStr = button.ID.ToString().Split('_');
            string recUserId = splitStr[0];
            string senderId = User.Identity.GetUserId();
            string insVals = "'" + recUserId + "','" + senderId + "','',1,'" + DateTime.Now.ToString() + "'";

            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) as [NotificationCount] FROM Notifications WHERE UserId = '" + recUserId + "' AND CreationUser = '" + senderId + "' AND FriendReq = 1;", db);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
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
            if (rowsAffected != 1)
            {
                throw new Exception("Query to create FR returned " + rowsAffected + " affected rows");
            }

            button.Enabled = false;
        }
    }
}