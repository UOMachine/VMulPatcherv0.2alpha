VMulPatcher v. 0.2 alpha
Copyright ©2019  Mutagen alias Venushja


VMulPatcher je urèen pro vývojáøe Ultima Online (GMs šardù) a také pro hráèe hry. Umí vkládat obrázky do Artù, ArtLands, Gumpù a Textur. Umí pracovat s formáty souborù .mul a také .uop. Tento program využívá pùvodní UO knihovny, to znamená neumí patchovat soubory zhruba od verze clientù 7.0.60 a výš. Ale do této verze pracuje spolehlivì.

Je nìkolik možností, jak s touto aplikací pracovat. 
Pro vývojáøe:
 
Jsou 2 možnosti patchování, pøes configuraèní soubory a nebo jen vložením .bmp obrázkù do složky.

Možnost pøes složku je úplnì nejjednoduší, tam staèí vložit obrázek .bmp s tím, že název souboru musí být ID grafiky.
Napø 0x000F.bmp tento obrázek se nahraje na pozici 0x000F v UO souboru.
Takže napø chci pøepsat pozici 0x000F v artech tak tento soubor "0x000F.bmp" vložím do Arts složky (pokud je to ArtLand tak ještì dát do složky Arts/Lands) a spustit program a vybrat 1 (mul soubory) nebo 11 pro uop soubory. !Zatím nejde kombinovat MUL a UOP takže pokud potøebujete pøímo vytvoøit UOP soubory tak ve složce MulFiles musí být .uop soubor)

Možnost pøes configuraèní soubor je takový, že podporuje pojmenované obrázky napø. pentagram.bmp (což první možnost vyhodnotí jako špatný formát souboru)

Ukázka v .ini konfiguraèním souboru:

pentagram = 0x0000
pentagram = {0x0001, 0x0002, 0x0003, 0xC000, 0xC001, 0xFDBC}
pentagram = [0x000A - 0x000F, 0x00A0 - 0x00F0]

vysvìtlím øádek po øádku:
nahradí jednu pozici
nahradí všechny vypsané pozice
nahradí všechny pozice od 0x000A až po 0x000F (za èárkou je možné vypsat další intervaly)
Komentování øádkù nejde vedle sebe, ale pod sebou, takto:

# nahradí jednu pozici
pentagram = 0x0000
# nahradí všechny vypsané pozice
pentagram = {0x0001, 0x0002, 0x0003, 0xC000, 0xC001, 0xFDBC}
# nahradí všechny pozice od 0x000A až po 0x000F (za èárkou je možné vypsat další intervaly)
pentagram = [0x000A - 0x000F, 0x00A0 - 0x00F0]

Tyto 2 možnosti se dají i kombinovat tzv. nastavený config tøeba s pentragram.bmp a zároven mít ve složce soubory s názvem pozic

Do složky MulFiles je potøeba vložit soubory, které chcete aktualizovat. Záleží, jaký formát dìláte.
Ve složce Patched pak najdete aktualizované soubory.


Pro hráèe:

Pøidal jsem podporu aplikace alias Drag&Drop. Jedná se o velmi lehkou, rychlou a spolehlivou cestu, jak aplikovat patch aniž by èlovìk musel cokoliv udìlat (až na výbìr jestli patchovat do MUL nebo UOP).
Následnì k tomu jsem vytvoøil program VMulPatcherCreator.exe, kterým si mùžete lehce vytvoøit vlastní patch, který pak mùžete sdílet s ostatními. Program je velmi jednoduchý a dynamicky kombinovatelný, tím chci øíct, že do jednoho patche je možno vložit jak Arty, LandArty, Gumpy, Textury zároveò a vytvoøí se jen jeden patch (aktuálnì se vytváøí pod jménem "Patch.pvmul").
Pro aplikování takto kombinovaného patche je ovšem nutné mít všechny MUL/UOP soubory ve složce "MulFiles" jinak je pravdìpoodbná šance, že program spadne èi vyhodí chybovou hlášku.
Pro vyzkoušení jsem pøidal do raru do složky "_Examples" jeden patch (na paøezy) a mùžete si sami ho zkusit aplikovat (opravdu se jedná jen o Drag&Drop použití).
Práce s VMulPatcherCreator.exe je intuitivní a snadná. Pokud z nìjakého dùvodu nechcete použít patch pomocí Drag&Drop, je možné si napsat nìjaký soubor.bat.

Napøíklad soubor start.bat ve kterém bude pøíkaz:

VMulPatcher.exe Patch.pvmul -f -g

Název souboru Patch.pvmul mùžete zmìnit a pøepsat. Jako napøíklad ve složce _Examples existuje Parezy.pvmul. Pøíkaz pro .bat soubor bude následující:

VMulPatcher.exe Parezy.pvmul -f -g

A to je asi všechno.




