# onlineRadioStreamDevice
Bir IoT projesi olarak internet tabanlı radio yayın akışı oynatma projesi<br>
<h3>PCB Tasarımı</h3><br>
Elektronik devre şeması ve PCB board tasarımı Eagle de tasarlandı.<br>
<img src="https://user-images.githubusercontent.com/73975473/201216242-054ea71a-02cf-44da-9f79-b13d3cfa4ee2.png" style="width:300px"/>
<img src="https://user-images.githubusercontent.com/73975473/201216245-f00607e7-061d-44e6-b626-f4953e34e26c.png" style="width:300px"/>
<h3>Masaüstü Uygulama</h3><br>
C# ile masaüstü uygulaması sayesinde cihazın set ayarları yapılabiliyor. Wifi bilgileri set edilebiliyor. Yayın akışı takip edilecek link bilgileri set edilebiliyor. Cihaza isim atanabiliyor. Tüm bu işlemler masaüstü uygulaması ile cihaz arasında serial haberleşme ile yapılıyor. Cihaza daha önceden set edilen bilgiler çekilebiliyor.<br>
<img src="https://user-images.githubusercontent.com/73975473/201216558-965a421c-7b98-42e9-a112-7acbc8f7148f.JPG" style="width:300px"/>
<h3>Gömülü Yazılım</h3><br>
ESP8266 kullanarak Wifi desteğinden faydalanarak gömülü yazılımı geliştirdim.<br>
EEPROM hafıza kullanıldı.<br>
Serial Haberleşme kullanıldı.<br>
WPS Bağlantı kullanıldı.<br>
I2S kullanıldı.<br>
