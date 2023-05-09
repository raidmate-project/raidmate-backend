using Amazon.CDK;

namespace Cdk;

public sealed class Program
{
	public static void Main(string[] args)
	{
		var app = new App();
		_ = new RaidmateStack(app, "RaidmateStack", new StackProps
		{
			StackName = "RaidmateBackendStack"
		});

		app.Synth();
	}
}
