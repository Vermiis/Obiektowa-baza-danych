using Db4objects.Db4o;
using Obiektowa_baza_danayc.Model;
using PropertyChanged;
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
    /// Interaction logic for AddEditWindow.xaml
    /// </summary>
    /// 
    [ImplementPropertyChanged]
    public partial class AddEditContactWindow : Window
    {
        public Contact _contact { get; set; }
        bool _edit = false;
        IObjectContainer _db;
        List<Phone> Phones;

        public AddEditContactWindow(IObjectContainer db)
        {
            InitializeComponent();
            _db = db;
            _contact = new Contact();
            _contact.Address = new Address();
            _contact.PhoneNumbers = new List<Phone>();
            Phones = new List<Phone>();
        }

        public AddEditContactWindow(IObjectContainer db, Contact contactToEdit)
        {
            InitializeComponent();
            _edit = true;
            _db = db;
            _contact = new Contact();
            _contact.Surename = contactToEdit.Surename;
            _contact.Name = contactToEdit.Name;

            TextBoxName.Text = contactToEdit.Name;
            TextBoxSurename.Text = contactToEdit.Surename;
            TextBoxCity.Text = contactToEdit.Address.City;
            TextBoxPostalCode.Text = contactToEdit.Address.PostalCode;
            TextBoxStreet.Text = contactToEdit.Address.Street;
            Phones = new List<Phone>();
            foreach (var x in contactToEdit.PhoneNumbers)
            {
                Phones.Add(x);
            }

            listBoxPhone.ItemsSource = null;
            listBoxPhone.ItemsSource = Phones;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {

            if (!_edit)
            {
                _contact.Name = TextBoxName.Text;
                _contact.Surename = TextBoxSurename.Text;

                if (String.IsNullOrEmpty(TextBoxCity.Text)
                    && String.IsNullOrEmpty(TextBoxPostalCode.Text)
                    && String.IsNullOrEmpty(TextBoxStreet.Text))
                {
                    _contact.Address = null;
                }
                else
                {
                    _contact.Address = new Address();
                    _contact.Address.City = TextBoxCity.Text;
                    _contact.Address.PostalCode = TextBoxPostalCode.Text;
                    _contact.Address.Street = TextBoxStreet.Text;
                }

                foreach (var x in Phones)
                {
                    _contact.PhoneNumbers.Add(x);
                }
                _db.Store(_contact);
            }
            else
            {
                IObjectSet result = _db.QueryByExample(_contact);
                Contact found = (Contact)result.Next();

                if (String.IsNullOrEmpty(TextBoxCity.Text)
                    && String.IsNullOrEmpty(TextBoxPostalCode.Text)
                    && String.IsNullOrEmpty(TextBoxStreet.Text))
                {
                    found.Address = null;
                }
                else
                {
                    if (found.Address == null)
                        found.Address = new Address();

                    found.Address.City = TextBoxCity.Text;
                    found.Address.Street = TextBoxPostalCode.Text;
                    found.Address.PostalCode = TextBoxStreet.Text;
                }
                found.Name = TextBoxName.Text;
                found.Surename = TextBoxSurename.Text;

                foreach(var x in found.PhoneNumbers)
                {
                    _db.Delete(x);
                }
                found.PhoneNumbers = new List<Phone>();

                foreach (var x in Phones)
                {
                    found.PhoneNumbers.Add(x);
                }
                _db.Store(found);
            }

            DialogResult = true;
            Close();
        }


        private void ButtonAddPhone_Click(object sender, RoutedEventArgs e)
        {
            AddEditPhoneWindow _addEditPhoneWindow = new AddEditPhoneWindow(Phones);
            _addEditPhoneWindow.Title = "Dodaj telefon";
            _addEditPhoneWindow.ShowDialog();
            if (_addEditPhoneWindow.DialogResult == true)
            {
                listBoxPhone.ItemsSource = null;
                listBoxPhone.ItemsSource = Phones;
            }
        }

        private void ButtonEditPhone_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxPhone.SelectedItem != null)
            {
                Phone phone = (Phone)listBoxPhone.SelectedItem;
                AddEditPhoneWindow _addEditPhoneWindow = new AddEditPhoneWindow(Phones, phone.Number);
                _addEditPhoneWindow.Title = "Edytuj telefon";
                _addEditPhoneWindow.ShowDialog();

                if (_addEditPhoneWindow.DialogResult == true)
                {
                    listBoxPhone.ItemsSource = null;
                    listBoxPhone.ItemsSource = Phones;
                }
            }
        }

        private void ButtonDeletePhone_Click(object sender, RoutedEventArgs e)
        {
            Phone phone = (Phone)listBoxPhone.SelectedItem;
            Phones.Remove(phone);
            listBoxPhone.ItemsSource = null;
            listBoxPhone.ItemsSource = Phones;
        }
    }
}
