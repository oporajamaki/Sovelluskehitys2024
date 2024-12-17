using System;
using System.Data;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ControlzEx.Theming;
using MahApps.Metro.Controls;
using Microsoft.Data.SqlClient;

namespace Sovelluskehitys2024
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        /// <summary>
        /// string polku = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\k2201452\\Documents\\levykauppa.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=True";
        /// </summary>
        string polku = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Käyttäjä\\OneDrive\\Tiedostot\\levykauppa.mdf;Integrated Security = True; Connect Timeout = 30;Encrypt=True";
        public MainWindow()
        {
            InitializeComponent();

            ThemeManager.Current.ChangeTheme(this, "Light.Taupe");

            try
            {
                PaivitaDataGrid("SELECT * FROM tuotteet", "tuotteet", tuotelista);
                PaivitaDataGrid("SELECT * FROM asiakkaat", "asiakkaat", asiakaslista);
                PaivitaDataGrid("SELECT * FROM tilaus", "tilaus", tilatut);
                PaivitaDataGrid("SELECT * FROM myyjat", "myyjat", myyjat);
                //PaivitaDataGrid("SELECT * FROM tilaus", "tilaus", tilatut);
                //PaivitaDataGrid("SELECT * FROM tilaukset", "tilaukset", tilatutlista_cb);
                // toimiiko tämä
                //PaivitaDataGrid("SELECT ti.id as id, a.nimi as asiakas, tu.nimi as tuote FROM tilaukset ti, asiakkaat a, tuotteet tu WHERE a.id=ti.asiakas_id AND tu.id=ti.tuote_id AND ti.toimitettu='0'", "tilaukset", tilauslista);
                //PaivitaDataGrid("SELECT ti.id as id, a.nimi as asiakas, tu.nimi as tuote FROM tilaukset ti, asiakkaat a, tuotteet tu WHERE a.id=ti.asiakas_id AND tu.id=ti.tuote_id AND ti.toimitettu='1'", "tilaukset", toimitetutlista);
                PaivitaComboBox(tuotelista_cb, tuotelista_cb_3);
                PaivitaComboBox(tilatutlista_cb, tuotelista_cb_3);
                PaivitaAsiakasComboBox();
                PaivitaComboBoxAsiakas();
                PaivitaComboBoxMyyja();
                PaivaMaara();
            }
            catch
            {
                tilaviesti.Text = "Ei tietokantayhteyttä";
            }
        }

        public void PaivaMaara()
        {
            InitializeComponent();
            paivamaara.Text = DateTime.Now.ToShortDateString();
        }

        private void PaivitaDataGrid(string kysely, string taulu, DataGrid grid)
        {
            SqlConnection yhteys = new SqlConnection(polku);
            yhteys.Open();

            SqlCommand komento = yhteys.CreateCommand();
            komento.CommandText = kysely;

            SqlDataAdapter adapteri = new SqlDataAdapter(komento);
            DataTable dt = new DataTable(taulu);
            adapteri.Fill(dt);

            grid.ItemsSource = dt.DefaultView;

            yhteys.Close();
        }

        private void PaivitaComboBox(ComboBox kombo1, ComboBox kombo2)
        {
            //tuotelista_cb.Items.Clear();

            SqlConnection yhteys = new SqlConnection(polku);
            yhteys.Open();

            SqlCommand komento = new SqlCommand("SELECT * FROM tuotteet", yhteys);
            SqlDataReader lukija = komento.ExecuteReader();

            DataTable taulu = new DataTable();
            taulu.Columns.Add("ID", typeof(string));
            taulu.Columns.Add("NIMI", typeof(string));
            taulu.Columns.Add("NIMI_LEVY", typeof(string));


            /* tehdään sidokset että comboboxissa näytetää datataulua*/
            kombo1.ItemsSource = taulu.DefaultView;
            kombo1.DisplayMemberPath = "NIMI_LEVY";
            kombo1.SelectedValuePath = "ID";

            kombo2.ItemsSource = taulu.DefaultView;
            kombo2.DisplayMemberPath = "NIMI_LEVY";
            kombo2.SelectedValuePath = "ID";

            while (lukija.Read()) // käsitellään kyselytulos rivi riviltä
            {
                int id = lukija.GetInt32(0);
                string nimi = lukija.GetString(1);
                string nimi2 = lukija.GetString(2);
                taulu.Rows.Add(id, nimi, $"{nimi} - {nimi2}"); // lisätään datatauluun rivi tietoineen
                //tuotelista_cb.Items.Add(lukija.GetString(1));
            }
            lukija.Close();

            yhteys.Close();
        }


        private void PaivitaAsiakasComboBox()
        {
            //tuotelista_cb.Items.Clear();

            SqlConnection yhteys = new SqlConnection(polku);
            yhteys.Open();

            SqlCommand komento = new SqlCommand("SELECT * FROM asiakkaat", yhteys);
            SqlDataReader lukija = komento.ExecuteReader();

            DataTable taulu = new DataTable();
            taulu.Columns.Add("ID", typeof(string));
            taulu.Columns.Add("NIMI", typeof(string));

            /* tehdään sidokset että comboboxissa näytetää datataulua*/
            asiakaslista_2.ItemsSource = taulu.DefaultView;
            asiakaslista_2.DisplayMemberPath = "NIMI";
            asiakaslista_2.SelectedValuePath = "ID";

            while (lukija.Read()) // käsitellään kyselytulos rivi riviltä
            {
                int id = lukija.GetInt32(0);
                string nimi = lukija.GetString(1);
                taulu.Rows.Add(id, nimi); // lisätään datatauluun rivi tietoineen
                //tuotelista_cb.Items.Add(lukija.GetString(1));
            }
            lukija.Close();

            yhteys.Close();
        }
        private void PaivitaComboBoxAsiakas()
        {
            //tuotelista_cb.Items.Clear();

            SqlConnection yhteys = new SqlConnection(polku);
            yhteys.Open();

            SqlCommand komento = new SqlCommand("SELECT * FROM asiakkaat", yhteys);
            SqlDataReader lukija = komento.ExecuteReader();

            DataTable taulu = new DataTable();
            taulu.Columns.Add("ID", typeof(string));
            taulu.Columns.Add("NIMI", typeof(string));

            /* tehdään sidokset että comboboxissa näytetää datataulua*/
            asiakaslista.ItemsSource = taulu.DefaultView;
            asiakaslista.DisplayMemberPath = "NIMI";
            asiakaslista.SelectedValuePath = "ID";

            while (lukija.Read()) // käsitellään kyselytulos rivi riviltä
            {
                int id = lukija.GetInt32(0);
                string nimi = lukija.GetString(1);
                taulu.Rows.Add(id, nimi); // lisätään datatauluun rivi tietoineen
                //tuotelista_cb.Items.Add(lukija.GetString(1));
            }
            lukija.Close();

            yhteys.Close();
        }
        private void PaivitaComboBoxMyyja()
        {
            //tuotelista_cb.Items.Clear();

            SqlConnection yhteys = new SqlConnection(polku);
            yhteys.Open();

            SqlCommand komento = new SqlCommand("SELECT * FROM myyjat", yhteys);
            SqlDataReader lukija = komento.ExecuteReader();

            DataTable taulu = new DataTable();
            taulu.Columns.Add("ID", typeof(string));
            taulu.Columns.Add("NIMI", typeof(string));

            /* tehdään sidokset että comboboxissa näytetää datataulua*/
            myyjalista.ItemsSource = taulu.DefaultView;
            myyjalista.DisplayMemberPath = "NIMI";
            myyjalista.SelectedValuePath = "ID";

            while (lukija.Read()) // käsitellään kyselytulos rivi riviltä
            {
                int id = lukija.GetInt32(0);
                string nimi = lukija.GetString(1);
                taulu.Rows.Add(id, nimi); // lisätään datatauluun rivi tietoineen
                //tuotelista_cb.Items.Add(lukija.GetString(1));
            }
            lukija.Close();

            yhteys.Close();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PaivitaDataGrid("SELECT * FROM tuotteet", "tuotteet", tuotelista);
            PaivitaComboBox(tuotelista_cb, tuotelista_cb_3);

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SqlConnection yhteys = new SqlConnection(polku);
            yhteys.Open();

            string kysely = "INSERT INTO tuotteet (artisti, levyn_nimi, hinta) VALUES ('" + artisti.Text + "','" + levy.Text + "'," + hinta.Text + ");";
            SqlCommand komento = new SqlCommand(kysely, yhteys);
            komento.ExecuteNonQuery();

            yhteys.Close();

            PaivitaDataGrid("SELECT * FROM tuotteet", "tuotteet", tuotelista);
            PaivitaComboBox(tuotelista_cb, tuotelista_cb_3);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            SqlConnection yhteys = new SqlConnection(polku);
            yhteys.Open();

            string id = tuotelista_cb.SelectedValue.ToString();
            string kysely = "DELETE FROM tuotteet WHERE id='" + id + "';";
            SqlCommand komento = new SqlCommand(kysely, yhteys);
            komento.ExecuteNonQuery();
            yhteys.Close();

            PaivitaDataGrid("SELECT * FROM tuotteet", "tuotteet", tuotelista);
            PaivitaComboBox(tuotelista_cb, tuotelista_cb_3);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            SqlConnection yhteys = new SqlConnection(polku);
            yhteys.Open();

            string kysely = "INSERT INTO asiakkaat (nimi, osoite, email) VALUES ('" + asiakasnimi.Text + "','" + asiakasosoite.Text + "','" + asiakasemail.Text + "');";
            SqlCommand komento = new SqlCommand(kysely, yhteys);
            komento.ExecuteNonQuery();

            yhteys.Close();

            PaivitaDataGrid("SELECT * FROM asiakkaat", "asiakkaat", asiakaslista);
            PaivitaAsiakasComboBox();
        }

        /* private void Button_Click_4(object sender, RoutedEventArgs e)
         {
             SqlConnection yhteys = new SqlConnection(polku);
             yhteys.Open();

             string asiakasID = asiakaslista_2.SelectedValue.ToString();
             string tuoteID = tuotelista_cb_2.SelectedValue.ToString();

             string sql = "INSERT INTO tilaukset (asiakas_id, tuote_id) VALUES ('" + asiakasID + "','" + tuoteID + "')";

             SqlCommand komento = new SqlCommand(sql, yhteys);
             komento.ExecuteNonQuery();

             yhteys.Close();

             PaivitaDataGrid("SELECT ti.id as id, a.nimi as asiakas, tu.nimi as tuote FROM tilaukset ti, asiakkaat a, tuotteet tu WHERE a.id=ti.asiakas_id AND tu.id=ti.tuote_id AND ti.toimitettu='0'", "tilaukset", tilauslista);
         }*/

        private void toimita_tilaus_Click(object sender, RoutedEventArgs e)
        {
            DataRowView rivinakyma = (DataRowView)((Button)e.Source).DataContext;
            String tilaus_id = rivinakyma[0].ToString();

            SqlConnection yhteys = new SqlConnection(polku);
            yhteys.Open();

            string sql = "UPDATE tilaukset SET toimitettu=1 WHERE id='" + tilaus_id + "';";

            SqlCommand komento = new SqlCommand(sql, yhteys);
            komento.ExecuteNonQuery();

            yhteys.Close();

            PaivitaDataGrid("SELECT ti.id as id, a.nimi as asiakas, tu.nimi as tuote FROM tilaukset ti, asiakkaat a, tuotteet tu WHERE a.id=ti.asiakas_id AND tu.id=ti.tuote_id AND ti.toimitettu='0'", "tilaukset", tilauslista);
            PaivitaDataGrid("SELECT ti.id as id, a.nimi as asiakas, tu.nimi as tuote FROM tilaukset ti, asiakkaat a, tuotteet tu WHERE a.id=ti.asiakas_id AND tu.id=ti.tuote_id AND ti.toimitettu='1'", "tilaukset", toimitetutlista);
        }
        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            SqlConnection yhteys = new SqlConnection(polku);
            yhteys.Open();

            string kysely = "INSERT INTO myyjat (nimi) VALUES ('" + myyjanimi.Text + "');";
            SqlCommand komento = new SqlCommand(kysely, yhteys);
            komento.ExecuteNonQuery();

            yhteys.Close();

            PaivitaDataGrid("SELECT * FROM myyjat", "myyjat", myyjat);
            PaivitaAsiakasComboBox();
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            using (SqlConnection yhteys = new SqlConnection(polku))
            {
                try
                {
                    yhteys.Open();

                    string asiakasID = asiakaslista_2.SelectedValue.ToString();
                    string myyjaID = myyjalista.SelectedValue.ToString();

                    // Muunnetaan paivamaara.Text DateTime-objektiksi
                    DateTime paiva = DateTime.Parse(paivamaara.Text);

                    string sql = "INSERT INTO tilaus (asiakas_id, myyja_id, paiva) VALUES (@asiakasID, @myyjaID, @paiva)";

                    using (SqlCommand komento = new SqlCommand(sql, yhteys))
                    {
                        // Lisätään parametrit turvallisesti
                        komento.Parameters.AddWithValue("@asiakasID", asiakasID);
                        komento.Parameters.AddWithValue("@myyjaID", myyjaID);
                        komento.Parameters.AddWithValue("@paiva", paiva);

                        komento.ExecuteNonQuery();
                        PaivitaDataGrid("SELECT * FROM tilaus", "tilaus", tilatut);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Virhe: " + ex.Message);
                }
            }
        }
    }
}

       /* private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            SqlConnection yhteys = new SqlConnection(polku);
            yhteys.Open();

            string tuoteID = asiakaslista_2.SelectedValue.ToString();
            string tilausID = myyjalista.SelectedValue.ToString();
            int maara = maara.SelectedValue.ToString();

            string sql = "INSERT INTO tilaukset (tuote_id, tilaus_id) VALUES ('" + tuoteID + "','" + tilausID + "'," + maara + ")";

            SqlCommand komento = new SqlCommand(sql, yhteys);
            komento.ExecuteNonQuery();

            yhteys.Close();

            PaivitaDataGrid("SELECT ti.id as id, a.nimi as asiakas, tu.nimi as tuote FROM tilaukset ti, asiakkaat a, tuotteet tu WHERE a.id=ti.asiakas_id AND tu.id=ti.tuote_id AND ti.toimitettu='0'", "tilaukset", tilatut);
        }

    }
}*/