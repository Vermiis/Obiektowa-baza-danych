using Db4objects.Db4o;
using Db4objects.Db4o.Config;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Obiektowa_baza_danych
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        static string path = "C:\\Users\\next\\Documents\\Visual Studio 2015\\Projects\\Obiektowa baza danych\\Obiektowa baza danayc\\baza";
        IObjectContainer db;
        List<Contact> Contacts { get; set; }
        List<Phone> Phones { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            IEmbeddedConfiguration config = Db4oEmbedded.NewConfiguration();
            config.Common.ObjectClass(typeof(Contact)).CascadeOnUpdate(true);
            config.Common.ObjectClass(typeof(Contact)).CascadeOnDelete(true);
            config.Common.ObjectClass(typeof(Contact)).CascadeOnActivate(true);

            db = Db4oEmbedded.OpenFile(config, path);

            Contacts = new List<Contact>();
            Phones = new List<Phone>();

            Contact contact = new Contact();
            IObjectSet result = db.QueryByExample(typeof(Contact));

            foreach (var x in result)
            {
                contact = (Contact)x;
                Contacts.Add(new Contact
                {
                    Address = new Address
                    {
                        City = contact.Address.City,
                        PostalCode = contact.Address.PostalCode,
                        Street = contact.Address.Street
                    },
                    Name = contact.Name,
                    Surename = contact.Surename,
                    Id = contact.Id,
                    PhoneNumbers = contact.PhoneNumbers
                });
            }

            listBoxContacts.ItemsSource = Contacts;
            listBoxPhones.ItemsSource = Phones;

            int contactsCount = db.Query<Contact>(typeof(Contact)).Count();
            int addressCount = db.Query<Contact>(typeof(Address)).Count();
            int phonesCount = db.Query<Contact>(typeof(Phone)).Count();

            StatisticsLabel.Content = "W bazie jest " + contactsCount + " kontaktów, " + addressCount + " adresów, " + phonesCount + " numerów telefonu"; 
        }
            
        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            Contact contact = new Contact { Name = string.IsNullOrEmpty(textBoxName.Text) == true ? null : textBoxName.Text, Surename = string.IsNullOrEmpty(textBoxSurename.Text) == true ? null : textBoxSurename.Text };
            IObjectSet result = db.QueryByExample(contact);
            Contacts.Clear();
            foreach (var x in result)
            {
                contact = (Contact)x;
                Contacts.Add(new Contact
                {
                    Address = new Address
                    {
                        City = contact.Address.City,
                        PostalCode = contact.Address.PostalCode,
                        Street = contact.Address.Street
                    },
                    Name = contact.Name,
                    Surename = contact.Surename,
                    Id = contact.Id,
                    PhoneNumbers = contact.PhoneNumbers.AsEnumerable().ToList()
                });
            }

            listBoxContacts.ItemsSource = null;
            listBoxPhones.ItemsSource = null;
            listBoxContacts.ItemsSource = Contacts;

            int contactsCount = db.Query<Contact>(typeof(Contact)).Count();
            int addressCount = db.Query<Contact>(typeof(Address)).Count();
            int phonesCount = db.Query<Contact>(typeof(Phone)).Count();

            StatisticsLabel.Content = "W bazie jest " + contactsCount + " kontaktów, " + addressCount + " adresów, " + phonesCount + " numerów telefonu";
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            AddEditContactWindow addEditContactWindow = new AddEditContactWindow(db);
            addEditContactWindow.Title = "Dodaj kontakt";
            addEditContactWindow.ShowDialog();
            
            if (addEditContactWindow.DialogResult == true)
            {
                Contacts = new List<Contact>();
                Contact contact = new Contact();
                IObjectSet result = db.QueryByExample(typeof(Contact));

                foreach (var x in result)
                {
                    contact = (Contact)x;
                    Contacts.Add(new Contact
                    {
                        Address = new Address
                        {
                            City = contact.Address == null ? "" : contact.Address.City,
                            PostalCode = contact.Address == null ? "" : contact.Address.PostalCode,
                            Street = contact.Address == null ? "" : contact.Address.Street
                        },
                        Name = contact.Name,
                        Surename = contact.Surename,
                        Id = contact.Id,
                        PhoneNumbers = contact.PhoneNumbers
                    });
                }

                listBoxContacts.ItemsSource = null;
                listBoxContacts.ItemsSource = Contacts;

                int contactsCount = db.Query<Contact>(typeof(Contact)).Count();
                int addressCount = db.Query<Contact>(typeof(Address)).Count();
                int phonesCount = db.Query<Contact>(typeof(Phone)).Count();

                StatisticsLabel.Content = "W bazie jest " + contactsCount + " kontaktów, " + addressCount + " adresów, " + phonesCount + " numerów telefonu";
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxContacts.SelectedIndex > -1)
            {
                Contact contact = (Contact)listBoxContacts.SelectedItem;       
                Contacts.Remove(contact);
                IObjectSet result = db.QueryByExample(contact);
                contact = (Contact)result.Next();
                db.Delete(contact);

                listBoxContacts.ItemsSource = null;
                listBoxContacts.ItemsSource = Contacts;

                int contactsCount = db.Query<Contact>(typeof(Contact)).Count();
                int addressCount = db.Query<Contact>(typeof(Address)).Count();
                int phonesCount = db.Query<Contact>(typeof(Phone)).Count();

                StatisticsLabel.Content = "W bazie jest " + contactsCount + " kontaktów, " + addressCount + " adresów, " + phonesCount + " numerów telefonu";

            }
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxContacts.SelectedIndex > -1)
            {
                Contact contact = (Contact)listBoxContacts.SelectedItem;
                AddEditContactWindow addEditContactWindow = new AddEditContactWindow(db, contact);
                addEditContactWindow.Title = "Edytuj kontakt";
                addEditContactWindow.ShowDialog();
                if (addEditContactWindow.DialogResult == true)
                {
                    Contacts = new List<Contact>();
                    contact = new Contact();
                    IObjectSet result = db.QueryByExample(typeof(Contact));

                    foreach (var x in result)
                    {
                        contact = (Contact)x;
                        Contacts.Add(new Contact
                        {
                            Address = new Address
                            {
                                City = contact.Address == null ? "" : contact.Address.City,
                                PostalCode = contact.Address == null ? "" : contact.Address.PostalCode,
                                Street = contact.Address == null ? "" : contact.Address.Street
                            },
                            Name = contact.Name,
                            Surename = contact.Surename,
                            Id = contact.Id,
                            PhoneNumbers = contact.PhoneNumbers
                        });
                    }

                    listBoxContacts.ItemsSource = null;
                    listBoxContacts.ItemsSource = Contacts;
                    listBoxPhones.ItemsSource = null;

                    int contactsCount = db.Query<Contact>(typeof(Contact)).Count();
                    int addressCount = db.Query<Contact>(typeof(Address)).Count();
                    int phonesCount = db.Query<Contact>(typeof(Phone)).Count();

                    StatisticsLabel.Content = "W bazie jest " + contactsCount + " kontaktów, " + addressCount + " adresów, " + phonesCount + " numerów telefonu";

                }
            }
        }

        private void listBoxContacts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Phones.Clear();
            if (listBoxContacts.SelectedIndex > -1)
            {
                Contact contact = (Contact)listBoxContacts.SelectedItem;
                foreach (var x in contact.PhoneNumbers)
                {
                    Phones.Add(x);
                }
                listBoxPhones.ItemsSource = null;
                listBoxPhones.ItemsSource = Phones;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            db.Close();
        }
    }
}
