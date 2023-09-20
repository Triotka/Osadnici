# Dokumentace
Program je rozdělen na dvě části logika hry a vizualizace. Na hru bylo použito WPF C#.
## Hrací logika 
Hrací logika se nachází v souboru Game.cs.
### Game.cs
- Je zde třída Card.
    - Jedná se o kartu se surovinou.
- Je zde třída Recipe.
    - Jedná o jeden na stavbu dané věci.
- Je zde třída Game.
    - spojuje všechny věci potřebné pro jednu hru.
- Je zde třída Pirate.
    - Reprezentuje piráta, figurku ve hře, která umí blokovat pole a je přemisťovaná, když padne číslo 7.
- Je zde třída Dice.
    - Jedná se o dvě kostky, které se mohou házet.
- Je zde třída Board.
    - Obsahuje informace o hrací ploše.
- Je zde třída Town.
    - Jedná se o figurku města s vlastnostmi, které jsou popsané v pravidlech v uživatelské dokumentaci.
- Je zde třída Village.
    - Jedná se o figurku vesnice s vlastnostmi, které jsou popsané v pravidlech v uživatelské dokumentaci.
- Je zde třída Building.
    - Jedná se o figurky, které spadají pod building jako vesnice nebo město.
- Je zde třída Road.
    - Jedná se o figurku cesty s vlastnostmi, které jsou popsané v pravidlech v uživatelské dokumentaci.
- Je zde třída Neighbours.
    - Jedná se o pole sousedy pole, což je potřeba při vyhodnocování kostek nebo stavbě.
- Je zde třída Hexagon.
    - Jedná se o jedno šestiúhelníkové pole hrací plochy.
 Je zde třída Player.
    - Obsahuje údaje o jednom hráči.
 Je zde třída Pawn.
    - Jedná se o reprezentaci figurky.

## Vizualizace
Vizualizace je udělaná ve WPF C#. Program je rozdělen podle oken každému oknu náleží soubor .xaml a .xaml.cs.
### App
- Nachází se zde věci týkající se celé aplikace.
- Je zde zaručeno, že hra začne ve WindowPickPlayer.
- Jsou zde public třídy. Tyto třídy a jejich public (převážně static) funkce jsou použity, pokud chceme mít funkci, která je použita více okny.
- Je zde třída GenericWindow, která obsahuje funkce, které jsou využívány ve více oknech.
    - Jsou zde generické třídy, které dokaží vrátit jméno objectu, pokud má object property Name a funkce, která pokud má object property Name z něj vytáhne index.
    - void CreateExitButton(RoutedEventHandler handler, int size, Grid outerGrid) je to funkce, která po předání Grid daného okna, vytvoří tvar exit buttonu v pravém horním rohu o dané velikosti a handlerem.
    - List<List<Point>> GetListOfPoints(PointCollection points) je funkce, která z PointsCollection udělá List
    - Je tu několik funkcí na tvorbu tvarů, jako šestiúhelník, trojúhelník, kruh, čtverec, které jsou použity ve hře na pole nebo figurky
    - Je zde funkce, která po předání už nakresleného hexagonu vrátí jeho vrcholy.
    - Je zde funkce, která nastavuje styl společný pro všechna okna.
    - Jsou zde funkce, které vytvoří tlačítka pro nějakou akci a label na vrchu obrazovky na zprávy pro hráče.
- Je zde třída ColorMaker, která obsahuje všechny LinearBrush barvy  použité ve hře a funkce pojmenované podle toho co tvoří.
    - CreateButtonPaint() tvoří barvu tlačítek.
    - CreateCardPaint() tvoří barvu karet, které na sobě mají název materiálu nebo figurek.
    - CreateCardBackground() tvoří pozadí za kartami, které na sobě mají název materiálu nebo figurek.
    - BoardLamb() tvoří barvu suroviny lamb na hrací ploše
    - BoardWheat() tvoří barvu suroviny wheat na hrací ploše
    - BoardStone() tvoří barvu suroviny stone na hrací ploše
    - BoardWood() tvoří barvu suroviny wood na hrací ploše
    - BoardBrick() tvoří barvu suroviny brick na hrací ploše
    - BoardDesert() tvoří barvu počátečního pole piráta na hrací ploše

### WindowPickPlayers
- Je zde třída WindowPickPlayers.
    - Obsahuje funkce, které zaručí fungovaní exit tlačítka.
    - Obsahuje funkce, které zaručí fungovaní tlačítek výběru.

### MainWindow
- Vytvoří okno, kde probíhá většina hry s hrací plochou figurkami a kartami.
- Je zde třída ActionField.
    - Tvoří tlačítka na boku pro různé akce jako hod kostkou nebo přepnutí na dalšího hráče.
- Je zde třída CardsSet.
    - Tvoří tlačítka ve tvaru karet.
    - Je možnost vytvořit karty s figurkami nebo karty materiálu.
- Je zde třída Pawn.
    - Zobrazuje figurky hráčů na hrací ploše.
    - Zobrazuje figurku piráta na hrací ploše.
- Je zde třída DrawnBoard.
    - Zobrazuje hrací plochu s barvami a čísly podle defaultních pravidel.
- Je zde třída Start.
    - Používá se k určení centra větších objektů.
- Je zde třída MainWindow.
    - Jsou tu funkce, co zaručují funkčnost tlačítek.
    - Je tu funkce na tvorbu labelu pro statistiky hráče.

### WinnerWindow
- Obsahuje třídy, které zaručí zobrazení okna poté, co hráč vyhraje.
- Je zde třída WindowWinner.
    - Obsahuje funkce, které zaručí fungovaní exit tlačítka.
    - Obsahuje funkce, které zaručí fungovaní tlačítka restartu.
    - Obsahuje funkce, které zaručí výpis výherce.

### WindowHexagon
- Obsahuje třídy, které zaručí zobrazení okna, který odpovídá zoomu na jedno hrací pole a příslušné interakce s tím spjaté.
- Je zde třída WindowPickPlayers.
    - Obsahuje funkce, které zaručí fungovaní tlačítek.
- Je zde třída DrawnBigHexagon.
    - Obsahuje funkce, které vytvoří velký šestiúhelník, odpovídající vybranému poli.
    - Obsahuje funkce, které vytvoří tlačítka, pro stavbu na velkém šestiúhelníku.
- Je zde třída HexagonActionField
    - Obsahuje funkce, které vytvoří tlačítka pro akci jako zastavení stavění nebo umístění piráta.




