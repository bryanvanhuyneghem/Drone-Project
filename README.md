# Inhoud van deze repository

#### [Installatie](https://github.com/bryanvanhuyneghem/Drone-Project/blob/main/README.md#installatie-1)

#### [Opbouw van deze repository](https://github.com/bryanvanhuyneghem/Drone-Project/blob/main/README.md#opbouw-van-deze-repository-1)

#### [Gebruikershandleiding](https://github.com/bryanvanhuyneghem/Drone-Project/blob/main/README.md#gebruikershandleiding-1)

#### [Meer informatie](https://github.com/bryanvanhuyneghem/Drone-Project/blob/main/README.md#meer-informatie-1)

## Installatie
BELANGRIJKE OPMERKING: de executable DatCon.exe kan niet worden uitgevoerd op de virtuele server, omdat dit niet is toegelaten. Het converteren van een DAT-bestand naar een CSV-bestand (tijdens het uploaden van een DAT-bestand) is dus niet mogelijk op de server. Dit is wel gelukt op alle vier onze computers.

### Inhoud van de distributie

In de distributie van dit project bevinden zich volgende bestanden:
*	de DroneWebApp-map met alle code voor de webapplicatie;
*	het SQL-script DroneDB.sql om de databank aan te maken;
*	ChangeDataSourceName_Script.exe, een Perl-executable om Web.config aan te passen;
*	het verslag met handleidingen en documentatie.

### Installatiehandleiding voor ontwikkelaar

De installatiehandleiding is opgedeeld in vier delen:
*	Deel 1: Vereiste software
*	Deel 2: Aanmaken van de databank
*	Deel 3: Opstarten van de webapplicatie
* Deel 4: Publishen van de webapplicatie

#### Vereiste software

