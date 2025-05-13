using System;
using System.Windows;
using System.Windows.Input;

namespace WpfApp1.DoctorView
{
    /// <summary>
    /// Interaction logic for DoctorV.xaml
    /// </summary>
    public partial class DoctorV : Window
    {
        private int _accessID;

        // Konstruktor, amely fogadja az accessID-t
        public DoctorV(int accessID)
        {
            InitializeComponent();
            _accessID = accessID;  // Beállítjuk az accessID értékét
        }

        // Gomb eseménykezelők
        private void News_Click(object sender, RoutedEventArgs e)
        {
            // Hírek gomb kattintásának kezelése
            MessageBox.Show("Hírek megjelenítése...");
        }

        private void Treatments_Click(object sender, RoutedEventArgs e)
        {
            // Kezelések gomb kattintásának kezelése
            MessageBox.Show("Kezelések megjelenítése...");
        }

        private void MyAppointments_Click(object sender, RoutedEventArgs e)
        {
            // Foglalások gomb kattintásának kezelése
            MessageBox.Show("Foglalásaim megjelenítése...");
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            // Kijelentkezés gomb kattintásának kezelése
            MessageBox.Show("Kijelentkezés...");
            // Lehetőség a kijelentkezés logikájának kezelésére, pl. ablak bezárása:
            this.Close();
        }

        // Kép kattintási események
        private void Dora_Click(object sender, MouseButtonEventArgs e)
        {
            // Első orvos kép kattintásának kezelése
            MessageBox.Show("Dr. Mesterség Ash információk...");
        }

        private void Mesterseges_Click(object sender, MouseButtonEventArgs e)
        {
            // Második orvos kép kattintásának kezelése
            MessageBox.Show("Dr. Sipkovics Dóra információk...");
        }

        private void Musky_Click(object sender, MouseButtonEventArgs e)
        {
            // Harmadik orvos kép kattintásának kezelése
            MessageBox.Show("Dr. Muskovics Alan információk...");
        }

        private void UserGrid_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
