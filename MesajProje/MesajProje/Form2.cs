using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace MesajProje
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        public string numara;
        SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-S866UD2;Initial Catalog=Mesaj;Integrated Security=True");

        void gelenKutusu()
        {
            SqlDataAdapter da1 = new SqlDataAdapter("SELECT MESAJID, (AD + ' ' + SOYAD) AS 'GONDEREN', BASLIK, ICERIK FROM TBLMESAJLAR INNER JOIN TBLKISILER ON TBLMESAJLAR.GONDEREN = TBLKISILER.NUMARA WHERE ALICI =" + numara,baglanti);
            DataTable dt1 = new DataTable();
            da1.Fill(dt1);
            dataGridView1.DataSource = dt1;
        }

        void gidenKutusu()
        {
            SqlDataAdapter da2 = new SqlDataAdapter("SELECT MESAJID, (AD + ' ' + SOYAD) AS 'ALICI', BASLIK, ICERIK FROM TBLMESAJLAR INNER JOIN TBLKISILER ON TBLMESAJLAR.ALICI = TBLKISILER.NUMARA WHERE GONDEREN=" + numara, baglanti);
            DataTable dt2 = new DataTable();
            da2.Fill(dt2);
            dataGridView2.DataSource = dt2;
        }


        private void Form2_Load(object sender, EventArgs e)
        {
            LblNumara.Text = numara;

            gelenKutusu();

            gidenKutusu();

            //Ad Soyad Çekme
            baglanti.Open();
            SqlCommand komut = new SqlCommand("SELECT AD,SOYAD FROM TBLKISILER WHERE NUMARA=" + numara,baglanti);
            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                LblAdSoyad.Text = dr[0] + " " + dr[1];
            }
            baglanti.Close();
        }

        private void BtnGonder_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("INSERT INTO TBLMESAJLAR (GONDEREN,ALICI,BASLIK,ICERIK) VALUES (@P1,@P2,@P3,@P4)",baglanti);
            komut.Parameters.AddWithValue("@P1", numara);
            komut.Parameters.AddWithValue("@P2", MskAlici.Text);
            komut.Parameters.AddWithValue("@P3", TxtBaslik.Text);
            komut.Parameters.AddWithValue("@P4", RchMesaj.Text);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Mesaj Gönderildi!");
            gidenKutusu();
        }
    }
}
