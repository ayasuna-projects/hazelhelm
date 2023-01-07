namespace Ayasuna.Hazelhelm.Analyzers.Tests;

using System.Threading.Tasks;
using Xunit;
using Verify = Microsoft.CodeAnalysis.CSharp.Testing.XUnit.AnalyzerVerifier<UnitAnalyzer>;

public sealed class UnitAnalyzerTests
{
    private const string DiagnosticId = "AYASUNA_HAZELHELM_1000";

    /// <summary>
    /// Test whether the <see cref="UnitAnalyzer"/> reports <b>no</b> diagnostic for a single unique interface
    /// </summary>
    [Fact]
    public async Task No_diagnostic_should_be_reported_for_a_single_unique_interface()
    {
        await Verify.VerifyAnalyzerAsync
        (
            @"
    public interface ITest
    {
        string Property { get; }
    }
"
        );
    }

    /// <summary>
    /// Test whether the <see cref="UnitAnalyzer"/> reports <b>no</b> diagnostic for a multiple unique interfaces
    /// </summary>
    [Fact]
    public async Task No_diagnostic_should_be_reported_for_a_multiple_unique_interfaces()
    {
        await Verify.VerifyAnalyzerAsync
        (
            @"
    public interface ITest1
    {
    }

    public interface ITest2
    {
    }
"
        );
    }

    /// <summary>
    /// Test whether the <see cref="UnitAnalyzer"/> reports <b>no</b> diagnostic for a "duplicate" interface if at least one type parameter (on any interface) has a type constraint
    /// </summary>
    [Fact]
    public async Task No_diagnostic_should_be_reported_for_a_duplicate_interface_when_at_least_one_type_parameter_has_a_class_constraint()
    {
        await Verify.VerifyAnalyzerAsync
        (
            @"
    public interface ITest
    {
        string Property { get; }
    }

    public interface ITest<T> where T : class
    {
        string Property { get; }
    }

    public interface ITest<T, T2>
    {
        string Property { get; }
    }
"
        );
    }


    /// <summary>
    /// Test whether the <see cref="UnitAnalyzer"/> reports the <c>AYASUNA_HAZELHELM_1000</c> diagnostic for "duplicate" interfaces whose type parameters have no type constraints
    /// </summary>
    [Fact]
    public async Task The_AYASUNA_HAZELHELM_1000_diagnostic_should_be_reported_for_duplicate_interfaces_without_type_constraints()
    {
        var diagnosticBase = Verify.Diagnostic(DiagnosticId).WithArguments("2");

        await Verify.VerifyAnalyzerAsync
        (
            @"
    public interface ITest
    {
        string Property { get; }
    }

    public interface ITest<T>
    {
        string Property { get; }
    }

    public interface ITest<T, T2>
    {
        string Property { get; }
    }
",
            diagnosticBase.WithLocation(2, 5),
            diagnosticBase.WithLocation(7, 5),
            diagnosticBase.WithLocation(12, 5)
        );
    }

    /// <summary>
    /// Test whether the <see cref="UnitAnalyzer"/> reports the <c>AYASUNA_HAZELHELM_1000</c> diagnostic for "duplicate" interfaces whose type parameters only have supported type constraints
    /// </summary>
    [Fact]
    public async Task The_AYASUNA_HAZELHELM_1000_diagnostic_should_be_reported_for_duplicate_interfaces_with_supported_type_constraints()
    {
        var diagnosticBase = Verify.Diagnostic(DiagnosticId).WithArguments("4");

        await Verify.VerifyAnalyzerAsync
        (
            @"
    public interface ITest
    {
    }

    public interface ITest<T> where T : new()
    {
    }

    public interface ITest<T, T2> where T : struct where T2 : notnull
    {
    }

    public interface ITest<T, T2, T3> where T : struct where T2 : notnull where T3 : new()
    {
    }

    public interface ITest<T, T2, T3, T4> 
    {
    }
",
            diagnosticBase.WithLocation(2, 5),
            diagnosticBase.WithLocation(6, 5),
            diagnosticBase.WithLocation(10, 5),
            diagnosticBase.WithLocation(14, 5),
            diagnosticBase.WithLocation(18, 5)
        );
    }
}