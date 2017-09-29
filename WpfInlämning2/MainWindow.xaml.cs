using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfInlämning2.Data;

namespace WpfInlämning2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Context _context = new Context();

        public MainWindow()
        {
            InitializeComponent();

            if (_context.Customers.Any())
            {
                DataContext = _context.Customers.ToList().Select(c => new CustomerExtended
                {
                    City = c.City,
                    CompanyName = c.CompanyName,
                    DOB = c.DOB,
                    Email = c.Email,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    isPrivate = c.isPrivate,
                    NewsLetter = c.NewsLetter,
                    CustomerType = (bool)c.isPrivate ? "Privat" : "Företag",
                    Notes = c.Notes,
                    Phone = c.Phone,
                    Street = c.Street,
                    Zip = c.Zip
                });
            }
        }
        

        private void txtFirstName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtFirstName.Text != "")
            {
                lblFirstName.Visibility = Visibility.Hidden;
            }
            else lblFirstName.Visibility = Visibility.Visible;
        }

        private void txtLastName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtLastName.Text != "")
            {
                lblLastName.Visibility = Visibility.Hidden;
            }
            else lblLastName.Visibility = Visibility.Visible;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if ((!string.IsNullOrWhiteSpace( txtFirstName.Text) || !string.IsNullOrWhiteSpace( txtLastName.Text)) && !string.IsNullOrWhiteSpace ( Email.Text) )
            {
                Customer customer = new Customer
                {
                    isPrivate = (bool)rbtPrivate.IsChecked ? true : false,
                    CompanyName = txtCompany.Text.Trim(),
                    FirstName = txtFirstName.Text.Trim(),
                    LastName = txtLastName.Text.Trim(),
                    DOB = DOB.SelectedDate,
                    Street = Street.Text.Trim(),
                    Zip = Zip.Text.Trim(),
                    City = City.Text.Trim(),
                    Phone = Phone.Text.Trim(),
                    Email = Email.Text.Trim(),
                    NewsLetter = (bool)NewsLetter.IsChecked ? true : false,
                    Notes = Notes.Text.Trim()
                };

                
                _context.Customers.Add(customer);
                _context.SaveChanges();

                ClearAll();
                MessageBox.Show("Tack användaren är sparad!");
            }
            else MessageBox.Show("Namn & Email är obligatoriskt");
        }
        private void ClearAll()
        {
            var currentDAte = DateTime.Today;

            rbtPrivate.IsChecked = true;
            rbtCompany.IsChecked = false;
            txtCompany.Text = String.Empty;
            txtFirstName.Text = String.Empty;
            txtLastName.Text = String.Empty;
            DOB.SelectedDate = currentDAte;
            Street.Text = String.Empty;
            Zip.Text = String.Empty;
            City.Text = String.Empty;
            Phone.Text = String.Empty;
            Email.Text = String.Empty;
            NewsLetter.IsChecked = false;
            Notes.Text = String.Empty;
        }

        private void rbtPrivate_Checked(object sender, RoutedEventArgs e)
        {
            if (rbtPrivate.IsEnabled) txtCompany.IsEnabled = false;
        }

        private void rbtCompany_Checked(object sender, RoutedEventArgs e)
        {
            if (txtCompany != null)
            {

                if (rbtCompany.IsEnabled) txtCompany.IsEnabled = true;
            }
        }


        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            var searchResult = new List<Customer>();

            searchResult.AddRange(_context.Customers.Where(x => x.Email.Contains(txtSearch.Text)
            || x.Phone.Contains(txtSearch.Text)));

            if (searchResult.Count != 0)
            {

                var yass = "";
                var filter = searchResult.Select(s => s).Select(i => (bool)i.isPrivate);
                foreach (var item in filter)
                {
                    yass += BoolToString(item);
                }
                // MessageBox.Show(yass);

                ResultList.ItemsSource = searchResult.Select(c => new CustomerExtended
                {
                    City = c.City,
                    CompanyName = c.CompanyName,
                    DOB = c.DOB,
                    Email = c.Email,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    isPrivate = c.isPrivate,
                    NewsLetter = c.NewsLetter,
                    CustomerType = c.isPrivate.Value ? "Privat" : "Företag",
                    Notes = c.Notes,
                    Phone = c.Phone,
                    Street = c.Street,
                    Zip = c.Zip
                });

                txtSearch.Clear();
            }
            else
            {
                MessageBox.Show("Ingen användare hittad..");
                txtSearch.Clear();
            }
        }

        public string BoolToString(bool b)
        {
            if (b) { return "Privat"; } else { return "Företag"; }
        }

        private void txtCompany_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtCompany.Text != "") rbtCompany.IsChecked = true;

            else
            {
                rbtCompany.IsChecked = false;
                rbtPrivate.IsChecked = true;
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            ResultList.ItemsSource = null;
        }

    }
}
