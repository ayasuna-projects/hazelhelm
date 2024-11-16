namespace Ayasuna.Hazelhelm.Analyzers;

using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

/// <inheritdoc />
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class UnitAnalyzer : DiagnosticAnalyzer
{
    private static readonly DiagnosticDescriptor Rule = new
    (
        "AYASUNA_HAZELHELM_1000",
        "Duplicate interfaces found",
        "This interface shares it's name with {0} other interfaces that only differ in their generic type parameters, consider replacing all of them with one interface by using the 'Unit' type as placeholder type parameter",
        "Ayasuna.Design",
        DiagnosticSeverity.Info,
        true
    );

    /// <inheritdoc />
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    /// <inheritdoc />
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        context.RegisterSyntaxNodeAction(AnalyzeSyntaxNode, SyntaxKind.InterfaceDeclaration);
    }

    /// <summary>
    /// Analyzes the syntax node identified by the given <paramref name="ctx"/>
    /// </summary>
    /// <param name="ctx">The syntax node analysis context</param>
    private void AnalyzeSyntaxNode(SyntaxNodeAnalysisContext ctx)
    {
        var cancellationToken = ctx.CancellationToken;
        var declaredInterfaceSymbol = ctx.SemanticModel.GetDeclaredSymbol((InterfaceDeclarationSyntax)ctx.Node, cancellationToken)!;

        var canonicalInterfaceName = GetCanonicalInterfaceName(declaredInterfaceSymbol);

        var allInterfaceInSameNamespaceWithSameCanonicalName = declaredInterfaceSymbol
                .ContainingNamespace
                .GetTypeMembers()
                .Where(currentInterfaceSymbol => GetCanonicalInterfaceName(currentInterfaceSymbol).Equals(canonicalInterfaceName))
                .ToImmutableList()
            ;

        if (allInterfaceInSameNamespaceWithSameCanonicalName.Count <= 1)
        {
            return;
        }

        // If we found more than one interface with the same canonical name in the same namespace check if all those interfaces also have the same members and only define supported type constraints for their type parameters, if so we consider them mergeable

        var filteredInterfaces = allInterfaceInSameNamespaceWithSameCanonicalName
                .Where
                (
                    currentInterfaceSymbol => declaredInterfaceSymbol.MemberNames.All(currentInterfaceSymbol.MemberNames.Contains) &&
                                              currentInterfaceSymbol.MemberNames.All(declaredInterfaceSymbol.MemberNames.Contains)
                )
                .Where(HasOnlySupportedTypeConstraints)
                .ToImmutableList()
            ;

        if (filteredInterfaces.Count != allInterfaceInSameNamespaceWithSameCanonicalName.Count)
        {
            // At least one interface has different members or defines unsupported type constraints for it's type parameters
            return;
        }

        ctx.ReportDiagnostic
        (
            Diagnostic.Create
            (
                Rule,
                ctx.Node.GetLocation(),
                filteredInterfaces.Count - 1
            )
        );
    }

    /// <summary>
    /// Checks whether the given (interface) type <paramref name="symbol"/> only has defined supported generic type constraints for its type parameters. <br/>
    /// If the type is no generic type to begin with, this method also returns <c>true</c>. 
    /// </summary>
    /// <param name="symbol">The symbol to check</param>
    /// <returns>True if the given type is no generic type or if the type only defines supported generic type constraints for it's type parameters</returns>
    private static bool HasOnlySupportedTypeConstraints(INamedTypeSymbol symbol)
    {
        if (symbol.IsGenericType)
        {
            foreach (var typeParameter in symbol.TypeParameters)
            {
                if (typeParameter.ConstraintTypes.Length > 0)
                {
                    return false;
                }

                if (typeParameter.HasReferenceTypeConstraint)
                {
                    return false;
                }

                if (typeParameter.HasUnmanagedTypeConstraint)
                {
                    return false;
                }
            }
        }

        return true;
    }

    /// <summary>
    /// Gets the canonical interface name of the given <paramref name="symbol"/>, which is expected to represent an interface type
    /// </summary>
    /// <param name="symbol">The interface type</param>
    /// <returns>The canonical name of the interface</returns>
    private static string GetCanonicalInterfaceName(INamedTypeSymbol symbol)
    {
        return symbol.Name;
    }
}