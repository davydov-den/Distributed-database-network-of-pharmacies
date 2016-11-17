using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
namespace Database
{
    public partial class ListOrder : Form
    {
        OleDbConnection connLocal;
        OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=../../../../mainBase.accdb");
        public int numberBranch;    
        public ListOrder()
        {
            InitializeComponent();
        }

        private void List_Load(object sender, EventArgs e)
        {
            connLocal = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=../../../../localBase" + numberBranch + ".accdb");

            int i = 0;
            dataGridView1.Rows.Clear();
            connLocal.Open();
            conn.Open();
            OleDbCommand cmd = new OleDbCommand("Select * From Закупки", connLocal); //создаем команду
            OleDbCommand cmd2;
            OleDbDataReader dr2;
            using (OleDbDataReader dr = cmd.ExecuteReader())
            { //выполняем
                while (dr.Read())
                { //читаем результат
                    dataGridView1.Rows.Add();
                    dataGridView1[0, i].Value = dr.GetValue(1).ToString();
                    dataGridView1[1, i].Value = dr.GetValue(2).ToString();
                    dataGridView1[2, i].Value = dr.GetValue(3).ToString();
                    cmd2 = new OleDbCommand("Select Название From Лекарства where Код = " + dataGridView1[0, i].Value.ToString(), conn);
                    dr2 = cmd2.ExecuteReader();
                    dr2.Read();
                    dataGridView1[0, i].Value = dr2.GetValue(0);
                    dataGridView1.Rows[i].ReadOnly = true;
                    i++;
                }
            }
            conn.Close();
            connLocal.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            connLocal.Open();
            OleDbCommand cmd = new OleDbCommand("Delete FROM Закупки", connLocal); //создаем команду
            cmd.ExecuteNonQuery();
            connLocal.Close();
        }
    }
}
