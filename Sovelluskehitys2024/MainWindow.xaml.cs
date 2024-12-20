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
                PaivitaDataGrid2();
                //PaivitaDataGrid("SELECT * FROM tilaus", "tilaus", tilatut);
                //PaivitaDataGrid("SELECT * FROM tilaukset", "tilaukset", tilatutlista_cb);
                // toimiiko tämä
                //PaivitaDataGrid("SELECT ti.id as id, a.nimi as asiakas, tu.nimi as tuote FROM tilaukset ti, asiakkaat a, tuotteet tu WHERE a.id=ti.asiakas_id AND tu.id=ti.tuote_id AND ti.toimitettu='0'", "tilaukset", tilauslista);
                //PaivitaDataGrid("SELECT ti.id as id, a.nimi as asiakas, tu.nimi as tuote FROM tilaukset ti, asiakkaat a, tuotteet tu WHERE a.id=ti.asiakas_id AND tu.id=ti.tuote_id AND ti.toimitettu='1'", "tilaukset", toimitetutlista);
                PaivitaComboBox(tuotelista_cb, tuotelista_cb_3);
                //PaivitaComboBox(tilatutlista_cb, tuotelista_cb_3);
                PaivitaComboBoxTilausID();
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

        private void PaivitaDataGrid2()
        {
            using (SqlConnection yhteys = new SqlConnection(polku))
            {
                try
                {
                    yhteys.Open();

                    string sql = @"
                SELECT 
                    ti.id AS id,
                    tu.artisti + ' - ' + tu.levyn_nimi AS tuote,
                    t.id AS tilausid,
                    ti.maara AS maara
                FROM 
                    tilaukset ti
                    JOIN tilaus t ON ti.tilaus_id = t.id
                    JOIN tuotteet tu ON ti.tuote_id = tu.id";


                /*    string sql = @"
                SELECT 
                    ti.id AS id, 
                    a.nimi AS asiakas, 
                    tu.artisti + ' - ' + tu.levyn_nimi AS tuote, 
                    ti.maara
                FROM 
                    tilaukset ti
                    JOIN tilaus t ON ti.tilaus_id = t.id
                    JOIN asiakkaat a ON t.asiakas_id = a.id
                    JOIN tuotteet tu ON ti.tuote_id = tu.id";*/

                    SqlDataAdapter adapter = new SqlDataAdapter(sql, yhteys);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    tilauslista.ItemsSource = dt.DefaultView;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Virhe datagridin päivityksessä: " + ex.Message);
                }
            }
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
        /*private void PaivitaComboBoxTilausID()
        {
            //tuotelista_cb.Items.Clear();

            SqlConnection yhteys = new SqlConnection(polku);
            yhteys.Open();

            SqlCommand komento = new SqlCommand("SELECT * FROM tilaus", yhteys);
            SqlDataReader lukija = komento.ExecuteReader();

            DataTable taulu = new DataTable();
            taulu.Columns.Add("ID", typeof(string));
            taulu.Columns.Add("NIMI", typeof(string));

            /* tehdään sidokset että comboboxissa näytetää datataulua*/
            /*tilatutlista_cb.ItemsSource = taulu.DefaultView;
            tilatutlista_cb.DisplayMemberPath = "NIMI";
            tilatutlista_cb.SelectedValuePath = "ID";

            while (lukija.Read()) // käsitellään kyselytulos rivi riviltä
            {
                int id = lukija.GetInt32(0);
                string nimi = lukija.GetString(1);
                taulu.Rows.Add(id,nimi); // lisätään datatauluun rivi tietoineen
                //tuotelista_cb.Items.Add(lukija.GetString(1));
            }
            lukija.Close();

            yhteys.Close();
        }*/
        private void PaivitaComboBoxTilausID()
        {
            // Tyhjennetään ComboBox ennen päivittämistä
            tilatutlista_cb.ItemsSource = null;

            using (SqlConnection yhteys = new SqlConnection(polku))
            {
                try
                {
                    yhteys.Open();

                    SqlCommand komento = new SqlCommand("SELECT id FROM tilaus", yhteys);
                    SqlDataReader lukija = komento.ExecuteReader();

                    DataTable taulu = new DataTable();
                    taulu.Columns.Add("ID", typeof(int));

                    // Lisätään kyselytulokset DataTableen
                    while (lukija.Read())
                    {
                        int id = lukija.GetInt32(0);
                        taulu.Rows.Add(id);
                    }

                    lukija.Close();

                    // Asetetaan ComboBoxin lähteeksi DataTable
                    tilatutlista_cb.ItemsSource = taulu.DefaultView;
                    tilatutlista_cb.DisplayMemberPath = "ID";
                    tilatutlista_cb.SelectedValuePath = "ID";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Virhe: " + ex.Message);
                }
            }
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
        /*
        private void toimita_tilaus_Click(object sender, RoutedEventArgs e)
        {
            DataRowView rivinakyma = (DataRowView)((Button)e.Source).DataContext;
            string tilaus_id = rivinakyma["id"].ToString();
            string tuote_id = rivinakyma["tuote_id"].ToString();

            using (SqlConnection yhteys = new SqlConnection(polku))
            {
                yhteys.Open();

                // Lisää tieto toimitetut-tauluun
                string insertSql = @"
            INSERT INTO toimitetut (tilaus_id, tuote_id, toimituspaiva)
            VALUES (@tilaus_id, @tuote_id, GETDATE());";

                using (SqlCommand insertKomento = new SqlCommand(insertSql, yhteys))
                {
                    insertKomento.Parameters.AddWithValue("@tilaus_id", tilaus_id);
                    insertKomento.Parameters.AddWithValue("@tuote_id", tuote_id);
                    insertKomento.ExecuteNonQuery();
                }

                // Päivitä tilaukset-taulu, merkitse toimitetuksi
                string updateSql = "UPDATE tilaukset SET toimitettu = 1 WHERE id = @tilaus_id;";

                using (SqlCommand updateKomento = new SqlCommand(updateSql, yhteys))
                {
                    updateKomento.Parameters.AddWithValue("@tilaus_id", tilaus_id);
                    updateKomento.ExecuteNonQuery();
                }
            }

            // Päivitä DataGridit
            PaivitaDataGrid(
                "SELECT ti.id as id, tu.id as tuote_id, tu.artisti + ' - ' + tu.levyn_nimi as tuote, ti.maara " +
                "FROM tilaukset ti JOIN tuotteet tu ON ti.tuote_id = tu.id WHERE ti.toimitettu = 0",
                "tilaukset", tilauslista);

            PaivitaDataGrid(
                "SELECT t.id as id, tu.artisti + ' - ' + tu.levyn_nimi as tuote, t.toimituspaiva " +
                "FROM toimitetut t JOIN tuotteet tu ON t.tuote_id = tu.id",
                "toimitetut", toimitetutlista);
        }*/

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
        /*private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            SqlConnection yhteys = new SqlConnection(polku);
            yhteys.Open();

            string tuoteID = tilatutlista_cb.SelectedValue.ToString();
            string tilausID = tuotelista_cb_3.SelectedValue.ToString();
            int maara;

            string sql = "INSERT INTO tilaukset (tuote_id, tilaus_id) VALUES ('" + tuoteID + "','" + tilausID + "'," + maara + ")";

            SqlCommand komento = new SqlCommand(sql, yhteys);
            komento.ExecuteNonQuery();

            yhteys.Close();

            PaivitaDataGrid("SELECT ti.id as id, a.nimi as asiakas, tu.nimi as tuote FROM tilaukset ti, asiakkaat a, tuotteet tu WHERE a.id=ti.asiakas_id AND tu.id=ti.tuote_id AND ti.toimitettu='0'", "tilaukset", tilatut);
        }*/
        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            if (tuotelista_cb_3.SelectedValue == null || tilatutlista_cb.SelectedValue == null || string.IsNullOrEmpty(maaralista.Text))
            {
                MessageBox.Show("Kaikki kentät tulee täyttää ennen tilauksen lisäämistä.");
                return;
            }

            int tuoteID = Convert.ToInt32(tuotelista_cb_3.SelectedValue);
            int tilausID = Convert.ToInt32(tilatutlista_cb.SelectedValue);
            int maara;

            if (!int.TryParse(maaralista.Text, out maara))
            {
                MessageBox.Show("Määrän tulee olla numero.");
                return;
            }

            using (SqlConnection yhteys = new SqlConnection(polku))
            {
                try
                {
                    yhteys.Open();

                    string sql = "INSERT INTO tilaukset (tuote_id, tilaus_id, maara) VALUES (@tuoteID, @tilausID, @maara)";

                    using (SqlCommand komento = new SqlCommand(sql, yhteys))
                    {
                        komento.Parameters.AddWithValue("@tuoteID", tuoteID);
                        komento.Parameters.AddWithValue("@tilausID", tilausID);
                        komento.Parameters.AddWithValue("@maara", maara);

                        komento.ExecuteNonQuery();
                    }

                    MessageBox.Show("Tilaus lisätty onnistuneesti!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Virhe: " + ex.Message);
                }
            }
            PaivitaDataGrid2();
        }

        /*private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            // Tarkistetaan, että kentät eivät ole tyhjiä
            if (tilatutlista_cb.SelectedValue == null || tuotelista_cb_3.SelectedValue == null || string.IsNullOrEmpty(maaralista.Text))
            {
                MessageBox.Show("Kaikki kentät tulee täyttää ennen tilauksen lisäämistä.");
                return;
            }

            // Haetaan arvot käyttöliittymästä
            string tuoteID = tilatutlista_cb.SelectedValue.ToString();
            string tilausID = tuotelista_cb_3.SelectedValue.ToString();
            int maara;

            // Yritetään parsia määrä kokonaisluvuksi
            if (!int.TryParse(maaralista.Text, out maara))
            {
                MessageBox.Show("Määrän tulee olla numero.");
                return;
            }

            // Yhteys tietokantaan
            using (SqlConnection yhteys = new SqlConnection(polku))
            {
                try
                {
                    yhteys.Open();

                    // SQL-kysely parametrien kanssa
                    string sql = "INSERT INTO tilaukset (tuote_id, tilaus_id, maara) VALUES (@tuoteID, @tilausID, @maara)";

                    using (SqlCommand komento = new SqlCommand(sql, yhteys))
                    {
                        // Lisätään parametrit komennolle
                        komento.Parameters.AddWithValue("@tuoteID", tuoteID);
                        komento.Parameters.AddWithValue("@tilausID", tilausID);
                        komento.Parameters.AddWithValue("@maara", maara);

                        // Suoritetaan komento
                        komento.ExecuteNonQuery();
                    }

                    MessageBox.Show("Tilaus lisätty onnistuneesti!");

                    // Päivitetään DataGrid
                    PaivitaDataGrid(
                        "SELECT ti.id as id, a.nimi as asiakas, tu.nimi as tuote FROM tilaukset ti " +
                        "JOIN asiakkaat a ON a.id = ti.asiakas_id " +
                        "JOIN tuotteet tu ON tu.id = ti.tuote_id " +
                        "WHERE ti.toimitettu = '0'",
                        "tilaukset",
                        tilatut
                    );
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Virhe: " + ex.Message);
                }
            }
        }*/

    }
}

        

    
