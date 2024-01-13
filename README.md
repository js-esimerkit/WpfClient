# WpfClient

Jotta sovellus toimii, on tietokantapalvelin ja MyApi-sovellus oltava käynnissä.

Kun nuo ovat käynnissä avaa sovellus Visual Studio 2022:lla ja avaa tiedosto MainWindow.xaml.cs ja muokkaa muuttujassa **url** oleva portti oikeaksi.
Minulla se on siis tällainen   
<pre>
var url = "http://localhost:5028/api/login/";
</pre>

Sitten voit suorittaa sovelluksen klikkaamalla vihreää kolmiota.
Jos annat kirjautumisessa tiedot user01/pass01, pitäisi sen jälkeen onnistua kirjojen näyttäminen ruudulla.