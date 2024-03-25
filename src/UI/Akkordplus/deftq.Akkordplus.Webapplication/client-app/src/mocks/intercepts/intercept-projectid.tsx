import { rest } from "msw";
import { GetProjectResponse } from "api/generatedApi";

const InterceptProjectId = rest.get("https://localhost:5001/api/projects/123", (req, res, ctx) => {
  const project: GetProjectResponse = {
    description: "Project desc",
    id: "123",
    participants: [],
    title: "Mock project",
    pieceworkType: undefined,
    currentUserRole: "ProjectOwner",
  };
  return res(ctx.json(project));
});

export default InterceptProjectId;