In dit deel installeert u alle benodigde software om de webapplicatie te laten werken op Windows 7 en hoger.
1.	Installeer **SQL Server 2019** op uw machine. U kan deze software [hier](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) downloaden op de website van Microsoft.
*	Scrol naar beneden en kies de versie die u verkiest (Developer of Express).
* Klik ‘Download now’. Het programma downloadt.
*	Volg na het uitvoeren van het gedownloade bestand de instructies op het scherm.
2.	Installeer **SQL Server Management Studio** (18.4) (SSMS) op uw machine. U kan deze software [hier](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?redirectedfrom=MSDN&view=sql-server-ver15) downloaden.
*	Klik op ‘Download SQL Server Management Studio (SSMS)’. Het programma downloadt.
*	Volg na het uitvoeren van het gedownloade bestand de instructies op het scherm.
3.	Installeer **Visual Studio 2019** op uw machine. U kan deze software [hier](https://visualstudio.microsoft.com/vs/) downloaden.
*	Klik op ‘Download Visual Studio’ en kies de Community 2019 of de Professional 2019 versie naargelang uw eigen voorkeur. Het programma downloadt.
*	Volg na het uitvoeren van het bestand de instructies op het scherm. Kies tijdens de installatie om de volgende workloads te installeren: ‘ASP.NET and web development’ en ‘Data storage and processing’.
4.	Surf naar de website van **IvyTools**. Dit kan via [deze link](http://www.ivytools.net/index.html).
*	Op deze webpagina kunt u de gratis personal license key verkrijgen die u zult nodig hebben om de IvyTools software te activeren. Klik hiervoor op ‘Click here to get your free personal license key’.
*	Kopieer deze sleutel.
*	Navigeer in de distributie naar de map ‘drone1\IvyPdf_1.62’.
*	Voer het bestand IvyTemplateEditor.exe uit.
*	Navigeer via de balk bovenaan het programma naar ‘Help > About > Apply License Code’.
*	Plak de eerder gekopieerde sleutel in het veld en druk op OK.
*	Sluit IvyTemplateEditor af.
*	U heeft nu toegang tot de IvyParser dll-bestanden in de webapplicatie. Deze worden gebruikt bij het inlezen van een pdf.
5.	Surf naar de website van **Java**. Dit kan via [deze link](https://www.java.com/nl/).
*	Klik op deze webpagina op ‘Gratis Java-download’.
*	Scrol op de webpagina die verschijnt naar beneden tot bij de knop ‘Ga akkoord met de licentiebepalingen en start de download’, en klik op deze knop om een executable van Java te downloaden.
*	Start de executable.
*	Er verschijnt een scherm. Onderaan staat de knop ‘Install’. Deze zal de installatie automatisch starten.
*	Indien u al een of meerdere oudere versies van Java geïnstalleerd had op uw machine, komt nu de mogelijkheid om de verouderde versie te verwijderen. Indien u dit wil, kan u op ‘Uninstall’ klikken. Indien u de oude versies wil behouden kan u gewoon op ‘Next’ klikken.
*	Indien u koos voor het verwijderen van de oudere versies, komt er een scherm die samenvat welke versies verwijderd zijn. Hier kunt u gewoon op ‘Next’ klikken.
*	Na al deze stappen komt er een scherm dat bevestigt dat Java geïnstalleerd is. Klik op ‘Close’.
*	U kan nu aan de slag met Java op uw toestel.

#### Aanmaken van de databank

In dit deel maakt u de SQL-Serverdatabank aan.
1.	Start **SQL Server Management Studio** op en verbind met uw machine.
*	Het veld ‘Server name’ wordt automatisch ingevuld. Dit is tevens de naam die in het bestand Web.config gebruikt wordt om de brug te leggen naar de databank. Deze naam wordt automatisch ingevuld door het Perlscript in punt 9 (zie later).
*	Klik op ‘Connect’.
2.	Ga naar het menu ‘File’ bovenaan links.
3.	Kies ‘Open’ en ga naar ‘File’.
4.	Navigeer in de distributie naar het script **DroneDB.sql** en open dit.
5.	Klik op ‘Execute’ om het script uit te voeren.
6.	In het ‘Messages’-venster verschijnt  “Commands completed successfully”. U kan verifiëren dat de databank is aangemaakt met volgende stappen:
*	Klik in het ‘Object Explorer’-venster op ‘refresh’.
*	Vouw de Machinenaammap en Databasesmap open. Hierin bevindt zich nu de nieuwe database **DroneDB**. Merk op dat de Machinenaammap dezelfde naam heeft als de eerder genoteerde ‘Server name’.
7.	Een lege databank is nu aangemaakt en klaar voor gebruik.
8.	Sluit SQL Server Management Studio.
9.	In de distributie bevindt zich op het pad ‘DroneWebApp\DroneWebApp\Programs\Perl’ een bestand genaamd **ChangeDataSourceName_Script.exe**. Voer dit bestand uit om de juiste connection strings in te vullen in **Web.config**. Deze leggen de verbinding tussen de databank en de webapplicatie.


#### Opstarten van de webapplicatie met Visual Studio

1.	Voer de webapplicatie vanuit **Visual Studio** uit met F5.
2.	De allereerste keer kan een venster verschijnen dat u vraagt om het ‘IIS Express SSL certificate’ te vertrouwen. Klik ‘yes’.
3.	Er verschijnt een ‘security warning’. Klik ‘yes’.
4.	U kunt nu aan de slag met de dronewebapplicatie.

#### Publishen van de webapplicatie

Opmerking: Na het publishen van de webapplicatie is het niet mogelijk om .DAT bestanden te parsen in de applicatie. Er loopt iets mis bij het uitvoeren van DatCon.exe. Het is wel mogelijk om reeds geconverteerde .DAT bestanden up te loaden.

1.	Installeer IIS op uw machine.
*	Druk op de Windows + R toets om het ‘Run’ venster te openen.
*	Typ ‘appwiz.cpl’ in het tekstveld en druk op enter.
*	Nu opent het ‘Programma’s en Onderdelen’-venster. Klik hier op Windows-onderdelen in- of uitschakelen.
*	Vink de ‘Internet Information Services’ check box aan en klik op OK.
2.	Installeer .NET versie 4, dit kan u [hier](https://www.microsoft.com/nl-be/download/details.aspx?id=17851) downloaden.
*	Scrol naar beneden.
*	Klik op ‘Downloaden’ en volg de instructies op het scherm.
3.	Start SQL Server Management Studio op en verbind met uw machine.
4.	Start Visual Studio op als administrator en open het ‘DroneWebApp’-project.
5.	Klik rechts bij Solution Explorer op het project en kies ‘Publish’.
6.	Klik in het ‘Publish’-venster op Start.
7.	Kies bij ‘Pick a publish target’ voor ‘IIS, FTP, etc’ en klik op ‘Create Profile’.
*	Kies bij ‘Publish method’ voor File System
*	Navigeer bij ‘Target location’ naar de ‘wwwroot’ folder van IIS (bv C:\inetpub\wwwroot).
*	Klik op ‘Next’.
*	Selecteer bij ‘Configuration’ ‘Release’ en bij ‘File Publish Options’ ‘Delete all existing files prior to publish’.
*	Klik op ‘Save’.
8.	Klik nu op ‘Publish’.
9.	Open nu IIS Manager.
*	Druk op de Windows + R toets om het ‘Run’ venster te openen.
*	Typ ‘inetmgr’ in het tekstveld en klik ok OK.
10.	Vouw de serverknoop open bij ‘Verbindingen’ en ga bij ‘Sites’ naar ‘Default Web Site’.
11.	Klik op ‘Default Web Site’ en ga bij ‘Acties’ naar ‘Geavanceerde instellingen’.
*	Selecteer bij ‘Groep van toepassingen’ ‘DefaultAppPool’.
12.	Ga naar ‘Toepassingsgroepen’
*	Rechtsklik op ‘DefaultAppPool’ en selecteer ‘Basisinstellingen’.
*	Selecteer bij ‘.NET CLR-versie’ ‘.NET CLR-versie v4’
*	Selecteer bij ‘Beheerde pipeline-modus’ ‘Geïntegreerd’.
*	Klik op ‘OK’.
13.	Ga naar SQL Server Management Studio.
*	Open het ‘Security’-tabblad.
*	Klik rechts op ‘Logins’ en selecteer ‘New login’.
*	Geef als ‘Login name’ ‘IIS APPPOOL\DefaultAppPool’ in.
*	Ga daarna naar ‘User Mapping’ en selecteer ‘DroneDB’.
*	Selecteer als rol ‘db datareader’, ‘db datawriter’ en ‘db owner’.
*	Klik op ‘OK’.
14.	Ga terug naar de ‘inetpub’ folder.
* Rechtsklik op de ‘wwwroot’ folder en klik op eigenschappen.
* Ga naar het tablad beveiliging en klik op ‘Bewerken’.
*	Klik op toevoegen.
*	Typ ‘IIS APPPOOL\DefaultAppPool’ in het tekstveld ‘Geef de objectnamen op’.
*	Klik op ‘namen controleren’ en klik daarna op OK.
*	Selecteer de gebruiker ‘DefaultAppPool’ en vink ‘Volledig beheer’ aan.
*	Klik op toepassen en klik daarna op OK.
*	Klik nog eens op OK.
15.	Ga terug naar ‘IIS Manager’.
*	Klik rechts op ‘DroneWebApp’.
*	Klik bij ‘toepassing beheren’ op ‘bladeren’.
16.	De website wordt nu opgestart.


# Opbouw van deze repository:

Deze repository bevat volgende mappen:
* De **DroneWebApp** map met de solution en het project;
* De map **documenten** bevat het _analyseverslag_, _de eindpresentatie_, de _groepsbesprekingen_, het _tussentijds verslag_ en _de tussentijdse presentatie_;
* De map **demo_bestanden** bevat bestanden die gebruikt kunnen worden om de webapp te testen (uploaden van files);
* De map met het **logboek**;
* De map **verslag** met het verslag van sprint 1;
* Het **SQL-script** `DroneDB.sql` dat gebruikt wordt om de databank aan te maken.


## Gebruikershandleiding

U kan de gebruikershandleiding [hier](https://github.com/bryanvanhuyneghem/Drone-Project/tree/master/verslag/Gebruikershandleiding.pdf) vinden.

# Meer informatie

## Dit project kan opgedeeld worden in 5 grote doelstellingen die wij gedurende de sprints continu voor ogen hadden:

1. Alle aanwezige informatie in kaart brengen en een databankmodel creëren voor onze centrale databank;
2. Deze informatie parsen en wegschrijven naar deze databank;
3. Een webapplicatie en interface ontwikkelen zodat de gebruiker kan interageren met de data;
4. De data visualiseren met ArcGIS in een webviewer;
5. De mogelijkheid voorzien om sommige ingelezen data opnieuw te kunnen exporteren naar een gewenst formaat, zoals pdf of csv.

## Sprint 1 features

* Ontwerp van een databankmodel dat op een logische manier de relaties tussen de verschillende entiteiten beschrijft

* Keuze van de databank (SQL Server 2019) en ORM (Entity Framework 6)

* Ontwerp en implementatie van het model: de parser-klassen via een simple factory pattern

* Keuze voor MVC pattern

* Implementeren van de Controllers

* CRUD-functionaliteit voor dronevluchten, drones en piloten

* Schrijven van de Views (de website)

* Implementeren van searchable en sortable tabellen met paging

* Dependency Injection met Unity (makkelijk, redelijk belangrijk); 

* Uploaden van bestanden die geparset kunnen worden

* Voorzien van additionele razorpagina’s (View) die details geven van de data die ingelezen wordt

* Tonen van gepaste foutpagina’s op de website aan de gebruiker

## Sprint 2 features

* Projecttabel in databank met benodigde velden

* Totale vliegtijd van een drone automatisch berekenen en toevoegen aan de databank

* Velden uit logboeken (zoals type of activity) toegevoegd aan databank en mogelijkheid voorzien om deze velden in te vullen via de user interface

* Reverse geocoding, omvormen van latitude- en longitudecoördinaten naar de naam van de locatie

* Duidelijk weergeven van verplichte velden bij invullen van gegevens in de user interface

* Dronevluchten per piloot en drone in apart overzicht; te bekijken in user interface

* Extra functies GUI, zoals van piloot naar bijhorende vluchten kunnen gaan

* Bij het uploaden van files staat een progress bar die weergeeft hoeveel procent van de file reeds geüpload en verwerkt (geparset) is

* Controletool ctrl points in point cloud

* De track van de drone in de map view kunnen visualiseren op basis van een attribuut (hoogte, batterijstand...) met een gepaste color ramp

* Legende met informatie over de vlucht in de map vie
* Visualisatie van point clouds, gcp’s en ctrl’s

* Een toggle list bij de map view zodat de gebruiker zelf kan kiezen welke layer (soort data) getoond wordt van een dronevlucht

* Pop-up templates als de gebruiker op eender welk punt (gcp, ctrl...) klikt om de bijhorende coördinaten en andere attributen te zien

* Zoekbalk en measurement widget in de map view

* Er kan van attribuut (en bijhorende color ramp) gewisseld worden door op een bepaalde toets te drukken om zo de track te visualiseren. De legende van een vlucht kan verborgen of getoond worden, eveneens met een toets. 

## Sprint 3 features

* Meerdere bestanden tegelijk kunnen uploaden

* Bestanden uploaden kan nu veiliger gebeuren. Er werden verscheidene controles op bestandsgroottes en type bestanden toegevoegd. De gebruiker wordt nu ook op de hoogte gebracht van het al dan niet succesvol parsen van de bestanden die hij of zij uploadde.

* De progress bar geeft nu een totaal weer voor alle bestanden in plaats van elk afzonderlijk

* Elke piloot en drone hebben nu een eigen overzicht van hun dronevluchten

* Een drone heeft nu een threshold. Dit getal (een tijd) geeft aan wanneer de drone gecontroleerd moet worden. De administrator kan ook zelf een drone markeren voor controle. Na controle kan hij de drone afvinken en wordt een nieuwe threshold-tijd berekent.

* Notitie-sectie voor een drone (gemakkelijk, onbelangrijk); 

* Knoppen voor de map en upload (vrij gemakkelijk, onbelangrijk); 

* Images die door de drone genomen werden kunnen geüpload en bekeken worden op hun eigen pagina en in de map view  

* De parsers voor XYZ- en DAT-bestanden werden herschreven, wat leed tot grote performantiewinsten   

* Een map overview van alle dronevluchten waarbij de gebruiker naar een specifieke vlucht kan navigeren

* Verbeteringen in de map view GUI, waaronder een aanklikbare menu voor color coding 

* Het exporteren van de logboeken voor de piloten en drones naar PDF- en XYZ-bestanden

* Het voorzien van een loginpagina en bepaalde functionaliteiten enkel beschikbaar maken voor bepaalde rollen 

* Bij de verplichte velden wordt een duidelijke boodschap weergegeven wanneer deze niet ingevuld worden  

* DAT-bestanden worden automatisch via DatCon omgezet wanneer deze geüpload worden in de webapplicatie

* Breadcrumbing links om makkelijk naar een vorige pagina terug te kunnen keren

## Future Work

In dit onderdeel worden beperkingen van de drone-planningtool belicht, alsook enkele functionaliteiten die tijdens een future work toegevoegd zouden kunnen worden.  

### Beperkingen betreffende het uploaden van bestanden 

De eerste (en onmiddellijk de grootste) beperking van de tool betreft het feit dat slechts één gebruiker per keer bestanden kan uploaden. Dit zou opgelost kunnen worden door aan iedere gebruiker een unieke id toe te kennen tijdens het uploaden, zodat de server-side weet welke progress hij naar welke gebruiker moet sturen. Op dit moment krijgen de andere gebruikers die trachten om bestanden te uploaden een melding indien er reeds een gebruiker bestanden aan het uploaden is. De andere gebruikers krijgen dan onmiddellijk de progress bar van de gebruiker die aan het uploaden is te zien. Op deze manier kunnen zij ongeveer inschatten wanneer zij kunnen om beurt kunnen uploaden. 

Een tweede beperking betreft het feit dat de som van alle bestandsgroottes van de bestanden die een gebruikers probeert te uploaden niet groter kan zijn dan 2.147 GB (een signed int). Deze beperking wordt opgelegd in het bestand Web.config. Dit bestand laat geen grotere bestandsgroottes toe. Dit kan een probleem geven voor zeer grote XYZ-bestanden. Mogelijks moeten deze in twee stukken gehakt worden en na elkaar geüpload worden. In het geval dat de totaalsom van een reeks bestanden groter is dan 2.147 GB, dan moet de gebruiker zijn bestanden in twee of meerdere keren uploaden. 

Een derde beperking betreft het feit dat de huidige implementatie (met Entity Framework 6) van de RawImageParser tijdens het aanmaken van de thumbnails van de afbeeldingen plots zeer veel geheugen na een tijd meer en meer geheugen gaat gebruiken indien zeer veel afbeeldingen tegelijk geüpload worden. Dit kan mogelijks te maken hebben met de memory cap van de programmeeromgeving (Visual Studio 2019) die een out of memory geeft indien meer dan 3 GB aan memory gebruikt wordt. Dit geheugengebruik is van vrij korte duur en kan dus mogelijks op een server helemaal geen probleem geven. Het geheugen wordt immers snel weer vrijgegeven na het parsen van de geüploade afbeeldingen. Hoe dan ook zou het interessant kunnen zijn om de RawImageParser te herschrijven zodat hij gebruikmaakt van ADO.NET (analoog aan de DATParser en XYZParser). Dit zou het parsen van de afbeeldingen ook sneller laten verlopen. 

### Verwijderen van bestanden behorende bij een dronevlucht 

In een future work zou het ook mogelijk moeten kunnen zijn voor een admin om specifieke data per dronevlucht te verwijderen, indien bijvoorbeeld een foutief bestand geüpload zou zijn. In de betreffende tabellen zou gezocht kunnen worden met de FlightId van een dronevlucht, om zo alle informatie die te maken heeft met die dronevlucht uit de databank te verwijderen. Dit zou voor de data van elk bestand moeten kunnen. 

### Visualisatie 

* De mogelijkheid voorzien om kleuren van track dynamisch te laten aanpassen naargelang de keuze van de gebruiker. 
* De flow van de map view kan verbeterd worden op basis van callback functies en asynchrone events. 
* Een differential view implementeren om snel een situatie met een vorige situatie te kunnen vergelijken. Dit zou een nieuw raster maken waarbij de waarde gelijk is aan het verschil tussen de vorige en de laatste meting. 
* Via een KML een polygoon tekenen voor de dronevluchten (d.i. een flight plan). 
* De grenzen (uiterste limieten) van alle afbeeldingen tonen op de kaart. 
* De TIFF-afbeelding van een dronevlucht projecteren op de kaart. 
* De track van een dronevlucht aanpassen naar een polyline in plaats van verscheidene, aparte punten. 
* De puntenwolk visualiseren met LAS/LASZ. XYZ punten projecteren op de map is op dit moment niet haalbaar. Er zijn teveel individuelen punten die opgehaald moeten worden met een HTTP GET call en vervolgens getekend moeten worden op de kaart. Mogelijks kan een LAS-bestand hier soelaas brengen. 

### Logboeken 

Idealiter zouden de PDF-bestanden die worden gemaakt voor drone en piloot via de exportfunctionaliteit er wat kleurrijker mogen uitzien. 
