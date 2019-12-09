VMulPatcher v. 0.2 alpha
Copyright �2019  Mutagen alias Venushja


VMulPatcher je ur�en pro v�voj��e Ultima Online (GMs �ard�) a tak� pro hr��e hry. Um� vkl�dat obr�zky do Art�, ArtLands, Gump� a Textur. Um� pracovat s form�ty soubor� .mul a tak� .uop. Tento program vyu��v� p�vodn� UO knihovny, to znamen� neum� patchovat soubory zhruba od verze client� 7.0.60 a v��. Ale do t�to verze pracuje spolehliv�.

Je n�kolik mo�nost�, jak s touto aplikac� pracovat. 
Pro v�voj��e:
 
Jsou 2 mo�nosti patchov�n�, p�es configura�n� soubory a nebo jen vlo�en�m .bmp obr�zk� do slo�ky.

Mo�nost p�es slo�ku je �pln� nejjednodu��, tam sta�� vlo�it obr�zek .bmp s t�m, �e n�zev souboru mus� b�t ID grafiky.
Nap� 0x000F.bmp tento obr�zek se nahraje na pozici 0x000F v UO souboru.
Tak�e nap� chci p�epsat pozici 0x000F v artech tak tento soubor "0x000F.bmp" vlo��m do Arts slo�ky (pokud je to ArtLand tak je�t� d�t do slo�ky Arts/Lands) a spustit program a vybrat 1 (mul soubory) nebo 11 pro uop soubory. !Zat�m nejde kombinovat MUL a UOP tak�e pokud pot�ebujete p��mo vytvo�it UOP soubory tak ve slo�ce MulFiles mus� b�t .uop soubor)

Mo�nost p�es configura�n� soubor je takov�, �e podporuje pojmenovan� obr�zky nap�. pentagram.bmp (co� prvn� mo�nost vyhodnot� jako �patn� form�t souboru)

Uk�zka v .ini konfigura�n�m souboru:

pentagram = 0x0000
pentagram = {0x0001, 0x0002, 0x0003, 0xC000, 0xC001, 0xFDBC}
pentagram = [0x000A - 0x000F, 0x00A0 - 0x00F0]

vysv�tl�m ��dek po ��dku:
nahrad� jednu pozici
nahrad� v�echny vypsan� pozice
nahrad� v�echny pozice od 0x000A a� po 0x000F (za ��rkou je mo�n� vypsat dal�� intervaly)
Komentov�n� ��dk� nejde vedle sebe, ale pod sebou, takto:

# nahrad� jednu pozici
pentagram = 0x0000
# nahrad� v�echny vypsan� pozice
pentagram = {0x0001, 0x0002, 0x0003, 0xC000, 0xC001, 0xFDBC}
# nahrad� v�echny pozice od 0x000A a� po 0x000F (za ��rkou je mo�n� vypsat dal�� intervaly)
pentagram = [0x000A - 0x000F, 0x00A0 - 0x00F0]

Tyto 2 mo�nosti se daj� i kombinovat tzv. nastaven� config t�eba s pentragram.bmp a z�roven m�t ve slo�ce soubory s n�zvem pozic

Do slo�ky MulFiles je pot�eba vlo�it soubory, kter� chcete aktualizovat. Z�le��, jak� form�t d�l�te.
Ve slo�ce Patched pak najdete aktualizovan� soubory.


Pro hr��e:

P�idal jsem podporu aplikace alias Drag&Drop. Jedn� se o velmi lehkou, rychlou a spolehlivou cestu, jak aplikovat patch ani� by �lov�k musel cokoliv ud�lat (a� na v�b�r jestli patchovat do MUL nebo UOP).
N�sledn� k tomu jsem vytvo�il program VMulPatcherCreator.exe, kter�m si m��ete lehce vytvo�it vlastn� patch, kter� pak m��ete sd�let s ostatn�mi. Program je velmi jednoduch� a dynamicky kombinovateln�, t�m chci ��ct, �e do jednoho patche je mo�no vlo�it jak Arty, LandArty, Gumpy, Textury z�rove� a vytvo�� se jen jeden patch (aktu�ln� se vytv��� pod jm�nem "Patch.pvmul").
Pro aplikov�n� takto kombinovan�ho patche je ov�em nutn� m�t v�echny MUL/UOP soubory ve slo�ce "MulFiles" jinak je pravd�poodbn� �ance, �e program spadne �i vyhod� chybovou hl�ku.
Pro vyzkou�en� jsem p�idal do raru do slo�ky "_Examples" jeden patch (na pa�ezy) a m��ete si sami ho zkusit aplikovat (opravdu se jedn� jen o Drag&Drop pou�it�).
Pr�ce s VMulPatcherCreator.exe je intuitivn� a snadn�. Pokud z n�jak�ho d�vodu nechcete pou��t patch pomoc� Drag&Drop, je mo�n� si napsat n�jak� soubor.bat.

Nap��klad soubor start.bat ve kter�m bude p��kaz:

VMulPatcher.exe Patch.pvmul -f -g

N�zev souboru Patch.pvmul m��ete zm�nit a p�epsat. Jako nap��klad ve slo�ce _Examples existuje Parezy.pvmul. P��kaz pro .bat soubor bude n�sleduj�c�:

VMulPatcher.exe Parezy.pvmul -f -g

A to je asi v�echno.




