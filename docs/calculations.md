# Akkord+ beregninger

Jeg har med vilje skrevet på dansk, men tilføjet engelske termer enkelte steder.

## Projekt opmåling

Et projekt (`project`) i Akkord+ indeholder en struktur af foldere. Elektrikernes opmåling (`registration`) organiseres i folderne. 

Kun godkendte opmålinger i projektet bliver inkluderet i beregninger ne(dette skal afklares).

Værdier der ændres over tid eks. som en del af overenskomst benyttes i beregninger. Et projekt kan således benytte forskellige værdier af eks. betalingsfaktor igennem udførelsen af et projekt.

## Model

Opmåling af udført arbejde skal holdes adskildt fra importeret kalkulationer, ellers vil det ikke være muligt at beregne fremdrift og færdiggørelsesgrad.
Det planlagte arbejde kan imporeteres enten fra eks. Kalkia eller tastes ind af akkordholderen.

Opmåling af enkelte enheder kan udføres af forskellige personer eller udføres over flere dage. Godkendelse af opmåling kan også ske mens arbejdet er under udførsel.

### Konceptuel model

![Folders](/docs/Images/calculations.png)

### Ocvervejelser

- Skal det være muligt at rette tidlige opmålinger og eks. godkendte opmålinger? Akkorddeltageren kan måske rette en opmåling indtil den sendes til godkendelse.
- Presentation af opmålinger kan vises i som summering på enheden som default, så kun godkendte opmålinger tæller med i visningen.

## Akkordsum / lønsum

Akkordsum (`piecework sum`) er værdien af det totale udførte arbejde. Beregningen er baseret på tiderne på ydelserne, og altså ikke på tiden det har taget at udføre arbejdet.

## Færdiggørelsesgrad / Fremdrift

Fremdrift `progress` er andelen af det totale arbejde der er udført. Tallet giver kun mening hvis der er angivet planlagte meter/stk. på enhederne i projektet.

## Optræk

Optræk er en regulering af elektrikernes løbende udbetalte løn. Hvis akkord arbejdet går hurtigere eller langsommere kan lønnen reguleres således at man rammer tættere på den reelle timeløn når projektet gøres op. 
