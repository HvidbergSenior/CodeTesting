import { rest } from "msw";
import { GetGroupedWorkItemsQueryResponse } from "api/generatedApi";

const InterceptGroupedItemsWidget = rest.get("https://localhost:5001/api/projects/123/folders/*/workitems/grouped?maxHits=3", (req, res, ctx) => {
  const groupedItems: GetGroupedWorkItemsQueryResponse = {
    groupedWorkItems: [
      { amount: 20, paymentDkr: 300, text: "[mock] Hul i mur", id: "12423432" },
      { amount: 40.5, paymentDkr: 5000, text: "[mock] Lampeudtag", id: "153352" },
      { amount: 40000, paymentDkr: 5000.42, text: "[mock] Kabellægning", id: "543413" },
    ],
  };
  //return res(ctx.delay(1000), ctx.json(groupedItems));
  return res(ctx.delay(2000), ctx.status(404));
  //return res(ctx.delay(2000), ctx.status(200));
});

const InterceptGroupedItemsTable = rest.get("https://localhost:5001/api/projects/123/folders/*/workitems/grouped?maxHits=50", (req, res, ctx) => {
  const groupedItems: GetGroupedWorkItemsQueryResponse = {
    groupedWorkItems: [
      { amount: 20000, paymentDkr: 3000000, text: "[mock] Hul i mur", id: "1423534542" },
      { amount: 40, paymentDkr: 4363.65, text: "[mock] Lampeudtag", id: "1354364363" },
      { amount: 432, paymentDkr: 325.23, text: "[mock] Hul i beton", id: "76585656" },
      { amount: 32, paymentDkr: 546.35, text: "[mock] Samlemuffe", id: "764457743" },
      { amount: 120.52, paymentDkr: 125.5, text: "[mock] Stikkontakt", id: "43563463" },
      { amount: 53, paymentDkr: 543, text: "[mock] Kabellægning", id: "32526266" },
      { amount: 43, paymentDkr: 655.7, text: "[mock] Hul i letbeton", id: "33534643" },
      { amount: 32, paymentDkr: 652.4, text: "[mock] Stor samlemuffe", id: "2223225231" },
      { amount: 98, paymentDkr: 865.4, text: "[mock] Hul i træ", id: "543364333" },
      { amount: 34, paymentDkr: 54.7, text: "[mock] Hul i gips", id: "2145355" },
      { amount: 54, paymentDkr: 87.4, text: "[mock] Loftlampe", id: "62627677" },
      { amount: 21, paymentDkr: 42, text: "[mock] Kabeludtræk", id: "1235343" },
    ],
  };
  return res(ctx.delay(1000), ctx.json(groupedItems));
});

const InterceptGroupedWorkItems = { InterceptGroupedItemsTable, InterceptGroupedItemsWidget };
export default InterceptGroupedWorkItems;
