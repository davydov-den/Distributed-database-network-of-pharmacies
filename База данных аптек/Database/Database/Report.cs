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
    public partial class Report : Form
    {
        OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=../../../../mainBase.accdb");
        public Report()
        {
            InitializeComponent();
        }

        private void Report_Load(object sender, EventArgs e)
        {
            radioButton1.Checked = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int i = 0;
            string command;
            conn.Open();
            if (radioButton2.Checked)
            {
                dataGridView1.Visible = false;
                dataGridView2.Visible = true;
                dataGridView3.Visible = false;
                command = "SELECT Лекарства.Название,SUM (Продажи.Количество), SUM (Продажи.Количество*Лекарства.Цена) " +
                " FROM Лекарства INNER JOIN Продажи ON Лекарства.[Код] = Продажи.[Номер товара]" +
                " GROUP BY Лекарства.Название" +
                " ORDER BY SUM (Продажи.Количество*Лекарства.Цена) DESC;";
                dataGridView2.Rows.Clear();
                OleDbCommand cmd = new OleDbCommand(command, conn); //создаем команду
                using (OleDbDataReader dr = cmd.ExecuteReader())
                { //выполняем
                    while (dr.Read())
                    { //читаем результат
                        dataGridView2.Rows.Add();
                        dataGridView2[0, i].Value = dr.GetValue(0).ToString();
                        dataGridView2[1, i].Value = dr.GetValue(1).ToString();
                        dataGridView2[2, i].Value = dr.GetValue(2).ToString();
                        dataGridView2.Rows[i].ReadOnly = true;
                        i++;
                    }
                }
            }
            else
                if (radioButton1.Checked)
                {
                    dataGridView1.Visible = true;
                    dataGridView2.Visible = false;
                    dataGridView3.Visible = false;
                    command = "SELECT Филиалы.Код, Филиалы.Название, SUM(Продажи.Количество), SUM(Лекарства.Цена*Продажи.Количество)"+
                    " FROM Филиалы INNER JOIN (Лекарства INNER JOIN Продажи ON Лекарства.[Код] = Продажи.[Номер товара]) ON Филиалы.[Код] = Продажи.[Номер продавца]"+
                    " GROUP BY Филиалы.Код, Филиалы.Название"+
                    " ORDER BY SUM(Лекарства.Цена*Продажи.Количество) DESC;";
                    dataGridView1.Rows.Clear();
                    OleDbCommand cmd = new OleDbCommand(command, conn); //создаем команду
                    using (OleDbDataReader dr = cmd.ExecuteReader())
                    { //выполняем
                        while (dr.Read())
                        { //читаем результат
                            dataGridView1.Rows.Add();
                            dataGridView1[0, i].Value = dr.GetValue(0).ToString();
                            dataGridView1[1, i].Value = dr.GetValue(1).ToString();
                            dataGridView1[2, i].Value = dr.GetValue(2).ToString();
                            dataGridView1[3, i].Value = dr.GetValue(3).ToString();
                             dataGridView1.Rows[i].ReadOnly = true;
                            i++;
                        }
                    }
                }
                else
                {
                    dataGridView1.Visible = false;
                    dataGridView2.Visible = false;
                    dataGridView3.Visible = true;
                    command = "SELECT Продажи.[Номер продавца],Продажи.[Номер филиала], SUM(Продажи.Количество), SUM(Продажи.Количество*Лекарства.Цена)"+
                    " FROM Лекарства INNER JOIN Продажи ON Лекарства.[Код] = Продажи.[Номер товара]"+
                    " GROUP BY Продажи.[Номер филиала], Продажи.[Номер продавца]"+
                    " ORDER BY  SUM(Продажи.Количество*Лекарства.Цена) DESC;";
                    dataGridView3.Rows.Clear();
                    OleDbCommand cmd = new OleDbCommand(command, conn); //создаем команду
                    using (OleDbDataReader dr = cmd.ExecuteReader())
                    { //выполняем
                        while (dr.Read())
                        { //читаем результат
                            dataGridView3.Rows.Add();
                            dataGridView3[0, i].Value = dr.GetValue(0).ToString();
                            dataGridView3[1, i].Value = dr.GetValue(1).ToString();
                            dataGridView3[2, i].Value = dr.GetValue(2).ToString();
                            dataGridView3[3, i].Value = dr.GetValue(3).ToString();
                            dataGridView3.Rows[i].ReadOnly = true;
                            i++;
                        }
                    }
                }
            conn.Close();
        }
    }
}
