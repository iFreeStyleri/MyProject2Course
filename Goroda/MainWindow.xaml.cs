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
using System.Media;
using System.Windows.Media.Animation;
using MaterialDesignThemes.Wpf;
using Goroda.DataSource;
using System.Data.Entity.Infrastructure;

namespace Goroda
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool StateCard { get; set; }
        private int TempId { get; set; }
        public MainWindow()
        {
            StateCard = false;
            InitializeComponent();
            InitEvents();
            Update();
        }

        private void AnimLeft_Click(object sender, RoutedEventArgs e)
        {
             CountryCard.LeftAnimCard();
             CityCard.LeftAnimCard();
             StreetCard.LeftAnimCard();
        }
        private void AnimRight_Click(object sender, RoutedEventArgs e)
        {
            CountryCard.RightAnimCard();
            CityCard.RightAnimCard();
            StreetCard.RightAnimCard();
        }

        private void InitEvents()
        {
            CityButLeft.Click += AnimLeft_Click;
            CountryButLeft.Click += AnimLeft_Click;

            CountryButRight.Click += AnimRight_Click;
            StreetButRight.Click += AnimRight_Click;

            ButCollapse.Click += ButCollapse_Click;
            ButExit.Click += (s, a) => { Application.Current.Shutdown(); };

            UpCard.MouseLeftButtonDown += (s,a) => { DragMove(); };
        }

        private void ButCollapse_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Update()
        {
            using (var db = new AppDataContext())
            {
                try
                {
                    CountryCardView.Items.Clear();
                    StreetCardView.Items.Clear();
                    CityCardView.Items.Clear();
                    AddCityCountryList.Items.Clear();
                    AddStreetCityList.Items.Clear();

                    foreach (var country in db.Countries)
                    {
                        AddCityCountryList.Items.Add(country.Name);
                    }
                    foreach(var city in db.Cities)
                    {
                        AddStreetCityList.Items.Add(city.Name);
                    }


                    CountryCardView.Items.AddRange(db.Countries.ToArray());
                    StreetCardView.Items.AddRange(db.Streets.ToArray());
                    CityCardView.Items.AddRange(db.Cities.ToArray());
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    Clipboard.SetText(ex.Message);
                }
            }
        }


        private async void CityDel_Click(object sender, RoutedEventArgs e)
        {
            if (CityCardView.SelectedItem != null)
            {
                using (var db = new AppDataContext())
                {
                    try
                    {
                        var temp = await db.Cities.FindAsync(((CityView)CityCardView.SelectedItem).Id);
                        db.Cities.Remove(temp);
                        await db.SaveChangesAsync();
                    }
                    catch (DbUpdateException)
                    {
                        MessageBox.Show("Удалите все районы этого города", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    Update();
                }
            }
        }

        private async void StreetDel_Click(object sender, RoutedEventArgs e)
        {
            if (StreetCardView.SelectedItem != null)
            {
                using (var db = new AppDataContext())
                {
                    var temp = await db.Streets.FindAsync(((Street)StreetCardView.SelectedItem).Id);
                    db.Streets.Remove(temp);
                    await db.SaveChangesAsync();
                    Update();
                }
            }
            
        }

        private async void CountryDel_Click(object sender, RoutedEventArgs e)
        {
            if (CountryCardView.SelectedItem != null)
            {
                using (var db = new AppDataContext())
                {
                    try
                    {
                        var temp = await db.Countries.FindAsync(((Country)CountryCardView.SelectedItem).Id);
                        db.Countries.Remove(temp);
                        await db.SaveChangesAsync();
                    }
                    catch (DbUpdateException)
                    {
                        MessageBox.Show("Удалите все города этой страны", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    Update();
                }
            }
        }

        private void ButAddCancel_Click(object sender, RoutedEventArgs e)
        {
           AddCountry.DownAnimCard(new TextBox[] { AddCountryNameText, AddCountryPopText, AddCountryPresText });
           GridControl.IsEnabled = true;
        }

        private void CountryAdd_Click(object sender, RoutedEventArgs e)
        {
            AddCountry.IsEnabled = false;
            AddCountry.UpAnimCard();
            GridControl.IsEnabled = false;
        }

        private async void AddCountryBut_Click(object sender, RoutedEventArgs e)
        {
            if (AddCountryNameText.Text != null && AddCountryPopText.Text != null && AddCountryPresText.Text != null)
            {
                if (!StateCard)
                {
                    try
                    {
                        using (var db = new AppDataContext())
                        {
                            db.Countries.Add(new Country() { Name = AddCountryNameText.Text, PopulationCount = decimal.Parse(AddCountryPopText.Text), President = AddCountryPresText.Text });
                            await db.SaveChangesAsync();
                            AddCountry.DownAnimCard(new TextBox[] { AddCountryNameText, AddCountryPopText, AddCountryPresText });
                            GridControl.IsEnabled = true;
                        }
                    }
                    catch (FormatException)
                    {
                        MessageBox.Show("Вы ввели некорректное данные в строку населения!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else if(StateCard)
                {
                    try
                    {
                        using (var db = new AppDataContext())
                        {
                            var country = db.Countries.Find(TempId);

                            country.Name = AddCountryNameText.Text;
                            country.PopulationCount = decimal.Parse(AddCountryPopText.Text);
                            country.President = AddCountryPresText.Text;
                            await db.SaveChangesAsync();
                        }
                        StateCard = false;
                    }
                    catch (FormatException)
                    {
                        MessageBox.Show("Вы ввели некорректное данные в строку населения!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                AddCountry.DownAnimCard(new TextBox[] { AddCountryNameText, AddCountryPopText, AddCountryPresText });
                GridControl.IsEnabled = true;
                Update();
            }
            else
            {
                MessageBox.Show("Вы ввели некорректные данные!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void AddCityCNameSearchText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (AddCityCNameSearchText.Text != null)
            {
                AddCityCountryList.Items.Clear();
                using (var db = new AppDataContext())
                {
                    foreach (var country in db.Countries)
                    {
                        if (country.Name.Contains(AddCityCNameSearchText.Text))
                        {
                            AddCityCountryList.Items.Add(country.Name);
                        }
                    }
                }
            }
        }

        private async void AddCityBut_Click(object sender, RoutedEventArgs e)
        {
            if(AddCityNameText.Text != null && AddCityPopText.Text != null && AddCityCountryList.SelectedItem != null)
            {
                if (!StateCard)
                {
                    try
                    {
                        using (var db = new AppDataContext())
                        {
                            Country tempCountry = new Country();
                            foreach (var country in db.Countries)
                            {
                                if (country.Name == (string)AddCityCountryList.SelectedItem)
                                {
                                    tempCountry = country;
                                    break;
                                }
                            }

                            db.Cities.Add(new City() { Name = AddCityNameText.Text, Country = tempCountry, PopulationCount = decimal.Parse(AddCityPopText.Text) });
                            await db.SaveChangesAsync();
                        }
                        AddCity.DownAnimCard(new TextBox[] { AddCityNameText, AddCityPopText, AddCityCNameSearchText });
                        GridControl.IsEnabled = true;
                        Update();
                    }
                    catch (FormatException)
                    {
                        MessageBox.Show("Вы ввели некорректные количество населения!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else if(StateCard)
                {
                    try
                    {
                        using (var db = new AppDataContext())
                        {
                            var tempCity = db.Cities.Find(TempId);
                            tempCity.Name = AddCityNameText.Text;
                            tempCity.PopulationCount = int.Parse(AddCityPopText.Text);

                            foreach(var country in db.Countries)
                            {
                                if(country.Name == (string)AddCityCountryList.SelectedItem)
                                {
                                    tempCity.Country = country;
                                    break;
                                }
                            }
                            await db.SaveChangesAsync();
                        }
                        AddCity.DownAnimCard(new TextBox[] { AddCityNameText, AddCityPopText, AddCityCNameSearchText });
                        GridControl.IsEnabled = true;
                        StateCard = false;
                        Update();
                    }
                    catch (FormatException)
                    {
                        MessageBox.Show("Вы ввели некорректные количество населения!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Вы ввели некорректные данные!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CityAdd_Click(object sender, RoutedEventArgs e)
        {
            AddCity.UpAnimCard();
            GridControl.IsEnabled = false;
        }

        private void ButAddCityCancel_Click(object sender, RoutedEventArgs e)
        {
            AddCity.DownAnimCard(new TextBox[] { AddCityNameText, AddCityPopText });
            GridControl.IsEnabled = true;
        }

        private void AddStreetCityNameSearchText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(AddStreetCityNameSearchText.Text != null)
            {
                AddStreetCityList.Items.Clear();
                using(var db = new AppDataContext())
                {
                    foreach(var city in db.Cities)
                    {
                        if(city.Name.Contains(AddStreetCityNameSearchText.Text))
                        {
                            AddStreetCityList.Items.Add(city.Name);
                        }
                    }
                }
            }
        }

        private async void AddStreetBut_Click(object sender, RoutedEventArgs e)
        {
            if(AddStreetCityList.SelectedItem != null && AddStreetNameText.Text != null)
            {
                if (!StateCard)
                {
                    using (var db = new AppDataContext())
                    {
                        City tempCity = new City();

                        foreach (var city in db.Cities)
                        {
                            if (city.Name == (string)AddStreetCityList.SelectedItem)
                            {
                                tempCity = city;
                            }
                        }

                        db.Streets.Add(new Street() { City = tempCity, Name = AddStreetNameText.Text });
                        await db.SaveChangesAsync();
                    }
                }
                else if(StateCard)
                {
                    using(var db = new AppDataContext())
                    {
                        var street = db.Streets.Find(TempId);
                        street.Name = AddStreetNameText.Text;
                        
                        foreach(var city in db.Cities)
                        {
                            if(city.Name == (string)AddStreetCityList.SelectedItem)
                            {
                                street.City = city;
                            }
                        }

                        await db.SaveChangesAsync();
                    }
                    StateCard = false;
                }
                AddStreet.DownAnimCard(new TextBox[] { AddStreetNameText, AddStreetCityNameSearchText });
                GridControl.IsEnabled = true;
                Update();
            }

        }

        private void StreetAdd_Click(object sender, RoutedEventArgs e)
        {
            AddStreet.UpAnimCard();
            GridControl.IsEnabled = false;
        }

        private void ButAddStreetCancel_Click(object sender, RoutedEventArgs e)
        {
            AddStreet.DownAnimCard(new TextBox[] {AddStreetCityNameSearchText, AddStreetNameText });
            GridControl.IsEnabled = true;
        }

        private void StreetUpd_Click(object sender, RoutedEventArgs e)
        {
            if (StreetCardView.SelectedItem != null)
            {
                TempId = ((Street)StreetCardView.SelectedItem).Id;
                AddStreet.UpAnimCard();
                GridControl.IsEnabled = false;
                StateCard = true;
            }
        }

        private void CountryUpd_Click(object sender, RoutedEventArgs e)
        {
            if (CountryCardView.SelectedItem != null)
            {
                TempId = ((Country)CountryCardView.SelectedItem).Id;
                AddCountry.UpAnimCard();
                GridControl.IsEnabled = false;
                StateCard = true;
            }
        }

        private void CityUpd_Click(object sender, RoutedEventArgs e)
        {
            if (CityCardView.SelectedItem != null)
            {
                TempId = ((City)CityCardView.SelectedItem).Id;
                AddCity.UpAnimCard();
                GridControl.IsEnabled = false;
                StateCard = true;
            }
        }
    }
}
