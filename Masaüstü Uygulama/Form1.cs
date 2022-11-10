using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO; //dosya işlemleri için özellikle resim kullanmak için kullandık
using System.IO.Ports;

namespace Radioorder_Setting_Radio
{
  
    public partial class Form1 : Form
    {
        String[] portlistesi;
        bool baglanti_durumu = false;
        String islem ="";
        public Form1()
        {
            InitializeComponent();
        }
        void portlistele()
        {
            comboBox1.Items.Clear();  //combox içini temizliyoruz
            portlistesi = SerialPort.GetPortNames(); // Serialport isimlerini diziye ekliyoruz
            foreach (string portadi in portlistesi)   //foreach dizilerin uzunluğu kadar çalışır
            {
                comboBox1.Items.Add(portadi);
                if (portlistesi[0] != null)
                {
                    comboBox1.SelectedItem = portlistesi[0];
                }
            }


        }
        void connect_show()
        {
            label4.Enabled = true;
            label5.Enabled = true;
            label6.Enabled = true;
            label7.Enabled = true;
            label8.Enabled = true;

            textBox1.Enabled = true;
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            textBox4.Enabled = true;

            button3.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;
            button6.Enabled = true;
            button7.Enabled = true;
            button8.Enabled = true;
            button9.Enabled = true;
            button10.Enabled = true;
            button11.Enabled = true;
        }
        void connect_hide()
        {
            label4.Enabled = false;
            label5.Enabled = false;
            label6.Enabled = false;
            label7.Enabled = false;
            label8.Enabled = false;

            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;

            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            button7.Enabled = false;
            button8.Enabled = false;
            button9.Enabled = false;
            button10.Enabled = false;
            button11.Enabled = false;
        }

        private void buttonexit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void buttonminimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen; // Ekranı ortada konumlandırıcaktır
            portlistele();
            connect_hide();
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            portlistele();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (baglanti_durumu==false)
            {
                
                    serialPort1.PortName = comboBox1.GetItemText(comboBox1.SelectedItem);
                    serialPort1.BaudRate = 115200;
                    serialPort1.Open();

                    serialPort1.DiscardOutBuffer(); // giden veri hafızasını temizler
                    serialPort1.DiscardInBuffer(); // gelen veri hafızasını temizler

                    comboBox1.Enabled = false;
                    connect_show();
                    button2.Enabled = false;
                    baglanti_durumu = true;
                    button1.Text = "Çıkar";
                    button1.BackColor = Color.FromArgb(198, 40, 40);



            }
            else
            {
                //while dan çıkarmak için
                serialPort1.WriteLine("W");
                System.Threading.Thread.Sleep(50);

                serialPort1.Close();
                connect_hide();
                baglanti_durumu = false;
                button1.BackColor = Color.FromArgb(0, 230, 118);
                button1.Text = "Bağlan";
                comboBox1.Enabled = true;
                button2.Enabled = true;
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                listBox1.Items.Clear();

            }




        }

        private void button3_Click(object sender, EventArgs e)
        {
            //*** Link bilgileri yükleme kısmı
            timer1.Enabled = true;
            islem = "link";
            listBox1.Items.Add("--> *Müzik URL: " + textBox3.Text);
            serialPort1.DiscardOutBuffer(); // giden veri hafızasını temizler
            serialPort1.DiscardInBuffer(); // gelen veri hafızasını temizler
            // serialPort1.Write("&#"+textBox1.Text+"#"+textBox2.Text+"#"+textBox3.Text);
            //  &#BIM#prttpx369#http://studio21.radiolize.com:8230/mp3cikis.mp3
            //http://s1.radiorder.live:8000/radio.mp3
            serialPort1.Write("*"+textBox3.Text);
            System.Threading.Thread.Sleep(50);
            textBox3.Clear();
            

        }

        private void button8_Click(object sender, EventArgs e)
        {
            //*** Cihaz İsim bilgileri yükleme kısmı

            timer1.Enabled = true;
            islem = "name";
            listBox1.Items.Add("--> *CİHAZ İSMİ: " + textBox4.Text);

            serialPort1.DiscardOutBuffer(); // giden veri hafızasını temizler
            serialPort1.DiscardInBuffer(); // gelen veri hafızasını temizler
            serialPort1.Write("%" + textBox4.Text);
            System.Threading.Thread.Sleep(50);
            textBox4.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //*** Wifi bilgileri yükleme kısmı

            //serialPort1.DiscardOutBuffer(); // giden veri hafızasını temizler
            //serialPort1.DiscardInBuffer(); // gelen veri hafızasını temizler
            timer1.Enabled = true;
            islem = "wifi";
            listBox1.Items.Add("--> *SSID :" + textBox1.Text);
            listBox1.Items.Add("--> *PASSWORD :" + textBox2.Text);
            serialPort1.WriteLine("#" + textBox1.Text + ":" + textBox2.Text);
            System.Threading.Thread.Sleep(50);
            textBox1.Clear();
            textBox2.Clear();

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            islem = "bilgicek";
            

        }

