using namespace DoFramework.Testing;
using namespace DoFramework.Validators;
using namespace System.Reflection;
using module "..\..\..\..\Objects\Validators\ProxyTypeValidator.psm1";

Describe 'ProxyTypeValidatorTests' {
    BeforeAll {
        Class SampleClass {}

        Add-Type -Language CSharp -TypeDefinition "public sealed class SealedClass {}";
        
        Add-Type -Language CSharp -TypeDefinition "public class NormalClass {}";
        
        Add-Type -Language CSharp -TypeDefinition "public abstract class AbstractClass {}";

        Add-Type -Language CSharp -TypeDefinition "public interface AnInterface {}";
    }

    Context 'Tests' {
        it 'Valid if Powershell Class' {
            # Arrange
            [ProxyTypeValidator] $sut = [ProxyTypeValidator]::new();
            
            # Act
            [ValidationResult] $result = $sut.Validate([SampleClass]);
            
            # Assert
            $result.IsValid | Should -Be $true;

            $result.Errors.Count | Should -Be 0;
        }

        it 'Valid if C# Class' {
            # Arrange
            [ProxyTypeValidator] $sut = [ProxyTypeValidator]::new();
            
            # Act
            [ValidationResult] $result = $sut.Validate([NormalClass]);
            
            # Assert
            $result.IsValid | Should -Be $true;

            $result.Errors.Count | Should -Be 0;
        }

        it 'Valid if abstract C# Class' {
            # Arrange
            [ProxyTypeValidator] $sut = [ProxyTypeValidator]::new();
            
            # Act
            [ValidationResult] $result = $sut.Validate([AbstractClass]);
            
            # Assert
            $result.IsValid | Should -Be $true;

            $result.Errors.Count | Should -Be 0;
        }

        it 'Valid if C# Interface' {
            # Arrange
            [ProxyTypeValidator] $sut = [ProxyTypeValidator]::new();
            
            # Act
            [ValidationResult] $result = $sut.Validate([AnInterface]);
            
            # Assert
            $result.IsValid | Should -Be $true;

            $result.Errors.Count | Should -Be 0;
        }

        it 'Invalid if Sealed C# Class' {
            # Arrange
            [ProxyTypeValidator] $sut = [ProxyTypeValidator]::new();
            
            # Act
            [ValidationResult] $result = $sut.Validate([SealedClass]);
            
            # Assert
            $result.IsValid | Should -Be $false;

            $result.Errors.Count | Should -Be 1;

            $result.Errors[0] | Should -Be "Cannot create a proxy for type $([SealedClass].FullName) - it must either be an interface or an unsealed type.";
        }
    }
}