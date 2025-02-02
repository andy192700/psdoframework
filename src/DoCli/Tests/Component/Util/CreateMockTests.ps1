using namespace DoFramework.Testing;
using namespace System.Collections.Generic;
using namespace System.Management.Automation;
using module "..\Do\Modules\TestClassModule.psm1";
using module "..\Common\TestContext.psm1";

Describe 'CreateProxyClassTests' {
    BeforeEach {
        [TestContext] $script:context = [TestContext]::new();

        $script:context.SetCurrentPathToTestProject();
    }

    AfterEach {
        $script:context.ResetCurrentPath();
    }

    it 'Cannot instantiate invalid class' {
        # Arrange / Act
        $result = doing mock -type ([SealedClass]);

        # Assert
        $result | Should -Be $null;
    }

    it 'Creates basic PowerShell class' {
        # Arrange
        [Type] $testType = [BasicClass];

        # Act
        [ProxyResult] $result = doing mock -type $testType;

        # Assert
        $result | Should -Not -Be $null;
        $result.Proxy | Should -Not -Be $null;
        $result.Instance | Should -Not -Be $null;
        
        $result.Instance.GetType().Name | Should -Be "$($testType.Name)Proxy";
        $result.Instance.GetType().BaseType.Name | Should -Be "$($testType.Name)";
    }

    it 'Fails As Constructor Does Not Exist' {
        # Arrange
        [Type] $testType = [BasicClass];
        [string] $testString = "AString";

        # Act
        $result = doing mock -type ($testType) -params @($testString);

        # Assert
        $result | Should -Be $null;
    }

    it 'Creates PowerShell class multiple constructors' {
        # Arrange
        [Type] $testType = [MultipleConstructors];
        [string] $testString = "AString";
        [int] $testInt = 3;

        # Act
        [ProxyResult] $result1 = doing mock -type $testType -params @($testString);
        [ProxyResult] $result2 = doing mock -type $testType -params @($testString, $testInt);

        # Assert
        $result1 | Should -Not -Be $null;
        $result1.Proxy | Should -Not -Be $null;
        $result1.Instance | Should -Not -Be $null;
        
        $result1.Instance.GetType().Name | Should -Be "$($testType.Name)Proxy";
        $result1.Instance.GetType().BaseType.Name | Should -Be "$($testType.Name)";

        $result1.Instance.InjectedString = $testString;
        $result1.Instance.InjectedInt = 0;
        
        $result2 | Should -Not -Be $null;
        $result2.Proxy | Should -Not -Be $null;
        $result2.Instance | Should -Not -Be $null;
        
        $result2.Instance.GetType().Name | Should -Be "$($testType.Name)Proxy";
        $result2.Instance.GetType().BaseType.Name | Should -Be "$($testType.Name)";

        $result2.Instance.InjectedString = [string]::Empty;
        $result2.Instance.InjectedInt = $testInt;
    }

    it 'Is possible to override specific method overloads' {
        # Arrange
        [Type] $testType = [OverloadedMethods];
        [string] $returnString1 = "I am the return string 1.";
        [string] $returnString2 = "I am the return string 2.";
        [string] $returnString3 = "I am the return string 3.";
        [string] $input1 = "Input1";
        [int] $input2 = 2;
        [string] $methodName = "StringMethod";

        # Act
        [ProxyResult] $result = doing mock -type $testType;

        $result.Proxy.MockMethod($methodName, {
            return $returnString1;
        });

        $result.Proxy.MockMethod($methodName, {
            param ([string] $input1)
            return $returnString2;
        });

        $result.Proxy.MockMethod($methodName, {
            param ([string] $input1, [int] $input2)
            return $returnString3;
        });

        # Assert
        $result | Should -Not -Be $null;
        $result.Proxy | Should -Not -Be $null;
        $result.Instance | Should -Not -Be $null;
        
        $result.Instance.GetType().Name | Should -Be "$($testType.Name)Proxy";
        $result.Instance.GetType().BaseType.Name | Should -Be "$($testType.Name)";

        $result.Instance.StringMethod() | Should -Be $returnString1;
        $result.Instance.StringMethod($input1) | Should -Be $returnString2;
        $result.Instance.StringMethod($input1, $input2) | Should -Be $returnString3;
        
        $result.Proxy.CountCalls($methodName) | Should -Be 3;
        $result.Proxy.CountCalls($methodName, (doing args -input1 $input1)) | Should -Be 1;        
        $result.Proxy.CountCalls($methodName, (doing args -input1 $input1 -input2 $input2)) | Should -Be 1;
    }
}

