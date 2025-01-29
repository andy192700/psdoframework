using namespace DoFramework.Processing;
using namespace DoFramework.Services;

Describe 'ServiceContainerTests' {

    BeforeEach {
        class ExampleType {
            [int] $myInt;
            [float] $myFloat;
            [double] $myDouble;
            [bool] $myBool;
            [char] $myChar;
            [byte] $myByte;
            [short] $myShort;
            [long] $myLong;
            [decimal] $myDecimal;
            [string] $myString;
        }

        [ServiceContainer] $script:sut = [ServiceContainer]::new()
        
        $script:sut.RegisterService([ISession], [Session]);
        $script:sut.RegisterService([IContext], [Context]);        
    }

    it 'Configures object with all parameters set' {
        # Arrange
        [IContext] $context = $script:sut.GetService([IContext]);
        $context.AddOrUpdate("ExampleType.myInt", 222);
        $context.AddOrUpdate("ExampleType.myFloat", 1.111);
        $context.AddOrUpdate("ExampleType.myDouble", 7.77);
        $context.AddOrUpdate("ExampleType.myBool", $true);
        $context.AddOrUpdate("ExampleType.myChar", 'c');
        $context.AddOrUpdate("ExampleType.myByte", [byte]2);
        $context.AddOrUpdate("ExampleType.myShort", [short]17);
        $context.AddOrUpdate("ExampleType.myLong", 44444424);
        $context.AddOrUpdate("ExampleType.myDecimal", [decimal]1.14);
        $context.AddOrUpdate("ExampleType.myString", "exampleString2");
        
        # Act
        $script:sut.Configure([ExampleType]);

        [ExampleType] $result = $sut.GetService([ExampleType]);

        # Assert
        $result.myInt | Should -Be 222;
        $result.myFloat | Should -Be ([float]1.111);
        $result.myDouble | Should -Be 7.77;
        $result.myBool | Should -Be $true;
        $result.myChar | Should -Be 'c';
        $result.myByte | Should -Be ([byte]2);
        $result.myShort | Should -Be ([short]17);
        $result.myLong | Should -Be 44444424;
        $result.myDecimal | Should -Be ([decimal]1.14);
        $result.myString | Should -Be "exampleString2";
    }

    it 'Configures object with no parameters set' {
        # Arrange / Act
        $script:sut.Configure([ExampleType]);

        [ExampleType] $result = $sut.GetService([ExampleType]);

        # Assert
        $result.myInt | Should -Be 0;
        $result.myFloat | Should -Be ([float]0);
        $result.myDouble | Should -Be 0;
        $result.myBool | Should -Be $false;
        $result.myChar | Should -Be '';
        $result.myByte | Should -Be ([byte]0);
        $result.myShort | Should -Be ([short]0);
        $result.myLong | Should -Be 0;
        $result.myDecimal | Should -Be ([decimal]0);
        $result.myString | Should -Be $null;
    }

    it 'Incorrect Type Should Throw' {
        # Arrange
        [IContext] $context = $script:sut.GetService([IContext]);

        $context.AddOrUpdate("ExampleType.myInt", "exampleString3");

        # Act
        $func = {
            $script:sut.Configure([ExampleType]);
        };

        # Assert
        $func | Should -Throw;
    }
}