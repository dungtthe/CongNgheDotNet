using BUS;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace GUI
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private static IProductService productService = ProductService.GI();

        private void frmMain_Load(object sender, EventArgs e)
        {
            LoadProductsToListView(lvDanhSachSanPham, productService.GetAllProduct());
        }



        private static CultureInfo vietnamCulture = new CultureInfo("vi-VN");
        private void LoadProductsToListView(System.Windows.Forms.ListView listView, List<ProductDTO> products)
        {
            listView.Items.Clear();

            foreach (var product in products)
            {
                ListViewItem item = new ListViewItem(Util.ConvertIdProductToString(product.Id, products));
                item.SubItems.Add(product.Name);
                item.SubItems.Add(product.Quantity.ToString());
                item.SubItems.Add(product.UnitPrice.ToString("C0", vietnamCulture));
                item.SubItems.Add(product.Origin);
                item.SubItems.Add(product.ExpireDate.ToString("dd/MM/yyyy"));
                listView.Items.Add(item);
            }
        }

        private void lvDanhSachSanPham_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (lvDanhSachSanPham.SelectedItems.Count > 0)
                {
                    ListViewItem selectedItem = lvDanhSachSanPham.SelectedItems[0];
                    txtMaSP.Text = selectedItem.SubItems[0].Text;
                    txtTenSP.Text = selectedItem.SubItems[1].Text;
                    txtSoLuong.Text = selectedItem.SubItems[2].Text;

                    ProductDTO productDTO = productService.FindProductById(Util.ConvertStringToProductId(txtMaSP.Text));
                    if (productDTO == null)
                    {
                        MessageBox.Show("Có lỗi xảy ra.");
                        return;
                    }
                    txtDonGia.Text = productDTO.UnitPrice.ToString();
                    txtXuatXu.Text = selectedItem.SubItems[4].Text;

                    DateTime ngayHetHan;
                    if (DateTime.TryParseExact(selectedItem.SubItems[5].Text, "dd/MM/yyyy", null, DateTimeStyles.None, out ngayHetHan))
                    {
                        dtpNgayHetHan.Value = ngayHetHan;
                    }
                    else
                    {
                        MessageBox.Show("Có lỗi xảy ra.");
                    }
                }
            }
            catch
            {
                MessageBox.Show("Có lỗi xảy ra.");
            }
        }

        private void btnThemSP_Click(object sender, EventArgs e)
        {
            try
            {

                string name = txtTenSP.Text;
                int quantity = int.Parse(txtSoLuong.Text);
                decimal unitPrice = decimal.Parse(txtDonGia.Text);
                string origin = txtXuatXu.Text;
                DateTime expireDate = dtpNgayHetHan.Value;
                ProductDTO newProduct = new ProductDTO()
                {
                    Name = name,
                    Quantity = quantity,
                    UnitPrice = unitPrice,
                    Origin = origin,
                    ExpireDate = expireDate
                };
                if (productService.AddProduct(newProduct) != null)
                {
                    MessageBox.Show("Sản phẩm mới đã được thêm thành công!");
                    LoadProductsToListView(lvDanhSachSanPham, productService.GetAllProduct());
                }
                else
                {
                    MessageBox.Show("Có lỗi xảy ra.");
                }
            }
            catch
            {
                MessageBox.Show("Có lỗi xảy ra!");
            }
        }




        private void txtDonGia_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtDonGia_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnDeleteSPByID_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtMaSP.Text))
                {
                    MessageBox.Show("Vui lòng chọn sản phẩm để xóa!");
                    return;
                }

                int id = Util.ConvertStringToProductId(txtMaSP.Text);
                ProductDTO productDTO = productService.FindProductById(id);
                if (productDTO == null)
                {
                    MessageBox.Show("Có lỗi xảy ra.");
                    return;
                }


                if (productService.DeleteProductById(id))
                {
                    MessageBox.Show("Xóa thành công!");
                    LoadProductsToListView(lvDanhSachSanPham, productService.GetAllProduct());
                }
                else
                {
                    MessageBox.Show("Có lỗi xảy ra.");
                }
            }
            catch
            {
                MessageBox.Show("Có lỗi xảy ra.");
            }
        }

        private void btnUpdateSPByID_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtMaSP.Text))
                {
                    MessageBox.Show("Vui lòng chọn sản phẩm để cập nhật!");
                    return;
                }

                int id = Util.ConvertStringToProductId(txtMaSP.Text);
                ProductDTO productDTO = productService.FindProductById(id);

                if (productDTO == null)
                {
                    MessageBox.Show("Có lỗi xảy ra!");
                    return;
                }

                productDTO.Name = txtTenSP.Text;
                productDTO.Quantity = int.Parse(txtSoLuong.Text);
                productDTO.UnitPrice = decimal.Parse(txtDonGia.Text);
                productDTO.Origin = txtXuatXu.Text;
                productDTO.ExpireDate = dtpNgayHetHan.Value;

                if (productService.UpdateProduct(productDTO) != null)
                {
                    MessageBox.Show("Cập nhật sản phẩm thành công!");
                    LoadProductsToListView(lvDanhSachSanPham, productService.GetAllProduct());
                }
                else
                {
                    MessageBox.Show("Có lỗi xảy ra!");
                }
            }
            catch
            {
                MessageBox.Show("Có lỗi xảy ra!");
            }
        }

        private void btnKiemTraKhoCoSPQuaHanHayKhong_Click(object sender, EventArgs e)
        {
            var l = productService.GetAllProduct();
            bool hasExpiredProduct = l.Any(p => p.ExpireDate < DateTime.Now);

            if (hasExpiredProduct)
            {
                MessageBox.Show("Có sản phẩm đã hết hạn.");
            }
            else
            {
                MessageBox.Show("Không có sản phẩm nào hết hạn.");
            }
        }

        private void btnXoaSPTheoXuatXuBatKi_Click(object sender, EventArgs e)
        {
            try
            {
                string originToDelete = txtXoaSPTheoXuatXuBatKi.Text.ToLower().Trim();

                if (string.IsNullOrEmpty(originToDelete))
                {
                    MessageBox.Show("Vui lòng nhập xuất xứ cần xóa!");
                    return;
                }
                var products = productService.GetAllProduct();
                var productsToDelete = products.Where(p => p.Origin.ToLower().Equals(originToDelete, StringComparison.OrdinalIgnoreCase)).ToList();
                if (productsToDelete.Count == 0)
                {
                    MessageBox.Show("Không có sản phẩm nào với xuất xứ được chọn.");
                    return;
                }
                foreach (var product in productsToDelete)
                {
                    productService.DeleteProductById(product.Id);
                }
                MessageBox.Show($"{productsToDelete.Count} sản phẩm với xuất xứ '{originToDelete}' đã bị xóa.");
                LoadProductsToListView(lvDanhSachSanPham, productService.GetAllProduct());
            }
            catch
            {
                MessageBox.Show("Có lỗi xảy ra");
            }
        }

        private void btnXoaToanBoSPQuaHan_Click(object sender, EventArgs e)
        {
            try
            {
                var products = productService.GetAllProduct();

                var expiredProducts = products.Where(p => p.ExpireDate < DateTime.Now).ToList();

                if (expiredProducts.Count == 0)
                {
                    MessageBox.Show("Không có sản phẩm nào đã hết hạn.");
                    return;
                }

                foreach (var product in expiredProducts)
                {
                    productService.DeleteProductById(product.Id);
                }

                MessageBox.Show($"{expiredProducts.Count} sản phẩm đã hết hạn đã bị xóa.");
                LoadProductsToListView(lvDanhSachSanPham, productService.GetAllProduct());
            }
            catch
            {
                MessageBox.Show("Có lỗi xảy ra");
            }
        }

        private void btnXoaToanBoSPTrongKho_Click(object sender, EventArgs e)
        {
            try
            {

                List<int> ins = new List<int>(productService.GetAllProduct().Count);
                foreach (var p in productService.GetAllProduct())
                {
                    ins.Add(p.Id);
                }

                foreach (var idDe in ins)
                {
                    productService.DeleteProductById(idDe);
                }
                MessageBox.Show("Xong");
                LoadProductsToListView(lvDanhSachSanPham, productService.GetAllProduct());
            }
            catch
            {
                MessageBox.Show("Có lỗi xảy ra");
            }

        }

        private void btn1SPCoDonGiaCaoNhat_Click(object sender, EventArgs e)
        {
            try
            {
                lvDSSPForTimKiem.Items.Clear();
                var products = productService.GetAllProduct();

                var maxPriceProduct = products.OrderByDescending(p => p.UnitPrice).FirstOrDefault();

                if (maxPriceProduct != null)
                {
                    LoadProductsToListView(lvDSSPForTimKiem,new List<ProductDTO>()
                    {
                        maxPriceProduct
                    });
                }
                else
                {
                    MessageBox.Show("Không có sản phẩm nào trong danh sách.");
                }
            }
            catch
            {
                MessageBox.Show("Có lỗi xảy ra");
            }
        }

        private void btn1SPDenTuNhatBan_Click(object sender, EventArgs e)
        {
            try
            {
                lvDSSPForTimKiem.Items.Clear();
                var products = productService.GetAllProduct();
                var productFromJapan = products.FirstOrDefault(p => p.Origin.Equals("Nhật Bản", StringComparison.OrdinalIgnoreCase));

                if (productFromJapan != null)
                {
                    LoadProductsToListView(lvDSSPForTimKiem, new List<ProductDTO>()
                    {
                        productFromJapan
                    });
                }
                else
                {
                    MessageBox.Show("Không có sản phẩm nào đến từ Nhật Bản.");
                }
            }
            catch
            {
                MessageBox.Show("Có lỗi xảy ra");
            }
        }

        private void btnXuatToanBoSPQuaHan_Click(object sender, EventArgs e)
        {
            try
            {
                lvDSSPForTimKiem.Items.Clear();
                var products = productService.GetAllProduct();

                var expiredProducts = products.Where(p => p.ExpireDate < DateTime.Now).ToList();

                if (expiredProducts.Count == 0)
                {
                    MessageBox.Show("Không có sản phẩm nào đã hết hạn.");
                    return;
                }

                LoadProductsToListView(lvDSSPForTimKiem, expiredProducts);
            }
            catch
            {
                MessageBox.Show("Có lỗi xảy ra");
            }
        }

        private void btnXuatCacSPCoDonGiaTrongKhoangMinMax_Click(object sender, EventArgs e)
        {
            try
            {
                lvDSSPForTimKiem.Items.Clear();
                int min = int.Parse(txtDonGiaMin.Text);
                int max=int.Parse(txtDonGiaMax.Text);
                var products = productService.GetAllProduct();
                var productsInRange = products.Where(p => p.UnitPrice >= min && p.UnitPrice <= max).ToList();
                if(productsInRange.Count >0)
                {
                    LoadProductsToListView(lvDSSPForTimKiem, productsInRange);
                }
                else
                {
                    MessageBox.Show("Không có sản phẩm nào");
                }
            }
            catch
            {
                MessageBox.Show("Có lỗi xảy ra");
            }
        }
    }
}
