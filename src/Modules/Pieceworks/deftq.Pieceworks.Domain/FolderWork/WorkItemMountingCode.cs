using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.FolderWork
{
    public sealed class WorkItemMountingCode : ValueObject
    {
        public int MountingCode { get; private set; }
        public string Text { get; private set; }
        
        private WorkItemMountingCode()
        {
            MountingCode = 0;
            Text = string.Empty;
        }

        private WorkItemMountingCode(int mountingCode, string text)
        {
            MountingCode = mountingCode;
            Text = text;
        }

#pragma warning disable MA0051
        public static WorkItemMountingCode FromCode(int code)
        {
            switch (code)
            {
                case 3:
                    return new WorkItemMountingCode(3, "Uden reference til bygnings-dele.");
                case 7:
                    return new WorkItemMountingCode(7, "Limet");
                case 10:
                    return new WorkItemMountingCode(10, "I rille");
                case 11:
                    return new WorkItemMountingCode(11, "I rør");
                case 12:
                    return new WorkItemMountingCode(12, "I jord");
                case 14:
                    return new WorkItemMountingCode(14, "Efterfølgende kabler og rør i/på føringsvej eller bygnings-del under fælles befæstigelse.");
                case 15:
                    return new WorkItemMountingCode(15, "Kabler og rør i/på føringsvej u. fastgørelse.");
                case 16:
                    return new WorkItemMountingCode(16, "Kabler og rør i/på føringsvej fastgjort med strips med nor-mal befæstigelsesafstand.");
                case 17:
                    return new WorkItemMountingCode(17,
                        "Kabler og rør i/på føringsvej fastgjort med polbøjler eller krydsbinding med normal be-fæstigelsesafstand.");
                case 18:
                    return new WorkItemMountingCode(18, "Kabler og rør med spredt be-fæstigelse i/på føringsvej");
                case 19:
                    return new WorkItemMountingCode(19, "Kabler opkvejlet, hvor det for-langes");
                case 20:
                    return new WorkItemMountingCode(20, "Kabler og rør i/på forskal-ling/hulmur/pladevæg");
                case 25:
                    return new WorkItemMountingCode(25,
                        "Fastgjort med skruer/bolte og møtrik, boltsæt, T-bolt, skruer i gevindhuller og lign., uden boring");
                case 30:
                    return new WorkItemMountingCode(30, "I/på træ");
                case 32:
                    return new WorkItemMountingCode(32, "Kabler og rør clipset på træ");
                case 33:
                    return new WorkItemMountingCode(33, "Kabler og rør på træ med spredt befæstigelse");
                case 40:
                    return new WorkItemMountingCode(40, "I/på kunststof");
                case 50:
                    return new WorkItemMountingCode(50, "I letbeton");
                case 60:
                    return new WorkItemMountingCode(60, "I tegl. På tegl/letbeton");
                case 62:
                    return new WorkItemMountingCode(62, "Kabler og rør clipset på tegl/letbeton");
                case 63:
                    return new WorkItemMountingCode(63, "Kabler og rør på tegl/letbeton med spredt befæstigelse");
                case 70:
                    return new WorkItemMountingCode(70, "I/på jern t.o.m. 2,5mm tyk-kelse, excl. gevindskæring");
                case 71:
                    return new WorkItemMountingCode(71, "I/på jern over 2,5mm tyk-kelse, excl. gevindskæring");
                case 73:
                    return new WorkItemMountingCode(73, "Kabler og rør på jern med spredt befæstigelse");
                case 75:
                    return new WorkItemMountingCode(75, "I/på letmetal");
                case 80:
                    return new WorkItemMountingCode(80, "I/på beton");
                case 82:
                    return new WorkItemMountingCode(82, "Kabler og rør clipset på beton");
                case 83:
                    return new WorkItemMountingCode(83, "Kabler og rør på beton med spredt befæstigelse");
                case 85:
                    return new WorkItemMountingCode(85, "På beton med slagskrue");
                case 86:
                    return new WorkItemMountingCode(86, "Skudmontage");
                default:
                    return new WorkItemMountingCode(-1, "Ingen montagekode");
            }
        }
#pragma warning restore MA0051
    }
}