Describe 'CreateProxyInterfaceTests' {

    BeforeEach {        
        [ProxyResult] $script:sut = doing mock -type ([ISampleInterface]);
        
        [object] $script:testObj = [object]::new();

        [float] $script:testFloat = 3.1;
        
        [double] $script:testDouble = 65.522;

        [int] $script:testInt = 1337;

        [List[string]] $script:testOneItemList = [List[string]]::new();

        $testOneItemList.Add("one");

        [List[string]] $script:testList = [List[string]]::new();

        $testList.Add("one");
        $testList.Add("two");
        $testList.Add("three");

        [Dictionary[string, string]] $script:testOneItemDict = [Dictionary[string, string]]::new();

        $testOneItemDict["one"] = "no. one";

        [Dictionary[string, string]] $script:testDict = [Dictionary[string, string]]::new();

        $testDict["one"] = "no. one";
        $testDict["two"] = "no. two";

        [string[]] $script:testOneItemArray = [string[]]::new(1);

        $script:testOneItemArray[0] = "some entry";

        [string[]] $script:testArray = [string[]]::new(5);

        for ($i = 0; $i -lt 5; $i++) {
            $script:testArray[$i] = $i.ToString();
        }
    }
    
    Context 'Tests' {
        it 'Invokes void' {
            # Arrange / Act
            $sut.Instance.VoidNoParams();

            # Assert
            $sut.Proxy.CountCalls("VoidNoParams") | Should -Be 1;
            $sut.Proxy.CountCalls("VoidWithParams") | Should -Be 0;
        }

        it 'Mocks interface Property Getter' {
            # Arrange
            [string] $sampleValue = "abcdefgh123456";

            $sut.Proxy.MockProperty("StringProperty", {
                return $sampleValue;
            });

            # Act
            [string] $result = $sut.Instance.StringProperty;

            # Assert
            $result | Should -Be $sampleValue;
            $sut.Proxy.CountPropertyCalls("StringProperty") | Should -Be 1;
        }
        
        it 'Invokes void with params' {
            # Arrange / Act
            $sut.Instance.VoidWithParams($testObj, $testFloat);

            # Assert
            $sut.Proxy.CountCalls("VoidNoParams") | Should -Be 0;
            $sut.Proxy.CountCalls("VoidWithParams", (doing args -someObj $testObj -someFloat $testFloat)) | Should -Be 1;
        }
        
        it 'Returns double no params' {
            # Arrange
            $sut.Proxy.MockMethod("ReturnsADoubleNoParams", {
                return $testDouble;
            });

            # Act
            [double] $result = $sut.Instance.ReturnsADoubleNoParams();

            # Assert
            $result | Should -Be $testDouble;

            $sut.Proxy.CountCalls("ReturnsADoubleNoParams") | Should -Be 1;
            $sut.Proxy.CountCalls("ReturnsADoubleWithParams") | Should -Be 0;
        }
        
        it 'Returns double with params' {
            # Arrange
            $sut.Proxy.MockMethod("ReturnsADoubleWithParams", {
                param([object] $someObj, [float] $someFloat)

                return $testDouble;
            });

            # Act
            [double] $result = $sut.Instance.ReturnsADoubleWithParams($testObj, $testFloat);

            # Assert
            $result | Should -Be $testDouble;

            $sut.Proxy.CountCalls("ReturnsADoubleNoParams") | Should -Be 0;
            $sut.Proxy.CountCalls("ReturnsADoubleWithParams", (doing args -someObj $testObj -someFloat $testFloat)) | Should -Be 1;
        }
        
        it 'Returns list no params' {
            # Arrange
            $sut.Proxy.MockMethod("ReturnsAListNoParams", {
                return $testList;
            });

            # Act
            [List[string]] $result = $sut.Instance.ReturnsAListNoParams();

            # Assert
            $result.Count | Should -Be $testList.Count;
            foreach ($item in $result) {
                $result[$item] | Should -Be $testList[$item];
            }

            $sut.Proxy.CountCalls("ReturnsAListNoParams") | Should -Be 1;
            $sut.Proxy.CountCalls("ReturnsAListWithParams") | Should -Be 0;
        }
        
        it 'Returns one item list no params' {
            # Arrange
            $sut.Proxy.MockMethod("ReturnsAListNoParams", {
                return $testOneItemList;
            });

            # Act
            [List[string]] $result = $sut.Instance.ReturnsAListNoParams();

            # Assert
            $result.Count | Should -Be 1;
            $result[0] | Should -Be $testOneItemList[0];

            $sut.Proxy.CountCalls("ReturnsAListNoParams") | Should -Be 1;
            $sut.Proxy.CountCalls("ReturnsAListWithParams") | Should -Be 0;
        }

        it 'Returns list with params' {
            # Arrange
            $sut.Proxy.MockMethod("ReturnsAListWithParams", {
                param([object] $someObj, [float] $someFloat)

                return $testList;
            });

            # Act
            [List[string]] $result = $sut.Instance.ReturnsAListWithParams($testObj, $testFloat);

            # Assert
            $result.Count | Should -Be $testList.Count;
            foreach ($item in $result) {
                $result[$item] | Should -Be $testList[$item];
            }

            $sut.Proxy.CountCalls("ReturnsAListNoParams") | Should -Be 0;
            $sut.Proxy.CountCalls("ReturnsAListWithParams", (doing args -someObj $testObj -someFloat $testFloat)) | Should -Be 1;
        }
        
        it 'Returns dictionary no params' {
            # Arrange
            $sut.Proxy.MockMethod("ReturnsADictionaryNoParams", {
                return $testDict;
            });

            # Act
            [Dictionary[string, string]] $result = $sut.Instance.ReturnsADictionaryNoParams();

            # Assert
            $result.Keys.Count | Should -Be $testdict.Keys.Count;
            foreach ($key in $result.Keys) {
                $result[$key] | Should -Be $testDict[$key];
            }            

            $sut.Proxy.CountCalls("ReturnsADictionaryNoParams") | Should -Be 1;
            $sut.Proxy.CountCalls("ReturnsADictionaryWithParams") | Should -Be 0;
        }

        it 'Returns dictionary with params' {
            # Arrange
            $sut.Proxy.MockMethod("ReturnsADictionaryWithParams", {
                param([int] $someInt)

                return $testDict;
            });

            # Act
            [Dictionary[string, string]] $result = $sut.Instance.ReturnsADictionaryWithParams($testInt);

            # Assert
            $result.Keys.Count | Should -Be $testdict.Keys.Count;
            foreach ($key in $result.Keys) {
                $result[$key] | Should -Be $testDict[$key];
            }            

            $sut.Proxy.CountCalls("ReturnsADictionaryNoParams") | Should -Be 0;
            $sut.Proxy.CountCalls("ReturnsADictionaryWithParams", (doing args -someInt $testInt)) | Should -Be 1;
        }

        it 'Returns dictionary with params' {
            # Arrange
            $sut.Proxy.MockMethod("ReturnsADictionaryWithParams", {
                param([int] $someInt)

                return $testOneItemDict;
            });

            # Act
            [Dictionary[string, string]] $result = $sut.Instance.ReturnsADictionaryWithParams($testInt);

            # Assert
            $result.Keys.Count | Should -Be 1;
            $result[0] | Should -Be $testDict[0];

            $sut.Proxy.CountCalls("ReturnsADictionaryNoParams") | Should -Be 0;
            $sut.Proxy.CountCalls("ReturnsADictionaryWithParams", (doing args -someInt $testInt)) | Should -Be 1;
        }

        it 'Returns array no params' {
            # Arrange
            $sut.Proxy.MockMethod("ReturnsAnArrayNoParams", {
                return $testArray;
            });

            # Act
            [string[]] $result = $sut.Instance.ReturnsAnArrayNoParams();

            # Assert
            $result.Length | Should -Be $testArray.Length;
            for ($j = 0; $j -lt $testArray.Length; $j++) {
                $result[$j] | Should -Be $testArray[$j];
            }

            $sut.Proxy.CountCalls("ReturnsAnArrayNoParams") | Should -Be 1;
            $sut.Proxy.CountCalls("ReturnsAnArrayWithParams") | Should -Be 0;
        }

        it 'Returns a one item array no params' {
            # Arrange
            $sut.Proxy.MockMethod("ReturnsAnArrayNoParams", {
                return $testOneItemArray;
            });

            # Act
            [string[]] $result = $sut.Instance.ReturnsAnArrayNoParams();

            # Assert
            $result.Length | Should -Be 1;
            $testOneItemArray[0] | Should -Be $testOneItemArray[0];

            $sut.Proxy.CountCalls("ReturnsAnArrayNoParams") | Should -Be 1;
            $sut.Proxy.CountCalls("ReturnsAnArrayWithParams") | Should -Be 0;
        }

        it 'Returns array with params' {
            # Arrange
            $sut.Proxy.MockMethod("ReturnsAnArrayWithParams", {
                param([int] $someInt)

                return $testArray;
            });

            # Act
            [string[]] $result = $sut.Instance.ReturnsAnArrayWithParams($testInt);

            # Assert
            $result.Length | Should -Be $testArray.Length;
            for ($j = 0; $j -lt $testArray.Length; $j++) {
                $result[$j] | Should -Be $testArray[$j];
            }

            $sut.Proxy.CountCalls("ReturnsAnArrayNoParams") | Should -Be 0;
            $sut.Proxy.CountCalls("ReturnsAnArrayWithParams", (doing args -someInt $testInt)) | Should -Be 1;
        }

        it 'Returns empty array' {
            # Arrange
            $sut.Proxy.MockMethod("ReturnsAnArrayNoParams", {
                return [string[]]::new(0);
            });

            # Act
            [string[]] $result = $sut.Instance.ReturnsAnArrayNoParams();

            # Assert
            $result.Length | Should -Be 0;

            $sut.Proxy.CountCalls("ReturnsAnArrayNoParams") | Should -Be 1;
            $sut.Proxy.CountCalls("ReturnsAnArrayWithParams") | Should -Be 0;
        }
        
        it 'Returns empty list' {
            # Arrange
            $sut.Proxy.MockMethod("ReturnsAListWithParams", {
                param([object] $someObj, [float] $someFloat)

                return [List[string]]::new();
            });

            # Act
            [List[string]] $result = $sut.Instance.ReturnsAListWithParams($testObj, $testFloat);

            # Assert
            $result.Count | Should -Be 0;

            $sut.Proxy.CountCalls("ReturnsAListNoParams") | Should -Be 0;
            $sut.Proxy.CountCalls("ReturnsAListWithParams", (doing args -someObj $testObj -someFloat $testFloat)) | Should -Be 1;
        }

        it 'Returns empty list' {
            # Arrange / Act
            for ($i = 0; $i -lt 10; $i++) {
                $sut.Instance.ReturnsAListWithParams($testObj, $testFloat);
            }

            # Assert
            $result.Count | Should -Be 0;

            $sut.Proxy.CountCalls("ReturnsAListNoParams") | Should -Be 0;
            $sut.Proxy.CountCalls("ReturnsAListWithParams", (doing args -someObj $testObj -someFloat $testFloat)) | Should -Be 10;

            $sut.Proxy.Reset();
            
            $sut.Proxy.CountCalls("ReturnsAListWithParams") | Should -Be 0;
        }

        It 'Mocks Generic Interface' {
            # Arrange
            [ProxyResult] $result = doing mock -type ([IList[string]]);

            # Act
            $result.Instance.Clear();

            # Assert
            $result.Proxy.CountCalls("Clear") | Should -Be 1;
        }
    }
}