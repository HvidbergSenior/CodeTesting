import { rest } from "msw";
import { ConfigResponse } from "api/generatedApi";

const InterceptConfig = rest.get("https://localhost:5001/api/config", (req, res, ctx) => {
  const config: ConfigResponse = {
    azureAdB2C: {
      clientId: "669449fd-20a0-49f0-8544-350a0ab27f47",
      authority: "https://akkordplusdk.b2clogin.com/akkordplusdk.onmicrosoft.com/B2C_1_test_flow",
      knownAuthority: "akkordplusdk.b2clogin.com",
    },
    featureFlags: {},
  };
  return res(ctx.json(config));
});

export default InterceptConfig;
