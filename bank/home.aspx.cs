using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;


public partial class home : System.Web.UI.Page
{
    MySqlConnection con = new MySqlConnection("server=localhost;user id=root;database=bank;persistsecurityinfo=True;password=Sejal2001");

    MySqlCommand cmd;
    MySqlCommand cmd2;
    MySqlCommand cmd3;
    MySqlCommand cm;
    protected void Page_Load(object sender, EventArgs e)
    {
        con.Open();
        cmd = new MySqlCommand("select * from user", con);
        MySqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            DropDownList1.Items.Add(reader["name"].ToString());
            DropDownList2.Items.Add(reader["name"].ToString());
          
        }
        con.Close();
    }
    public void BindData()
    {
         con.Open();
        cmd = new MySqlCommand("select * from user", con);
        MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        adp.Fill(ds);
        GridView1.DataSource = ds;
        GridView1.DataBind();
        cmd.Dispose();
        con.Close();

            }



    protected void Button2_Click(object sender, EventArgs e)
    {
        BindData();
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.Redirect("Home.aspx");
    }

    protected void Button3_Click(object sender, EventArgs e)
    {
        Response.Redirect("Transaction history.aspx");

    }

    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        int B = int.Parse(GridView1.SelectedRow.Cells[1].Text);
        Label0.Text = "ID :-" + B;
        String name = GridView1.SelectedRow.Cells[2].Text;
        Label1.Text = "Name :-" + name;
        String email = GridView1.SelectedRow.Cells[3].Text;
        Label2.Text = "Email :-" + email;
        int A = int.Parse(GridView1.SelectedRow.Cells[4].Text);
        Label3.Text = "Amount :-" + A;
        Label0.Visible = true;
        Label1.Visible = true;
        Label2.Visible = true;
        Label3.Visible = true;


    }

        protected void Button4_Click(object sender, EventArgs e)
    {
      
        con.Open();
        int x = int.Parse(TextBox1.Text);

        cm = new MySqlCommand( "select Balance from user where Name = @Sender" , con);
        cm.Parameters.AddWithValue("@Sender", DropDownList1.SelectedItem.Value);
        int Result = (int)cm.ExecuteScalar();
        int c = cm.ExecuteNonQuery();
        if (c != 0)
        {

            
            if (Result >= x)
            {
                string sqlquery = "insert into transaction (Sender,Receiver ,Amount) values (@Sender,@Receiver,@Amount)";
                cmd = new MySqlCommand(sqlquery, con);
                cmd.Parameters.AddWithValue("@Sender", DropDownList1.SelectedItem.Value);
                cmd.Parameters.AddWithValue("@Receiver", DropDownList2.SelectedItem.Value);
                cmd.Parameters.AddWithValue("@Amount", TextBox1.Text);


                string sqlquery3 = "update user set Balance=Balance+(@Amount) where Name = @Receiver ";
                cmd3 = new MySqlCommand(sqlquery3, con);
                cmd3.Parameters.AddWithValue("@Receiver", DropDownList2.SelectedItem.Value);
                cmd3.Parameters.AddWithValue("@Amount", TextBox1.Text);
                int a = cmd3.ExecuteNonQuery();

                string sqlquery2 = "update user set Balance = Balance-(@Amount) where Name = @Sender ";
                cmd2 = new MySqlCommand(sqlquery2, con);
                cmd2.Parameters.AddWithValue("@Sender", DropDownList1.SelectedItem.Value);
                cmd2.Parameters.AddWithValue("@Amount", TextBox1.Text);
                int b = cmd2.ExecuteNonQuery();

                if (a != 0 && b != 0)
                {
                    Response.Write("<script> alert('Transfer Successful')</script>");
                }
                else
                {
                    Response.Write("<script> alert('Transfer failed')</script>");
                }

                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    Response.Write("<script> alert('Transfer Successful')</script>");
                }
                else
                {
                    Response.Write("<script> alert('Transfer failed ')</script>");
                }
            }
            else
            {
                Response.Write("<script> alert('Balance is low than amount to transfer ')</script>");
            }
        }
        else
        {
            Response.Write("<script> alert('Transfer failed ')</script>");
        }

        con.Close();
    }
}