        private void button6_Click(object sender, EventArgs e)
        {
            //*** Wifi Bilgileri temizleme
            serialPort1.WriteLine("4");
            textBox1.Clear();
            textBox2.Clear();
            listBox1.Items.Add("--> *Cihazdaki WiFi Verileri Silindi!");
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //*** Cihaz İsim Bilgileri temizleme
            serialPort1.WriteLine("5");
            textBox4.Clear();
            listBox1.Items.Add("--> *Cihaza Atanan İsim Silindi!");
            listBox1.SelectedIndex = listBox1.Items.Count - 1;

        }
        private void button7_Click(object sender, EventArgs e)
        {
            //*** Link Bilgileri temizleme
            serialPort1.WriteLine("6");
            textBox3.Clear();
            listBox1.Items.Add("--> *Cihazdaki Link Verisi Silindi!");
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {
            islem = "sifirla";
            
            timer1.Enabled = true;         
           
            System.Threading.Thread.Sleep(300);

            //*** Wifi Bilgileri temizleme
            serialPort1.WriteLine("4");
            System.Threading.Thread.Sleep(100);
            //*** Cihaz İsim Bilgileri temizleme
            serialPort1.WriteLine("5");
            System.Threading.Thread.Sleep(100);
            //*** Link Bilgileri temizleme
            serialPort1.WriteLine("6");
            System.Threading.Thread.Sleep(100);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
            progressBar1.Increment(5);

            if (progressBar1.Value == 0 && islem == "wifi") {
               
            }
            if (progressBar1.Value == 30 && islem == "wifi") { listBox1.Items.Add("--> *WiFi Bilgileri Yükleniyor!"); }
            if (progressBar1.Value == 90 && islem == "wifi") { listBox1.Items.Add("--> *WiFi Bilgileri Kaydedildi!"); }

            
            if (progressBar1.Value == 30 && islem == "name") { listBox1.Items.Add("--> *İsim Bilgileri Yükleniyor!"); }
            if (progressBar1.Value == 90 && islem == "name") { listBox1.Items.Add("--> *İsim Bilgileri Kaydedildi!"); }

            
            if (progressBar1.Value == 30 && islem == "link") { listBox1.Items.Add("--> *İsim Bilgileri Yükleniyor!"); }
            if (progressBar1.Value == 90 && islem == "link") { listBox1.Items.Add("--> *İsim Bilgileri Kaydedildi!"); }


            if (progressBar1.Value == 0 && islem =="sifirla") { listBox1.Items.Add("--> *Sıfırlama İşlemi Başlatıldı!"); }
            if (progressBar1.Value == 40 && islem=="sifirla") { listBox1.Items.Add("--> *Cihazdaki Tüm Veriler Siliniyor!"); }
            if (progressBar1.Value == 90 && islem =="sifirla"){ listBox1.Items.Add("--> *Sıfırlama İşlemi Tamamlandı!"); }
            
                if (progressBar1.Value == 20 && islem == "bilgicek") {
                    //*** Cihaz Wifi bilgisi çekme kısmı
                    serialPort1.WriteLine("1");
                    string datawifi = serialPort1.ReadLine();
                    if (datawifi[0] == '#')
                    {
                        char[] ayir = { '#', ':', };
                        string[] WiFi = datawifi.Split(ayir);
                        listBox1.Items.Add("--> WiFi: " + WiFi[1]);
                        listBox1.Items.Add("--> PASSWORD: " + WiFi[2]);
                        System.Threading.Thread.Sleep(50);
                    }
                    else
                    {
                        listBox1.Items.Add("--> Cihazda Wifi bilgilerine ait veri yok!");
                    }
                }
                if (progressBar1.Value == 50 && islem == "bilgicek") {
                    //*** Cihaz isim bilgisi çekme kısmı
                    serialPort1.WriteLine("2");
                    string dataname = serialPort1.ReadLine();
                    if (dataname[0] == '%')
                    {
                        string[] deviceName = dataname.Split('%');
                        listBox1.Items.Add("--> Cihaz İsim:  " + deviceName[1]);
                    }

                    else
                    {
                        listBox1.Items.Add("--> Cihaza Kayıtlı isim verisi yok!");
                    }

                }
                if (progressBar1.Value == 90 && islem == "bilgicek") {

                    //*** Cihaz Link bilgisi çekme kısmı
                    serialPort1.WriteLine("3");
                    string datalink = serialPort1.ReadLine();

                    if (datalink[0] == '*')
                    {
                        string[] Link = datalink.Split('*');
                        listBox1.Items.Add("--> URL: " + Link[1]);
                    }

                    else
                    {
                        listBox1.Items.Add("--> Cihazda Link verisi yok !");
                    }
                }


                if (progressBar1.Value == 100)
                {
                    islem = "";
                    progressBar1.Value = 0;
                    timer1.Stop();
                } 
            
        }

      private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control == true && e.KeyCode == Keys.C)
          {
              string s = listBox1.SelectedItem.ToString();
              Clipboard.SetData(DataFormats.StringFormat, s);
          }
        }

        int Move;
        int Mouse_X;
        int Mouse_Y;
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (Move == 1)
            {
                this.SetDesktopLocation(MousePosition.X - Mouse_X, MousePosition.Y - Mouse_Y);
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            Move = 0;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            Move = 1;
            Mouse_X = e.X;
            Mouse_Y = e.Y;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }
    }
    }
