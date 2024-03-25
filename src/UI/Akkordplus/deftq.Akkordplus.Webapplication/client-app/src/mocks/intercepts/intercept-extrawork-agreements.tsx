import { rest } from "msw";

const InterceptExtraWorkAgreements = rest.get("https://localhost:5001/api/projects/*/extraworkagreements/rates  ", (req, res, ctx) => {
  return res(ctx.status(200));
});
export default InterceptExtraWorkAgreements;
