using System.Globalization;
using deftq.BuildingBlocks.Domain;

namespace deftq.Catalog.Domain.MaterialCatalog
{
    public sealed class MountingCode : ValueObject
    {
        public int Code { get; private set; }
        public string Text { get; private set; }

        private MountingCode()
        {
            Code = 0;
            Text = String.Empty;
        }

        private MountingCode(int code, string text)
        {
            Code = code;
            Text = text;
        }

        public static MountingCode Empty()
        {
            return new MountingCode();
        }

#pragma warning disable MA0051
        public static MountingCode FromCode(int code)
        {
            switch (code)
            {
                case 0:
                    return new MountingCode(0, "Grundtid");
                case 1:
                    return new MountingCode(1, "PM");
                case 2:
                    return new MountingCode(2, "Uden fastgørelse/reference til bygningsdel");
                case 3:
                    return new MountingCode(3, "Uden reference til bygnings-dele.");
                case 5:
                    return new MountingCode(5, "I dåse");
                case 6:
                    return new MountingCode(6, "Clipset i dåse");
                case 7:
                    return new MountingCode(7, "Limet");
                case 10:
                    return new MountingCode(10, "I rille");
                case 11:
                    return new MountingCode(11, "I rør");
                case 12:
                    return new MountingCode(12, "I jord");
                case 14:
                    return new MountingCode(14, "Eftf. kabel/rør under f.bøjle");
                case 15:
                    return new MountingCode(15, "I/på føringsvej ufastgjort");
                case 16:
                    return new MountingCode(16, "I/på føringsvej med strips");
                case 17:
                    return new MountingCode(17, "I/på føringsvej med bøjler");
                case 18:
                    return new MountingCode(18, "I/på føringsvej spredt.bef.");
                case 19:
                    return new MountingCode(19, "Opkvejlet");
                case 20:
                    return new MountingCode(20, "I pladevæg/forskalling/hulmur");
                case 21:
                    return new MountingCode(21, "I/på pladevæg ");
                case 25:
                    return new MountingCode(25, "Fastgjort med skruer/bolte og møtrik, boltsæt, T-bolt, skruer i gevindhuller og lign., uden boring");
                case 26:
                    return new MountingCode(26, "I profilskinne o.l. med T-bolt");
                case 27:
                    return new MountingCode(27, "Nedhængt i wire, pendelrør ell.lign.");
                case 30:
                    return new MountingCode(30, "I/på træ");
                case 31:
                    return new MountingCode(31, "PM");
                case 32:
                    return new MountingCode(32, "På træ clipset");
                case 33:
                    return new MountingCode(33, "På træ spredt bef.");
                case 34:
                    return new MountingCode(34, "PM");
                case 40:
                    return new MountingCode(40, "I/på kunststof");
                case 50:
                    return new MountingCode(50, "I letbeton");
                case 60:
                    return new MountingCode(60, "I-på tegl/på letbeton");
                case 62:
                    return new MountingCode(62, "På tegl/letbeton clipset");
                case 63:
                    return new MountingCode(63, "Kabler og rør på tegl/letbeton med spredt befæstigelse");
                case 64:
                    return new MountingCode(64, "I blank mur");
                case 70:
                    return new MountingCode(70, "I/på jern t.o.m. 2,5mm tyk-kelse, excl. gevindskæring");
                case 71:
                    return new MountingCode(71, "I/på jern over 2,5mm tyk-kelse, excl. gevindskæring");
                case 73:
                    return new MountingCode(73, "Kabler og rør på jern med spredt befæstigelse");
                case 75:
                    return new MountingCode(75, "I/på letmetal");
                case 80:
                    return new MountingCode(80, "I/på beton");
                case 81:
                    return new MountingCode(81, "PM");
                case 82:
                    return new MountingCode(82, "Kabler og rør clipset på beton");
                case 83:
                    return new MountingCode(83, "Kabler og rør på beton med spredt befæstigelse");
                case 85:
                    return new MountingCode(85, "På beton med slagskrue");
                case 86:
                    return new MountingCode(86, "Skudmontage");
                default:
                    return new MountingCode(-1, "Ingen montagekode");
            }
        }
#pragma warning restore MA0051

        public string CodeAsText()
        {
            return Code.ToString("00", new NumberFormatInfo());
        }
    }
}
