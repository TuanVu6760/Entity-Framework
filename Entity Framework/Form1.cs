using Entity_Framework.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Entity_Framework
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            using (Models.Model1 model = new Models.Model1())
            {
                List<SinhVien> sinhViens = model.SinhViens.Include("Khoa").ToList();
                List<Khoa> khoa = model.Khoas.ToList();

                if (khoa == null || khoa.Count == 0)
                {
                    MessageBox.Show("No faculty data available.");
                    return;
                }

                if (sinhViens == null || sinhViens.Count == 0)
                {
                    MessageBox.Show("No student data available.");
                    return;
                }
                // Điền danh sách khoa vào ComboBox
                FillFacultyCombobox(khoa);

                // Hiển thị danh sách sinh viên trong DataGridView
                BindGrid(sinhViens);
            }
        }
        private void FillFacultyCombobox(List<Khoa> khoaList)
        {
            // Đặt datasource cho ComboBox với danh sách các khoa
            cbbKhoa.DataSource = khoaList;
            cbbKhoa.DisplayMember = "TenKhoa"; // Hiển thị tên khoa
            cbbKhoa.ValueMember = "MaKhoa"; // Lưu giá trị mã khoa
        }
        private void BindGrid(List<SinhVien> sinhViens)
        {
            // Xóa các hàng hiện có trong DataGridView trước khi hiển thị danh sách mới
            dgvSinhVien.Rows.Clear();

            foreach (var sinhVien in sinhViens)
            {
                // Thêm sinh viên vào DataGridView
                int index = dgvSinhVien.Rows.Add();
                dgvSinhVien.Rows[index].Cells[0].Value = sinhVien.MSSV;
                dgvSinhVien.Rows[index].Cells[1].Value = sinhVien.HoTen;

                // Kiểm tra null trước khi truy cập thuộc tính TenKhoa
                dgvSinhVien.Rows[index].Cells[2].Value = sinhVien.Khoa != null ? sinhVien.Khoa.TenKhoa : "N/A"; // Nếu Khoa null, hiển thị N/A
                dgvSinhVien.Rows[index].Cells[3].Value = sinhVien.DTB;
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            using (Models.Model1 model = new Models.Model1())
            {
                // Tạo sinh viên mới từ các ô nhập liệu
                SinhVien newStudent = new SinhVien()
                {
                    MSSV = txtMSSV.Text,
                    HoTen = txtHoten.Text,
                    DTB = Convert.ToDouble(txtDTB.Text),
                    MaKhoa = (int)cbbKhoa.SelectedValue // Lấy mã khoa từ ComboBox
                };

                // Thêm sinh viên vào cơ sở dữ liệu
                model.SinhViens.Add(newStudent);
                model.SaveChanges();

                // Cập nhật lại DataGridView
                List<SinhVien> sinhViens = model.SinhViens.Include("Khoa").ToList();
                BindGrid(sinhViens);
                MessageBox.Show("Thêm sinh viên thành công!");
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvSinhVien.SelectedRows.Count > 0)
            {
                // Lấy MSSV từ sinh viên được chọn
                string selectedMSSV = dgvSinhVien.SelectedRows[0].Cells[0].Value.ToString();

                using (Models.Model1 model = new Models.Model1())
                {
                    // Tìm sinh viên trong cơ sở dữ liệu dựa trên MSSV
                    SinhVien studentToEdit = model.SinhViens.FirstOrDefault(sv => sv.MSSV == selectedMSSV);

                    if (studentToEdit != null)
                    {
                        // Cập nhật thông tin sinh viên từ các ô nhập liệu
                        studentToEdit.HoTen = txtHoten.Text;
                        studentToEdit.DTB = Convert.ToDouble(txtDTB.Text);
                        studentToEdit.MaKhoa = (int)cbbKhoa.SelectedValue; // Lấy mã khoa từ ComboBox

                        // Lưu thay đổi vào cơ sở dữ liệu
                        model.SaveChanges();

                        // Cập nhật lại DataGridView
                        List<SinhVien> sinhViens = model.SinhViens.Include("Khoa").ToList();
                        BindGrid(sinhViens);
                        MessageBox.Show("Cập nhật sinh viên thành công!");
                    }
                }
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvSinhVien.SelectedRows.Count > 0)
            {
                // Lấy MSSV từ sinh viên được chọn
                string selectedMSSV = dgvSinhVien.SelectedRows[0].Cells[0].Value.ToString();

                using (Models.Model1 model = new Models.Model1())
                {
                    // Tìm sinh viên trong cơ sở dữ liệu dựa trên MSSV
                    SinhVien studentToDelete = model.SinhViens.FirstOrDefault(sv => sv.MSSV == selectedMSSV);

                    if (studentToDelete != null)
                    {
                        // Xóa sinh viên khỏi cơ sở dữ liệu
                        model.SinhViens.Remove(studentToDelete);
                        model.SaveChanges();

                        // Cập nhật lại DataGridView
                        List<SinhVien> sinhViens = model.SinhViens.Include("Khoa").ToList();
                        BindGrid(sinhViens);
                        MessageBox.Show("Xóa sinh viên thành công!");
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn sinh viên để xóa.");
             }
        }
    }
}
