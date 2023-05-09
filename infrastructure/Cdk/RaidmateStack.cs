using System.Collections.Generic;
using System.IO;
using Amazon.CDK;
using Amazon.CDK.AWS.APIGateway;
using Amazon.CDK.AWS.IAM;
using Amazon.CDK.AWS.Lambda;
using Constructs;

namespace Cdk;

public class RaidmateStack : Stack
{
	private const string AmazonApiGatewayInvokeFullAccess = "AmazonAPIGatewayInvokeFullAccess";

	internal RaidmateStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
	{
		var clientId = Node.TryGetContext("clientId") as string;
		var clientSecret = Node.TryGetContext("clientSecret") as string;

		var raidmateFunction = new Function(this, "RaidmateBackend", new FunctionProps
		{
			Runtime = Runtime.DOTNET_6,
			Code = Code.FromAsset(Path.Join("output", "RaidmateBackend.zip")),
			Handler = "RaidmateBackend",
			Timeout = Duration.Seconds(30),
			MemorySize = 512,
			Environment = new Dictionary<string, string>
			{
				{ "BLIZZARD_CLIENT_ID", clientId },
				{ "BLIZZARD_CLIENT_SECRET", clientSecret }
			}
		});

		raidmateFunction.Role?.AddManagedPolicy(
				ManagedPolicy.FromAwsManagedPolicyName(AmazonApiGatewayInvokeFullAccess));

		_ = new Alias(this, "raidmateAlias", new AliasProps
		{
			AliasName = "live",
			Version = raidmateFunction.CurrentVersion
		});

		_ = new LambdaRestApi(this, "RaidmateBackendRestApi", new LambdaRestApiProps
		{
			RestApiName = "Raidmate Backend API",
			Handler = raidmateFunction
		});
	}
}
