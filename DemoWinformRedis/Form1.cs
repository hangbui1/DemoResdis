using DemoWinformRedis.Models;
using ServiceStack.Redis.Generic;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ServiceStack.Redis;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using DemoWinformRedis.Dictionary;

namespace DemoWinformRedis
{
    public partial class Form1 : Form
    {

        DataTable dt = new DataTable();

        public Form1()
        {

            IDatabase db = RedisConnectorHelper.Connection.GetDatabase();
            InitializeComponent();
            var employees = new RedisDictionary<int, Employee>("employees");

            if (db.Database < 1)
            {
                employees.Add(100, new Employee() { Id = "100", Name = "John", AddressEmail = "email1", DateJoin = DateTime.Now.AddYears(-2) });
                employees.Add(200, new Employee() { Id = "200", Name = "Peter", AddressEmail = "email2", DateJoin = DateTime.Now.AddMonths(-15) });
                employees.Add(300, new Employee() { Id = "300", Name = "Hanna", AddressEmail = "email3", DateJoin = DateTime.Now.AddMonths(-10) });
            }

            ICollection<Employee> values = employees.Values;

            dt.Columns.Add("ID");
            dt.Columns.Add("Name");
            dt.Columns.Add("AddressEmail");
            dt.Columns.Add("Date Join");
            foreach (var oItem in values)
            {
                dt.Rows.Add(new object[] { oItem.Id, oItem.Name, oItem.AddressEmail, oItem.DateJoin.ToShortDateString() });
            }
            grdListEmployee.DataSource = dt;
            grdListEmployee.Columns[0].Width = 70;
            grdListEmployee.Columns[1].Width = 150;
            grdListEmployee.Columns[2].Width = 150;
            grdListEmployee.Columns[3].Width = 100;
        }

        

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtId.Text.ToString()) && string.IsNullOrEmpty(txtName.Text.ToString()))
            {
                MessageBox.Show("Please check! \nValue ID or Name incorrect!", "Not Success");
            }
            else
            {
                dt.Rows.Add(new object[] { txtId.Text.ToString(), txtName.Text.ToString(), txtAddressEmail.Text.ToString(), dtDateJoin.Value.ToString("yyyy/MM/dd") });
                grdListEmployee.DataSource = dt;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            var employees = new RedisDictionary<int, Employee>("employees");

            foreach (DataRow dr in dt.Rows)
            {
                employees.Add(int.Parse(dr[0].ToString()), new Employee() { Id = dr[0].ToString(), Name = dr[1].ToString(), AddressEmail = dr[2].ToString(), DateJoin = DateTime.Parse(dr[3].ToString()) });
            }
        }
    }
}
