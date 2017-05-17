using Obiektowa_baza_danayc.Model;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Obiektowa_baza_danych
{
    /// <summary>
    /// Interaction logic for AddEditPhone.xaml
    /// </summary>
    public partial class AddEditPhoneWindow : Window
    {
        List<Phone> _Phones{get; set;}
        string _phonenumber;
        bool _edit;

        public AddEditPhoneWindow(List<Phone> Phones)
        {
            InitializeComponent();
            _Phones = Phones;

        }
        public AddEditPhoneWindow(List<Phone> Phones, string phonenumber)
        {
            InitializeComponent();
            _Phones = Phones;      
            TextBoxNumber.Text = Phones.First(n => n.Number == phonenumber).Number;
            TextBoxOperator.Text = Phones.First(n => n.Number == phonenumber).MobileOperator;
            TextBoxType.Text = Phones.First(n => n.Number == phonenumber).Type;
            _phonenumber = phonenumber;
            _edit = true;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (!_edit)
            {
                _Phones.Add(new Phone { Number = TextBoxNumber.Text, MobileOperator = TextBoxOperator.Text, Type = TextBoxType.Text});
            }
            else
            {
                _Phones.First(n => n.Number == _phonenumber).Number = TextBoxNumber.Text;
                _Phones.First(n => n.Number == _phonenumber).MobileOperator = TextBoxOperator.Text;
                _Phones.First(n => n.Number == _phonenumber).Type = TextBoxType.Text;
            }
            DialogResult = true;
            Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
