# EVA-2-exercise-Reversi-3-WinRT
The Event Driven Application Development 2 course exercise at hungarian university ELTE IK. http://people.inf.elte.hu/groberto/elte_eva2/index.html

3. beadandó feladat: WinRT grafikus felületű alkalmazás
Közös követelmények:
• A beadandók dokumentációból, valamint programból állnak, utóbbi csak a
megfelelő dokumentáció bemutatásával értékelhető. Csak funkcionálisan teljes, a
feladatnak megfelelő, önállóan megvalósított, személyesen bemutatott program
fogadható el.
• A megvalósításnak felhasználóbarátnak, és könnyen kezelhetőnek kell lennie. A
szerkezetében törekednie kell az objektumorientált szemlélet megtartására.
• A programot MVVM architektúrában kell felépíteni, amelyben a megjelenítés
rétege elkülönül a modellnézettől, a modelltől, valamint az adatkezeléstől. A
modell és az adatkezelés nem tartalmazhat semmilyen grafikus felületbeli
osztályra történő hivatkozást, csak eseményeket küldhet a nézetmodellnek. A
nézetmodell nem tartalmazhat semmilyen játékbeli adatot, a nézet pedig
semmilyen háttérkódot.
• A modell működését egységtesztek segítségével kell ellenőrizni. Nem kell teljes
körű tesztet végezni, azonban a lényeges funkciókat, és azok hatásait ellenőrizni
kell.
• A dokumentációnak jól áttekinthetőnek, megfelelően formázottnak kell lennie,
tartalmaznia kell a fejlesztő adatait, a feladatleírást, a feladat elemzését,
felhasználói eseteit (UML felhasználói esetek diagrammal), a program
szerkezetének leírását (UML osztálydiagrammal), valamint a tesztesetek leírását.
A dokumentáció ne tartalmazzon kódrészleteket, illetve képenyőképeket. A
megjelenő diagramokat megfelelő szerkesztőeszköz segítségével kell előállítani.
A dokumentációt elektronikusan, PDF formátumban kell leadni.

17. Reversi
Készítsünk programot, amellyel az alábbi Reversi játékot játszhatjuk.
A játékot két játékos játssza n × n-es négyzetrácsos táblán fekete és fehér
korongokkal. Kezdéskor a tábla közepén X alakban két-két korong van elhelyezve
mindkét színből. A játékosok felváltva tesznek le újabb korongokat. A játék
lényege, hogy a lépés befejezéseként az ellenfél ollóba fogott, azaz két oldalról
(vízszintesen, függőlegesen vagy átlósan) közrezárt bábuit (egy lépésben akár
több irányban is) a saját színünkre cseréljük.
Mindkét játékosnak, minden lépésben ütnie kell. Ha egy állásban nincs olyan
lépés, amivel a játékos ollóba tudna fogni legalább egy ellenséges korongot,
passzolnia kell és újra ellenfele lép. A játékosok célja, hogy a játék végére minél
több saját színű korongjuk legyen a táblán.
A játék akkor ér véget, ha a tábla megtelik, vagy ha mindkét játékos passzol. A
játék győztese az a játékos, akinek a játék végén több korongja van a táblán. A
játék döntetlen, ha mindkét játékosnak ugyanannyi korongja van a játék végén.
A program biztosítson lehetőséget új játék kezdésére a táblaméret megadásával
(10 × 10, 20 × 20, 30 × 30), játék szüneteltetésére, valamint játék mentésére és
betöltésére. Ismerje fel, ha vége a játéknak, és jelenítse meg, melyik játékos
győzött. A program folyamatosan jelezze külön-külön a két játékos gondolkodási
idejét (azon idők összessége, ami az előző játékos lépésétől a saját lépéséig tart,
ezt is mentsük el és töltsük be).
