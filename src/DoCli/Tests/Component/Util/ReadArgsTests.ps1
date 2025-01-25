using namespace System.Collections.Generic;

Describe "ReadArgsTests" {
    it "Returns Empty Dictionary" {
        # Arrange/Act
        [Dictionary[string, object]] $result = doing Read-Args;

        # Assert
        $result | Should -Not -Be $null;

        $result.Count | Should -Be 0;
    }

    it "Returns Populated Dictionary" {
        # Arrange
        [string] $stringArg = "thestring";
        [int] $intArg = 5;
        [List[string]] $listArg = [List[String]]::new();

        for ($i = 0; $i -lt 10; $i++) {
            $listArg.Add($i.ToString());
        }

        [Dictionary[string, object]] $result = doing Read-Args -item1 $stringArg -item2 $intArg -item3 $listArg;

        # Assert
        $result | Should -Not -Be $null;

        $result.Count | Should -Be 3;

        $result["item1"] | Should -Be $stringArg;
        $result["item2"] | Should -Be $intArg;
        $result["item3"] | Should -Be $listArg;
    }
}