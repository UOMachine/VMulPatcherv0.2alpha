VMulPatcher v. 0.2 alpha
Copyright �2019  Mutagen alias Venushja


VMulPatcher wurde f�r Ultima Online-Entwickler (GMs) und f�r Gamer entwickelt. Es kann Bilder in Art, ArtLands, Gump und Textures einf�gen. Es kann mit den Dateiformaten .mul und .uop arbeiten. Dieses Programm verwendet die urspr�ngliche UO-Bibliothek. Dies bedeutet, dass keine Dateien von Clients ab Version 7.0.60 gepatcht werden k�nnen. Aber bis zu dieser Version funktioniert das zuverl�ssig.

Es gibt verschiedene M�glichkeiten, mit dieser Anwendung zu arbeiten.
F�r Entwickler:
 
Es gibt zwei Optionen zum Patchen, entweder �ber Konfigurationsdateien oder durch einfaches Einf�gen von BMP-Dateien in einen Ordner.

Die Option �ber den Ordner ist die einfachste. F�gen Sie einfach das BMP-Bild ein. Der Dateiname muss eine Grafik-ID sein.
Beispiel: 0x000F.bmp Dieses Bild wird an die Position 0x000F in der UO-Datei hochgeladen.
Zum Beispiel m�chte ich die Position 0x000F in arty �berschreiben, also f�ge ich diese Datei "0x000F.bmp" in den Arts-Ordner ein (wenn es ArtLand ist, dann in den Arts / Lands-Ordner) und starte das Programm und w�hle 1 (Mul-Dateien) oder 11 f�r UOP-Dateien. Es ist nicht m�glich, MUL und UOP zu kombinieren. Wenn Sie also UOP-Dateien direkt erstellen m�ssen, muss sich eine .uop-Datei im MulFiles-Ordner befinden.

Die Option in der Konfigurationsdatei ist so, dass sie benannte Bilder unterst�tzt, z. B. pentagram.bmp (wodurch die erste Option als ung�ltiges Dateiformat ausgewertet wird).

Beispiel in der INI-Konfigurationsdatei:

pentagram = 0x0000
pentagram = {0x0001, 0x0002, 0x0003, 0xC000, 0xC001, 0xFDBC}
pentagram = [0x000A - 0x000F, 0x00A0 - 0x00F0]

Ich erkl�re Zeile f�r Zeile:
ersetzt eine Position
ersetzt alle aufgelisteten Positionen
ersetzt alle Positionen von 0x000A bis 0x000F
Das Kommentieren von Zeilen erfolgt nicht nebeneinander, sondern untereinander wie folgt:

# ersetzt eine Position
pentagram = 0x0000
# ersetzt alle aufgelisteten Positionen
pentagram = {0x0001, 0x0002, 0x0003, 0xC000, 0xC001, 0xFDBC}
# ersetzt alle Positionen von 0x000A bis 0x000F (Nach dem Komma k�nnen weitere Intervalle geschrieben werden)
pentagram = [0x000A - 0x000F, 0x00A0 - 0x00F0]

Diese 2 Optionen k�nnen mit dem sogenannten Konfigurationssatz mit pentragram.bmp kombiniert werden und haben auch Dateien in dem Ordner mit dem Namen positions

Sie m�ssen die zu aktualisierenden Dateien in den MulFiles-Ordner legen. Es h�ngt davon ab, welches Format Sie verwenden.
Sie finden die aktualisierten Dateien dann im Ordner Patched.


F�r Spieler:

Ich habe Unterst�tzung f�r Drag & Drop hinzugef�gt. Dies ist eine sehr einfache, schnelle und zuverl�ssige Methode, um einen Patch anzuwenden, ohne etwas tun zu m�ssen (au�er um zu entscheiden, ob auf MUL oder UOP gepatcht werden soll).
Dann habe ich VMulPatcherCreator.exe erstellt, mit dem Sie ganz einfach Ihren eigenen Patch erstellen k�nnen, den Sie dann mit anderen teilen k�nnen. Das Programm ist sehr einfach und dynamisch kombinierbar, das hei�t, es ist m�glich, sowohl Arty, LandArty, Gumpy als auch Texture gleichzeitig in einen Patch einzuf�gen und nur einen Patch zu erstellen (derzeit unter dem Namen "Patch.pvmul" erstellt).
Um diesen kombinierten Patch anwenden zu k�nnen, m�ssen sich jedoch alle MUL / UOP-Dateien im Ordner "MulFiles" befinden. Andernfalls besteht die M�glichkeit, dass das Programm abst�rzt oder eine Fehlermeldung ausgibt.
Zum Testen habe ich einen Patch (zu den St�mpfen) zum Ordner "_Examples" hinzugef�gt und Sie k�nnen es selbst ausprobieren (es ist wirklich nur Drag & Drop-Verwendung).
Das Arbeiten mit VMulPatcherCreator.exe ist intuitiv und einfach. Wenn Sie aus irgendeinem Grund keinen Patch per Drag & Drop anwenden m�chten, k�nnen Sie eine .bat-Datei schreiben.

Zum Beispiel die Datei start.bat, in der der Befehl lautet:

VMulPatcher.exe Patch.pvmul -f -g

Sie k�nnen den Namen der Datei Patch.pvmul �ndern und �berschreiben. Beispielsweise ist Parezy.pvmul im Ordner _Examples vorhanden. Der Befehl f�r die BAT-Datei lautet wie folgt:

VMulPatcher.exe Parezy.pvmul -f -g

Und das war's auch schon.




