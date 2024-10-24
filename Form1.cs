using bai8_DB.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bai8_DB
{
    public partial class Form1 : Form
    {
        private readonly Model1 db = new Model1();
        private Student student;
        public Form1()
        {
            InitializeComponent();
            LoadMajors();
            LoadData();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'schoolDBDataSet1.Students' table. You can move, or remove it, as needed.
            this.studentsTableAdapter1.Fill(this.schoolDBDataSet1.Students);
        }

        private void LoadData()
        {           
            dataGridView1.DataSource = db.Students.ToList();

        }
        private void LoadMajors()
        {
            comboBox1.Items.Add("Công nghệ thông tin");
            comboBox1.Items.Add("Kế toán");
            comboBox1.Items.Add("Ngôn ngữ");
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //Kiểm tra xem các trường nhập liệu có hợp lệ không
            if (string.IsNullOrWhiteSpace(txtFullName.Text) ||
                string.IsNullOrWhiteSpace(txtAge.Text) ||
                comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin sinh viên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var student = new Student
            {
                FullName = txtFullName.Text,
                Age = int.Parse(txtAge.Text),
                Major = comboBox1.SelectedItem.ToString()
            };

            // Thêm sinh viên vào cơ sở dữ liệu
            db.Students.Add(student);
            db.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu
            LoadData(); // Cập nhật lại DataGridView
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem có sinh viên nào được chọn không
            if (student != null)
            {
                var confirmResult = MessageBox.Show("Bạn có chắc chắn muốn xóa sinh viên này?", "Xác nhận xóa", MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    db.Students.Remove(student);
                    db.SaveChanges();
                    LoadData(); // Cập nhật danh sách sinh viên
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sinh viên để xóa.");
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            //Kiểm tra xem có sinh viên nào được chọn không
            if (student != null)
            {
                // Kiểm tra thông tin nhập vào
                if (string.IsNullOrWhiteSpace(txtFullName.Text) ||
                    !int.TryParse(txtAge.Text, out int age) ||
                    comboBox1.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin: Họ tên, Tuổi và Ngành học.");
                    return;
                }

                // Cập nhật thông tin sinh viên
                student.FullName = txtFullName.Text;
                student.Age = age;
                student.Major = comboBox1.SelectedItem.ToString();

                db.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu
                LoadData(); // Cập nhật danh sách sinh viên
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sinh viên để sửa.");
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                student = (Student)dataGridView1.CurrentRow.DataBoundItem;
                txtFullName.Text = student.FullName;
                txtAge.Text = student.Age.ToString();
                comboBox1.SelectedItem = student.Major;
            }

        }
    }
}
