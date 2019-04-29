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

namespace LTUBook.Account
{
    public partial class Search : System.Web.UI.Page
    {
        SqlConnection db;
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void userSearch(object sender, EventArgs e)
        {
            db = new SqlConnection("Data Source = (LocalDb)\\MSSQLLocalDB;Initial Catalog=aspnet-LTUBook-20190228033437;Integrated Security=True");
            db.Open();

            TableHeaderRow header = new TableHeaderRow();
            header.Cells.Add(new TableHeaderCell { CssClass = "text-center", Text = "User Results" });
            SearchTable.Rows.Add(header);

            string searchName = SearchBox.Text;
            SqlCommand cmd = new SqlCommand("SELECT * FROM AspNetUsers WHERE FullName LIKE '%" + searchName + "%';", db);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                string queriedName = dr.GetValue(15).ToString();
                if (queriedName != null)
                {
                    TableRow row = new TableRow();
                    row.Cells.Add(new TableCell { Text = queriedName });
                    SearchTable.Rows.Add(row);
                }
            }
            dr.Close();
            db.Close();
        }
    }
}