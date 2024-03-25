using deftq.BuildingBlocks.DataAccess.InitialData;
using deftq.Catalog.Domain.OperationCatalog;
using deftq.Catalog.Domain.Time;
using Marten;
using Marten.Services;

namespace deftq.Catalog.Infrastructure.DemoData
{
    public class DemoOperations : IDemoDataProvider
    {
        public async Task Populate(IDocumentStore documentStore, CancellationToken cancellation)
        {
            await ImportDemoOperations(documentStore, cancellation);
        }

#pragma warning disable MA0051
        private static async Task ImportDemoOperations(IDocumentStore documentStore, CancellationToken cancellation)
#pragma warning restore MA0051
        {
            await using var session = await documentStore.OpenSessionAsync(new SessionOptions(), cancellation);

            await UpsertOperation(session, "D488E143-959F-4EEB-8A7D-0C79A9FD03D9", "001139000001", "051131", "=<5mm gevind i jern t.o.m. 2,5mm", 6480);
            await UpsertOperation(session, "0559FED0-3E8E-40E9-997B-0ED74C7D10F6", "001139000002", "051131", ">5 - 8mm gevind i jern t.o.m. 2,5mm", 12660);
            await UpsertOperation(session, "51BCFA70-DE5C-457D-B644-4A3F45A0F9D5", "009500000016", "009500", "I/på føringsvej type 2, strips", 44340);
            await UpsertOperation(session, "D4E346F8-9F7A-46A3-81E7-3440D33F6A55", "009500000017", "009500", "I/på føringsvej type 3, polbøjler/X-strips", 61500);
            await UpsertOperation(session, "BA8F023A-EEFE-4D95-B4E8-3FBEC945E6DC", "009500000030", "009500", "I/på træ", 32280);
            await UpsertOperation(session, "8B4764A8-F115-49F5-BD03-E0E8A3649394", "009500000032", "009500", "Clipset på træ", 11220);
            await UpsertOperation(session, "B13C288B-821F-4D63-A2AB-05AC11401C33", "009500000060", "009500", "I/på tegl", 72060);
            await UpsertOperation(session, "13F21FD1-EB8A-472C-B8E4-0530C3FBCD97", "009500000062", "009500", "Clipset på tegl", 24420);
            await UpsertOperation(session, "7A3ABAB9-CF8A-40B7-8104-7EB0B63295EF", "009500000070", "009500", "I/på jern t.o.m. 2,5mm tykkelse", 127260);
            await UpsertOperation(session, "F081CC76-3928-463C-82B2-4D7972087C7D", "009500000071", "009500", "I/på jern over 2,5mm tykkelse", 161580);
            await UpsertOperation(session, "A8AD781A-76E4-423D-8DA5-5321F8F9A66B", "009500000080", "009500", "I/på beton", 71280);
            await UpsertOperation(session, "7F732410-1352-4F12-8C1B-F27274A98683", "009500000082", "009500", "Clipset på beton", 24000);
            await UpsertOperation(session, "DAE38FF1-7E7E-4E12-B1FC-D8C0D6AED812", "009500000086", "009500", "Skudmontage", 19560);
            await UpsertOperation(session, "74907DD0-D42F-433F-911C-75189E58E8A8", "050101000001", "050101", "Hul i træ <=3 cm, Ø -10 mm", 8160);
            await UpsertOperation(session, "6E636922-4E30-43E2-85E7-C85692571CBE", "050101000002", "050101", "Hul i træ <=3 cm, Ø o/10-20 mm", 22620);
            await UpsertOperation(session, "4EAA0C33-A3E5-461B-B6D7-B1E528C4BC24", "050101000003", "050101", "Hul i træ <=3 cm, Ø o/20-40 mm", 35340);
            await UpsertOperation(session, "F29A5A20-EA52-4A90-AD3C-BC8A050B3A16", "050101000004", "050101", "Hul i træ <=3 cm, Ø o/40-60 mm", 49680);
            await UpsertOperation(session, "EE98C589-5476-49FC-BE23-559DAE61FEB0", "050101000005", "050101", "Hul i træ <=3 cm, Ø o/60-80 mm", 65040);
            await UpsertOperation(session, "9DBC355B-589C-4FA7-8FCE-FE3433BF481C", "050101000006", "050101", "Hul i træ <=3 cm, Ø o/80-100 mm", 80400);
            await UpsertOperation(session, "B46C38DF-CBFD-4C53-B829-CB3AE1F5243D", "050101000007", "050101", "Hul i træ <=3 cm, Ø o/100-120 mm", 95220);
            await UpsertOperation(session, "E5DD34C1-F4C9-4535-A64C-D3815B58EF0B", "050101000008", "050101", "Hul i træ <=3 cm, Ø o/120-150 mm", 118800);
            await UpsertOperation(session, "1407FD8F-141F-4339-9ABB-0BE70663CEB4", "050101000009", "050101", "Hul i træ <=3 cm, Ø o/150-180 mm", 141780);
            await UpsertOperation(session, "4BDF6411-7779-4A10-82AE-86EC64D281B7", "050102000001", "050102", "Hul i træ o/3-6 cm, Ø -10 mm", 10380);
            await UpsertOperation(session, "7F8999DC-36F6-4722-9DEB-7BCCC6F9DED7", "050102000002", "050102", "Hul i træ o/3-6 cm, Ø o/10-20 mm", 23220);
            await UpsertOperation(session, "6CF6E43D-AE97-4EB7-B227-973B2A262FC8", "050102000003", "050102", "Hul i træ o/3-6 cm, Ø o/20-40 mm", 36000);
            await UpsertOperation(session, "AB129187-62CB-4337-82BE-468E07D20E16", "050102000004", "050102", "Hul i træ o/3-6 cm, Ø o/40-60 mm", 53220);
            await UpsertOperation(session, "25C8ABFF-FEBA-4256-913A-057EEB3314F2", "050102000005", "050102", "Hul i træ o/3-6 cm, Ø o/60-80 mm", 70020);
            await UpsertOperation(session, "84235D4D-07B1-4A87-957F-14884F7E8F03", "050102000006", "050102", "Hul i træ o/3-6 cm, Ø o/80-100 mm", 86760);
            await UpsertOperation(session, "32EBF0B4-2283-49D6-91C5-9335B288BE22", "050102000008", "050102", "Hul i træ o/3-6 cm, Ø o/120-150 mm", 126360);
            await UpsertOperation(session, "E223C614-C14E-47FC-88B2-F71FBD93380A", "050102000009", "050102", "Hul i træ o/3-6 cm, Ø o/150-180 mm", 150480);
            await UpsertOperation(session, "37996118-6C0C-44A5-8D02-66A4BEE6B67F", "050103000001", "050103", "Hul i træ o/6-11 cm, Ø -10 mm", 23760);
            await UpsertOperation(session, "5B31A379-6682-4A65-A741-2873F8EB8BF1", "050103000002", "050103", "Hul i træ o/6-11 cm, Ø o/10-20 mm", 38340);
            await UpsertOperation(session, "2CD16A22-EA01-457D-9E72-ED6E0ADA8FEC", "050103000003", "050103", "Hul i træ o/6-11 cm, Ø o/20-40 mm", 67500);
            await UpsertOperation(session, "6C38CF31-225C-426A-9458-CFC09EC16C76", "050103000004", "050103", "Hul i træ o/6-11 cm, Ø o/40-60 mm", 96660);
            await UpsertOperation(session, "5B7294BE-7CA2-4147-9611-391CDF77248C", "050103000005", "050103", "Hul i træ o/6-11 cm, Ø o/60-80 mm", 125820);
            await UpsertOperation(session, "FF8DC88A-7479-437F-8D58-AFF095AD7228", "050103000006", "050103", "Hul i træ o/6-11 cm, Ø o/80-100 mm", 154980);
            await UpsertOperation(session, "09A719E1-2E20-4741-9DBB-81E5D59A3CD8", "050104000002", "050104", "Hul i træ o/11-16 cm, Ø o/10-20 mm", 59640);
            await UpsertOperation(session, "818C490F-67E9-480F-806B-56F82CD48DE0", "050201000001", "050201", "Hul i letbet.<=13 cm tyk, Ø -10 mm.", 23580);
            await UpsertOperation(session, "1988203E-003F-4B1E-89B1-DAC89FB79EFF", "050201000002", "050201", "Hul i letbet.<=13 cm tyk, Ø o/10-20 mm", 28500);
            await UpsertOperation(session, "EA5CE8D9-A985-4F5F-99A7-98AA80DE1BB3", "050201000003", "050201", "Hul i letbet.<=13 cm tyk, Ø o/20-40 mm", 43380);
            await UpsertOperation(session, "02EDA93C-4F24-4930-B12A-6E1A84A1E17A", "050201000004", "050201", "Hul i letbet.<=13 cm tyk, Ø o/40-60 mm", 60720);
            await UpsertOperation(session, "4CB6F845-94B6-43C7-8BBE-5BD5A6367C38", "050201000006", "050201", "Hul i letbet.<=13 cm tyk, Ø o/80-100 mm", 103620);
            await UpsertOperation(session, "01B717DE-C62C-4D51-9984-0FD1240FC4F4", "050201000010", "050201", "Firkant-hul t.o.m. 150cm2", 319860);
            await UpsertOperation(session, "1E812F35-DECE-4A33-A11B-098CCC16E920", "050201000011", "050201", "Firkant-hul o/150 t.o.m. 200cm2", 314580);
            await UpsertOperation(session, "26E4D76D-C6C3-4F2A-8F3F-44FB70850F09", "050201000012", "050201", "Firkant-hul o/200 t.o.m. 300cm2", 359460);
            await UpsertOperation(session, "A052C319-8A47-469D-8B2F-9D3BB76EE072", "050201000013", "050201", "Firkant-hul o/300 t.o.m. 400cm2", 406500);
            await UpsertOperation(session, "8DB1D74C-6764-4A88-89D3-8A91E76E5D53", "050201000014", "050201", "Firkant-hul o/400 t.o.m. 600cm2", 486840);
            await UpsertOperation(session, "5C875A7B-F5F0-471C-835A-FBF456DA668F", "050202000001", "050202", "Hul i letbeton o/13-27 cm, Ø -10 mm", 34020);
            await UpsertOperation(session, "B5E286C6-8FA1-4A1D-9FF2-9E0D6DB7DD2D", "050202000002", "050202", "Hul i letbeton o/13-27 cm, Ø o/10-20 mm", 45300);
            await UpsertOperation(session, "79121325-BB0B-4266-ABBB-D0635CB2799D", "050202000003", "050202", "Hul i letbeton o/13-27 cm, Ø o/20-40 mm", 73620);
            await UpsertOperation(session, "8C911B39-B362-436A-916F-80FCDF1B67A0", "050202000004", "050202", "Hul i letbeton o/13-27 cm, Ø o/40-60 mm", 108300);
            await UpsertOperation(session, "86EBB8F4-C2BF-47C9-A3A0-E6E8FD9ADAE4", "050202000005", "050202", "Hul i letbeton o/13-27 cm, Ø o/60-80 mm", 142500);
            await UpsertOperation(session, "CB48E41D-EA43-4EBD-89EB-E44DC640E210", "050202000006", "050202", "Hul i letbeton o/13-27 cm, Ø o/80-100 mm", 184860);
            await UpsertOperation(session, "C71E32AA-DC7A-4780-8FE5-CB66B7AA3257", "050301000001", "050301", "Hul i tegl <=13 cm, Ø -10 mm", 27180);
            await UpsertOperation(session, "491E44B1-4E75-4BB4-A7F3-E567596CF0B7", "050301000002", "050301", "Hul i tegl <=13 cm, Ø o/10-20 mm", 49080);
            await UpsertOperation(session, "FF516FD2-8CDD-42CF-A825-0EB6D48E04B7", "050301000003", "050301", "Hul i tegl <=13 cm, Ø o/20-40 mm", 93300);
            await UpsertOperation(session, "AF256D6B-2525-44B7-9AEC-8EA74A78090D", "050301000004", "050301", "Hul i tegl <=13 cm, Ø o/40-60 mm", 135360);
            await UpsertOperation(session, "1040D295-2D9F-4885-84D7-CF9D9F5CEF43", "050302000001", "050302", " Hul i tegl o/13-27 cm, Ø -10 mm", 40800);
            await UpsertOperation(session, "CD354C18-268B-40ED-AF74-7F6FA6F17756", "050302000002", "050302", " Hul i tegl o/13-27 cm, Ø o/10-20 mm", 61020);
            await UpsertOperation(session, "F53FD941-5DAD-4106-91FA-D3D8ECD72DD9", "050302000003", "050302", " Hul i tegl o/13-27 cm, Ø o/20-40 mm", 149460);
            await UpsertOperation(session, "5F284D94-A1DA-400F-84C9-36AD0593FDC7", "050302000004", "050302", " Hul i tegl o/13-27 cm, Ø o/40-60 mm", 233580);
            await UpsertOperation(session, "064FEDF4-B6C0-47C2-A783-ADBFBCCBF716", "050401000001", "050401", "Hul i beton <=10cm, Ø -10 mm", 34800);
            await UpsertOperation(session, "D0C93CAC-6B1E-4C21-9308-4CB6CBC8C8F0", "050401000002", "050401", "Hul i beton <=10cm, Ø o/10-20 mm", 59640);
            await UpsertOperation(session, "EDD92C39-FCB6-44B5-8799-B71F59378B28", "050401000003", "050401", "Hul i beton <=10cm, Ø o/20-40 mm", 109320);
            await UpsertOperation(session, "376271FD-78E2-4E74-A8DA-C4355EB6AF98", "050401000004", "050401", "Hul i beton <=10cm, Ø o/40-60 mm. Excl. vand og til- og afrigning", 159000);
            await UpsertOperation(session, "DEAA1AC6-9CFD-4760-AD7A-B43DB951A4A9", "050401000005", "050401", "Hul i beton <=10cm, Ø o/60-80 mm. Excl. vand og til- og afrigning", 208680);
            await UpsertOperation(session, "43704A5F-8055-4C42-81BA-58AB2FBA43AD", "050401000006", "050401", "Hul i beton <=10cm, Ø o/80-100 mm. Excl. vand og til- og afrigning", 258360);
            await UpsertOperation(session, "14A1FE15-609C-4151-902C-C74BA24F00AC", "050402000001", "050402", "Hul i beton o/10-20cm, t.o.m. Ø 10 mm", 71580);
            await UpsertOperation(session, "4A1082FB-AB65-407E-9B4E-D7563BBEA388", "050402000002", "050402", "Hul i beton o/10-20cm, Ø o/10-20 mm", 134400);
            await UpsertOperation(session, "7E9AC4BA-AE99-4B4F-B6C8-71754C507064", "050402000003", "050402", "Hul i beton o/10-20cm, Ø o/20-40 mm", 217620);
            await UpsertOperation(session, "108CC58F-2B41-4003-9CE9-E23F805E3449", "050402000004", "050402", "Hul i beton o/10-20cm, Ø o/40-60 mm. Excl. vand og til- og afrigning", 264120);
            await UpsertOperation(session, "41970F4D-3264-4064-9492-A17EC1D71C28", "050402000005", "050402", "Hul i beton o/10-20cm, Ø o/60-80 mm. Excl. vand og til- og afrigning", 295920);
            await UpsertOperation(session, "B64D0282-154B-4364-B93E-2FCA1CFFC4F6", "050402000006", "050402", "Hul i beton o/10-20cm, Ø o/80-100 mm. Excl. vand og til- og afrigning", 320580);
            await UpsertOperation(session, "776C0550-1D9F-43F3-80E9-8D9352005C6E", "050403000001", "050403", "Hul i beton o/20-30cm, Ø-10 mm", 95400);
            await UpsertOperation(session, "BBA6CB38-364D-4DE3-908D-C14F399BEA1F", "050403000002", "050403", "Hul i beton o/20-30cm, Ø o/10-20 mm", 147060);
            await UpsertOperation(session, "2812ED7A-FE06-48CA-8E83-69DFFC94C14E", "050403000003", "050403", "Hul i beton o/20-30cm, Ø o/20-40 mm", 244740);
            await UpsertOperation(session, "E3CDBA3C-10BF-4AF7-9F40-67DF3ACF6E28", "050403000004", "050403", "Hul i beton o/20-30cm, Ø o/40-60 mm. Excl. vand og til- og afrigning", 302280);
            await UpsertOperation(session, "2971C413-F143-4CB1-ABCB-1D337E770306", "050403000005", "050403", "Hul i beton o/20-30cm, Ø o/60-80 mm. Excl. vand og til- og afrigning", 390900);
            await UpsertOperation(session, "0A6308A8-D74D-422C-90F0-E13836A19955", "050403000006", "050403", "Hul i beton o/20-30cm, Ø o/80-100 mm. Excl. vand og til- og afrigning", 478920);
            await UpsertOperation(session, "0EE1C624-C85D-4274-83C4-AED26C891248", "050501000001", "050501", "Hul i gips <=15mm, Ø 10 mm", 13860);
            await UpsertOperation(session, "43C6981D-59F0-493E-A7EE-C3F8E3EDE5A6", "050501000002", "050501", "Hul i gips <=15mm, Ø o/10-20 mm", 16440);
            await UpsertOperation(session, "DB3641E4-A262-4D10-9D2D-606A18DB4C2D", "050501000003", "050501", "Hul i gips <=15mm, Ø o/20-40 mm", 30180);
            await UpsertOperation(session, "1AB9B58A-884F-47B4-A920-5B82D224FA2F", "050501000004", "050501", "Hul i gips <=15mm, Ø o/40-60 mm", 36960);
            await UpsertOperation(session, "4B639CA3-DC2E-4D87-B09F-6A5FF6140B36", "050501000005", "050501", "Hul i gips <=15mm, Ø o/60-80 mm", 50580);
            await UpsertOperation(session, "6649F9DD-6E53-4E18-B129-02B508C96162", "050501000013", "050501", "Firkanthul i gips <=15mm, t.o.m. 150cm2", 216900);
            await UpsertOperation(session, "61F70FE6-5EB9-42C5-98BB-1853FD7968A9", "050501000014", "050501", "Firkanthul i gips <=15mm, o/150 t.o.m. 200cm2", 237840);
            await UpsertOperation(session, "8FD2638D-1D85-457F-AC8B-C42B91C14B4C", "050501000015", "050501", "Firkanthul i gips <=15mm, o/200 t.o.m. 300cm2", 253920);
            await UpsertOperation(session, "43C08AA6-8786-4E65-8067-E148309E35DC", "050501000016", "050501", "Firkanthul i gips <=15mm, o/300 t.o.m. 400cm2", 262800);
            await UpsertOperation(session, "DAD42543-E2CD-4677-99E4-CE0822D046FC", "050502000001", "050502", "Hul i gips o/15-30mm, Ø -10 mm", 14940);
            await UpsertOperation(session, "C5F5C2A3-ECF8-4649-A28E-218F3EE99D5D", "050502000002", "050502", "Hul i gips o/15-30mm, Ø o/10-20 mm", 13200);
            await UpsertOperation(session, "63523491-5E49-4A74-A1E6-A9946037EF0B", "050502000003", "050502", "Hul i gips o/15-30mm, Ø o/20-40 mm", 34800);
            await UpsertOperation(session, "8C705D74-87C9-445B-A24B-2C1B8618E42E", "050502000004", "050502", "Hul i gips o/15-30mm, Ø o/40-60 mm", 42720);
            await UpsertOperation(session, "BF91FE96-B992-4890-AA65-62FAE595C3A6", "050502000005", "050502", "Hul i gips o/15-30mm, Ø o/60-80 mm", 53160);
            await UpsertOperation(session, "6BB8D4B7-E17E-465B-9310-2FC07A513085", "050502000007", "050502", "Hul i gips o/15-30mm, Ø o/100-120 mm", 82080);
            await UpsertOperation(session, "B751F4E7-C49D-4B0C-AF0C-A99EDA36D89C", "050502000016", "050502",
                "Firkanthul i gips o/15-30mm, t.o.m. 150cm2", 231240);
            await UpsertOperation(session, "45E65D94-E098-436E-A349-9D38C2A154DF", "050502000017", "050502",
                "Firkanthul i gips o/15-30mm, o/150 t.o.m. 200cm2", 273060);
            await UpsertOperation(session, "09DD4B81-1A54-4B0E-B066-E6BAB676CF3B", "050502000018", "050502",
                "Firkanthul i gips o/15-30mm, o/200 t.o.m. 300cm2", 280320);
            await UpsertOperation(session, "E27F7E57-E374-4EAF-8D16-B73CA56EDF9B", "050502000019", "050502",
                "Firkanthul i gips o/15-30mm, o/300 t.o.m. 400cm2", 603960);
            await UpsertOperation(session, "FD2883C7-D250-49DA-8E8A-69E003194AB4", "050505000001", "050505", "Hul til forfradåse 1M i 1 lag gips",
                36960);
            await UpsertOperation(session, "098C4E44-D0A9-4B05-ADFB-86774F83C154", "050505000002", "050505",
                "Hul til forfradåse 1½ / 2M i 1 lag gips", 87180);
            await UpsertOperation(session, "8C6A9DBE-1313-42EE-BA21-FE5A8B162E71", "050505000003", "050505",
                "Hul til forfradåse 2½ / 3M i 1 lag gips", 137340);
            await UpsertOperation(session, "E02D9F8E-654E-47AF-B4A0-0F2F606758AE", "050505000006", "050505", "Hul til forfradåse 1M i 2 lag gips",
                42720);
            await UpsertOperation(session, "9D66BE6B-2E36-4798-9EAF-805233949221", "050505000007", "050505",
                "Hul til forfradåse 1½ / 2M i 2 lag gips", 107640);
            await UpsertOperation(session, "EB5A81B7-89C0-4427-B2D2-DD272197A0BF", "050505000008", "050505",
                "Hul til forfradåse 2½ / 3M i 2 lag gips", 172500);
            await UpsertOperation(session, "C9ECA9D4-7F32-42BF-9317-294F6B7B20EC", "050505000010", "050505",
                "Hul til Euro forfradåse Ø68-75 i 1 lag gips", 47520);
            await UpsertOperation(session, "AE1CD199-7A2C-4F4E-AD84-1DF2E7FDFF7B", "050505000011", "050505",
                "Hul til Euro forfradåse Ø68-75 i 2 lag gips", 46260);
            await UpsertOperation(session, "5D54221C-86A9-4743-B7B8-9A9F31FF2C50", "050505000012", "050505",
                "Hul til Euro forfradåse Ø68-75 i 3 lag gips", 46800);
            await UpsertOperation(session, "DB297E82-D123-4850-8B07-BB2AE9F5929D", "050551000005", "050551", "Udmåle og opmærke til 3½M", 100140);
            await UpsertOperation(session, "95721647-A91F-40D3-9038-48DFD37855C5", "050551000021", "050551", "Hul for forfradåse 1M i 1 lag gips",
                43200);
            await UpsertOperation(session, "3E739454-74A9-40F5-B3C1-5ED740D42E8B", "050551000031", "050551", "Hul for forfradåse 1M i 2 lag gips",
                29520);
            await UpsertOperation(session, "29AF94A1-D427-4284-A0F7-C75403C7EE62", "050551000037", "050551", "Hul for 1½M  i 2 lag gips", 89400);
            await UpsertOperation(session, "78D8135F-F12A-4F15-A47D-363F7B08B41A", "050551000038", "050551", "Hul for 2M  i 2 lag gips", 171900);
            await UpsertOperation(session, "CCF77F4D-AAA8-4537-9261-2F80175AD62E", "050551000039", "050551", "Hul for 2½M i 2 lag gips", 190740);
            await UpsertOperation(session, "E63049ED-9BAE-467C-B6F4-3A8CE76AE51C", "050551000041", "050551", "Hul for forfradåse 1M i 3 lag gips",
                69720);
            await UpsertOperation(session, "089FB02D-FEAA-4165-86DA-C2833CDE14FD", "050551000047", "050551", "Hul for 1½M  i 3 lag gips", 133980);
            await UpsertOperation(session, "FD60CF7C-76EC-4714-9852-E6975DDDA17F", "050551000048", "050551", "Hul for 2M  i 3 lag gips", 223080);
            await UpsertOperation(session, "06738AC7-CC40-45E5-89A5-C25260AB5C55", "050551000049", "050551", "Hul for 2½M i 3 lag gips", 271200);
            await UpsertOperation(session, "709D5F3B-C105-4656-AC99-108FBE0A75B7", "050601000001", "050602", "Hul i kunststof o/2-4 mm. Ø -10 mm",
                11760);
            await UpsertOperation(session, "159AC590-1F5E-4E94-8111-A282C21F2574", "050601000002", "050602", "Hul i kunststof o/2-4 mm. Ø o/10-20 mm",
                27420);
            await UpsertOperation(session, "347C3005-EAA8-40F2-BAE7-C02AED8727C6", "050601000003", "050602", "Hul i kunststof o/2-4 mm. Ø o/20-30 mm",
                41220);
            await UpsertOperation(session, "DA36BCE5-291F-4DCE-B159-F1C8FD9E3590", "050601000004", "050602", "Hul i kunststof o/2-4 mm. Ø o/30-40 mm",
                50640);
            await UpsertOperation(session, "C1242F94-A31C-4BAA-A06E-381A67E74D37", "050601000005", "050602", "Hul i kunststof o/2-4 mm. Ø o/40-60 mm",
                65520);
            await UpsertOperation(session, "A149C25D-5F02-4393-83DE-3E72CE02CC18", "050601000006", "050602", "Hul i kunststof o/2-4 mm. Ø o/60-80 mm",
                88020);
            await UpsertOperation(session, "ADC0E30B-5990-43AE-8001-1904837FD5F1", "050601000007", "050602",
                "Hul i kunststof o/2-4 mm. Ø o/80-100 mm", 110940);
            await UpsertOperation(session, "AB0EB5A5-932F-491C-BBDE-FAADBF7B3FB1", "050601000008", "050602",
                "Hul i kunststof o/2-4 mm. Ø o/100-125 mm", 135780);
            await UpsertOperation(session, "EB8E78BA-63DC-4230-A835-EA4D3BF7D7E6", "050701000001", "050701", "Hul i letmetal <=2 mm. Ø -5 mm", 11100);
            await UpsertOperation(session, "3C6E1C89-17C6-483C-BF31-9CE50FAA2B48", "050701000002", "050701", "Hul i letmetal <=2 mm. Ø o/5-10 mm",
                21840);
            await UpsertOperation(session, "580D33FC-567B-4824-8B82-CA343DEE399C", "050701000003", "050701", "Hul i letmetal <=2 mm. Ø o/10-20 mm",
                37860);
            await UpsertOperation(session, "224544B8-9B77-402B-A24D-B1C76706C85E", "050701000004", "050701", "Hul i letmetal <=2 mm. Ø o/20-30 mm",
                53820);
            await UpsertOperation(session, "E15715E4-A0F9-4F77-9EF8-11A6806CBF65", "050701000005", "050701", "Hul i letmetal <=2 mm. Ø o/30-40 mm",
                73980);
            await UpsertOperation(session, "595524E6-62B4-46B8-945F-D99362BFF1A7", "050701000006", "050701", "Hul i letmetal <=2 mm. Ø o/40-60 mm",
                93300);
            await UpsertOperation(session, "596D2979-5C7A-48F6-814B-903CE5134991", "050701000007", "050701", "Hul i letmetal <=2 mm. Ø o/60-80 mm",
                133800);
            await UpsertOperation(session, "E195340D-E9DC-462D-A4A6-CDE056A17FCE", "050701000008", "050701", "Hul i letmetal <=2 mm. Ø o/80-100 mm",
                165840);
            await UpsertOperation(session, "703961A3-9021-443B-9F36-749F0600A92D", "050702000001", "050702", "Hul i letmetal o/2-4 mm, Ø -5 mm",
                15240);
            await UpsertOperation(session, "FBC3179C-9BBD-4CC8-B754-8CCB3E1244C5", "050702000002", "050702", "Hul i letmetal o/2-4 mm, Ø o/5-10 mm",
                25200);
            await UpsertOperation(session, "CDC4B282-BDE7-4B1A-8620-9BA3366542B3", "050702000003", "050702", "Hul i letmetal o/2-4 mm, Ø o/10-20 mm",
                47940);
            await UpsertOperation(session, "AD4A6C6F-833A-45D7-980E-A5BB50C11DC5", "050702000004", "050702", "Hul i letmetal o/2-4 mm, Ø o/20-30 mm",
                76800);
            await UpsertOperation(session, "F30D3E48-69C7-465E-A438-012948EFB4DB", "050702000005", "050702", "Hul i letmetal o/2-4 mm, Ø o/30-40 mm",
                93480);
            await UpsertOperation(session, "41DE36C8-082A-41C5-899C-0177C7982100", "050702000006", "050702", "Hul i letmetal o/2-4 mm, Ø o/40-60 mm",
                139020);
            await UpsertOperation(session, "3874463F-323F-47DC-B8BC-32818FAAB3CD", "050702000007", "050702", "Hul i letmetal o/2-4 mm, Ø o/60-80 mm",
                184560);
            await UpsertOperation(session, "A3806EC0-885E-4C63-B2E6-3F63C9965E25", "050702000008", "050702", "Hul i letmetal o/2-4 mm, Ø o/80-100 mm",
                230100);
            await UpsertOperation(session, "29B3325C-5B59-4B7E-BA50-9961AB7BACFB", "050801000001", "050801",
                "Hul i jern t.o.m. 2,5mm, Ø 0 t.o.m. 5 mm", 17040);
            await UpsertOperation(session, "3A59358C-28DB-432B-9F0C-208EAD74A004", "050801000002", "050801",
                "Hul i jern t.o.m. 2,5mm, over Ø 5 t.o.m. 10 mm", 25620);
            await UpsertOperation(session, "E10ADAE8-0D6C-43F7-B7F0-A6214590481C", "050801000003", "050801",
                "Hul i jern t.o.m. 2,5mm, over Ø 10 t.o.m. 20 mm", 41160);
            await UpsertOperation(session, "FB26BE01-8AE1-4357-939B-31D0CF998581", "050801000004", "050801",
                "Hul i jern t.o.m. 2,5mm, over Ø 20 t.o.m. 30 mm", 58080);
            await UpsertOperation(session, "776C2F96-41C6-4A7A-A522-33DB0C688B1C", "050801000005", "050801",
                "Hul i jern t.o.m. 2,5mm, over Ø 30 t.o.m. 40 mm", 62100);
            await UpsertOperation(session, "B873CB7A-0BF1-4F16-9F02-CF085FD8AE7D", "050801000006", "050801",
                "Hul i jern t.o.m. 2,5mm, over Ø 40 t.o.m. 60 mm", 83040);
            await UpsertOperation(session, "1CF58971-EC6C-4E33-ABDA-F2D235B386D7", "050801000007", "050801",
                "Hul i jern t.o.m. 2,5mm, over Ø 60 t.o.m. 80 mm", 95820);
            await UpsertOperation(session, "501C7177-F052-44B6-9445-553DE17DC961", "050801000008", "050801",
                "Hul i jern t.o.m. 2,5mm, over Ø 80 t.o.m. 100 mm", 124980);
            await UpsertOperation(session, "596F2F97-A9A1-4779-9AEF-02970E4EC0D7", "050801000010", "050801", "Klippe hak i stålrigle ell.lign.",
                45840);
            await UpsertOperation(session, "F79CA040-990C-47E4-9B45-86BD9CD71309", "050801000012", "050801", "Firkant hul i jern t.o.m. 100cm2",
                323760);
            await UpsertOperation(session, "C13E6327-0497-4D90-9ABB-D317794697F5", "050802000001", "050802",
                "Hul i jern over 2,5 t.o.m 4,5mm, Ø 0 t.o.m. 5 mm", 41040);
            await UpsertOperation(session, "4BD41F46-2611-4E1F-89CD-F8C93798628D", "050802000002", "050802",
                "Hul i jern over 2,5 t.o.m 4,5mm, over Ø 5 t.o.m. 10 mm", 48960);
            await UpsertOperation(session, "4D0CDBD3-8BB0-4E87-B603-3343EE64A691", "050802000003", "050802",
                "Hul i jern over 2,5 t.o.m 4,5mm, over Ø 10 t.o.m. 20 mm", 64800);
            await UpsertOperation(session, "7037D631-E981-4A3E-A137-A12E128739FE", "050802000004", "050802",
                "Hul i jern over 2,5 t.o.m 4,5mm, over Ø 20 t.o.m. 30 mm", 80640);
            await UpsertOperation(session, "3F25433E-D014-4825-8404-A8439F08E886", "050802000005", "050802",
                "Hul i jern over 2,5 t.o.m 4,5mm, over Ø 30 t.o.m. 40 mm", 96480);
            await UpsertOperation(session, "511370DA-FEEF-4CC0-8D57-10E2AACDCB68", "050802000006", "050802",
                "Hul i jern over 2,5 t.o.m 4,5mm, over Ø 40 t.o.m. 60 mm", 128160);
            await UpsertOperation(session, "92ED04EE-0BF7-4092-B86F-41A7F33B87A5", "050802000007", "050802",
                "Hul i jern over 2,5 t.o.m 4,5mm, over Ø 60 t.o.m. 80 mm", 159840);
            await UpsertOperation(session, "29542DCE-1380-423A-AC06-7F063A657969", "050803000001", "050803",
                "Hul i jern over 4,5 t.o.m 8,5mm, Ø 0 t.o.m. 5 mm", 44700);
            await UpsertOperation(session, "FB54FC5B-F6C9-466F-816A-AEF7D5D894CA", "050803000002", "050803",
                "Hul i jern over 4,5 t.o.m 8,5mm, over Ø 5 t.o.m. 10 mm", 84900);
            await UpsertOperation(session, "3F65D912-7F25-4078-B7A8-51895E77A33D", "050803000003", "050803",
                "Hul i jern over 4,5 t.o.m 8,5mm, over Ø 10 t.o.m. 20 mm", 160980);
            await UpsertOperation(session, "2F91EAE7-2E28-47E6-9CE6-454D91CDFBE1", "050803000004", "050803",
                "Hul i jern over 4,5 t.o.m 8,5mm, over Ø 20 t.o.m. 30 mm", 238620);
            await UpsertOperation(session, "BC4EF1D5-53B1-48DB-A9FD-2B3458EA2FDF", "050803000005", "050803",
                "Hul i jern over 4,5 t.o.m 8,5mm, over Ø 30 t.o.m. 40 mm", 315900);
            await UpsertOperation(session, "D4FE19D6-72A6-4175-9C4C-DF708B72BB36", "050803000006", "050803",
                "Hul i jern over 4,5 t.o.m 8,5mm, over Ø 40 t.o.m. 60 mm", 470520);
            await UpsertOperation(session, "FEC6E2B2-3F1A-474E-9786-0AB55E6C8B6A", "050803000007", "050803",
                "Hul i jern over 4,5 t.o.m 8,5mm, over Ø 60 t.o.m. 80 mm", 625200);
            await UpsertOperation(session, "B7FCE168-8813-49B9-8D21-25D24CE5AD93", "050901000001", "050901",
                "Hul i porøse materialer <=3cm tyk, Ø-20mm", 12720);
            await UpsertOperation(session, "42B1079F-DC06-425A-B5D8-FB8A26078768", "050901000002", "050901",
                "Hul i porøse materialer <=3cm tyk, Ø o/20-40mm", 29700);
            await UpsertOperation(session, "E1DF4C13-9FEF-4FCC-BAC7-84C265E1B93E", "050901000003", "050901",
                "Hul i porøse materialer <=3cm tyk, Ø o/40-60mm", 41700);
            await UpsertOperation(session, "5070DCAD-891C-4D4B-A3F0-7E69F01B5DC5", "050901000004", "050901",
                "Hul i porøse materialer <=3cm tyk, Ø o/60-100mm", 43620);
            await UpsertOperation(session, "17C4DF42-F3E1-4571-9573-08B4A1CCB1A9", "050901000005", "050901",
                "Hul i porøse materialer <=3cm tyk, Ø o/100-150mm", 44760);
            await UpsertOperation(session, "B376DF21-F800-479E-93BF-07800B318569", "050901000006", "050901",
                "Hul i porøse materialer <=3cm tyk, Ø o/150-200mm", 54540);
            await UpsertOperation(session, "D9D99BCC-63AA-4B61-9CCF-E054FE46320E", "050901000007", "050901",
                "Hul i porøse materialer <=3cm tyk, Ø o/200-300mm", 65340);
            await UpsertOperation(session, "03B3FF7A-A48E-449C-969E-6B87DC4052D1", "050901000008", "050901",
                "Hul i porøse materialer <=3cm tyk, Ø o/300-400mm", 79080);
            await UpsertOperation(session, "C0093307-2E0A-49A4-A707-5A32A3D6D055", "050901000009", "050901",
                "Hul i porøse materialer <=3cm tyk, Ø o/400-500mm", 90480);
            await UpsertOperation(session, "05F637BF-BB9D-45F1-A218-797A6452A825", "051102000002", "052102",
                "Tætne gennemføring t.o.m. 100x100mm m.fugemasse. Pr. tætning", 49740);
            await UpsertOperation(session, "0B97495C-B0E3-42E5-BAD3-10077DABAF80", "052101000001", "052101",
                "Tætne gennemføring t.o.m. 50x50mm m.specialtape. Pr. tætning", 166140);
            await UpsertOperation(session, "4314D4FC-4866-4180-B26F-FD8B3C73AA4F", "052101000002", "052101",
                "Tætne gennemføring t.o.m. 50x50mm/ø32 m.fugemasse. Pr. tætning", 39540);
            await UpsertOperation(session, "CADA959A-4ED9-47D0-A06E-A33A5F8DEF25", "070000000010", "070000",
                "Tilpasse og opsætte/fastgøre trempel i form af forskallingsbræt ell.l.", 190680);
            await UpsertOperation(session, "BA20E989-245E-4267-98F3-DE574854BF32", "070000000011", "070000",
                "Oplægge færdig fremstillet trempel ell. l. u/ fastgørelse", 29760);
            await UpsertOperation(session, "6D86B501-3677-4E2D-9FE7-555FC32F82BD", "070000000012", "070000", "Loftplade nedtaget ell. oplagt", 38340);
            await UpsertOperation(session, "888A3B5B-AD55-4F7B-A16C-06FB47BDBAB6", "070000000201", "070000",
                "Fjerne klods i udsparring t.o.m. 20x20x20 cm", 74580);
            await UpsertOperation(session, "1EEE4B21-81C9-4295-97CC-45C6A11B17A0", "070000000205", "070000",
                "Fjerne isolering i hul for forfradåse ell.lign.", 54060);
            await UpsertOperation(session, "09331D4B-C141-47FE-B5A0-5C0B00B6E842", "090000000001", "090000",
                "Fræse riller i letbeton for rør ell. kabel t.o.m. 20mm. Pr.m", 30180);
            await UpsertOperation(session, "2ADF3008-F041-43DB-8214-1F1A2E4F9FAC", "090000000002", "090000",
                "Skære riller i letbeton incl. udhugn. for rør ell. kabel t.o.m. 20mm", 121020);
            await UpsertOperation(session, "431DE92D-0591-4FC0-9E5C-FB0B1DED01C0", "090000000003", "090000",
                "Fræse rille i tegl for rør ell. kabel t.o.m. 20mm", 49560);
            await UpsertOperation(session, "511841E4-D17A-4FB1-94BA-3590EBC098EF", "090000000004", "090000",
                "Skære riller i tegl incl. udhugn. for rør ell. kabel t.o.m. 20mm", 287460);
            await UpsertOperation(session, "A324E58B-5F30-450E-A0DD-9E6EEFA1A104", "090000000012", "090000",
                "Skære riller i beton incl. udhugn. for rør ell. kabel t.o.m. 20mm", 1105200);
            await UpsertOperation(session, "61E34D5E-0E1F-4E14-87EF-F312B1408D3E", "090000000020", "090000",
                "Cementere og udfylde rille til færdig puds  t.o.m. 25mm bred. Pr. m.", 80700);
            await UpsertOperation(session, "B097CC54-A44D-4DF0-A282-672DA7C93881", "100210000001", "000210", "Fastgjort  med strips", 44340);
            await UpsertOperation(session, "34EA1ED8-A03F-436D-A10C-0E5F9A5D4E28", "100210000006", "000210", "Fastgjort  med polbøjler/X-strips",
                61500);
            await UpsertOperation(session, "DC708BE5-AE4F-4ADD-820C-13AE27C8E5F1", "100210000007", "000210", "Fastgjort på træ med clips", 11220);
            await UpsertOperation(session, "58E6B8CA-BF42-420F-9D48-8FD13D51419B", "100210000008", "000210", "Fastgjort på træ med bøjler", 32280);
            await UpsertOperation(session, "F879B707-7F7D-40DD-83AC-9CAE7783F83A", "100210000009", "000210", "Fastgjort på tegl med clips", 24420);
            await UpsertOperation(session, "4AC61E73-B011-4E54-AA97-AF1325A6C6FD", "100210000010", "000210", "Fastgjort på tegl med bøjler", 72060);
            await UpsertOperation(session, "DE1D283F-647A-4A30-B19D-E4951F233C67", "100210000011", "000210", "Fastgjort på beton med clips", 24000);
            await UpsertOperation(session, "79BC7B37-864B-47E2-A25B-4E3EB138DC7B", "100210000012", "000210", "Fastgjort på beton med bøjler", 71280);
            await UpsertOperation(session, "9B4071F8-4D13-45BD-B3B4-D1C4CE113D97", "100210000014", "000210",
                "Fastgjort på jern t.o.m. 2mm,  med bøjler", 0);
            await UpsertOperation(session, "471C7022-0741-45AE-915B-D14605A28C67", "100210000015", "000210",
                "Fastgjort på jern over 2mm,  med bøjler, excl. evt. gevind", 32280);
            await UpsertOperation(session, "3D39B84B-3869-488D-9B6F-F9CF9164B699", "120100000001", "120100",
                "Opsætning af fordelingsanlæg <=0,05m2 D>40-280mm", 239820);
            await UpsertOperation(session, "249C4AE8-0508-4F8F-A7B6-C51949809952", "120100000002", "120100",
                "Opsætning af fordelingsanlæg >0,05-0,1m2 D>40-280mm", 251040);
            await UpsertOperation(session, "917DA964-96DA-48C7-A832-C1DCBB34D491", "120100000004", "120100",
                "Opsætning af fordelingsanlæg >0,1-0,2m2 D>40-280mm", 328080);
            await UpsertOperation(session, "40648C1F-0723-44F7-BA78-E7BE63AB0AA9", "120100000005", "120100",
                "Opsætning af fordelingsanlæg >0,2-0,4m2 D>40-280mm", 318480);
            await UpsertOperation(session, "0163FE68-1754-4C67-90D5-E2023F0F45FC", "120100000006", "120100",
                "Opsætning af fordelingsanlæg >0,4-0,7m2 D>40-280mm", 392580);
            await UpsertOperation(session, "7A7E8485-A3AF-4D08-817C-79130F4FDEA9", "120100000007", "120100",
                "Opsætning af fordelingsanlæg >0,7-1,0m2 D>40-280mm", 453420);
            await UpsertOperation(session, "0E2CFEEE-04FE-44C3-9C82-9E6ED1F35146", "120100000008", "120100",
                "Opsætning af fordelingsanlæg >1,0-1,5m2 D>40-280mm", 541080);
            await UpsertOperation(session, "23397B1F-5E67-4CEC-BA16-9781249FB364", "120100000009", "120100",
                "Opsætning af fordelingsanlæg >1,5-2,0m2 D>40-280mm", 678360);
            await UpsertOperation(session, "D71AEC2F-9E59-4FB9-9EA3-FD9B668B0058", "120100000010", "120100",
                "Opsætning af fordelingsanlæg >2,0-3,0m2 D>40-280mm", 903240);
            await UpsertOperation(session, "B1756C71-BBF5-4CA1-9AE0-33821E012CB0", "120100000011", "120100",
                "Opsætning af fordelingsanlæg >3,0-4,0m2 D>40-280mm", 1135740);
            await UpsertOperation(session, "34E39747-B8A0-4682-9812-7AE4BFD646D4", "120100000012", "120100",
                "Opsætning af fordelingsanlæg >4,0-6,0m2 D>40-280mm", 1577940);
            await UpsertOperation(session, "D957CA75-473D-4C94-8D34-EE983DE7F3FE", "120150000002", "120150",
                "Opsætning af fordelingsanlæg >0,05-0,1m2 D>280-450mm", 320640);
            await UpsertOperation(session, "E9A949D8-8C7C-4C8D-8D44-64C0C6FF9B28", "120150000003", "120150",
                "Opsætning af fordelingsanlæg >0,1-0,2m2 D>280-450mm", 349920);
            await UpsertOperation(session, "29DB746E-B3DB-472F-824C-CB57176057A4", "120150000004", "120150",
                "Opsætning af fordelingsanlæg >0,2-0,4m2 D>280-450mm", 408540);
            await UpsertOperation(session, "16680580-CB53-4F8F-A805-1916F69DAB2A", "120150000005", "120150",
                "Opsætning af fordelingsanlæg >0,4-0,7m2 D>280-450mm", 496500);
            await UpsertOperation(session, "7828BB1D-86D6-46BA-8DCA-15C2955E23B1", "120150000006", "120150",
                "Opsætning af fordelingsanlæg >0,7-1,0m2 D>280-450mm", 584400);
            await UpsertOperation(session, "508B6950-9985-40E5-8F90-B85B23B6FE2C", "120150000007", "120150",
                "Opsætning af fordelingsanlæg >1,0-1,5m2 D>280-450mm", 730980);
            await UpsertOperation(session, "B58C1554-5A46-4BB3-8E36-F80B20B41398", "120150000008", "120150",
                "Opsætning af fordelingsanlæg >1,5-2,0m2 D>280-450mm", 877560);
            await UpsertOperation(session, "3F4BA96C-CCB4-493F-B5A4-37873247DE00", "120150000009", "120150",
                "Opsætning af fordelingsanlæg >2,0-3,0m2 D>280-450mm", 1170660);
            await UpsertOperation(session, "47861B5C-8BB7-4512-82D8-F87277446B71", "120150000010", "120150",
                "Opsætning af fordelingsanlæg >3,0-4,0m2 D>280-450mm", 1463760);
            await UpsertOperation(session, "926C96EA-3999-4E3F-AEFA-CF4B8EA7F001", "120150000011", "120150",
                "Opsætning af fordelingsanlæg >4,0-6,0m2 D>280-450mm", 2050020);
            await UpsertOperation(session, "2EE6EAA0-47B4-4452-97A3-B1F0E85B42B8", "120200000001", "120200", "Demont. låge t.o.m. 0,1m2", 33300);
            await UpsertOperation(session, "27B54008-970C-4951-A248-1AB597C680E3", "120200000002", "120200", "Montere låge t.o.m. 0,1m2", 40140);
            await UpsertOperation(session, "DEB0F8CB-9C3D-4F2A-9087-1E4A166F8E9E", "120200000003", "120200", "Demont. låge over 0,1 t.o.m. 0,25m2",
                38700);
            await UpsertOperation(session, "D372B993-7E22-4873-8EC2-10C7EB3BB218", "120200000004", "120200", "Montere låge over 0,1 t.o.m. 0,25m2",
                49440);
            await UpsertOperation(session, "4AB211AA-3079-4D0A-82E4-35B926FFE021", "120200000005", "120200", "Demont. låge over 0,25 t.o.m. 0,55m2",
                36960);
            await UpsertOperation(session, "20767551-9C56-4A4F-BB1A-887AB6505CF1", "120200000006", "120200", "Montere låge over 0,25 t.o.m. 0,55m2",
                61260);
            await UpsertOperation(session, "5742E80B-C4A1-4DC5-8297-5B1507B0466E", "120200000007", "120200", "Demont. låge over 0,55 t.o.m. 1,15m2",
                41880);
            await UpsertOperation(session, "66E4074A-3BDA-4509-BBE4-4334E9AFDFF6", "120200000008", "120200", "Montere låge over 0,55 t.o.m. 1,15m2",
                84840);
            await UpsertOperation(session, "DB706D8D-34E3-477A-9368-94B5339E0615", "120201000103", "120201", "Montere flange, med skruer", 320220);
            await UpsertOperation(session, "F944D653-4B50-4A43-9757-EF6BE684AB0F", "120201000105", "120201", "Isætte montagebeslag", 39480);
            await UpsertOperation(session, "6A8877EC-DA60-43A8-9AA8-25454B18E92E", "120201000107", "120201",
                "Påskrue/-nitte dinskinne indtil 0,5m længde", 93660);
            await UpsertOperation(session, "4BA952DC-E40A-46AE-A9F8-39EB69D6782F", "120201000109", "120201", "Montere hængselsæt", 133020);
            await UpsertOperation(session, "737A7C55-F5EC-49E8-A8EE-FAE52874FEA1", "120201000110", "120201", "Montere opsætningsbeslag", 68640);
            await UpsertOperation(session, "38D12EAE-025A-4C36-AF06-D778A080D0A1", "120201000111", "120201", "Montere / afmontere afdækning u/skruer",
                35160);
            await UpsertOperation(session, "62019484-FF3C-4447-9F8B-FDC116B45C41", "120201000112", "120201", "Montere / afmontere afdækning m/skruer",
                47160);
            await UpsertOperation(session, "6D27A167-2F36-4F9E-AFE5-F8AF17F89226", "120201000117", "120201", "Isætte travers. Klemme, clipse ell.l",
                72180);
            await UpsertOperation(session, "061D8E64-7ABC-4D2F-9AF8-7C180AAD8CFB", "120201000120", "120201", "Isætte travers. m/skruer", 111060);
            await UpsertOperation(session, "8262DD79-9EC4-40AF-A547-534FF5CF7C15", "120201000121", "120201", "Demontere travers, m/skruer", 148680);
            await UpsertOperation(session, "EBE9C658-D8E1-4561-94F3-E72DF1E416FB", "120201000124", "120201", "Tætne kabelindføring med fugemasse",
                125160);
            await UpsertOperation(session, "FE937A3D-6B4B-4F7B-864D-0A24A21F2657", "120300000000", "120300", "¤ FASTGJORT I TRÆ ¤", 0);
            await UpsertOperation(session, "444C1988-6FBD-4A6F-8DEA-B5B8FE7E6C70", "120300000001", "120300", "På træ med skrue str. t.o.m. 5", 16140);
            await UpsertOperation(session, "1CE90884-6D0F-4E9A-A792-212622A9007F", "120300000002", "120300",
                "På træ med skrue over str. 5 t.o.m. 7,5", 45960);
            await UpsertOperation(session, "693CD368-5A14-4866-91DC-0E6774AC4727", "120300000003", "120300",
                "På træ med skrue over str. 7,5 t.o.m.10", 46800);
            await UpsertOperation(session, "010C84EB-5916-4530-B36B-EBF426284C3F", "120300000010", "120300", "¤ FASTGJORT I TEGL/LETBETON ¤", 0);
            await UpsertOperation(session, "91F15A29-0CBE-464E-B047-4B9323A62C0F", "120300000011", "120300",
                "På tegl/letbeton med skrue str. t.o.m. 5", 36060);
            await UpsertOperation(session, "91235F49-0165-4332-9E81-5E0EB28B4D07", "120300000012", "120300",
                "På tegl/letbeton med skrue over str. 5 t.o.m. 7,5", 66720);
            await UpsertOperation(session, "74621A51-1038-436F-8418-5B2636F253BC", "120300000013", "120300",
                "På tegl/letbeton med skrue over str. 7,5 t.o.m.10", 77100);
            await UpsertOperation(session, "3FC6BA80-31F0-4A21-A9FE-7D0FD1B9204F", "120300000015", "120300",
                "På tegl/letbeton med eksp.bolt ell.lign. over str. 7,5 t.o.m.10", 92820);
            await UpsertOperation(session, "B66D41A3-C650-4BC0-88A6-B810454930B5", "120300000020", "120300", "¤ FASTGJORT I BETON ¤", 0);
            await UpsertOperation(session, "B789188D-888B-4035-8125-778D4EDD660A", "120300000021", "120300", "På beton med skrue str. t.o.m. 5",
                35700);
            await UpsertOperation(session, "1B2B56AC-A807-4C67-98DD-30CA65C52C5F", "120300000022", "120300",
                "På beton med skrue over str. 5 t.o.m. 7,5", 88500);
            await UpsertOperation(session, "66CE3AA4-4553-46ED-8DBB-5A2BF499D720", "120300000023", "120300",
                "På beton med betonanker ell.lign. str. 8 t.o.m. 10mm", 117420);
            await UpsertOperation(session, "1ABA0E01-D1FA-4151-899C-9C18874C3857", "120300000024", "120300",
                "På beton med slagskrue over str. 7,5 t.o.m.10", 58560);
            await UpsertOperation(session, "0737A446-F26A-4AD0-8F8B-C54B0193921D", "120300000030", "120300",
                "¤ FASTGJORT I JERN UNDER 2,5MM TYKKELSE EXCL. SKÆRING AF GEVIND ¤", 0);
            await UpsertOperation(session, "4F5A4F91-8B0D-4CDB-B768-8B2B582F4958", "120300000031", "120300",
                "På jern t.o.m. 2,5mm tyk med skrue&møtrik str. t.o.m. 5", 55800);
            await UpsertOperation(session, "7A6D8609-6142-4CCA-BFAD-4FF117141590", "120300000032", "120300",
                "På jern t.o.m. 2,5mm tyk med bolt&møtrik over str. 5 t.o.m.7,5", 64920);
            await UpsertOperation(session, "B96026F8-8EB7-4040-A501-326B3B131A70", "120300000033", "120300",
                "På jern t.o.m. 2,5mm tyk med bolt&møtrik over str. 7,5 t.o.m.10", 65100);
            await UpsertOperation(session, "5D90BFEF-0E0B-4073-B239-27D90E5E16A1", "120300000040", "120300",
                "¤ FASTGJORT I JERN OVER 2,5MM TYKKELSE EXCL. SKÆRING AF GEVIND ¤", 0);
            await UpsertOperation(session, "66FF91C8-5EEA-463D-AA54-5D34A2A264E0", "120300000041", "120300",
                "På jern over 2,5mm tyk med skrue&møtrik str. t.o.m. 5", 80760);
            await UpsertOperation(session, "F9E7BA8D-2F1A-4D22-B682-C2625E9E726E", "120300000042", "120300",
                "På jern over 2,5mm tyk med bolt&møtrik over str. 5 t.o.m.7,5", 104220);
            await UpsertOperation(session, "E53AD26A-60DD-4B66-B3D8-62450CECF2A6", "120300000043", "120300",
                "På jern over 2,5mm tyk med bolt&møtrik over str. 7,5 t.o.m.10", 111720);
            await UpsertOperation(session, "00DB42BB-DE9B-435E-BF88-93B2C01AE861", "120501000002", "120501",
                "Ledn. forb. 2-5x1,5-2,5# CU i tavler, kasser og fordelingsanlæg", 254700);
            await UpsertOperation(session, "BBDE8551-1E14-4CB9-930F-43D49CE7197D", "120501000003", "120501",
                "Ledn. forb. 2-5x4-6# CU i tavler, kasser og fordelingsanlæg", 366960);
            await UpsertOperation(session, "6E547D9F-3DE4-46B6-9748-F070B1DC99EC", "120501000004", "120501",
                "Ledn. forb. 2-5x10-16# CU i tavler, kasser og fordelingsanlæg", 612120);
            await UpsertOperation(session, "B01B273C-9725-4D65-9326-B594B52A1221", "120501000005", "120501",
                "Ledn. forb. 2-5x25-35# CU i tavler, kasser og fordelingsanlæg", 952620);
            await UpsertOperation(session, "95C86E6C-9001-431A-A811-971A693B2FFC", "120501000006", "120501",
                "Ledn. forb. 2-5x50# CU i tavler, kasser og fordelingsanlæg", 1026600);
            await UpsertOperation(session, "C2403F60-6BB8-4598-B3A1-2B51A6CFD30D", "120501000007", "120501",
                "Ledn. forb. 2-5x70# CU i tavler, kasser og fordelingsanlæg", 1307160);
            await UpsertOperation(session, "4791C755-DEDF-40E9-A397-04C2297AF7E6", "120501000008", "120501",
                "Ledn. forb. 2-5x95# CU i tavler, kasser og fordelingsanlæg", 1633140);
            await UpsertOperation(session, "F0014129-F104-4C1C-B8D6-BA54B718480C", "120501000009", "120501",
                "Ledn. forb. 2-5x120# CU i tavler, kasser og fordelingsanlæg", 1959180);
            await UpsertOperation(session, "40EB6321-87E3-4ECD-B37A-79CFB2605DBF", "120501000010", "120501",
                "Ledn. forb. 2-5x150# CU i tavler, kasser og fordelingsanlæg", 2179620);
            await UpsertOperation(session, "16C3589F-736A-4226-BF7E-5B062F6D2CBC", "120501000011", "120501",
                "Ledn. forb. 2-5x185# CU i tavler, kasser og fordelingsanlæg", 2806860);
            await UpsertOperation(session, "C8C4A678-3D18-4B14-A2A3-4FC10B438CA8", "120501000012", "120501",
                "Ledn. forb. 2-5x240# CU i tavler, kasser og fordelingsanlæg", 2898120);
            await UpsertOperation(session, "86F4D8E7-02D9-4510-A3B3-ACB195D58143", "120501000014", "120501",
                "Ledn. forb. 1x<=1# CU i tavler, kasser og fordelingsanlæg", 35100);
            await UpsertOperation(session, "17EFE6D3-CE60-4C8A-A487-40FDA37E47D0", "120501000015", "120501",
                "Ledn. forb. 1x1,5-2,5# CU i tavler, kasser og fordelingsanlæg", 77520);
            await UpsertOperation(session, "1400E824-F8DF-4A64-B9B5-F90A93895276", "120501000016", "120501",
                "Ledn. forb. 1x4-6# CU i tavler, kasser og fordelingsanlæg", 129840);
            await UpsertOperation(session, "C5CCFD48-165C-4D09-96DD-BEFB6CEE4064", "120501000017", "120501",
                "Ledn. forb. 1x10-16# CU i tavler, kasser og fordelingsanlæg", 166080);
            await UpsertOperation(session, "8AA1D412-20D8-41AC-8CAF-8DF05021FBF5", "120501000018", "120501",
                "Ledn. forb. 1x25-35# CU i tavler, kasser og fordelingsanlæg", 186900);
            await UpsertOperation(session, "6264BFF5-9229-4371-8724-984B73A9CE47", "120501000019", "120501",
                "Ledn. forb. 1x50# CU i tavler, kasser og fordelingsanlæg", 185160);
            await UpsertOperation(session, "7B4D7252-1DAE-4F7F-85A1-8E66F7660DE1", "120501000020", "120501",
                "Ledn. forb. 1x70# CU i tavler, kasser og fordelingsanlæg", 195960);
            await UpsertOperation(session, "3FB6F08A-E591-40CA-AA12-3FBAD6F0124C", "120501000021", "120501",
                "Ledn. forb. 1x95# CU i tavler, kasser og fordelingsanlæg", 263160);
            await UpsertOperation(session, "A794A635-CFE9-4FA6-BFC6-115DF0F3C97C", "120501000022", "120501",
                "Ledn. forb. 1x120# CU i tavler, kasser og fordelingsanlæg", 307080);
            await UpsertOperation(session, "718732A1-A5FE-41A1-942C-1C880D25D8B7", "120501000023", "120501",
                "Ledn. forb. 1x150# CU i tavler, kasser og fordelingsanlæg", 359760);
            await UpsertOperation(session, "EC9C541F-B585-4364-9A75-F46D18D1327D", "120501000024", "120501",
                "Ledn. forb. 1x185# CU i tavler, kasser og fordelingsanlæg", 421260);
            await UpsertOperation(session, "5789977D-DA41-4960-B154-F5297F85265A", "120501000025", "120501",
                "Ledn. forb. 1x240# CU i tavler, kasser og fordelingsanlæg", 517860);
            await UpsertOperation(session, "6E781F0D-0FBF-4412-8BC1-E5FEAB0D9DB7", "120501000026", "120501",
                "Ledn. forb. 1x300# CU i tavler, kasser og fordelingsanlæg", 623280);
            await UpsertOperation(session, "9F7195FF-4F59-476F-A590-B804AF6521EB", "120501000027", "120501",
                "Ledn. forb. 1x400# CU i tavler, kasser og fordelingsanlæg", 798960);
            await UpsertOperation(session, "41D3D2E0-287A-4D02-B6DA-A85148059C77", "120501000028", "120501",
                "Ledn. forb. 1x500# CU i tavler, kasser og fordelingsanlæg", 974700);
            await UpsertOperation(session, "6814E991-210F-4795-ABD1-2790DA00A53B", "120501000029", "120501",
                "Ledn. forb. 1x 625# CU i tavler, kasser og fordelingsanlæg", 1194300);
            await UpsertOperation(session, "370FFA7D-21FF-4DE0-903E-BF6D43D9A77D", "120501000030", "120501",
                "Ledn. forb. 1x 1000# CU i tavler, kasser og fordelingsanlæg", 1853100);
            await UpsertOperation(session, "00232037-49B8-495D-AD70-CFBA58A456F3", "120501000035", "120501",
                "Ledn.forb. svagstrøm 1x0,2-1# Pr. leder", 68640);
            await UpsertOperation(session, "4817C01A-C306-409E-96C9-10CB43CF1FE2", "120501000042", "120501",
                "Rundpresse 120-240# CU med hydr. presseværktøj", 57360);
            await UpsertOperation(session, "A08ACFEC-2E10-49B4-9C07-23DECFB1796C", "120501000051", "120501",
                "Sløjfeledn. 1,5-6# op til 0,5m fremstillet og isat", 59640);
            await UpsertOperation(session, "03019CD9-FF98-4351-BECC-1E5971B3B650", "120501000052", "120501",
                "Sløjfeledn. 0,2-1# op til 0,5m. Fremstillet og isat", 88320);
            await UpsertOperation(session, "089B66D8-B1DE-4DF9-AF77-1F707520029F", "120502000001", "120502",
                "Ledn.forb. 2-5x10-16#AL-S i tavler, kasser og fordelingsanlæg", 684300);
            await UpsertOperation(session, "39202427-C07C-494C-AD24-2D84E725F452", "120502000002", "120502",
                "Ledn.forb. 2-5x25-35# AL-S i tavler, kasser og fordelingsanlæg", 834780);
            await UpsertOperation(session, "2796B954-54B2-4E47-8854-0C22F82C53A0", "120502000003", "120502",
                "Ledn.forb. 2-5x50# AL-S i tavler, kasser og fordelingsanlæg", 1148040);
            await UpsertOperation(session, "0E507929-4AA6-41E4-A34A-587C7595862A", "120502000004", "120502",
                "Ledn.forb. 2-5x70# AL-S i tavler, kasser og fordelingsanlæg", 1227960);
            await UpsertOperation(session, "0F25D455-9528-4A01-AE59-D3BF7304B706", "120502000005", "120502",
                "Ledn.forb. 2-5x95# AL-S i tavler, kasser og fordelingsanlæg", 1307820);
            await UpsertOperation(session, "3B4164B7-7FA6-4B51-AD93-1BFF05FBE404", "120502000007", "120502",
                "Ledn.forb. 2-5x150# AL-S i tavler, kasser og fordelingsanlæg", 1359720);
            await UpsertOperation(session, "38D0E6B2-E706-4AEB-A029-719CB0514B8D", "120502000008", "120502",
                "Ledn.forb. 2-5x185# AL-S i tavler, kasser og fordelingsanlæg", 1869000);
            await UpsertOperation(session, "2F0114E0-BA2A-4F68-B5D1-1CE420CC2D85", "120502000009", "120502",
                "Ledn.forb. 2-5x240# AL-S i tavler, kasser og fordelingsanlæg", 1967100);
            await UpsertOperation(session, "BE543FE9-B7D3-4C30-8081-E851AAC2904C", "120502000010", "120502",
                "Ledn.forb. 2-5x300# AL-S i tavler, kasser og fordelingsanlæg", 2451000);
            await UpsertOperation(session, "214CEB4B-84C9-4C8C-9215-081932922BAF", "120502000011", "120502",
                "Ledn.forb. 1x16# AL-S i tavler, kasser og fordelingsanlæg", 154680);
            await UpsertOperation(session, "A888F73C-76B1-44D7-9323-AB37214A094B", "120502000012", "120502",
                "Ledn. forb. 1X25-35# AL-S i tavler, kasser og fordelingsanlæg", 173580);
            await UpsertOperation(session, "A8FF2541-C1C2-4B58-8B12-D7CECE7AF1EF", "120502000013", "120502",
                "Ledn. forb. 1X50# AL-S i tavler, kasser og fordelingsanlæg", 188520);
            await UpsertOperation(session, "88AE48BF-08FB-4B09-8B3B-9DB97AEDF4D3", "120502000014", "120502",
                "Ledn. forb. 1x70# AL-S i tavler, kasser og fordelingsanlæg", 195000);
            await UpsertOperation(session, "AEA8A69F-C59E-4538-9D2A-B6DA12D6EBCD", "120502000015", "120502",
                "Ledn. forb. 1X95# AL-S i tavler, kasser og fordelingsanlæg", 233220);
            await UpsertOperation(session, "8B1B6CA8-708D-4768-BD7A-B7EDE3B54F2C", "120502000016", "120502",
                "Ledn. forb. 1X120# AL-S i tavler, kasser og fordelingsanlæg", 258060);
            await UpsertOperation(session, "FD02FF77-32E8-4542-A115-17D9C0C69B4A", "120502000017", "120502",
                "Ledn. forb. 1x150# AL-S i tavler, kasser og fordelingsanlæg", 291960);
            await UpsertOperation(session, "AAC499ED-0E69-40B0-B4D8-D5360E19D2B7", "120502000018", "120502",
                "Ledn. forb. 1X185# AL-S i tavler, kasser og fordelingsanlæg", 322680);
            await UpsertOperation(session, "818D9BC4-2F7B-4FE0-86DB-ABA35E1E3AD1", "120502000019", "120502",
                "Ledn. forb. 1X240# AL-S i tavler, kasser og fordelingsanlæg", 377340);
            await UpsertOperation(session, "6D23694E-24F9-4F2C-85BA-C5BA2C1D26E9", "120502000020", "120502",
                "Ledn. forb. 1X300# AL-S i tavler, kasser og fordelingsanlæg", 436980);
            await UpsertOperation(session, "D4D059D7-984B-432D-9EFB-F6A1CF7F254F", "120502000021", "120502",
                "Ledn. forb. 1X400# AL-S i tavler, kasser og fordelingsanlæg", 536400);
            await UpsertOperation(session, "C5D2262B-105D-4149-A337-59ABF37C344A", "120502000022", "120502",
                "Ledn. forb. 1X500# AL-S i tavler, kasser og fordelingsanlæg", 635820);
            await UpsertOperation(session, "BF321F7F-A8D6-47EA-84E9-DD9EB2FB0363", "120502000023", "120502",
                "Ledn. forb. 1X625# AL-S i tavler, kasser og fordelingsanlæg", 760080);
            await UpsertOperation(session, "7F8547F0-181A-4143-8F79-96E62006B62D", "120502000024", "120502",
                "Ledn. forb. 1X1000# AL-S i tavler, kasser og fordelingsanlæg", 1132800);
            await UpsertOperation(session, "79BC454F-64A6-4CD8-B207-EC9626BC6935", "120502000025", "120502",
                "Rundpresse 35-95# AL-S med hydr. presseværktøj", 28380);
            await UpsertOperation(session, "8DC48648-010D-49C1-958E-CC7AE2B9599B", "120502000026", "120502",
                "Rundpresse 120-240# AL-S med hydr. presseværktøj", 65340);
            await UpsertOperation(session, "CA30BF70-89B4-4545-83C3-E99DCDBF4B02", "120502000029", "120502",
                "Rundpresse 120-240# AL-S med håndværktøj", 153000);
            await UpsertOperation(session, "538D5A76-ED06-44DE-9BA9-D56831326775", "120502000032", "120502",
                "Rundpresse 120-240# AL-M med håndværktøj", 0);
            await UpsertOperation(session, "2F0F6056-B177-4CC3-B6BA-CBF6A4ECA1F9", "120502000206", "120502",
                "*Ledn.forb. 2-5x120# AL-S i tavler, kasser og fordelingsanlæg", 1597740);
            await UpsertOperation(session, "045CD79C-3FF9-4BFD-808B-03960C20FA37", "120503000101", "120503",
                "Ledn. forb. 2-5x10-16# AL-M i tavler, kasser og fordelingsanlæg", 585840);
            await UpsertOperation(session, "FD15077E-F44B-40A5-9AD9-F18D4EE7107E", "120503000102", "120503",
                "Ledn. forb. 2-5x25-35# AL-M i tavler, kasser og fordelingsanlæg", 799260);
            await UpsertOperation(session, "46902052-6FE1-4C08-8F66-9707A4857007", "120503000103", "120503",
                "Ledn. forb. 2-5x50# AL-M i tavler, kasser og fordelingsanlæg", 953040);
            await UpsertOperation(session, "DDE110A2-413B-4DE4-A1B8-90F35A8D93CC", "120503000104", "120503",
                "Ledn. forb. 2-5x70# AL-M i tavler, kasser og fordelingsanlæg", 1110720);
            await UpsertOperation(session, "39118384-0E15-4011-BD7F-473DAA4ADD46", "120503000105", "120503",
                "Ledn. forb. 2-5x95# AL-M i tavler, kasser og fordelingsanlæg", 1307820);
            await UpsertOperation(session, "527162C1-B180-4E7E-96D7-45C8EA3A85C9", "120503000106", "120503",
                "Ledn. forb. 2-5x120# AL-M i tavler, kasser og fordelingsanlæg", 1504920);
            await UpsertOperation(session, "0C4FBA43-4790-4816-8167-8229C7E97CB9", "120503000107", "120503",
                "Ledn. forb. 2-5x150# AL-M i tavler, kasser og fordelingsanlæg", 1741440);
            await UpsertOperation(session, "4B248423-6F82-4267-BE92-BDA1F2413B04", "120503000108", "120503",
                "Ledn. forb. 2-5x185# AL-M i tavler, kasser og fordelingsanlæg", 1869000);
            await UpsertOperation(session, "72C44442-E235-43DF-AE3E-F2ADAC63AFCC", "120503000109", "120503",
                "Ledn. forb. 2-5x240# AL-M i tavler, kasser og fordelingsanlæg", 2072580);
            await UpsertOperation(session, "921D85C6-3F59-4A6B-915E-171D2D3624AC", "120503000110", "120503",
                "Ledn. forb. 2-5x300# AL-M i tavler, kasser og fordelingsanlæg", 2451000);
            await UpsertOperation(session, "3CA85243-50E6-4A1A-A9B4-3A894E61781A", "120503000111", "120503",
                "Ledn. forb.1 x 16# AL-M i tavler, kasser og fordelingsanlæg", 154680);
            await UpsertOperation(session, "97B94087-9B61-496B-AE7A-CC80C528E942", "120503000112", "120503",
                "Ledn. forb.1 x 25-35# AL-M i tavler, kasser og fordelingsanlæg", 182880);
            await UpsertOperation(session, "82424842-AAB5-4F02-9B27-97AA7C0DD4CE", "120503000113", "120503",
                "Ledn. forb.1 x 50# AL-M i tavler, kasser og fordelingsanlæg", 188460);
            await UpsertOperation(session, "FE1D4CAD-1A07-482A-8DDB-D70BF1A93F83", "120503000114", "120503",
                "Ledn. forb.1 x 70# AL-M i tavler, kasser og fordelingsanlæg", 208380);
            await UpsertOperation(session, "76FB76E8-A6F5-4906-AE8A-0E3365281BB8", "120503000115", "120503",
                "Ledn. forb.1 x 95# AL-M i tavler, kasser og fordelingsanlæg", 233220);
            await UpsertOperation(session, "7CE4B293-4DDF-49F3-AE8C-2CCE71FCB74F", "120503000116", "120503",
                "Ledn. forb.1 x 120# AL-M i tavler, kasser og fordelingsanlæg", 258060);
            await UpsertOperation(session, "641F6086-15C7-4873-81D0-B9DB73D72266", "120503000117", "120503",
                "Ledn. forb.1 x 150# AL-M i tavler, kasser og fordelingsanlæg", 287880);
            await UpsertOperation(session, "7B2FE6A6-D054-4628-90A8-2C1DE95BD156", "120503000118", "120503",
                "Ledn. forb.1 x 185# AL-M i tavler, kasser og fordelingsanlæg", 322680);
            await UpsertOperation(session, "2CBB2264-E37E-40B8-B164-658D795929B2", "120503000119", "120503",
                "Ledn. forb.1 x 240# AL-M i tavler, kasser og fordelingsanlæg", 377340);
            await UpsertOperation(session, "D90723D4-DADD-42CB-B8AC-4A16E4401835", "120503000120", "120503",
                "Ledn. forb.1 x 300# AL-M i tavler, kasser og fordelingsanlæg", 436980);
            await UpsertOperation(session, "FC5F2460-4FAF-40FA-8CCB-3F27333480B7", "120503000121", "120503",
                "Ledn. forb.1 x 400# AL-M i tavler, kasser og fordelingsanlæg", 536400);
            await UpsertOperation(session, "3CF9223E-8EC3-4C5D-8898-F7067D5012E5", "120503000122", "120503",
                "Ledn. forb.1 x 500# AL-M i tavler, kasser og fordelingsanlæg", 635820);
            await UpsertOperation(session, "D35EE928-1121-42B5-9D02-1AE7BDCE04E0", "120503000123", "120503",
                "Ledn. forb.1 x 625# AL-M i tavler, kasser og fordelingsanlæg", 760080);
            await UpsertOperation(session, "200F410A-8541-4C80-A0DC-721651C1D1A2", "120503000124", "120503",
                "Ledn. forb.1 x 1000# AL-M i tavler, kasser og fordelingsanlæg", 1132800);
            await UpsertOperation(session, "E9A9DE2F-02B1-4874-BA73-A1DA7E5FF348", "120700000001", "120700",
                "Afbarke kabel ud over 50cm ø1-20mm. pr. m", 128880);
            await UpsertOperation(session, "E5BBF168-3949-452D-AB04-973EEF560C81", "120700000002", "120700",
                "Afbarke kabel ud over 50cm ø20-40mm. pr. m", 164280);
            await UpsertOperation(session, "CD64EA3F-6512-4603-9637-8A091FFE74D2", "120700000003", "120700",
                "Afbarke kabel ud over 50cm ø40-60mm. pr. m", 392700);
            await UpsertOperation(session, "942E46D0-4F2B-474E-80E7-428D0FD552A9", "120700000005", "120700",
                "Afbarke kabel m/ koncentrisk leder eller skærm ud over 50cm ø 1 - 20mm. pr. m", 303000);
            await UpsertOperation(session, "B126E9A6-BBCA-4B85-8FF3-C5CD080E70E2", "120700000006", "120700",
                "Afbarke kabel m/ koncentrisk leder eller skærm ud over 50cm ø 20-40mm. pr. m", 312000);
            await UpsertOperation(session, "F83F0510-7D76-444C-80AA-20D16A7573F8", "120700000007", "120700",
                "Afbarke kabel m/ koncentrisk leder eller skærm ud over 50cm ø 40-60mm. pr. m", 438300);
            await UpsertOperation(session, "25D1FAF0-5BC3-40E4-A70A-F9F987AD6472", "120700000019", "120700",
                "Flette koncentrisk leder t.o.m. 95#, pr. m.", 103380);
            await UpsertOperation(session, "32A7D785-A986-473E-AAB8-CC41657A0C0D", "120800000002", "120800",
                "Flex/tape ø1-3mm på samlinger, ledn.- og kabelender o.l. Pr. m.", 654060);
            await UpsertOperation(session, "FB9BEF15-0D23-4E2D-B522-458E00BA5551", "120800000009", "120800",
                "Flex/tape ø>3-6mm på samlinger, ledn.- og kabelender o.l. Pr. m.", 1137480);
            await UpsertOperation(session, "CB006A80-041E-4CD0-A740-A66FAFF8C128", "120800000016", "120800",
                "Flex/tape ø>6-12mm på samlinger, ledn.-/ kabelender o.l. Pr. m.", 605160);
            await UpsertOperation(session, "115C8228-5972-462C-ABE6-F09007D21648", "120800000023", "120800",
                "Flex/tape ø>12-24mm. på samlinger, ledn.-/ kabelender o.l. Pr. m.", 536700);
            await UpsertOperation(session, "493D1236-4579-4BE5-A521-1C1B567F2B73", "120800000029", "120800",
                "Flex/tape ø>24-40mm, på samlinger, ledn.-/ kabelender o.l. Pr. m.", 679380);
            await UpsertOperation(session, "4128000E-F120-482E-A1EE-4240EB5CB810", "120800000035", "120800",
                "Flex/tape ø>40-60mm, på samlinger, ledn.-/ kabelender o.l. Pr. m.", 1944840);
            await UpsertOperation(session, "EC77ED74-8872-4E61-9001-475D9AC25AC6", "120800000041", "120800",
                "Flex/tape ø>60-80mm. på samlinger, ledn.-/ kabelender o.l. Pr. m.", 2118840);
            await UpsertOperation(session, "249303F5-D198-44FD-8356-D16399671300", "123301000013", "123301",
                "Demont. sidebeklæ. u. brug af værktøj.>1,20 t.o.m. 1,60 m2", 32280);
            await UpsertOperation(session, "F2A7A201-2393-4370-A2C9-888747733CCA", "123301000014", "123301",
                "Mont. sidebeklæ. u. brug af værktøj.>1,20 t.o.m. 1,60 m2", 64800);
            await UpsertOperation(session, "194F1BD7-34B2-483E-8D68-F51464A75589", "130100000007", "130100",
                "Tilpasse lagdelt gummipakning t.o.m. PG 13,5/20mm", 24960);
            await UpsertOperation(session, "AE5863CF-E1E9-49A8-B0BA-26AB45F886F3", "130100000008", "130100",
                "Tilpasse lagdelt gummipakning over PG 13,5/20mm t.o.m. PG 21/32mm", 38640);
            await UpsertOperation(session, "9C02A36E-BC99-403A-8243-7D0F2D2FE596", "130100000030", "130100", "Udslå blanket", 28320);
            await UpsertOperation(session, "302C50C6-7C12-4B2F-AD4D-F7C90E5984B8", "130100000102", "130100", "Medflg. forskr.t.o.m PG13,5/M20 isat",
                94020);
            await UpsertOperation(session, "1ABDBF5F-561F-4F7B-A58E-C1B9E205F192", "130100000103", "130100",
                "Medflg.forskr. over PG 13,5/20mm t.o.m. PG 21/M32 isat", 105000);
            await UpsertOperation(session, "962A1721-015B-495F-8FFB-80DE26256F52", "150100000002", "150100",
                "Ledn. forb. i komponenter & brugsgenst. 2-5 leder 1,5 -2,5#", 344760);
            await UpsertOperation(session, "6238A7F2-D132-4FC8-A669-8397BF0029A2", "150100000003", "150100",
                "Ledn. forb. i komponenter & brugsgenst. 6-10 ledere 1,5-2,5#", 537000);
            await UpsertOperation(session, "DD4F7BF7-7716-42CE-BA80-8A7445E23BEF", "150100000005", "150100",
                "Ledn. forb. i komponenter & brugsgenst. 2-5 leder 4-6#", 460320);
            await UpsertOperation(session, "3F4123B4-8A50-41D3-83C4-E194C485527E", "150100000007", "150100",
                "Ledn. forb. i komponenter & brugsgenst. 2-5 leder 10-16#", 421380);
            await UpsertOperation(session, "1A7FEF17-5AC0-49FD-8F18-76785CA109AA", "150100000009", "150100",
                "Ledn. forb. i komponenter & brugsgenst. 2-5 leder 25-35#", 754380);
            await UpsertOperation(session, "F2D4A726-2224-4389-8FE5-E559B724A377", "150100000021", "150100",
                "Ledn. forb. udover 5 pr. kabel 1,5-2,5# Pr. leder", 77520);
            await UpsertOperation(session, "63910C1C-5749-4321-B699-C313FD763434", "150100000022", "150100",
                "Kabler ud over 1, 6-10 x 1,5-2,5# Pr. kabel", 537000);
            await UpsertOperation(session, "2A1D11A3-8A15-4523-B8C9-533F5298D22E", "150100000024", "150100",
                "Ledn. forb. udover 5, pr. kabel 4-6# - pr. leder.", 129840);
            await UpsertOperation(session, "10DA66D3-3C94-4C78-8A44-110CEA9C7CD6", "150100000025", "150100",
                "Ledn. forb. udover 5, pr. kabel 10-16# - pr. leder.", 166080);
            await UpsertOperation(session, "9326A441-2977-4030-B780-C42A228E7C0A", "150100000030", "150100",
                "Styrekabel for tilbagemelding, mm, 2-5 x 0,5-1,5#,Pr. kabel.", 133920);
            await UpsertOperation(session, "CA460E59-3276-4C20-9A03-E545A6D7CF78", "152000000001", "152000", "Ledn.forb. 2-5x1,5-2,5# Pr. kabel",
                344760);
            await UpsertOperation(session, "F9319DB9-692E-40AC-9443-A25FE59734AD", "152000000003", "152000", "Ledn.forb. 6-10x1,5-2,5# Pr. kabel",
                537000);
            await UpsertOperation(session, "570E03E6-B8E8-412A-B524-44E927961E28", "152000000004", "152000", "Ledn.forb. 2-5x4-6# Pr. kabel", 460320);
            await UpsertOperation(session, "CA6AF2B6-5572-4D9F-A3FF-F86C3B61A9AD", "152000000005", "152000", "Ledn.forb. 2-5x10-16# Pr. kabel",
                421380);
            await UpsertOperation(session, "0BEECE61-0F61-4BE8-9533-812DBAAC65C8", "160000000004", "160000", "Ilægge medfølgende pakning", 199320);
            await UpsertOperation(session, "9FCD23F6-AC2E-474D-AC31-320FC2B31480", "170011000028", "170011", "Isætte medfølgende aflastning", 34860);
            //await UpsertOperation(session, "CD01565E-7900-4472-9138-628583FFAF11", "170021000008", "170021", "1 pol monteret uden skruer", -69120);
            //await UpsertOperation(session, "2C2934C1-DFBB-4C98-8E2D-62411FB85AFB", "170021000017", "170021", "Kroneafbr. monteret uden skruer", -114780);
            //await UpsertOperation(session, "73BD5DAA-E918-4074-9C76-6A70145DAF86", "170021000020", "170021", "Stikk. monteret uden skruer", -40680);
            //await UpsertOperation(session, "997FF456-34D3-4B07-9A1C-22B456B2A769", "170021000029", "170021", "Bl.dæksel 1M monteret uden skruer", -29820);
            await UpsertOperation(session, "99E0469D-A0BA-42EE-9674-CBB6F72E7422", "170022000002", "170022", "Påsætte pakning 1M - for planforsænkning", 64080);
            await UpsertOperation(session, "86FDD26F-267B-4302-B5BF-F57F70EE81E0", "170099000007", "170099", "1½M monteret med skruer hvor det er krævet eller aftalt. Pr. ramme", 55140);
            await UpsertOperation(session, "B9F63A68-2F78-4423-8322-A6E4DA2E92BF", "170099000102", "170099", "Fjerne manchet for 20mm rør. Pr manchet", 6300);
            await UpsertOperation(session, "EF173577-DCDB-4E63-9F2F-BC70C7E0FAF9", "170100000201", "170100",
                "Ledningsforbindelser bag blænddæksel 1M", 121920);
            await UpsertOperation(session, "D5D7852F-F932-4B77-B0C8-F28ED9FD9C79", "170100000202", "170100",
                "Ledningsforbindelser bag blænddæksel 1½ og 2M", 122400);
            await UpsertOperation(session, "904D8C37-AC3C-4798-AD18-BA38F7606D3E", "170100000203", "170100",
                "Ledningsforbindelser bag Clic blænddæksel for stikk.", 133440);
            await UpsertOperation(session, "CF6FA62E-252D-47D6-9F59-402D3767D1D1", "170141000001", "170141", "Fjerne dæksel", 16140);
            await UpsertOperation(session, "F49FA38E-755D-4E49-BD5F-26FDD2BA54A5", "170200000001", "170200",
                "Hul for 2M Euro-dåse i gips <=15mm/1 lag, Ø o/60-80 mm", 179760);
            await UpsertOperation(session, "012F5B05-3842-4BB3-ACBD-EBF4918A0C83", "170200000002", "170200",
                "Hul for 3M Euro-dåse i gips <=15mm/1 lag, Ø o/60-80 mm", 230400);
            await UpsertOperation(session, "3670953C-9B3E-4119-8D64-1D9FD803A373", "170200000003", "170200",
                "Hul for 4M Euro-dåse i gips <=15mm/1 lag, Ø o/60-80 mm", 280980);
            await UpsertOperation(session, "4B7D0358-C1D5-4347-9BC7-AD7B1C00E6F0", "170200000011", "170200",
                "Hul for 2M Euro-dåse i gips o/15-30mm/2 lag, Ø o/60-80 mm", 184860);
            await UpsertOperation(session, "DD9B87F7-5DC2-4EC9-A58B-963DB21D9D38", "170200000012", "170200",
                "Hul for 3M Euro-dåse i gips o/15-30mm/2 lag, Ø o/60-80 mm", 238020);
            await UpsertOperation(session, "23B3B550-1270-43EA-B859-D3D8A2643E2B", "170200000013", "170200",
                "Hul for 4M Euro-dåse i gips o/15-30mm/2 lag, Ø o/60-80 mm", 291180);
            await UpsertOperation(session, "E669B93E-E7D7-4022-A958-FBB845CECF55", "170200000021", "170200",
                "Hul for 2M Euro-dåse i træ <=3 cm/1 lag, Ø o/60-80 mm", 254640);
            await UpsertOperation(session, "CDF34826-F3C4-44EC-9D8C-4D8E7E06D43A", "170200000022", "170200",
                "Hul for 3M Euro-dåse i træ <=3 cm/1 lag, Ø o/60-80 mm", 342660);
            await UpsertOperation(session, "6ABD1F12-FBB3-4933-BE99-D218E845A81A", "170200000023", "170200",
                "Hul for 4M Euro-dåse i træ <=3 cm/1 lag, Ø o/60-80 mm", 430680);
            await UpsertOperation(session, "68F32C9E-ACF5-4CE7-B7F4-80C9B22C1C05", "172000000011", "172000",
                "Udskiftning til afbr.skruer, hvor dette er aftalt. Pr. afbr/stikk m.v.", 33120);
            await UpsertOperation(session, "1BD1AB3F-4086-4026-B026-F26B3F6109D3", "174000000020", "174000",
                "Indmurings-/støbningsdåse anvendt som lampested uden udtag", 291000);
            await UpsertOperation(session, "0C35D791-C652-4F09-8900-837C33493A5F", "174000000021", "174000",
                "Forfradåse anvendt som lampested uden udtag", 291000);
            await UpsertOperation(session, "A98204CC-2F3D-4650-B4EB-B574BEE59420", "174000000022", "174000",
                "Forfradåse ø68 anvendt som lampested uden udtag", 310020);
            await UpsertOperation(session, "D067DE51-4FF6-4E33-AE4B-33FA45B1E433", "184001000027", "184001",
                "Kabel for styrestrøm 2-5x0,5-1,5# mont. i rep. afbr./adskiller", 133920);
            await UpsertOperation(session, "1836E013-C302-4157-9327-4F7A792BBE7F", "184001000028", "184001", "Skærm i medflg. løs klemme/spændebånd",
                88200);
            await UpsertOperation(session, "C4199110-C097-4A87-BDBC-04011485CB39", "184002000002", "184002", "Ledn. forb. 2-5x1,5-2,5# Pr. kabel",
                264840);
            await UpsertOperation(session, "A20F479D-E78D-401E-AFF4-B399BC26D9EE", "184002000008", "184002",
                "Ledn. forb. udover 5 pr. kabel 1,5-2,5# Pr. leder", 77520);
            await UpsertOperation(session, "870FF605-4F7F-45F8-AC6B-D2A86BB03AB2", "184002000009", "184002", "Ledn.forb. 2-5x4-6# Pr. kabel", 404700);
            await UpsertOperation(session, "548F787B-37EB-4395-810C-958F0C401C87", "184002000010", "184002",
                "Ledn. forb. udover 5, pr. kabel 4-6# - pr. leder.", 129840);
            await UpsertOperation(session, "21FEAD7C-A2D5-4622-A78F-664B4DC8FC85", "184002000018", "184002",
                "Kabel for styrestrøm 2-5x0,5-1,5# mont. i rep. afbr./adskiller", 133920);
            await UpsertOperation(session, "EEDF9E4F-858D-4079-B79B-E1EF8137DAC9", "184002000020", "184002", "Skærm i medflg. løs klemme/spændebånd",
                88200);
            await UpsertOperation(session, "2026CB4A-6FCD-4867-A50D-91E1724254AC", "184002000031", "184002", "Ledn.forb. 3x4 EMC m/3xjord. Pr. kabel",
                440640);
            await UpsertOperation(session, "90A7E779-7176-43DC-A63F-DA1F7C274F74", "200200000020", "200200",
                "PL/PV dåse anvendt som lampested uden udtag", 310020);
            await UpsertOperation(session, "77A26A0B-5640-4529-8560-EE1361336501", "201000000002", "201000", "2 kabler i roset, 2 t.o.m 5x1,5#",
                372180);
            await UpsertOperation(session, "69CB5C8F-A5BE-4993-8BED-45C9370F7CBE", "201000000003", "201000", "3 kabler i roset, 2 t.o.m 5x1,5#",
                531060);
            await UpsertOperation(session, "DB19AC66-38A1-49EF-A334-2BA1B45B8F1B", "201000000004", "201000", "4 kabler i roset, 2 t.o.m 5x1,5#",
                592800);
            await UpsertOperation(session, "DE143CC8-5770-4D15-B225-7077C39BEA03", "201000000005", "201000", "5 kabler i roset, 2 t.o.m 5x1,5#",
                924600);
            await UpsertOperation(session, "F88E63DC-B6D9-413E-9C2B-7909BA33DD3D", "201000000006", "201000", "1 kabel i roset, 2 t.o.m 5x2,5#",
                181860);
            await UpsertOperation(session, "423FDD84-D965-4EB2-A16F-B39B26B8BABA", "201000000007", "201000", "2 kabler i roset, 2 t.o.m 5x2,5#",
                387540);
            await UpsertOperation(session, "74E023B3-4CEB-4982-A2A0-288D0B1E1553", "201000000010", "201000",
                "Tilledning tom 3 x tom 1,5# for belysn./brugsgenst.", 183420);
            await UpsertOperation(session, "BB702969-8467-40A6-87AF-E6F3E43C2715", "201000000011", "201000",
                "Tilledning 4 x tom 1,5# for belysn./brugsgenst.", 186000);
            await UpsertOperation(session, "93F1FAD5-09CE-484B-944A-F59EF680A724", "201000000012", "201000",
                "Tilledning 5 x tom 1,5# for belysn./brugsgenst.", 222780);
            await UpsertOperation(session, "6331E655-057B-424A-866C-8D9B954BB376", "214000000021", "214000",
                "1 kabel t.o.m. 5x2,5# afsluttet i membr.-/forskr.dåse", 132120);
            await UpsertOperation(session, "1312C55C-2E40-41C2-9B41-0806F3643484", "214000000022", "214000",
                "2 kabler i membr.-/forskr.dåse, t.o.m 5x2,5#", 318180);
            await UpsertOperation(session, "A4C7DEF6-F98F-423A-8E05-39039C304DA5", "214000000023", "214000",
                "3 kabler i membr.-/forskr.dåse, t.o.m 5x2,5#", 466980);
            await UpsertOperation(session, "C65DE825-3CA3-49E5-87E2-75BE1D95DA6F", "214000000024", "214000",
                "4 kabler i membr.-/forskr.dåse, t.o.m 5x2,5#", 678660);
            await UpsertOperation(session, "26BED913-5058-44B9-9DCC-3247C9ADD7FB", "214000000025", "214000",
                "5 kabler i membr.-/forskr.dåse, t.o.m 5x2,5#", 1092240);
            await UpsertOperation(session, "4C5227E5-4A92-4627-8E4F-8E35354C22EC", "214000000026", "214000",
                "Ledn. forb ud over 5 pr. kabel: Pr. leder", 77520);
            await UpsertOperation(session, "6F7B9AD1-E8D0-4AD5-8E5C-BB5FBB36B600", "214000000030", "214000",
                "Tilledning tom 3x tom 1,5# for belysn./brugsgenst.", 183420);
            await UpsertOperation(session, "60D7EE38-F166-40D5-9633-12E8BFD42C02", "214000000031", "214000",
                "Tilledning 4 x tom 1,5# for belysn./brugsgenst.", 186000);
            await UpsertOperation(session, "3EC351A7-8A42-4422-9263-74FF28CE3E40", "214000000032", "214000",
                "Tilledning 5 x tom 1,5# for belysn./brugsgenst.", 222780);
            await UpsertOperation(session, "D06D0162-AEEC-42A8-A96C-58D89F6F7737", "214010000013", "214010", "Indlæg/klemrække isat trykket/clipset",
                7680);
            await UpsertOperation(session, "61BD5772-0FB8-48B7-9DD2-F88E7996C4FD", "214010000015", "214010", "Medleveret aflastning isat med skruer",
                30480);
            await UpsertOperation(session, "34DF27AF-5DE6-4F88-9999-3BAF31C7E6B4", "214010000016", "214010",
                "Medleveret aflastning Hensel ds. isat trykket/clipset", 13260);
            await UpsertOperation(session, "02C87490-C35A-4DE5-A421-6F38D5DF2E65", "214010000063", "214010", "Medleveret membr. nippel isat", 44220);
            await UpsertOperation(session, "2C9AA91F-BF97-45FE-912B-3A14DE676BEE", "264000000030", "264000", "Skærm til sensor tilpasset og påsat",
                33960);
            await UpsertOperation(session, "F00FBCC6-DC7D-4FCB-97C9-4ED8E09BE1E8", "264000000157", "264000", "Ledere udover 9, pr leder.", 51360);
            await UpsertOperation(session, "35855712-B570-48AD-9C4C-C752B910D754", "267100000009", "267100",
                "Identificere forprogrammerede komponenter efter tegning.", 35040);
            await UpsertOperation(session, "FDCA4D83-50F3-484B-B75C-9A9618FCB1E4", "267100000017", "267100",
                "Påsætte piktogrammer på betjeningspanel/rumtermostat", 84360);
            await UpsertOperation(session, "519E91EB-A6F1-429A-B001-9621728181BD", "270000000001", "270000", "Fremstille/skrive skilte til mærkning",
                26520);
            await UpsertOperation(session, "E789915A-4FE4-40B1-B6FE-1E76FB400E07", "270000000002", "270000",
                "Påsætte færdigtrykt mærkning på tavle eller komponent", 29160);
            await UpsertOperation(session, "BFC9148A-70B5-4C12-B07C-3B9612F54A05", "270000000003", "270000", "Permanent opmærkning m. tuschpen",
                23760);
            await UpsertOperation(session, "63B82CFD-F2F5-4048-9A4A-BF3443264149", "270000000004", "270000",
                "Påsætte færdigtrykte klistermærker på kabel/mærkeplade", 29100);
            await UpsertOperation(session, "F6940749-3983-443A-9311-F1C3EE5BC7BD", "270000000005", "270000",
                "Påsætte færdigtrykte skilte m/ strips ell. lign. på kabel", 73980);
            await UpsertOperation(session, "0A49117B-BBE7-49F7-966D-EE0FC092BAB7", "301000000020", "301000", "Overskære stålrør t.o.m. 25 mm.",
                114900);
            await UpsertOperation(session, "BF6948B4-6C89-4302-9A38-272E01FB07CE", "321110000002", "321110",
                "2 t.o.m. 5X0,75-2,5# tilledning fremført incl. punktvis fastgørelse", 28440);
            await UpsertOperation(session, "04767F36-6DDB-47A9-B7D0-3563D15D187F", "321110000006", "321110",
                "2 t.o.m. 5X4-6# tilledning fremført incl. punktvis fastgørelse", 42660);
            await UpsertOperation(session, "1920DA30-5769-44C7-A562-C21A602D52F5", "321110000011", "321110",
                "2 t.o.m. 5X0,75-2,5# tilledning opkvejlet hvor dette forlanges", 19260);
            await UpsertOperation(session, "6E9D4E14-9B6E-430E-8A13-46FB03F430FA", "361010000003", "361010", "Overskæring af småkanal", 108120);
            await UpsertOperation(session, "BFEBDEEE-1B3E-4046-AA78-D11C1B0EF91B", "361010000004", "361010", "Overskæring af låg for småkanal",
                70140);
            await UpsertOperation(session, "16585B66-E0A6-4964-9C70-D71212867E06", "361010000005", "361010",
                "Skrå op til 45gr. overskæring af småkanal", 162120);
            await UpsertOperation(session, "348336A9-B30D-4084-88A1-F7B402100609", "361010000006", "361010",
                "Skrå op til 45gr. overskæring af låg for småkanal", 105240);
            await UpsertOperation(session, "BD3D794A-A533-4955-B96C-CD88B218A0AE", "361022000004", "361022",
                "Overskæring inst. kanal over 70 t.o.m. 90mm bred", 108420);
            await UpsertOperation(session, "E6207D9A-6209-42D2-A7C4-DCB5EEACB73F", "361022000005", "361022",
                "Overskæring inst. kanal over 90 t.o.m. 135mm bred", 105480);
            await UpsertOperation(session, "B133D95B-44E2-4D31-92CF-877BC4D60690", "361022000006", "361022",
                "Overskæring inst. kanal over 135 t.o.m. 230mm bred", 139980);
            await UpsertOperation(session, "D2089444-19D3-4C43-B1E6-A472BAB6D066", "361022000018", "361022",
                "Skrå op til 45gr. overskæring inst. kanal over 70 t.o.m. 90mm bred", 162600);
            await UpsertOperation(session, "E16C48F9-0D71-47A6-813B-9C258E1770B4", "361022000019", "361022",
                "Skrå op til 45gr. overskæring inst. kanal over 90 t.o.m. 135mm bred", 158160);
            await UpsertOperation(session, "CD1B184A-BA7C-42B6-B790-C135E3957019", "361022000020", "361022",
                "Skrå op til 45gr. overskæring inst. kanal over 135 t.o.m. 230mm bred", 210000);
            await UpsertOperation(session, "171B7BD4-8744-41D0-A544-F5C2E731DE80", "361090000003", "361090", "Overskæring af skilleplade", 36840);
            await UpsertOperation(session, "007237CE-2FA5-44D6-99F8-BA6591C6EABE", "361090000004", "361090", "Overskæring af skilleplade dobbelt",
                48840);
            await UpsertOperation(session, "4F7610E6-3DE9-4856-8615-3017FF2B0481", "361090000010", "361090", "Fjerne beskyttelsesfolie. Pr. m kanal",
                41820);
            await UpsertOperation(session, "3EE22B07-D5ED-41F7-8BD0-66A42336A376", "361090000014", "361090",
                "Afklippe og isætte del til skillespor. Pr. del", 15480);
            await UpsertOperation(session, "473FD342-95B4-4BCA-AAB8-D9B6B1438512", "361090000015", "361090",
                "Isætte og 2 tilpasse kiler til hjørne. <90gr", 38880);
            await UpsertOperation(session, "1E3D9128-7B8C-49EE-8537-A0A17F4BD6B3", "361090000016", "361090", "Tilpasse hjørne >90gr", 53340);
            await UpsertOperation(session, "7B45CA66-BA0B-4C55-8D36-C3D082F87651", "369900000001", "369900",
                "Overskæring kanallåg kunststof. Over 25 t.o.m. 50mm bred, 4 t.o.m. 25mm høj", 46380);
            await UpsertOperation(session, "DBDBCE17-9B23-412B-9CF0-C9A73DDCD084", "369900000002", "369900",
                "Overskæring kanallåg kunststof. Over 50 t.o.m. 90mm bred, 4 t.o.m. 25mm høj", 82860);
            await UpsertOperation(session, "DBED9976-5EF1-48F3-8499-1C10D5CF655B", "369900000006", "369900",
                "Overskære kanallåg kunststof skrå op til 45gr. O/25 - 50mm bred", 69600);
            await UpsertOperation(session, "DC0E954D-E705-4882-9D46-433E42AF4545", "369900000007", "369900",
                "Overskære kanallåg kunststof skrå op til 45gr. O/50 - 90mm bred", 124320);
            await UpsertOperation(session, "11D89BBA-0521-4B40-942A-2CBBAC150399", "369900000008", "369900",
                "Overskære kanallåg kunststof skrå op til 45gr. O/90 - 135mm bred", 123480);
            await UpsertOperation(session, "A96BD47C-F3B8-4F37-BED9-92A049418FB2", "369900000011", "369900",
                "Overskæring kanallåg alu. Over 50 t.o.m. 90mm bred, 4 t.o.m. 25mm høj", 75540);
            await UpsertOperation(session, "6837676C-CFE1-4C08-84CD-D64C0F6DCA0A", "369900000017", "369900",
                "Overskære kanallåg alu skrå op til 45gr. O/50 - 90mm bred", 113280);
            await UpsertOperation(session, "46A7DC11-1BBC-4DEE-9B66-4E2FD0913B68", "369900000021", "369900",
                "Overskæring kanallåg stål. Over 50 t.o.m. 90mm bred, 4 t.o.m. 25mm", 84180);
            await UpsertOperation(session, "8E38663C-88E8-48FA-8BDD-E3D31B20003D", "369900000027", "369900",
                "Overskære kanallåg stål skrå op til 45gr. O/50 - 90mm bred", 126240);
            await UpsertOperation(session, "CAA9DEF3-7C6C-4789-898D-8A69592CB4B6", "369900000050", "369900",
                "Fjerne beskyttelsesfolie. pr. m kanallåg", 11280);
            await UpsertOperation(session, "5F43B33C-D46C-4CA1-A753-370DFC0C481A", "371010000001", "371010",
                "Overskæring kabelbakke t.o.m 50mm indvendig målt", 116700);
            await UpsertOperation(session, "B4CA1678-AC02-4DED-ACC8-445DCCC9BE39", "371010000002", "371010",
                "Overskæring kabelbakke over 50mm t.o.m 75mm indvendig målt", 125340);
            await UpsertOperation(session, "BAF091BF-11E1-4846-8E5D-48A3BF2AFC9C", "371010000003", "371010",
                "Overskæring kabelbakke over 75mm t.o.m 100mm indvendig målt", 153300);
            await UpsertOperation(session, "5B76FB16-64CB-464A-A19E-BE0C4198C175", "371010000004", "371010",
                "Overskæring kabelbakke over 100mm t.o.m 150mm indvendig målt", 170280);
            await UpsertOperation(session, "BD117B33-B9ED-4AE9-BE77-9C6E3EDEB375", "371010000005", "371010",
                "Overskæring kabelbakke over 150mm t.o.m 200mm indvendig målt", 208260);
            await UpsertOperation(session, "24FCDC1F-46C6-4A53-9AB3-7D54D9BB2B61", "371010000006", "371010",
                "Overskæring kabelbakke over 200mm t.o.m 250mm indvendig målt", 189540);
            await UpsertOperation(session, "5CA05C7B-62C7-4E87-A147-BBE018804D45", "371010000007", "371010",
                "Overskæring kabelbakke over 250mm t.o.m 300mm indvendig målt", 191040);
            await UpsertOperation(session, "0DCC4A78-5646-45FA-ABC6-F016547A705D", "371010000008", "371010",
                "Overskæring kabelbakke over 300mm t.o.m 400mm indvendig målt", 220260);
            await UpsertOperation(session, "6BAD60A9-67E4-4CC8-9F32-7A41AE37F7A6", "371010000009", "371010",
                "Overskæring kabelbakke over 400mm t.o.m 500mm indvendig målt", 248340);
            await UpsertOperation(session, "E89AA115-E202-45DC-965D-F040E4607E94", "371010000010", "371010",
                "Overskæring kabelbakke over 500mm t.o.m 600mm indvendig målt", 256920);
            await UpsertOperation(session, "18EA6275-8E07-4241-BD59-0A0D50B9294B", "371010000107", "371010",
                "Overskæring kabelbakke over 250 t.o.m 300mm indvendig målt", 199140);
            await UpsertOperation(session, "7365D470-AC50-4262-88E7-1646FD50D21D", "371011000007", "371011",
                "Overskæring kabelbakke m/skinne over 250mm t.o.m 300mm indvendig målt", 195300);
            await UpsertOperation(session, "CCA51621-68BA-4572-BBB2-46C4EB6246F8", "371011000010", "371011",
                "Overskæring kabelbakke m/skinne over 500mm t.o.m 600mm indvendig målt", 236940);
            await UpsertOperation(session, "98BE64A9-B895-4837-AA39-C3A8786DFB3F", "371013000001", "371013",
                "45 gr. overskæring kabelbakke t.o.m 50mm indvendig målt", 175020);
            await UpsertOperation(session, "76E0183E-F089-4BE9-B667-B938198BBA73", "371013000002", "371013",
                "45 gr. overskæring kabelbakke over 50mm t.o.m 75mm indvendig målt", 187980);
            await UpsertOperation(session, "10386300-A469-4832-B329-DFF841F78C0E", "371013000003", "371013",
                "45 gr. overskæring kabelbakke over 75mm t.o.m 100mm indvendig målt", 229980);
            await UpsertOperation(session, "F52C57CF-4A73-40A6-BD0E-35A9E55919DF", "371013000004", "371013",
                "45 gr. overskæring kabelbakke over 100mm t.o.m 150mm indvendig målt", 255420);
            await UpsertOperation(session, "94263CE9-C79A-41A2-BD58-2369A1897565", "371013000005", "371013",
                "45 gr. overskæring kabelbakke over 150mm t.o.m 200mm indvendig målt", 312360);
            await UpsertOperation(session, "3916443D-C4BA-45D6-84E6-ED88F9383984", "371013000006", "371013",
                "45 gr. overskæring kabelbakke over 200mm t.o.m 250mm indvendig målt", 284280);
            await UpsertOperation(session, "031ED417-D054-48CB-9542-35399AF3C064", "371013000007", "371013",
                "45 gr. overskæring kabelbakke over 250mm t.o.m 300mm indvendig målt", 298680);
            await UpsertOperation(session, "F80D31CC-1BCF-48C7-A4F7-E15CB12983DA", "371013000008", "371013",
                "45 gr. overskæring kabelbakke over 300mm t.o.m 400mm indvendig målt", 330420);
            await UpsertOperation(session, "6A2901EF-A1C5-4199-871A-A8BE0C4A034C", "371013000009", "371013",
                "45 gr. overskæring kabelbakke over 400mm t.o.m 500mm indvendig målt", 372540);
            await UpsertOperation(session, "EEE53E1A-791D-4815-9D81-4D074399A4D5", "371013000010", "371013",
                "45 gr. overskæring kabelbakke over 500mm t.o.m 600mm indvendig målt", 385380);
            await UpsertOperation(session, "185FC9F2-2224-4258-AB64-979683D2727C", "371016000001", "371016",
                "Overskæring kabelbakke uperf. t.o.m 50mm indvendig målt", 157980);
            await UpsertOperation(session, "079B98F8-191F-4C4B-BC00-E4CABE1FB542", "371016000002", "371016",
                "Overskæring kabelbakke uperf. o/ 50 t.o.m 75mm indvendig målt", 168480);
            await UpsertOperation(session, "867C1AD9-DC45-465F-835E-B601AA9DE5A4", "371016000003", "371016",
                "Overskæring kabelbakke uperf. o/ 75 t.o.m 100mm indvendig målt", 177000);
            await UpsertOperation(session, "AAB7F255-23BD-4AD0-A80A-CBD22D18C3C4", "371016000004", "371016",
                "Overskæring kabelbakke uperf. o/ 100 t.o.m 150mm indvendig målt", 201720);
            await UpsertOperation(session, "C2C34923-A42A-4AF9-A603-04BE421D2B92", "371016000005", "371016",
                "Overskæring kabelbakke uperf. o/ 150 t.o.m 200mm indvendig målt", 216660);
            await UpsertOperation(session, "9014D0C3-4F02-4251-B7EE-4A8F74739394", "371016000006", "371016",
                "Overskæring kabelbakke uperf. o/ 200 t.o.m 250mm indvendig målt", 231600);
            await UpsertOperation(session, "5190C09E-8EDE-4076-8BB9-8CC17A8CEBE7", "371016000007", "371016",
                "Overskæring kabelbakke uperf. o/ 250 t.o.m 300mm indvendig målt", 246600);
            await UpsertOperation(session, "DD6BCB28-B478-41BF-B7DC-EC747D36C477", "371016000008", "371016",
                "Overskæring kabelbakke uperf. o/ 300 t.o.m 400mm indvendig målt", 276480);
            await UpsertOperation(session, "59F69A5F-A60E-4511-9E4C-E67554546B46", "371016000009", "371016",
                "Overskæring kabelbakke uperf. o/ 400 t.o.m 500mm indvendig målt", 294960);
            await UpsertOperation(session, "949FF5C5-B845-43A9-9813-AB9881662098", "371016000010", "371016",
                "Overskæring kabelbakke uperf. o/ 500 t.o.m 600mm indvendig målt", 336360);
            await UpsertOperation(session, "E37B33ED-E34B-4E0F-9E59-D80262FB98A7", "371017000001", "371017",
                "45gr. overskæring kabelbakke uperf. t.o.m 50mm indvendig målt", 237000);
            await UpsertOperation(session, "8EDE2DBF-9010-460A-A801-E0968E4FA802", "371017000002", "371017",
                "45gr. overskæring kabelbakke uperf. o/50 t.o.m 75mm indvendig målt", 252720);
            await UpsertOperation(session, "5CF61E50-35B1-4AC1-86E2-408A16DDE167", "371017000003", "371017",
                "45gr. overskæring kabelbakke uperf. o/75 t.o.m 100mm indvendig målt", 265500);
            await UpsertOperation(session, "3F9568C6-C5E6-40AC-84BA-EBA87BCCC215", "371017000004", "371017",
                "45gr. overskæring kabelbakke uperf. o/100 t.o.m 150mm indvendig målt", 302580);
            await UpsertOperation(session, "A28A4864-AC23-4C91-9E07-9C01AE9DC453", "371017000005", "371017",
                "45gr. overskæring kabelbakke uperf. o/150 t.o.m 200mm indvendig målt", 324960);
            await UpsertOperation(session, "4460BAAB-4E8B-4A1C-8765-7024B86D4FED", "371017000006", "371017",
                "45gr. overskæring kabelbakke uperf. o/200 t.o.m 250mm indvendig målt", 347400);
            await UpsertOperation(session, "4FE33FED-DBA2-4864-AC3A-BB7EACF85187", "371017000007", "371017",
                "45gr. overskæring kabelbakke uperf. o/250 t.o.m 300mm indvendig målt", 369900);
            await UpsertOperation(session, "646A7824-4F90-46E4-8088-087F44566636", "371017000008", "371017",
                "45gr. overskæring kabelbakke uperf. o/300 t.o.m 400mm indvendig målt", 414720);
            await UpsertOperation(session, "C5D55AB1-E43F-4101-B076-EF313F53CA5C", "371017000009", "371017",
                "45gr. overskæring kabelbakke uperf. o/400 t.o.m 500mm indvendig målt", 442440);
            await UpsertOperation(session, "26BA55E2-065E-485B-AB90-3D78DD47D73B", "371017000010", "371017",
                "45gr. overskæring kabelbakke uperf. o/500 t.o.m 600mm indvendig målt", 504540);
            await UpsertOperation(session, "129B5270-03E8-465B-B5A4-3668AE5A8786", "371018000021", "371018",
                "Udskæring for 50mm afgrening m AV50/80 ell. tilsvarende", 48600);
            await UpsertOperation(session, "2CDF5CBE-D194-4082-8103-0982AE53FB28", "371018000022", "371018",
                "Udskæring for 75mm afgrening m AV50/80 ell. tilsvarende", 56700);
            await UpsertOperation(session, "99656C7D-ABDA-4DF7-ACE6-20CE8F76EAD7", "371018000023", "371018",
                "Udskæring for 100mm afgrening m AV50/80 ell. tilsvarende", 56700);
            await UpsertOperation(session, "A6DB362A-99BA-4830-BD15-AF4E04A8275E", "371018000024", "371018",
                "Udskæring for 150mm afgrening m AV50/80 ell. tilsvarende", 72900);
            await UpsertOperation(session, "363A6AC1-695E-43F5-8FF9-F7E096117C80", "371018000025", "371018",
                "Udskæring for 200mm afgrening m AV50/80 ell. tilsvarende", 89100);
            await UpsertOperation(session, "29393CA9-D389-45CC-A43F-A45FCF1B7995", "371018000026", "371018",
                "Udskæring for 300mm afgrening m AV50/80 ell. tilsvarende", 121500);
            await UpsertOperation(session, "8162753A-B7BE-4F4E-A627-2CFFCAD3F4A6", "371018000027", "371018",
                "Udskæring for 400mm afgrening m AV50/80 ell. tilsvarende", 153900);
            await UpsertOperation(session, "4BC0D41B-95A6-410F-A331-ACB94A94960D", "371018000028", "371018",
                "Udskæring for 500mm afgrening m AV50/80 ell. tilsvarende", 186300);
            await UpsertOperation(session, "2B04E36C-A79D-4F2F-9F9A-3D515E952747", "371018000029", "371018",
                "Udskæring for 600mm afgrening m AV50/80 ell. tilsvarende", 218700);
            await UpsertOperation(session, "1C63A92B-F550-4D35-88C0-2C2ACBC97308", "371020000001", "371020",
                "Bukke kabelbakke t.o.m 50mm indvendig målt", 259560);
            await UpsertOperation(session, "EAEEAF72-CBF5-4BDF-A775-888C2C22FD74", "371020000002", "371020",
                "Bukke kabelbakke t.o.m 75mm indvendig målt", 246240);
            await UpsertOperation(session, "11C085BC-71D5-44CE-953D-A1B98FEC19E5", "371020000003", "371020",
                "Bukke kabelbakke t.o.m 100mm indvendig målt", 232500);
            await UpsertOperation(session, "952E2FED-FE75-41B8-9ED5-6765361A46B9", "371020000004", "371020",
                "Bukke kabelbakke t.o.m 150mm indvendig målt", 239160);
            await UpsertOperation(session, "9261AE56-D2DF-4B75-ACB0-E01737BDC86D", "371020000005", "371020",
                "Bukke kabelbakke t.o.m 200mm indvendig målt", 243600);
            await UpsertOperation(session, "2BA8BA9E-A053-4FE0-B223-863ADF1C27AD", "371020000006", "371020",
                "Bukke kabelbakke t.o.m 250mm indvendig målt", 245160);
            await UpsertOperation(session, "5B8B82E6-8F0F-4B22-9F84-3C907C6D5183", "371020000007", "371020",
                "Bukke kabelbakke t.o.m 300mm indvendig målt", 249420);
            await UpsertOperation(session, "2A598E85-843E-4FE2-894F-76210D5C0A07", "371020000008", "371020",
                "Bukke kabelbakke t.o.m 400mm indvendig målt", 249540);
            await UpsertOperation(session, "84B8F3C8-1808-41B6-B1A1-3ADB4E354E91", "371020000009", "371020",
                "Bukke kabelbakke t.o.m 500mm indvendig målt", 315660);
            await UpsertOperation(session, "EA0DC34C-15A5-41FC-BE8E-6B0B34647A93", "371020000010", "371020",
                "Bukke kabelbakke t.o.m 600mm indvendig målt", 392880);
            await UpsertOperation(session, "6B2E1E90-4752-4303-9C60-CD49C84621FB", "371020000011", "371020",
                "Bukke kabelbakke t.o.m 700mm indvendig målt", 490860);
            await UpsertOperation(session, "F7171B16-8BD4-4A88-9A68-35D219ABAD29", "371020000012", "371020",
                "Bukke kabelbakke t.o.m 800mm indvendig målt", 594000);
            await UpsertOperation(session, "9A75B105-E007-4F20-AE59-41A472097462", "371020000013", "371020",
                "Bukke kabelbakke t.o.m 900mm indvendig målt", 714840);
            await UpsertOperation(session, "931A096D-D4C2-4B77-99E0-225733F181D3", "371020000014", "371020",
                "Bukke kabelbakke t.o.m 1000mm indvendig målt", 853320);
            await UpsertOperation(session, "FE954196-0EF9-4144-95E2-21A2AC87919D", "371030000001", "371030",
                "Fremstille afgrening/sving til 50mm bakke, incl bortskæring af kant", 409560);
            await UpsertOperation(session, "059853F9-4DD2-42E5-9E2E-3B1DE7135EC3", "371030000002", "371030",
                "Fremstille afgrening/sving til 75mm bakke, incl bortskæring af kant", 441540);
            await UpsertOperation(session, "FA91CB59-DE38-49A4-AEFA-59AAAE693DA1", "371030000003", "371030",
                "Fremstille afgrening/sving til 100mm bakke, incl bortskæring af kant", 473520);
            await UpsertOperation(session, "AC3F0C1B-51FB-4C9F-A344-AAB2C4405CF1", "371030000004", "371030",
                "Fremstille afgrening/sving til 150mm bakke, incl bortskæring af kant", 537540);
            await UpsertOperation(session, "5F3D72D0-980E-4B1C-815D-3619DFC47EDA", "371030000005", "371030",
                "Fremstille afgrening/sving til 200mm bakke, incl. bortskæring af kant", 593520);
            await UpsertOperation(session, "9D4A5777-1D77-4081-8DFE-3C3EEDC99FBE", "371030000006", "371030",
                "Fremstille afgrening/sving til 250mm bakke, incl bortskæring af kant", 665520);
            await UpsertOperation(session, "5F76A101-AD92-42FC-83DA-9220E1F280E9", "371030000007", "371030",
                "Fremstille afgrening/sving til 300mm bakke, incl. bortskæring af kant", 760860);
            await UpsertOperation(session, "1931653B-0072-4128-8215-0FF468D697B4", "371030000008", "371030",
                "Fremstille afgrening/sving til 400mm bakke, incl. bortskæring af kant", 818460);
            await UpsertOperation(session, "91CEFD96-0B16-4B95-BADF-8C78C8AF73A7", "371030000009", "371030",
                "Fremstille afgrening/sving til 500mm bakke, incl. bortskæring af kant", 915780);
            await UpsertOperation(session, "B2650AF6-BBED-4F97-A0E9-1B7644570925", "371030000010", "371030",
                "Fremstille afgrening/sving til 600mm bakke, incl bortskæring af kant", 1113420);
            await UpsertOperation(session, "5CC58AB5-3FFC-42CC-A6CC-BD5B0F191515", "372010000001", "372010", "Overskæring pr. vange", 82440);
            await UpsertOperation(session, "A528AEC9-24AE-4AF7-8E5B-9C41825FB988", "374011000001", "374011",
                "Overskæring/-klipning af gitterbakke. Bredde t.o.m 55mm udvendig målt", 73560);
            await UpsertOperation(session, "9C27B4C1-8E5F-45F9-8C6C-DBCBC4150C99", "374011000002", "374011",
                "Overskæring/-klipning af gitterbakke. Bredde t.o.m 75mm udvendig målt", 63060);
            await UpsertOperation(session, "44BE80F7-F64A-461A-AD56-64CF3931F7ED", "374011000003", "374011",
                "Overskæring/-klipning af gitterbakke. Bredde t.o.m 125mm udvendig målt", 74160);
            await UpsertOperation(session, "93EA9186-F29C-4BD5-8FBF-7EFACF9EA565", "374011000004", "374011",
                "Overskæring/-klipning af gitterbakke. Bredde t.o.m 155mm udvendig målt", 71460);
            await UpsertOperation(session, "ACEE0B05-6A64-4320-9C1B-514ECC3ED6D9", "374011000005", "374011",
                "Overskæring/-klipning af gitterbakke. Bredde t.o.m 225mm udvendig målt", 83880);
            await UpsertOperation(session, "0CD0A4A3-842C-45BD-83C8-BBD76C1BDB93", "374011000006", "374011",
                "Overskæring/-klipning af gitterbakke. Bredde t.o.m 325mm udvendig målt", 89160);
            await UpsertOperation(session, "A8F91F0B-3A9E-4118-AED4-61D420E8D28E", "374011000008", "374011",
                "Overskæring/-klipning af gitterbakke. Bredde t.o.m 525mm udvendig målt", 105600);
            await UpsertOperation(session, "A613F3B3-F05A-4318-AF8E-EBAD79D754F6", "374011000009", "374011",
                "Overskæring/-klipning af gitterbakke. Bredde t.o.m 625mm udvendig målt", 120180);
            await UpsertOperation(session, "F06E2E13-43DE-4851-A1A3-F32234CB2CA2", "374012000001", "374012",
                "Overskæring/-klipning af gitterbakke. Bredde t.o.m 55mm udvendig målt", 119760);
            await UpsertOperation(session, "9F9289FE-8C3D-4507-AE5E-F44CE8B0C0FD", "374012000002", "374012",
                "Overskæring/-klipning af gitterbakke. Bredde t.o.m 75mm udvendig målt", 123300);
            await UpsertOperation(session, "252DB4C3-A718-4E38-8A3F-8E1243FB4702", "374012000003", "374012",
                "Overskæring/-klipning af gitterbakke. Bredde t.o.m 125mm udvendig målt", 132180);
            await UpsertOperation(session, "CFFFC670-DE79-4187-B9CC-E08619421A74", "374012000004", "374012",
                "Overskæring/-klipning af gitterbakke. Bredde t.o.m 155mm udvendig målt", 137520);
            await UpsertOperation(session, "F7423A71-6439-49EA-8A76-93BE8C3AE644", "374012000005", "374012",
                "Overskæring/-klipning af gitterbakke. Bredde t.o.m 225mm udvendig målt", 149940);
            await UpsertOperation(session, "FFBF5584-C727-4FC0-A745-BC875287D180", "374012000006", "374012",
                "Overskæring/-klipning af gitterbakke. Bredde t.o.m 325mm udvendig målt", 167700);
            await UpsertOperation(session, "68777CD7-D0F4-494B-9999-1E8E0ADDA403", "374012000007", "374012",
                "Overskæring/-klipning af gitterbakke. Bredde t.o.m 425mm udvendig målt", 185460);
            await UpsertOperation(session, "1BA016F9-25DC-433A-9B4F-0F5138E5DBAF", "374012000008", "374012",
                "Overskæring/-klipning af gitterbakke. Bredde t.o.m 525mm udvendig målt", 203220);
            await UpsertOperation(session, "7207B3DA-99B0-4E1A-A67E-A0CD9BE18601", "374012000009", "374012",
                "Overskæring/-klipning af gitterbakke. Bredde t.o.m 625mm udvendig målt", 220980);
            await UpsertOperation(session, "7D18D6F5-96DE-43A5-B241-E6B14434809D", "374013000001", "374013",
                "Sving vandret. Bredde t.o.m 55mm udvendig målt. Ex løsdele", 268140);
            await UpsertOperation(session, "0555E10D-34BC-4026-8C76-4FF4687C47F3", "374013000002", "374013",
                "Sving vandret. Bredde t.o.m 75mm udvendig målt. Ex løsdele", 283860);
            await UpsertOperation(session, "08D7C837-C8E0-4558-A593-96A7C8F038E1", "374013000003", "374013",
                "Sving vandret. Bredde t.o.m 125mm udvendig målt. Ex løsdele", 323220);
            await UpsertOperation(session, "C6310E98-802A-4869-A807-2BD893AEB9AA", "374013000004", "374013",
                "Sving vandret. Bredde t.o.m 155mm udvendig målt. Ex løsdele", 346860);
            await UpsertOperation(session, "7032F30A-0F44-4EAB-AA6D-A45683DAD1ED", "374013000005", "374013",
                "Sving vandret.Bredde t.o.m 225mm udvendig målt. Ex løsdele", 401940);
            await UpsertOperation(session, "2B6D494E-0F40-4FB1-BA74-960CC1402D71", "374013000006", "374013",
                "Sving vandret. Bredde t.o.m 325mm udvendig målt. Ex løsdele", 480660);
            await UpsertOperation(session, "1A183021-9630-40A9-BD1D-CCE2640E1A49", "374013000007", "374013",
                "Sving vandret.Bredde t.o.m 425mm udvendig målt. Ex løsdele", 559380);
            await UpsertOperation(session, "3A511103-3D37-4356-A4E5-49119AAC74E7", "374013000008", "374013",
                "Sving vandret. Bredde t.o.m 525mm udvendig målt. Ex løsdele", 638100);
            await UpsertOperation(session, "FDF075BC-11B8-43F9-9B8D-EEE1AFD48E31", "374013000009", "374013",
                "Sving vandret. Bredde t.o.m 625mm udvendig målt. Ex løsdele", 716820);
            await UpsertOperation(session, "A35F7B3F-C6D1-4956-9B3F-1CFDF2BDBAE7", "374013000021", "374013",
                "Stor bøjning vandret 55mm gitterbakke. Ex løsdele", 40860);
            await UpsertOperation(session, "BB49B932-04F3-4FC9-A5B5-16B3191B1F55", "374013000022", "374013",
                "Stor bøjning vandret 75mm gitterbakke. Ex løsdele", 75060);
            await UpsertOperation(session, "24A5DFF0-7330-48FE-96FB-ED7EF6459E6A", "374013000023", "374013",
                "Stor bøjning vandret 125mm gitterbakke. Ex løsdele", 119040);
            await UpsertOperation(session, "35137A9D-25E2-4A83-933E-8020210F8946", "374013000024", "374013",
                "Stor bøjning vandret 155mm gitterbakke. Ex løsdele", 201900);
            await UpsertOperation(session, "EBB124B6-D6EB-4B60-8B34-7FB4F281A1DC", "374013000025", "374013",
                "Stor bøjning vandret 225mm gitterbakke. Ex løsdele", 266460);
            await UpsertOperation(session, "9ECDEC53-BC5C-426E-9EFD-5D9ACFA3D538", "374013000026", "374013",
                "Stor bøjning vandret 325mm gitterbakke. Ex løsdele", 446280);
            await UpsertOperation(session, "5B51D20C-8BE5-4148-B1B1-B3C4422B536E", "374013000027", "374013",
                "Stor bøjning vandret 425mm gitterbakke. Ex løsdele", 695100);
            await UpsertOperation(session, "5DAEEC96-F9EB-4237-B80B-32CDC82E4BFA", "374013000028", "374013",
                "Stor bøjning vandret 525mm gitterbakke. Ex løsdele", 801540);
            await UpsertOperation(session, "EE024536-8950-490D-B669-8007838D4359", "374013000029", "374013",
                "Stor bøjning vandret 625mm gitterbakke. Ex løsdele", 943740);
            await UpsertOperation(session, "349B379F-2775-4F17-B7D0-91728D396F75", "374014000001", "374014",
                "Afgrening. Bredde t.o.m 55mm udvendig målt. Ex løsdele", 167460);
            await UpsertOperation(session, "EA1C2AB8-A40A-49EB-831B-2FAC7F0341FC", "374014000002", "374014",
                "Afgrening. Bredde t.o.m 75mm udvendig målt. Ex løsdele", 167880);
            await UpsertOperation(session, "B9AD259C-3FB7-4EF3-A7CD-775D9498D0CE", "374014000003", "374014",
                "Afgrening. Bredde t.o.m 125mm udvendig målt. Ex løsdele", 168840);
            await UpsertOperation(session, "FF6B9244-5C08-4D35-8E32-073F68982E9D", "374014000004", "374014",
                "Afgrening. Bredde t.o.m 155mm udvendig målt. Ex løsdele", 169440);
            await UpsertOperation(session, "A82C0C0A-E687-4B6A-8FAF-BA18BE868F7C", "374014000005", "374014",
                "Afgrening. Bredde t.o.m 225mm udvendig målt. Ex løsdele", 170820);
            await UpsertOperation(session, "E92BD8C3-2060-422A-91F7-B7935D91533D", "374014000006", "374014",
                "Afgrening. Bredde t.o.m 325mm udvendig målt. Ex løsdele", 172800);
            await UpsertOperation(session, "2D2B5614-B470-4997-B566-405E6CBE78BB", "374014000007", "374014",
                "Afgrening. Bredde t.o.m 425mm udvendig målt. Ex løsdele", 174780);
            await UpsertOperation(session, "EC3F330F-3DEB-4589-84AD-FEC7CA76B043", "374014000008", "374014",
                "Afgrening. Bredde t.o.m 525mm udvendig målt. Ex løsdele", 176760);
            await UpsertOperation(session, "CD72B41D-FF86-4EE5-803C-A2408A0765C7", "374014000009", "374014",
                "Afgrening. Bredde t.o.m 625mm udvendig målt. Ex løsdele", 178740);
            await UpsertOperation(session, "086CF845-B957-4A8C-99CC-57CDB618769D", "374015000001", "374015",
                "Bukning gitterbakke. Bredde t.o.m 55mm udvendig målt. Ex løsdele", 103560);
            await UpsertOperation(session, "F5386A2D-8185-4106-9F07-3642B9AF8CE2", "374015000002", "374015",
                "Bukning gitterbakke. Bredde t.o.m 75mm udvendig målt. Ex løsdele", 106380);
            await UpsertOperation(session, "25B092B1-8191-4620-A53E-952024776207", "374015000003", "374015",
                "Bukning gitterbakke. Bredde t.o.m 125mm udvendig målt. Ex løsdele", 132600);
            await UpsertOperation(session, "FB302A19-A4C9-4E8B-A3B2-4DEAA277F713", "374015000004", "374015",
                "Bukning gitterbakke. Bredde t.o.m 155mm udvendig målt. Ex løsdele", 159060);
            await UpsertOperation(session, "4E1E9B83-BCEE-448E-9E84-7E8F74013398", "374015000005", "374015",
                "Bukning gitterbakke. Bredde t.o.m 225mm udvendig målt. Ex løsdele", 190560);
            await UpsertOperation(session, "B3A47B9C-5F0B-4CA1-942F-30E7ED1BB2BF", "374015000006", "374015",
                "Bukning gitterbakke. Bredde t.o.m 325mm udvendig målt. Ex løsdele", 180600);
            await UpsertOperation(session, "9B78BD82-61A3-4A2C-A2F0-C7B783FD6B87", "374015000007", "374015",
                "Bukning gitterbakke. Bredde t.o.m 425mm udvendig målt. Ex løsdele", 336720);
            await UpsertOperation(session, "A4D35E6E-DA05-4FA4-842D-868436FF0A7D", "374015000008", "374015",
                "Bukning gitterbakke. Bredde t.o.m 525mm udvendig målt. Ex løsdele", 402540);
            await UpsertOperation(session, "8B801380-9A08-4284-AF70-91D94186C226", "374015000009", "374015",
                "Bukning gitterbakke. Bredde t.o.m 625mm udvendig målt. Ex løsdele", 468360);
            await UpsertOperation(session, "0D77BAE3-76E6-4914-A961-946D71454A05", "379921000084", "379921", "Antal befæstigelser i loftophæng",
                140220);
            await UpsertOperation(session, "5A60B38A-97F4-4E84-9A16-BC88659EA970", "379921000085", "379921", "Antal befæstigelser direkte i beton",
                280620);
            await UpsertOperation(session, "34C4712F-2423-4945-B274-5ECCBD7B2D23", "379922000021", "379922", "Antal befæstigelser ud over 2 Kode 25",
                46800);
            await UpsertOperation(session, "28AECEC0-26E6-4DBC-B8C8-E464C491BC8D", "379922000022", "379922", "Antal befæstigelser ud over 2 Kode 30",
                46800);
            await UpsertOperation(session, "EA303B2A-505D-41DD-81ED-2D059B6F0C0B", "379922000023", "379922", "Antal befæstigelser ud over 2 Kode 60",
                92820);
            await UpsertOperation(session, "9379FAB2-BD0F-4C75-8FC8-716F384EEC1A", "379922000024", "379922", "Antal befæstigelser ud over 2 Kode 70",
                65100);
            await UpsertOperation(session, "0CC8264F-B111-40D1-B5C0-12B694A4BF72", "379922000025", "379922", "Antal befæstigelser ud over 2 Kode 71",
                104220);
            await UpsertOperation(session, "C524CC39-17E7-4A0A-BAB2-FC69E7513E03", "379922000026", "379922", "Antal befæstigelser ud over 2 Kode 80",
                117420);
            await UpsertOperation(session, "C9F697FC-74A0-4B4D-896E-F57EB72AEE8D", "379922000027", "379922", "Antal befæstigelser ud over 2 Kode 85",
                58560);
            await UpsertOperation(session, "536ABF98-AAB0-4DBA-8C0C-D58F893013FD", "379929000003", "379929", "Overskæring gevindstang 10mm", 40320);
            await UpsertOperation(session, "9340CACA-D6F0-476B-9D25-4680F73491A6", "379929000005", "379929",
                "Overskæring profilskinne o.l. <=50X50X3mm", 101880);
            await UpsertOperation(session, "91431260-F480-4D39-82CC-69316CF9F39B", "379929000009", "379929", "Overskæring af pendler", 63840);
            await UpsertOperation(session, "A6AC58A3-6D4C-4B76-80BD-83CED4ADBD27", "379929000010", "379929",
                "Overskæring af pendler o/2,5x4 t.o.m. 6x6cm", 55380);
            await UpsertOperation(session, "DD2A298C-F98E-41D1-9768-F19D748402BD", "379929000011", "379929", "Overskæring af dobb. pendel", 64260);
            await UpsertOperation(session, "198A5F37-E115-4490-9AB0-0F3E3D1EB243", "379960000002", "379960", "Overskæring deleskinner", 71220);
            await UpsertOperation(session, "9B1416B8-1A70-441E-870C-E6104E2DF18C", "379960000010", "379960", "Overskæring skillespor", 78960);
            await UpsertOperation(session, "DA924DBE-20F4-43F2-AF29-5257ECF6B576", "379960000013", "379960",
                "Isætte medfølg.samlebeslag type RV (OBO)", 22440);
            await UpsertOperation(session, "6E1C95ED-7932-4011-BFF7-E6CA2F43F0E0", "379990000001", "379990", "Ilægge kantbånd", 196560);
            await UpsertOperation(session, "2F12FF47-3CC8-463B-B9DA-5B7CD59D1598", "379990000002", "379990", "Male overskæringer og huller", 16620);
            await UpsertOperation(session, "4A06E85F-4309-426D-92C3-7838F0CB3BF4", "397000000050", "397000", "Nedhængt i 2 stk. wire incl. opretning",
                185100);
            await UpsertOperation(session, "EE66E6D8-4B7B-49F8-8C52-057F79A1293F", "397000000051", "397000", "Wire udover 2 stk., incl.opretning.",
                92580);
            await UpsertOperation(session, "260E2D2C-72EF-4CC0-B8C7-610C4284EE93", "422001000011", "422001",
                "Skruetid for aflastning af flexrør pr. spot", 32280);
            await UpsertOperation(session, "ECAF5484-B11B-4D35-8D7E-4955B96BF58F", "422001000012", "422001", "Videresløjfning med kabel ell. flexrør",
                77640);
            await UpsertOperation(session, "19954C76-0D87-4218-AB3E-49D4DFCEBC59", "422001000031", "422001",
                "Downlight/spot <=10cm (Ø indbygningshul), opsat incl. 6  ledn. forb.", 55980);
            await UpsertOperation(session, "C028EA0B-A9CC-4D6D-A2F3-9305346F2E12", "430500000003", "430500",
                "Tilslutte indsats/LED-skærm m. stikforb.", 25980);
            await UpsertOperation(session, "CBB06450-67A7-4BB1-9453-4D2F138B8450", "452000000004", "452000", "Overskæring lys-/strømskinne", 118500);
            await UpsertOperation(session, "FBA8DACD-CBFE-4C8F-B35A-F6AD35D393A2", "452000000014", "452000", "Overskæring lys-/strømskinne", 118500);
            await UpsertOperation(session, "86071AFB-CAEC-4F14-8505-80757F3845FD", "460100000001", "460100",
                "Lysrørsarmatur <=150cm, <=10kg, opsat incl. 5 ledn. forb.", 401100);
            await UpsertOperation(session, "A8EC418C-A9C9-4D87-81ED-8801143CC3D6", "460100000002", "460100",
                "Lysrørsarmatur <=150cm, <=10kg, løst oplagt incl. 5 ledn. forb.", 103560);
            await UpsertOperation(session, "6854F947-B73A-4E07-8D56-87ABCDA728B5", "460100000003", "460100",
                "Lysrørsarmatur o/150-200cm, <=10kg, opsat incl. 5 ledn. forb.", 426600);
            await UpsertOperation(session, "D203844D-208B-458C-A41A-48E3BE3A76CB", "460100000012", "460100",
                "Klargøring af armatur. Adskille og montere fatninger, endeplader o.l.", 49800);
            await UpsertOperation(session, "1575057F-07CF-4FFF-ABD3-AA351837404E", "460100000013", "460100", "Ledn.forb. ud over 5 i skrueklemme",
                20520);
            await UpsertOperation(session, "6DCA0F80-D245-467E-9B10-54C155B05E72", "460100000014", "460100", "Ledn.forb. ud over 5 i fjederklemme",
                10380);
            await UpsertOperation(session, "7F123293-9069-4E2E-8F9F-36C62E9BBDB4", "460100000021", "460100",
                "Udpakke armatur, reflektor, skærm, gitter m.m.", 18660);
            await UpsertOperation(session, "0DDBE2C3-F705-43FE-A710-C751DB084D53", "460200000001", "460200",
                "Lysrørsarmatur stkt. <=150cm, <=10kg, opsat incl. 5 ledn. forb.", 597780);
            await UpsertOperation(session, "B8786422-2F9D-4CF3-B854-8DBD31093796", "460200000011", "460200",
                "Udpakke armatur, reflktor,skærm, gitter m.m.", 18660);
            await UpsertOperation(session, "7D0FC043-17CC-4A0F-AD93-7C53AB797578", "460200000012", "460200",
                "Klargøring af stænktæt armatur. Adskille og montere fatninger, endeplader o.l.", 67020);
            await UpsertOperation(session, "E48C5988-95F8-4146-B273-95FEFD05700F", "460200000013", "460200", "Ledn. forb. ud over 6 i skrueklemme",
                20520);
            await UpsertOperation(session, "1F8F3EB9-26CE-4CB8-BBCA-89706309B466", "460500000001", "460500", "Påsætte skærm, glas eller gitter",
                59340);
            await UpsertOperation(session, "F0D80120-7746-4967-84BA-AD60B9D29E62", "460500000004", "460500", "Fjerne beskyttelsesfilm/-folie", 26400);
            await UpsertOperation(session, "36BBB0CF-699B-4966-8825-9DE6670E7F52", "460500000005", "460500", "Montere opsætningsbeslag", 53280);
            await UpsertOperation(session, "57088AB6-E5C2-47BD-8365-33F6AD0F2B5E", "460500000006", "460500",
                "Påsat medfølgende \"clips\" på skærm (pr. stk.)", 10620);
            await UpsertOperation(session, "428BF09D-B796-4901-AAAC-7D40E0D7DC9C", "460600000002", "460600", "Montere reflektor/parabol", 53460);
            await UpsertOperation(session, "1A08D95A-C650-4A45-8933-D5E7A6875739", "460600000004", "460600", "Montere opsætningsbeslag", 21060);
            await UpsertOperation(session, "E4F33055-68C1-4654-AE12-38AC88A5F1E6", "461005000002", "461005",
                "Overskæring af blændstykke - Plast, pr. snit", 65880);
            await UpsertOperation(session, "4C26A114-E6BF-4972-B36E-66BA5AAB4931", "499990000001", "499990",
                "Ledningsforbindelse i armatur med skrueklemme indtil 1x1,5#", 20520);
            await UpsertOperation(session, "FA8B8C2F-C2E0-4E48-8972-B38591B531F1", "499990000010", "499990",
                "Ledningsforbindelse i armatur med fjederklemme indtil 1x1,5#", 10380);
            await UpsertOperation(session, "70913A78-1FCB-4D25-8C80-A3533A774880", "499990000020", "499990",
                "Ledningsforbindelse i trafo ell.lign. med skrueklemme 1x1,5#", 51360);
            await UpsertOperation(session, "351EF07D-DABB-4A82-99F2-E8B1AA0F7CB1", "499990000050", "499990",
                "Tilslutning af armatur med 3x1,5# excl. håndtering af tilledning.", 100080);
            await UpsertOperation(session, "D049AD75-FEDD-49D1-A526-97368D91A282", "499990000051", "499990", "Ledn.forb ud over 5 pr. leder", 20520);
            await UpsertOperation(session, "5F764880-941B-44E3-B048-09262D4F95F2", "499990000052", "499990", "Ledn.forb qc. ud over 5 Pr. leder",
                10380);
            await UpsertOperation(session, "3B6277B5-B855-4F02-820C-753617993FC5", "50602000010", "050601", "Hul i kunststof o/2-4 mm. Ø -10 mm",
                17100);
            await UpsertOperation(session, "FEF10D55-62EC-4EB6-9B0A-0A8B28ABAD06", "50602000011", "050601", "Hul i kunststof <=2 mm, Ø o/10-20 mm",
                18600);
            await UpsertOperation(session, "6E3EAEB0-092E-4C01-9117-FB1861C94B47", "50602000012", "050601", "Hul i kunststof <=2 mm, Ø o/20-30 mm",
                20100);
            await UpsertOperation(session, "A4E07E11-909A-47A4-9B5E-E45C893DC7A8", "50602000013", "050601", "Hul i kunststof <=2 mm, Ø o/30-40 mm",
                21660);
            await UpsertOperation(session, "28C472B1-025A-4857-B848-86E213DEE76F", "50602000014", "050601", "Hul i kunststof <=2 mm, Ø o/40-60 mm",
                24660);
            await UpsertOperation(session, "6B5EE327-2DBA-46C3-913F-4EC33BBCB88B", "50602000015", "050601", "Hul i kunststof <=2 mm, Ø o/60-80 mm",
                27660);
            await UpsertOperation(session, "F4EFA823-07EF-4A6E-B89E-D01DB38FC3D7", "50602000016", "050601", "Hul i kunststof <=2 mm, Ø o/80-100 mm",
                30660);
            await UpsertOperation(session, "D38FF184-9C3E-4E0F-A40C-C48623997860", "50602000017", "050601", "Hul i kunststof <=2 mm, Ø o/100-125 mm",
                34440);
            await UpsertOperation(session, "0AB67AB9-CA32-4FC6-A0A9-8451FFC32B60", "510000000001", "510000", "Lysstofrør t.o.m. Ø16 og 25W", 13260);
            await UpsertOperation(session, "7C6E5A25-CAF7-4590-BD2E-A8E4DE9FB48B", "510000000002", "510000", "Lysstofrør t.o.m. Ø16 over 25W", 19140);
            await UpsertOperation(session, "56ADB632-6B2E-4236-87F6-70D175A73B16", "510001000001", "510001",
                "Armatur leveret med lyskilde t.o.m. Ø16 og 25W", 13260);
            await UpsertOperation(session, "03B183BD-CFAA-4929-B5B5-FF76FE5B9D90", "601000000001", "601000",
                "Tilslutte motor med indtil 3x t.o.m. 2,5# incl. kortslutningsskinne", 205740);
            await UpsertOperation(session, "5F3CA8B6-4D77-46FA-89A4-E0FA008925AD", "601000000010", "601000",
                "Tilslutte motor med 4-5x t.o.m. 1,5#  incl. kortslutningsskinne", 431460);
            await UpsertOperation(session, "480E77B4-0B5B-4D09-85C0-0500DC4DD5F3", "601000000011", "601000",
                "Tilslutte motor med 4-5x2,5#  incl. kortslutningsskinne", 536220);
            await UpsertOperation(session, "CAB343D5-3F69-4420-B99E-38B544646E97", "601000000012", "601000",
                "Tilslutte motor med 4-5x4-6#  incl. kortslutningsskinne", 647580);
            await UpsertOperation(session, "E716339D-E1F7-40CD-8E30-A5D03BC7E3FD", "601000000040", "601000",
                "Tilslutte cirkulationspumpe ell. lign. m 2/3 pol stik", 254340);
            await UpsertOperation(session, "245E26F9-D3A7-4130-890C-5B99E11A8E0C", "651500000001", "651500", "Afdække hvis det kræves", 48060);
            await UpsertOperation(session, "7C58619F-38EA-4E78-B0C7-9DB824273987", "651501000005", "651501", "Bundplade af og på for forarbejdn.",
                117000);
            await UpsertOperation(session, "2A49FAC3-709D-4E01-9970-B802812EB8C1", "651510000001", "652000",
                "Forbinde skærm Kabeldiam.  t.o.m. ø12mm", 252420);
            await UpsertOperation(session, "9D7E6EC0-E444-4471-9BC3-371717F1834E", "651510000002", "652000",
                "Forbinde skærm Kabeldiam. over 12 t.o.m. 20mm", 382140);
            await UpsertOperation(session, "D1BF0E12-1D0A-4E89-8634-5BCB65945FFD", "651510000004", "652000", "Ledn.forb. 3-5x1,5-2,5#", 388200);
            await UpsertOperation(session, "DCA91041-C8E7-4B4F-90C4-7BCB6C54F4F6", "651510000005", "652000", "Ledn.forb. 3-5x4#", 470040);
            await UpsertOperation(session, "AC916E9D-2D3B-42DD-8E4B-57C1BEC59227", "651510000006", "652000", "Ledn.forb. 3-5x6#", 544860);
            await UpsertOperation(session, "A0529EF8-A043-4D70-A792-956256316697", "651510000020", "652000", "Ledn.forb. 1x1,5-2,5#", 77520);
            await UpsertOperation(session, "7D151EB4-EB0E-4933-AADB-34A86D5641D6", "651510000021", "652000", "Ledn.forb. 1x4#", 129840);
            await UpsertOperation(session, "AE9839A5-0D47-4DEB-8B45-3E3926A9E9FA", "651510000022", "652000", "Ledn.forb. 1x6#", 129840);
            await UpsertOperation(session, "B6FDC063-3DC3-49DB-B84E-0C0919D8ADD0", "651510000023", "652000", "Ledn.forb. 1x10#", 199560);
            await UpsertOperation(session, "9848514B-B925-4796-BCA5-7A1713E301D4", "651510000030", "652000",
                "Svagstrøms-/signalkabel 2-5x0,2-1# i klemmer", 256020);
            await UpsertOperation(session, "4B2A5265-9307-4276-8405-0401551BE18E", "651510000032", "652000",
                "Svagstrøms-/signalkabel 2-8x0,2-1# forbundet med stik", 35820);
            await UpsertOperation(session, "9889DC17-5832-412E-8354-57F7CA94F4EF", "651510000035", "652000", "Etablere stelforb. m 10#", 123900);
            await UpsertOperation(session, "7102CE02-45B6-4355-BAF8-89511CC27B2B", "710000000025", "710000", "Udpakning køleskab/skabsfryser",
                152400);
            await UpsertOperation(session, "072E6384-194C-4646-AD76-215E9CB88565", "710000000026", "710000",
                "Køleskab/skabsfryser opstillet og tilsluttet i stikk.", 236400);
            await UpsertOperation(session, "3B36387E-D79D-400D-B6D4-EB24462ECAB8", "710000000037", "710000", "Afdækningsliste", 151020);
            await UpsertOperation(session, "7793EC0B-E8D7-4317-980D-730066B81771", "710000000038", "710000",
                "Montere ventilationsrist over køle-/fryseskab", 323280);
            await UpsertOperation(session, "6702DF94-1281-4B6E-83B9-5F9AE8D1BDA5", "720000000005", "720000", "Udpakning nedfældningskomfur", 84240);
            await UpsertOperation(session, "DEF78A82-FB0C-4184-9EC7-073454751515", "720000000006", "720000",
                "Komfur nedfældning ilagt og tilsluttet i dåse/udtag", 788160);
            await UpsertOperation(session, "958F12C3-6FF3-44A1-9CAE-AB469021D3F2", "720000000017", "720000", "Udpakning indbygningsovn", 115500);
            await UpsertOperation(session, "5E7D4911-3A51-46F4-8A42-3E6D9E52C310", "720000000018", "720000",
                "Indbygningsovn indsat og tilsluttet i dåse/udtag", 651600);
            await UpsertOperation(session, "9DEF5E89-C224-4365-ADE2-D67E9B0819DD", "751000000019", "751000", "Udpakning opvasker", 143280);
            await UpsertOperation(session, "5862FBAA-E325-4EB5-B112-F691AB15391B", "751000000021", "751000",
                "Opvasker opstillet og tilsluttet i dåse/udtag", 1962180);
            await UpsertOperation(session, "822679F5-2987-41D6-846F-D4E02D1EF863", "751000000022", "751000", "Integreret front på opvasker", 836640);
            await UpsertOperation(session, "CF908D0B-5CD5-472E-95D9-321F812EAAD7", "751000000023", "751000", "Dampbeskytter påklæbet", 110880);
            await UpsertOperation(session, "A42E88B0-24E9-423C-8C48-C8BB5AF4AE54", "751000000024", "751000", "Dampbeskytter skruet/sømmet op",
                331440);
            await UpsertOperation(session, "C385BDE4-5BA3-4A36-9EDE-0F6B537EF419", "760000000040", "760000", "Vaskemaskine opstillet og tilsluttet",
                730140);
            await UpsertOperation(session, "E62D9716-0D63-4963-96AC-EAEF9935157B", "782000000006", "782000", "Udpakning emhætte", 205860);
            await UpsertOperation(session, "DBDD731A-CF40-4BBE-A314-1E01B665A728", "782000000011", "782000",
                "Montere opsætningsbeslag på emhætte/skab/væg", 182460);
            await UpsertOperation(session, "CAF913E0-A21F-4804-B66B-C9BDDA8D4B2D", "782000000012", "782000", "Opsætte emhætte på væg", 766140);
            await UpsertOperation(session, "317C13F9-976C-4AD8-A6B1-EF2DA90AE432", "782000000013", "782000",
                "Opsætte emfang på væg ell. frit hængende", 1362900);
            await UpsertOperation(session, "C44D4237-EBC1-4314-94C6-051A0164116A", "782000000015", "782000", "Flange for aftræk mont. på emhætte",
                58260);
            await UpsertOperation(session, "610F528B-2909-4D97-BC95-70F5EB3175A6", "782000000016", "782000", "Montere aftræksslange ø110-156mm",
                269760);
            await UpsertOperation(session, "60514BB1-B458-42A4-980A-BECD91573440", "850100000001", "850100",
                "Magnetlledning fremført til samlingssted. Pr. m.", 42600);
            await UpsertOperation(session, "6CA06EDB-0947-4432-B71F-57ABBAF8602B", "850100000003", "850100", "Åbne vindue, m. motor.", 55920);
            await UpsertOperation(session, "680F9F87-A824-4907-BEAD-B884EEA68540", "850100000004", "850100", "Lukke vindue, m. motor.", 48540);
            await UpsertOperation(session, "25BDD7D7-E333-4530-B805-9AE29C1D5EDE", "850300000011", "850300", "Udrulle opkvejlet rør", 30960);
            await UpsertOperation(session, "6824542D-AFC7-4075-8BEC-62A5448444E9", "850300000012", "850300",
                "Opkvejle overskydende rør. Hvis påkrævet", 32340);
            await UpsertOperation(session, "CE897401-8BF4-4A4A-802F-68A85D1CBCEB", "850300000013", "850300", "Fixere rør ved detekt., lydg. ell.lign",
                33660);
            await UpsertOperation(session, "C47D26EA-C8C6-4C6C-89BB-06B08C0A1877", "850300000101", "850300", "Kode og isætte adressekort", 23100);
            await UpsertOperation(session, "66ACA159-D213-42F7-883A-343D6DAC63D5", "850300000102", "850300", "Påklistre mærkning", 29160);
            await UpsertOperation(session, "183C003C-7F72-4CB3-B936-538C8871F586", "861000000001", "861000", "Tillæg for skærmet kabel, eks. kat7",
                36840);
            await UpsertOperation(session, "3830C1E4-0CC9-4527-8217-E2620277D8AF", "900100000011", "900100", "Ledningsforb. sv.str. 0,1-1# Pr. leder",
                68640);
            await UpsertOperation(session, "B07CDC97-9C77-4104-9442-B0865F10EA49", "900500000001", "900500", "Følere med indtil 3 ledn. forb. 0,2-1#",
                330660);
            await UpsertOperation(session, "3D4BE56C-5D20-4621-B38D-BB429567FFC1", "900500000003", "900500", "Følere med over 3-6 ledn. forb. 0,2-1#", 518820);
            await UpsertOperation(session, "9C46B5C3-E06D-4B63-A3BC-F399D2569F9D", "900500000012", "900500", "Tilslutte motorventil/aktuator m o/ 6-9 ledere t.o.m. 1#", 511560);
            await UpsertOperation(session, "82EE9C91-43E8-47E1-BC06-728A279CE051", "900701000021", "900701", "Splicesøm isat holder", 15240);

            await session.SaveChangesAsync(cancellation);
        }

        private static Task UpsertOperation(IDocumentSession session, string guid, string number, string group, string text, decimal value)
        {
            var operation = Operation.Create(OperationId.Create(Guid.Parse(guid)), OperationNumber.Create(number), OperationText.Create(text),
                OperationTime.Create(value));

            session.Delete(operation);
            session.Store(operation);

            return Task.CompletedTask;
        }
    }
}
