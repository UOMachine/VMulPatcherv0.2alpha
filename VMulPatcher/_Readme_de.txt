VMulPatcher v. 0.2 alpha
Copyright ©2019  Mutagen alias Venushja


VMulPatcher wurde für Ultima Online-Entwickler (GMs) und für Gamer entwickelt. Es kann Bilder in Art, ArtLands, Gump und Textures einfügen. Es kann mit den Dateiformaten .mul und .uop arbeiten. Dieses Programm verwendet die ursprüngliche UO-Bibliothek. Dies bedeutet, dass keine Dateien von Clients ab Version 7.0.60 gepatcht werden können. Aber bis zu dieser Version funktioniert das zuverlässig.

Es gibt verschiedene Möglichkeiten, mit dieser Anwendung zu arbeiten.
Für Entwickler:
 
Es gibt zwei Optionen zum Patchen, entweder über Konfigurationsdateien oder durch einfaches Einfügen von BMP-Dateien in einen Ordner.

Die Option über den Ordner ist die einfachste. Fügen Sie einfach das BMP-Bild ein. Der Dateiname muss eine Grafik-ID sein.
Beispiel: 0x000F.bmp Dieses Bild wird an die Position 0x000F in der UO-Datei hochgeladen.
Zum Beispiel möchte ich die Position 0x000F in arty überschreiben, also füge ich diese Datei "0x000F.bmp" in den Arts-Ordner ein (wenn es ArtLand ist, dann in den Arts / Lands-Ordner) und starte das Programm und wähle 1 (Mul-Dateien) oder 11 für UOP-Dateien. Es ist nicht möglich, MUL und UOP zu kombinieren. Wenn Sie also UOP-Dateien direkt erstellen müssen, muss sich eine .uop-Datei im MulFiles-Ordner befinden.

Die Option in der Konfigurationsdatei ist so, dass sie benannte Bilder unterstützt, z. B. pentagram.bmp (wodurch die erste Option als ungültiges Dateiformat ausgewertet wird).

Beispiel in der INI-Konfigurationsdatei:

pentagram = 0x0000
pentagram = {0x0001, 0x0002, 0x0003, 0xC000, 0xC001, 0xFDBC}
pentagram = [0x000A - 0x000F, 0x00A0 - 0x00F0]

Ich erkläre Zeile für Zeile:
ersetzt eine Position
ersetzt alle aufgelisteten Positionen
ersetzt alle Positionen von 0x000A bis 0x000F
Das Kommentieren von Zeilen erfolgt nicht nebeneinander, sondern untereinander wie folgt:

# ersetzt eine Position
pentagram = 0x0000
# ersetzt alle aufgelisteten Positionen
pentagram = {0x0001, 0x0002, 0x0003, 0xC000, 0xC001, 0xFDBC}
# ersetzt alle Positionen von 0x000A bis 0x000F (Nach dem Komma können weitere Intervalle geschrieben werden)
pentagram = [0x000A - 0x000F, 0x00A0 - 0x00F0]

Diese 2 Optionen können mit dem sogenannten Konfigurationssatz mit pentragram.bmp kombiniert werden und haben auch Dateien in dem Ordner mit dem Namen positions

Sie müssen die zu aktualisierenden Dateien in den MulFiles-Ordner legen. Es hängt davon ab, welches Format Sie verwenden.
Sie finden die aktualisierten Dateien dann im Ordner Patched.


Für Spieler:

Ich habe Unterstützung für Drag & Drop hinzugefügt. Dies ist eine sehr einfache, schnelle und zuverlässige Methode, um einen Patch anzuwenden, ohne etwas tun zu müssen (außer um zu entscheiden, ob auf MUL oder UOP gepatcht werden soll).
Dann habe ich VMulPatcherCreator.exe erstellt, mit dem Sie ganz einfach Ihren eigenen Patch erstellen können, den Sie dann mit anderen teilen können. Das Programm ist sehr einfach und dynamisch kombinierbar, das heißt, es ist möglich, sowohl Arty, LandArty, Gumpy als auch Texture gleichzeitig in einen Patch einzufügen und nur einen Patch zu erstellen (derzeit unter dem Namen "Patch.pvmul" erstellt).
Um diesen kombinierten Patch anwenden zu können, müssen sich jedoch alle MUL / UOP-Dateien im Ordner "MulFiles" befinden. Andernfalls besteht die Möglichkeit, dass das Programm abstürzt oder eine Fehlermeldung ausgibt.
Zum Testen habe ich einen Patch (zu den Stümpfen) zum Ordner "_Examples" hinzugefügt und Sie können es selbst ausprobieren (es ist wirklich nur Drag & Drop-Verwendung).
Das Arbeiten mit VMulPatcherCreator.exe ist intuitiv und einfach. Wenn Sie aus irgendeinem Grund keinen Patch per Drag & Drop anwenden möchten, können Sie eine .bat-Datei schreiben.

Zum Beispiel die Datei start.bat, in der der Befehl lautet:

VMulPatcher.exe Patch.pvmul -f -g

Sie können den Namen der Datei Patch.pvmul ändern und überschreiben. Beispielsweise ist Parezy.pvmul im Ordner _Examples vorhanden. Der Befehl für die BAT-Datei lautet wie folgt:

VMulPatcher.exe Parezy.pvmul -f -g

Und das war's auch schon.




