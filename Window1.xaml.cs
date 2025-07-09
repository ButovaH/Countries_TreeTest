using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private ObservableCollection<Country> _MyTree;
        public Window1()
        {
            InitializeComponent();

            Country firstCountry = new Country() { Name = "Россия" };

            Region first_region = new Region() { Name = "Московская область" };
            Region second_region = new Region() { Name = "Калининградская область" };

            City first_city = new City() { Name = "Мытищи" };
            City second_city = new City() { Name = "Королёв" };
            City third_city = new City() { Name = "Калининград" };

            firstCountry.Childs.Add(first_region);
            firstCountry.Childs.Add(second_region);

            first_region.Childs.Add(first_city);
            first_region.Childs.Add(second_city);

            second_region.Childs.Add(third_city);

            DataContext = this;


            _MyTree = new ObservableCollection<Country>()
            {
                firstCountry
            };
        }

        public ObservableCollection<Country> MyTree
        {
            get { return _MyTree; }
        }

        private void TreeViewItem_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as TreeViewItem;
            if (item != null)
            {
                item.IsSelected = true;
            }
        }

        private void countryContextMenu_Add_Click(object sender, RoutedEventArgs e)
        {
            var menu = (MenuItem)e.Source;
            object tmp = trView.SelectedItem;
            Country country1 = (Country)tmp;
            if (country1.IsSelected)
            {
                MessageBox.Show("Типа добавили регион");
            }
            else
            {
                MessageBox.Show("Не получилось добавить регион, т.к. isSelected = false..");
            }
        }
        private void MyTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            switch(trView.SelectedItem) {
                case Country selectedNode:
                {
                    selectedNode.AddCountryLeftClickCommand.Execute(selectedNode);
                    break;
                }
                case Region selectedNode:
                {
                    selectedNode.AddRegionLeftClickCommand.Execute(selectedNode);
                    break;
                }
                case City selectedNode:
                {
                    selectedNode.AddCityLeftClickCommand.Execute(selectedNode);
                    break;
                }
                default:
                {
                    MessageBox.Show("Нажали куда-то не туда");
                    break;
                }
            }
            //if (trView.SelectedItem is Country selectedNode)
            //{
            //    selectedNode.AddCountryLeftClickCommand.Execute(selectedNode);
            //}
        }

        private void regionContextMenu_Add_Click(object sender, RoutedEventArgs e)
        {
            object tmp = trView.SelectedItem;
            Region region = (Region)tmp;
            if (region.IsSelected)
            {
                MessageBox.Show("Типа добавили город");
            }
            else
            {
                MessageBox.Show("Не получилось добавить город, т.к. isSelected = false..");
            }
        }

        private void regionContextMenu_Delete_Click(object sender, RoutedEventArgs e)
        {
            object tmp = trView.SelectedItem;
            Region region = (Region)tmp;
            if (region.IsSelected)
            {
                MessageBox.Show("Типа удалили регион");
            }
            else
            {
                MessageBox.Show("Не получилось удалить регион, т.к. isSelected = false..");
            }
        }

        private void cityContextMenu_Delete_Click(object sender, RoutedEventArgs e)
        {
            object tmp = trView.SelectedItem;
            City city = (City)tmp;
            if (city.IsSelected)
            {
                MessageBox.Show("Типа удалили город");
            }
            else
            {
                MessageBox.Show("Не получилось удалить город, т.к. isSelected = false..");
            }
        }
    }

    public class Country :  INotifyPropertyChanged
    {
        public string Name { get; set; }

        public ObservableCollection<Region> Childs { get; set; }

        public Country()
        {
            Childs = new ObservableCollection<Region>();
            AddCountryCommand = new Command(country_add);
            AddCountryLeftClickCommand = new Command(country_left_click);
        }
        public ICommand AddCountryCommand { get; private set; }
        public ICommand AddCountryLeftClickCommand { get; private set; }

        private void country_add(object v)
        {
            MessageBox.Show("Типа добавили регион");
        }

        private void country_left_click(object v)
        {
            if (v is Country selectedNode)
            {
                // Выполните действия с выбранным узлом дерева
                MessageBox.Show($"Щелкнули левой кнопкой по стране {selectedNode.Name}");
            }
        }

        bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value != _isSelected)
                {
                    _isSelected = value;
                    this.OnPropertyChanged("IsSelected");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class Region : INotifyPropertyChanged
    {
        public string Name { get; set; }

        public ObservableCollection<City> Childs { get; set; }

        public Region()
        {
            Childs = new ObservableCollection<City>();
            AddRegionLeftClickCommand = new Command(region_left_click);
        }
        public ICommand AddRegionLeftClickCommand { get; private set; }

        private void region_left_click(object v)
        {
            if (v is Region selectedNode)
            {
                // Выполните действия с выбранным узлом дерева
                MessageBox.Show($"Щелкнули левой кнопкой по региону \"{selectedNode.Name}\"");
            }
        }

        bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value != _isSelected)
                {
                    _isSelected = value;
                    this.OnPropertyChanged("IsSelected");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class City : INotifyPropertyChanged
    {
        public string Name { get; set; }

        public City()
        {
            AddCityLeftClickCommand = new Command(city_left_click);
        }
        public ICommand AddCityLeftClickCommand { get; private set; }

        private void city_left_click(object v)
        {
            if (v is City selectedNode)
            {
                // Выполните действия с выбранным узлом дерева
                MessageBox.Show($"Щелкнули левой кнопкой по городу \"{selectedNode.Name}\"");
            }
        }

        bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value != _isSelected)
                {
                    _isSelected = value;
                    this.OnPropertyChanged("IsSelected");
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
