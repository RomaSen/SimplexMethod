using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

//Performed By Sen. R. S.


namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        bool check = true;
        bool can_we = true;
        static int col = 0, row = 0;
        int[] razresh = new int[2];
        int iter = 1;
        //Initialize form3
        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
        }
        //Creating a table for entering initial values
        private void Button1_Click(object sender, EventArgs e)
        {
            dataGridView2.Refresh();
            col = Convert.ToInt32(textBox2.Text); row = Convert.ToInt32(textBox1.Text);
            dataGridView1.RowCount = row + 1;
            dataGridView1.ColumnCount = (col + row) + 1;
            dataGridView1.Visible = true;
            button2.Visible = true;
            //Basic variable
            dataGridView1.TopLeftHeaderCell.Value = "B.V.";
            //Key column
            dataGridView1.Columns[0].HeaderText = "B.K.";
            for (int i = 1; i < dataGridView1.ColumnCount; i++)
            {
                dataGridView1.Columns[i].HeaderText = "X" + string.Format((i).ToString(), "0");
            }
            for (int i = 0; (i < (dataGridView1.Rows.Count - 1)); i++)
            {
                dataGridView1.Rows[i].HeaderCell.Value = "X" + string.Format((i + col + 1).ToString(), "0");
            }
            dataGridView1.Rows[dataGridView1.Rows.Count - 1].HeaderCell.Value = "F";
        }

        //Validation
        private void TextBox1_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || (e.KeyChar == Convert.ToChar(",")) || (e.KeyChar == '\b')) 
                return;
            else
            {
                char c = e.KeyChar;
                e.Handled = c >= 'а' && c <= 'я' || c >= 'А' && c <= 'Я' || c >= 'ё' && c <= 'Ё' || c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z';
            }
        }

        //Validation
        private void TextBox2_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || (e.KeyChar == Convert.ToChar(",")) || (e.KeyChar == '\b')) 
                return;
            else
            {
                char c = e.KeyChar;
                e.Handled = c >= 'а' && c <= 'я' || c >= 'А' && c <= 'Я' || c >= 'ё' && c <= 'Ё' || c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z';
            }
        }

        //Validation for table
        private void DataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TextBox tb = (TextBox)e.Control;
            tb.KeyPress += new KeyPressEventHandler(Tb_KeyPress);
        }

        //Validation
        void Tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar)) && (e.KeyChar != ',') && (e.KeyChar != '-'))
            {
                if (e.KeyChar != (char)Keys.Back)
                { e.Handled = true; }
            }
        }
        //simplex method
        private void Button2_Click(object sender, EventArgs e)
        {
            //When building a new solution, we update the old table
            dataGridView2.Refresh();
            dataGridView2.Update();
            for (int i = 0; i <= dataGridView2.RowCount - 1; i++)
                for (int j = 0; j <= dataGridView2.ColumnCount - 1; j++)
                    dataGridView2[j, i].Style.BackColor = System.Drawing.Color.White;
            double[,] mass = new double[dataGridView1.RowCount, dataGridView1.ColumnCount];
            dataGridView2.RowCount = dataGridView1.RowCount;
            dataGridView2.ColumnCount = dataGridView1.ColumnCount;
            dataGridView2.TopLeftHeaderCell.Value = "B.V.";
            dataGridView2.Columns[0].HeaderText = "B.K.";
            for (int i = 1; i < dataGridView2.ColumnCount; i++)
            {
                dataGridView2.Columns[i].HeaderText = "X" + string.Format((i).ToString(), "0");
            }
            dataGridView1.Rows[dataGridView1.Rows.Count - 1].HeaderCell.Value = "F";
            dataGridView2.Rows[dataGridView1.Rows.Count - 1].HeaderCell.Value = "F";
            for (int i = 0; i <= dataGridView1.RowCount - 1; i++)
                for (int j = 0; j <= dataGridView1.ColumnCount - 1; j++)
                {
                    if (dataGridView1[j, i].Value != DBNull.Value)
                        mass[i, j] = Convert.ToDouble(dataGridView1[j, i].Value);
                    else
                        mass[i, j] = 0;
                }
            double[,] temp_mass = new double[dataGridView1.RowCount, dataGridView1.ColumnCount];
            for (int i = 0; i <= dataGridView1.RowCount - 1; i++)
                for (int j = 0; j <= dataGridView1.ColumnCount - 1; j++)
                    temp_mass[i, j] = mass[i, j];
            // MAX or MIN
            if (comboBox1.SelectedIndex == 1)
            {
                check = true;
                can_we = true;
                iter = 1;
                MessageBox.Show("MIN", "START!!!");
                while (check)
                {
                    print(temp_mass);
                    razresh = MINFindingResolvingElem(temp_mass, dataGridView1.ColumnCount - 1, dataGridView1.RowCount - 1);
                    //CHECKING FOR OPTIMALITY
                    if (!check)
                    {
                        for (int i = 0; (i <= (dataGridView2.ColumnCount - 1)); i++)
                        {
                            dataGridView2[i, dataGridView2.RowCount - 1].Style.BackColor = System.Drawing.Color.LightGreen;
                        }
                        break;
                    }
                    //Checking if there is a solution
                    if (!can_we)
                    {
                        MessageBox.Show("This system has no solution!", "Sorry, we have error!");
                        dataGridView2.Refresh();
                        label3.Text = "";
                        label4.Text = "";
                        break;
                    }
                    temp_mass = find_next(temp_mass, razresh, col, row); 
                    dataGridView2[razresh[1], razresh[0] + dataGridView2.RowCount - dataGridView1.RowCount].Style.BackColor = System.Drawing.Color.Purple;
                }
                print_result(); //CHECK for the NUMBER of SOLUTIONS(infinity or not)
                label5.Text = $"MIN = " + (double)dataGridView2[0, dataGridView2.RowCount - 1].Value;
            }
            else if (comboBox1.SelectedIndex == 0)
            {
                check = true;
                can_we = true;
                iter = 1;
                MessageBox.Show("MAX", "START!!!");
                while (check)
                {
                    print(temp_mass);
                    razresh = MAXFindingResolvingElem(temp_mass, dataGridView1.ColumnCount - 1, dataGridView1.RowCount - 1);
                    if (!check)
                    {
                        for (int i = 0; (i <= (dataGridView2.ColumnCount - 1)); i++)
                        {
                            dataGridView2[i, dataGridView2.RowCount - 1].Style.BackColor = System.Drawing.Color.LightGreen;
                        }
                        break;
                    }
                    if (!can_we)
                    {
                        MessageBox.Show("This system has no solution!", "Sorry, we have error!");
                        dataGridView2.Refresh();
                        label3.Text = "";
                        label4.Text = "";
                        break;
                    }
                    temp_mass = find_next(temp_mass, razresh, col, row);
                    dataGridView2[razresh[1], razresh[0] + dataGridView2.RowCount - dataGridView1.RowCount].Style.BackColor = System.Drawing.Color.Purple;
                }
                print_result(); 
                label5.Text = $"MAX = " + (double)dataGridView2[0, dataGridView2.RowCount - 1].Value;
            }
            dataGridView2.Visible = true;
            label3.Visible = true;
            label4.Visible = true;
        }

        //Looking for a resolving element
        public int[] MAXFindingResolvingElem(double[,] massiv, int column, int row)
        {
            bool temp = false;
            int[] koord = new int[2];
            double save_inf = double.MaxValue;
            for (int i = 0; i < column; i++)
            {
                if (massiv[row, i] < 0)
                {
                    if (massiv[row, i] < save_inf)
                    {
                        //if the past is negative < new then save the coordinates
                        save_inf = massiv[row, i]; 
                        koord[1] = i;
                    }
                    //as long as we have at least one negative
                    temp = true;    
                }
            }
            //if no negative element is found, then this is the optimal plan
            check = temp; 
            temp = true;
            save_inf = double.MaxValue;
            double temp1 = double.MaxValue;
            for (int i = 0; i < row; i++)
            {
                //if we can calculate the coefficient and select the resolution element
                if (massiv[i, 0] > 0 && massiv[i, koord[1]] > 0) 
                {
                    temp1 = (massiv[i, 0] / massiv[i, koord[1]]);
                    //if the new ratio is less than the previous one, save the rows and update the value
                    if (temp1 < save_inf) 
                    {
                        save_inf = temp1;
                        koord[0] = i;
                    }
                    temp = false;
                }
            }
            //otherwise there is no solution
            if (temp)
            {
                can_we = false;
            }
            return koord;

        }
        //To search for the highest-resolution element when solving a minimum problem
        public int[] MINFindingResolvingElem(double[,] massiv, int column, int row)
        {
            bool temp = false;
            int[] koord = new int[2];
            double save_inf = double.MinValue;
            for (int i = 0; i < column; i++) 
            {
                if (massiv[row, i] > 0) 
                {
                    if (massiv[row, i] > save_inf)
                    {
                        save_inf = massiv[row, i]; 
                        koord[1] = i;
                    }
                    temp = true; 
                }
            }
            check = temp;
            temp = true;
            save_inf = double.MaxValue;
            double temp1 = double.MaxValue;
            for (int i = 0; i < row; i++)
            {
                if (massiv[i, 0] > 0 && massiv[i, koord[1]] > 0) 
                {
                    temp1 = (massiv[i, 0] / massiv[i, koord[1]]);
                    if (temp1 < save_inf) 
                    {
                        save_inf = temp1;
                        koord[0] = i;
                    }
                    temp = false;
                }
            }
            if (temp)
            {
                can_we = false;
            }
            return koord;
        }
        //Looking for the following table
        public double[,] find_next(double[,] massiv, int[] razresh, int column, int row)
        {
            double[,] last_vers = new double[dataGridView1.RowCount, dataGridView1.ColumnCount];
            for (int i = 0; i <= dataGridView1.RowCount - 1; i++)
                for (int j = 0; j <= dataGridView1.ColumnCount - 1; j++)
                {
                    if (j == razresh[1]) 
                        last_vers[i, j] = 0;
                    else if (i == razresh[0]) 
                        last_vers[i, j] = massiv[i, j] / massiv[razresh[0], razresh[1]];
                    else
                        last_vers[i, j] = ((massiv[i, j] * massiv[razresh[0], razresh[1]]) - (massiv[razresh[0], j] * massiv[i, razresh[1]])) / massiv[razresh[0], razresh[1]];
                }
            last_vers[razresh[0], razresh[1]] = 1;
            return last_vers;
        }
        //Displaying an array on the screen
        public void print(double[,] massiv)
        {
            int K = 0;
            dataGridView2.RowCount = dataGridView1.RowCount * iter;
            for (int i = dataGridView2.RowCount - dataGridView1.RowCount; i <= dataGridView2.RowCount - 1; i++) 
            {
                for (int j = 0; j <= (col + row); j++)
                {
                    dataGridView2[j, i].Value = Math.Round(massiv[K, j], 3);
                }
                K++;
            }
            if (iter == 1)
            {
                for (int i = dataGridView2.RowCount - dataGridView1.RowCount; (i < (dataGridView2.Rows.Count - 1)); i++)
                {
                    dataGridView2.Rows[i].HeaderCell.Value = "X" + string.Format((i + col + 1).ToString(), "0");
                }
                dataGridView2.Rows[dataGridView2.RowCount - 1].HeaderCell.Value = "F";
                for (int i = 0; (i <= dataGridView2.ColumnCount - 1); i++) 
                {
                    dataGridView2[i, dataGridView2.RowCount - 1].Style.BackColor = System.Drawing.Color.Red;
                }
            }
            else if (iter >= 2)
            {
                for (int i = dataGridView2.RowCount - dataGridView1.RowCount; (i < (dataGridView2.Rows.Count - 1)); i++)
                {
                    dataGridView2.Rows[i].HeaderCell.Value = dataGridView2.Rows[i - dataGridView1.RowCount].HeaderCell.Value;
                }
                dataGridView2.Rows[dataGridView2.RowCount - 1].HeaderCell.Value = "F";
                for (int i = 0; (i <= (dataGridView2.ColumnCount - 1)); i++)
                {
                    dataGridView2[i, dataGridView2.RowCount - 1].Style.BackColor = System.Drawing.Color.Red;
                }
                dataGridView2.Rows[razresh[0] + dataGridView1.RowCount * iter - dataGridView1.RowCount].HeaderCell.Value = "X" + string.Format((razresh[1]).ToString(), "0");
            }
            iter++;
        }
        // Output X*and Y*
        public void print_result()
        {
            bool besckone4 = false;
            string x = "", y = "";
            for (int i = col + 1; i < dataGridView2.ColumnCount; i++)
            {
                y += ($" {(double)dataGridView2[i, dataGridView2.RowCount - 1].Value}; ");

            }
            for (int i = 1; i < dataGridView2.ColumnCount; i++)
            {
                x += ($" {Find_x(i)}; ");
                if (i <= col)
                {
                    y += ($" {(double)dataGridView2[i, dataGridView2.RowCount - 1].Value}; ");
                }
                else if (Find_x(i) == 0)
                    besckone4 = true;
            }
            label3.Text = "X* (" + x + ")";
            label4.Text = "Y* (" + y + ")";
            if (besckone4)
                MessageBox.Show("This system has an infinite set of solutions [in place of the free member is 0]!", "I think I should warn you :)");
        }

      
        //Assisting method for searching for X
        public double Find_x(int x)
        {
            double next_x = 0;
            for (int i = dataGridView2.RowCount - dataGridView1.RowCount; (i < (dataGridView2.RowCount - 1)); i++)
            {
                if (((string)dataGridView2.Rows[i].HeaderCell.Value).Equals("X" + string.Format((x).ToString(), "0")))
                    next_x = (double)dataGridView2[0, i].Value;
            }
            return next_x;
        }
    }
}