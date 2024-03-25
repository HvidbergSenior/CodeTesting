import { rest } from "msw";

const InterceptBaseSupplements = rest.put("https://localhost:5001/api/projects/*/folders/*/basesupplements", (req, res, ctx) => {
  return res(ctx.status(200));
});

export default InterceptBaseSupplements;
