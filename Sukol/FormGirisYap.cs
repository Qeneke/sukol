﻿using System;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Sukol
{
    public partial class FormGirisYap : Form
    {
        Veritabani veritabani = new Veritabani();
        public FormGirisYap()
        {
            InitializeComponent();
        }

        private void GirisYap_Load(object sender, EventArgs e)
        {
            textBox_kullaniciAdi.Focus();
        }

        private void button_giris_Click(object sender, EventArgs e)
        {
            veritabani.baslat();
            veritabani.sorgu(
                "select " +
                " kullanicilar.id as kullanici_id," +

                " kullanicilar.isim as kullanici_isim," +
                " kullanicilar.soyisim as kullanici_soyisim," +

                " kullanicilar.ogrenci as kullanici_ogrenci," +
                " kullanicilar.ogretmen as kullanici_ogretmen," +
                " kullanicilar.gorevli as kullanici_gorevli," +

                " ogrenciler.okul_no as ogrenci_okul_no" +

                " from ((kullanicilar" +

                " left join ogrenciler on kullanicilar.id=ogrenciler.kullanici_id)" +
                " left join ogretmenler on kullanicilar.id=ogretmenler.kullanici_id)" +
                " left join gorevliler on kullanicilar.id=gorevliler.kullanici_id" +

                " where kullanicilar.kullanici_adi='" + textBox_kullaniciAdi.Text + "' and kullanicilar.sifre='" + maskedTextBox_sifre.Text + "'"
            );
            veritabani.calistir();
            OleDbDataReader oku = veritabani.oku();
            bool giris = false;
            while (oku.Read())
            {
                if (!giris)
                {
                    FormAna formAna = (FormAna)Application.OpenForms["FormAna"];
                    formAna.kullanici_id = Convert.ToInt32(oku["kullanici_id"]);
                    if (oku["kullanici_ogrenci"].ToString() == "True")
                        formAna.ogrenciGiris(Convert.ToInt32(oku["ogrenci_okul_no"]));
                    if (oku["kullanici_gorevli"].ToString() == "True")
                        formAna.gorevliGiris();
                    if (oku["kullanici_ogretmen"].ToString() == "True")
                        formAna.ogretmenGiris();
                    formAna.kullaniciGiris(oku["kullanici_isim"].ToString(), oku["kullanici_soyisim"].ToString());
                }
                giris = true;
            }
            veritabani.kapat();
            if (giris)
                Close();
        }

        private void button_x_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
