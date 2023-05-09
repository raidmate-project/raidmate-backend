using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Potential Code Quality Issues", "RECS0026:Possible unassigned object created by 'new'", Justification = "Constructs add themselves to the scope in which they are created")]
[assembly: SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix", Justification = "We want to name them *Stack", Scope = "namespaceanddescendants", Target = "~N:Cdk")]
