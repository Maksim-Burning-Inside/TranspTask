using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TranspTask
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void SetDefaultValue(DataGridView dataGrid)
        {
            for (int i = 0; i < dataGrid.Columns.Count; i++)
            {
                for (int j = 0; j < dataGrid.Rows.Count; j++)
                {
                    if (dataGrid[i, j].Value == null)
                        dataGrid[i, j].Value = "0";
                }
            }
        }

        private void addColumnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.Columns.Add("Column " + (dataGridView1.Columns.Count + 1).ToString(), "Consumer " + (dataGridView1.Columns.Count + 1).ToString());
            dataGridView3.Columns.Add("Column " + (dataGridView1.Columns.Count + 1).ToString(), "Demand " + (dataGridView1.Columns.Count).ToString());

            SetDefaultValue(dataGridView1);
            SetDefaultValue(dataGridView3);
        }

        private void addRowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].HeaderCell.Value = "Provider " + dataGridView1.Rows.Count.ToString();

                dataGridView2.Rows.Add();
                dataGridView2.Rows[dataGridView2.Rows.Count - 1].HeaderCell.Value = "Stock " + dataGridView1.Rows.Count.ToString();

                SetDefaultValue(dataGridView1);
                SetDefaultValue(dataGridView2);
            }
            catch
            {
                MessageBox.Show("You cannot add a row if the set of columns is empty.");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.Rows.Add();
            dataGridView1.Rows.Add();
            dataGridView1.Rows.Add();
            dataGridView1.Rows[0].HeaderCell.Value = "Provider 1";
            dataGridView1.Rows[1].HeaderCell.Value = "Provider 2";
            dataGridView1.Rows[2].HeaderCell.Value = "Provider 3";

            dataGridView3.Rows.Add();
            dataGridView3.Rows[0].HeaderCell.Value = "Demands";

            dataGridView2.Rows.Add();
            dataGridView2.Rows.Add();
            dataGridView2.Rows.Add();
            dataGridView2.Rows[0].HeaderCell.Value = "Stock 1";
            dataGridView2.Rows[1].HeaderCell.Value = "Stock 2";
            dataGridView2.Rows[2].HeaderCell.Value = "Stock 3";

            SetDefaultValue(dataGridView1);
            SetDefaultValue(dataGridView2);
            SetDefaultValue(dataGridView3);

            dataGridView1[0, 0].Value = "2";
            dataGridView1[0, 1].Value = "5";
            dataGridView1[0, 2].Value = "2";
            dataGridView1[1, 0].Value = "3";
            dataGridView1[1, 1].Value = "3";
            dataGridView1[1, 2].Value = "1";
            dataGridView1[2, 0].Value = "4";
            dataGridView1[2, 1].Value = "1";
            dataGridView1[2, 2].Value = "4";
            dataGridView1[3, 0].Value = "3";
            dataGridView1[3, 1].Value = "2";
            dataGridView1[3, 2].Value = "2";
            dataGridView2[0, 0].Value = "90";
            dataGridView2[0, 1].Value = "30";
            dataGridView2[0, 2].Value = "40";
            dataGridView3[0, 0].Value = "70";
            dataGridView3[1, 0].Value = "30";
            dataGridView3[2, 0].Value = "20";
            dataGridView3[3, 0].Value = "40";
        }

        private void deleteColumnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.Columns.RemoveAt(dataGridView1.Rows.Count - 1);
                dataGridView3.Columns.RemoveAt(dataGridView3.Rows.Count - 1);
            }
            catch
            {
                MessageBox.Show("Cannot be deleted, set of objects are empty.");
            }
        }

        private void deleteRowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 1);
                dataGridView2.Rows.RemoveAt(dataGridView2.Rows.Count - 1);
            }
            catch
            {
                MessageBox.Show("Cannot be deleted, set of objects are empty.");
            }
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<List<int>> cells = new List<List<int>>();
            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                cells.Add(new List<int>());
                for (int j = 0; j < dataGridView1.Rows.Count; j++)
                {
                    cells[i].Add(int.Parse(dataGridView1[i, j].Value.ToString()));
                }
            }

            List<double> stocks = new List<double>();
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                stocks.Add(double.Parse(dataGridView2[0, i].Value.ToString()));
            }

            List<double> demands = new List<double>();
            for (int i = 0; i < dataGridView3.Columns.Count; i++)
            {
                demands.Add(double.Parse(dataGridView3[i, 0].Value.ToString()));
            }

            Transport transport = new Transport(TranslatorData.Create_InformationHandler(cells, stocks, demands));
            InformationHandler solution = transport.StartAlgorithm();
            OutputSolution(solution);
        }

        private void OutputSolution(InformationHandler solution)
        {
            if (solution.TypeFull)
                label1.Text += " full";
            else
                label1.Text += " incomplete";

            int f_x = 0;

            for (int i = 0; i < solution.Matrix.Table.Count; i++)
            {
                for (int j = 0; j < solution.Matrix.Table[0].Count; j++)
                {
                    if (solution.Matrix.Table[i][j].Occupation)
                    {
                        try
                        {
                            dataGridView1[i, j].Value = solution.Matrix.Table[i][j].Goods.ToString();
                            f_x += (int)solution.Matrix.Table[i][j].Goods * solution.Matrix.Table[i][j].Cost;
                        }
                        catch
                        {

                        }
                    }
                    else
                    {
                        try
                        {
                            dataGridView1[i, j].Value = "----";
                        }
                        catch
                        {

                        }
                    }
                }
            }
            label2.Text += f_x.ToString();
        }
    }
}
