import { rest } from "msw";
import { GetApiProjectsByProjectIdApiResponse } from "api/generatedApi";

const InterceptProjectsById = rest.get("https://localhost:5001/api/projects/123", (req, res, ctx) => {
  const folder: GetApiProjectsByProjectIdApiResponse = { description: "yo", title: "MWS" };
  return res(ctx.json(folder));
});

export default InterceptProjectsById;
