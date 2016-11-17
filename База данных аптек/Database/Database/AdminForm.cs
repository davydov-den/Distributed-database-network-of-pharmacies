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
    public partial class AdminForm : Form
    {
        OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=../../../../mainBase.accdb");
        public string AdminLogin;
        int rows;
        public AdminForm()
        {
            InitializeComponent();
        }

        private void AdminForm_Load(object sender, EventArgs e)
        {
            dataGridView1.ContextMenuStrip = contextMenuStrip1;
            load(true);
        }

        string getCommand(bool type)
        {
            bool check = false;
            string command = "Select * From Лекарства";
            if (type)
                return command;
            command += " where ";
            if (textBox1.Text != "")
            {
                command += " Название = '" + textBox1.Text + "'";
                check = true;
            }
            if (textBox2.Text != "")
            {
                if (check)
                    command += " and ";
                command += " Производитель = '" + textBox2.Text + "'";
                check = true;
            }
            if (textBox3.Text != "")
            {
                if (check)
                    command += " and ";
                command += " Описание = '" + textBox3.Text + "'";
                check = true;
            }
            if (textBox4.Text != "")
            {
                if (check)
                    command += " and ";
                command += " Категория = '" + textBox4.Text + "'";
                check = true;
            }
            if (textBox5.Text != "")
            {
                if (check)
                    command += " and ";
                command += " Форм-фактор = '" + textBox5.Text + "'";
                check = true;
            }
            if (textBox6.Text != "")
            {
                if (check)
                    command += " and ";
                command += " Объем = " + textBox6.Text;
                check = true;
            }
            if (textBox7.Text != "")
                command += "and Цена = " + textBox7.Text;
            return command;
        }

        void load(bool type)
        {
            int i = 0;
            dataGridView1.Rows.Clear();
            conn.Open();
            OleDbCommand cmd = new OleDbCommand(getCommand(type), conn); //создаем команду
            using (OleDbDataReader dr = cmd.ExecuteReader())
            { //выполняем
                while (dr.Read())
                { //читаем результат
                    dataGridView1.Rows.Add();
                    dataGridView1[0, i].Value = dr.GetValue(1).ToString();
                    dataGridView1[1, i].Value = dr.GetValue(2).ToString();
                    dataGridView1[2, i].Value = dr.GetValue(3).ToString();
                    dataGridView1[3, i].Value = dr.GetValue(4).ToString();
                    dataGridView1[4, i].Value = dr.GetValue(5).ToString();
                    dataGridView1[5, i].Value = dr.GetValue(6).ToString();
                    dataGridView1[6, i].Value = dr.GetValue(7).ToString();
                    dataGridView1[7, i].Value = dr.GetValue(0).ToString();
                    dataGridView1.Rows[i].ReadOnly = true;
                    i++;
                }
            }
            conn.Close();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" && textBox2.Text == "" && textBox3.Text == "" && textBox4.Text == "" && textBox5.Text == "" && textBox6.Text == "" && textBox7.Text == "")
                load(true);
            else
                load(false);
        }

        private void уToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Действительно хотите удалить?", "Предупреждение", MessageBoxButtons.OKCancel, MessageBoxIcon.Stop) == DialogResult.OK)
            {
                conn.Open();
                OleDbCommand cmd = new OleDbCommand("Delete from Лекарства where Код = " + dataGridView1[7, rows].Value.ToString(), conn); //создаем команду
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            load(true);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            rows = e.RowIndex;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Действительно хотите сохранить?", "Предупреждение", MessageBoxButtons.OKCancel, MessageBoxIcon.Stop) == DialogResult.OK)
            {
                string command = "Update Лекарства set ";
                command += " Название = '" + dataGridView1[0, rows].Value.ToString() + "',";
                command += " Производитель = '" + dataGridView1[1, rows].Value.ToString() + "',";
                command += " Описание = '" + dataGridView1[2, rows].Value.ToString() + "',";
                command += " Категория = '" + dataGridView1[3, rows].Value.ToString() + "',";
                command += " [Форм-фактор] = '" + dataGridView1[4, rows].Value.ToString() + "',";
                command += " Объем = " + dataGridView1[5, rows].Value.ToString()+",";
                command += " Цена = " + dataGridView1[6, rows].Value.ToString();
                command += " where Код = " + dataGridView1[7, rows].Value.ToString() +";";
                conn.Open();
                OleDbCommand cmd = new OleDbCommand(command, conn); //создаем команду
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        private void редактироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows[rows].ReadOnly = false;
        }

        private void созданиеОтчетаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report newReport = new Report();
            newReport.ShowDialog();
        }
    }
}
