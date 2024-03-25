import { rest } from "msw";

const InterceptCopyMeasurementsOne = rest.post("https://localhost:5001/api/projects/123/folders/*/workitems/copy", (req, res, ctx) => {
  return res(ctx.json({ undefined }), ctx.status(200));
});

const InterceptCopyMeasurementsTwo = rest.post("https://localhost:5001/api/projects/123/folders/1.1/workitems/copy", (req, res, ctx) => {
  return res(ctx.json({ undefined }), ctx.status(500));
});

const InterceptCopyMeasurements = { InterceptCopyMeasurementsOne, InterceptCopyMeasurementsTwo };
export default InterceptCopyMeasurements;
