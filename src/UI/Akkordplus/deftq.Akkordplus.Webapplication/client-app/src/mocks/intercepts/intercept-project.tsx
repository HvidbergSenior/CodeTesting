import { rest } from "msw";
import { GetApiProjectsApiResponse } from "api/generatedApi";

const InterceptProjects = rest.get("https://localhost:5001/api/projects", (req, res, ctx) => {
  const project: GetApiProjectsApiResponse = {
    projects: [{ description: "Hej", projectId: "123", projectName: "Mock project", pieceworkType: "TwelveOneA" }],
  };
  return res(ctx.json(project));
});

export default InterceptProjects;
