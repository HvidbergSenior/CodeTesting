import { rest } from "msw";

const InterceptBaseRate = rest.put("https://localhost:5001/api/projects/*/folders/*/baserate", (req, res, ctx) => {
  return res(ctx.status(500));
});

export default InterceptBaseRate;
